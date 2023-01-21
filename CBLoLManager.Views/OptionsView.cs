using System;
using System.Collections.Generic;
using System.Drawing;

namespace CBLoLManager.Views;

public class OptionsView : BaseView
{
    private List<string> options = null;
    private List<string> checkBoxOptions = null;
    private List<string> checkBoxOptionsChecked = null;
    private PointF cursor = PointF.Empty;
    private bool down = false;
    private string selected = string.Empty;

    public IEnumerable<string> Checked => checkBoxOptionsChecked;

    public OptionsView(params string[] opts)
    {
        options = new List<string>(opts);
        checkBoxOptions = new List<string>();
        checkBoxOptionsChecked = new List<string>();
    }

    public void SetCheckBox(string opt)
    {
        checkBoxOptions.Add(opt);
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        g.FillRectangle(Brushes.Navy, new RectangleF(
            0f, bmp.Height - 50f, bmp.Width, 50f
        ));

        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        float x = 0f;
        foreach (var opt in this.options)
        {
            bool isCheck = checkBoxOptions.Contains(opt);
            bool isChecked = checkBoxOptionsChecked.Contains(opt);

            var rect = new RectangleF(
                x + 10f, bmp.Height - 40f, 180f, 30f
            );
            var brush = isCheck && isChecked ? Brushes.Green : Brushes.White;
            if (rect.Contains(cursor))
            {
                g.FillRectangle(brush, rect);
                g.DrawString(opt, font, Brushes.Navy, rect, format);

                if (down)
                    selected = opt;
                
                if (!down && selected == opt)
                {
                    if (isCheck)
                    {
                        if (isChecked)
                            checkBoxOptionsChecked.Remove(selected);
                        else 
                            checkBoxOptionsChecked.Add(selected);
                    }
                    if (OnOptionClick != null)
                        OnOptionClick(selected);
                }
            }
            else
            {
                g.DrawString(opt, font, brush, rect, format);
            }
            x += 200f;
        }
        if (!down)
            selected = string.Empty;
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
    }

    public event Action<string> OnOptionClick;
}