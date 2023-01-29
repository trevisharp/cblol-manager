using System;
using System.Drawing;
using System.Threading;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace CBLoLManager.Animations;

public static class AnimationExtension
{
    private static object grayScaleLockObj = new object();
    private static int grayScaleBarrierCount = 0;

    public static Image ToGrayScale(this Image img, double prop)
        => img.ToGrayScale((float)prop);

    public unsafe static Image ToGrayScale(this Image img, float prop)
    {
        var newImg = img.Clone() as Image;

        if (grayScaleBarrierCount > 0)
            return null;
        grayScaleBarrierCount++;
        
        prop = prop > 1f ? 1f : prop;
        prop = prop < 0f ? 0f : prop;
        float nprop = 1f - prop;

        var bmp = img as Bitmap;
        var newBmp = newImg as Bitmap;
        BitmapData data = null;
        BitmapData newData = null;

        try
        {
            data = bmp.LockBits(
                new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            newData = newBmp.LockBits(
                new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            var p = (byte*)data.Scan0.ToPointer();
            var n = (byte*)newData.Scan0.ToPointer();
            int wid = bmp.Width;

            Parallel.For(0, bmp.Height, j => {
                var line = p + j * data.Stride;
                var nline = n + j * newData.Stride;

                for (int i = 0; i < wid; i++, line += 4, nline += 4)
                {
                    if (line[3] == 0)
                    {
                        nline[3] = 0;
                        continue;
                    }

                    float value = 
                        0.11f * line[0] +
                        0.59f * line[1] +
                        0.30f * line[2];

                    nline[0] = (byte)(
                        line[0] * nprop + value * prop
                    );
                    nline[1] = (byte)(
                        line[1] * nprop + value * prop
                    );
                    nline[2] = (byte)(
                        line[2] * nprop + value * prop
                    );
                }
            });
        }
        catch (Exception ex)
        {
            newImg = null;
            throw ex;
        }
        finally
        {
            if (data != null)
                bmp.UnlockBits(data);
            if (newBmp != null)
                newBmp.UnlockBits(newData);
        }

        grayScaleBarrierCount--;
        return newImg;
    }
}
