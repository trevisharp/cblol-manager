using System.Drawing;

namespace CBLoLManager.Views;

using Model;

public class MatchView : BaseView
{
    int step = 0;
    Team a = null;
    Team b = null;

    Bitmap draft = null;

    public MatchView(Team A, Team B)
    {
        this.a = A;
        this.b = B;

        draft = Bitmap.FromFile("Img/draft.png") as Bitmap;
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {   
        var font = new Font(FontFamily.GenericMonospace, 20f);
        var font2 = new Font(FontFamily.GenericMonospace, 15f);
        StringFormat format = new StringFormat();
        format.LineAlignment = StringAlignment.Center;

        // Draft
        if (step == 0)
        {
            float hei = bmp.Width / (float)draft.Width * draft.Height;
            g.DrawImage(draft,
                new RectangleF(0, bmp.Height - hei, bmp.Width, hei),
                new RectangleF(0, 0, draft.Width, draft.Height),
                GraphicsUnit.Pixel);

            format.Alignment = StringAlignment.Far;
            g.DrawString(a.Organization.Name, font, Brushes.White,
                new RectangleF(bmp.Width * .26f, bmp.Height - hei + 10f, 350f, 30f), format);

            g.DrawString(a.TopLaner?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * -.09f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
                
            g.DrawString(a.Jungler?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .0f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            g.DrawString(a.MidLaner?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .09f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            g.DrawString(a.AdCarry?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .18f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            g.DrawString(a.Support?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .27f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
  
            format.Alignment = StringAlignment.Near;
            g.DrawString(b.Organization.Name, font, Brushes.White,
                new RectangleF(bmp.Width * .56f, bmp.Height - hei + 10f, 350f, 30f), format);

            g.DrawString(b.TopLaner?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .55f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
                
            g.DrawString(b.Jungler?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .64f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            g.DrawString(b.MidLaner?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .73f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            g.DrawString(b.AdCarry?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .82f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            g.DrawString(b.Support?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .91f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
        }
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
    }
}