using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

namespace CBLoLManager.Views;

using Model;

public class MarketRoundSumary : BaseView
{
    public Contract Contract { get; set; }
    public List<string> Events { get; set; }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        var font = new Font(FontFamily.GenericMonospace, 25f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        StringBuilder sb = new StringBuilder();

        if (Contract.Accepted)
            sb.AppendLine("Sua proposta foi aceita!");
        else sb.AppendLine("Você recebeu uma contra-proposta!");

        foreach (var ev in this.Events)
        {
            sb.AppendLine(ev);
        }

        g.DrawString(sb.ToString(), font, Brushes.White, 
            new RectangleF(0, 0, bmp.Width, bmp.Height), format);
    }

    bool isdown = false;
    public override void MouseMove(PointF cursor, bool down)
    {
        if (down)
            isdown = true;
        
        if (!down && isdown && Exit != null)
            Exit();
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
    }

    public event Action Exit;
}