using System;
using System.Linq;
using System.Drawing;

namespace CBLoLManager.Views;

using GameRule;
using Model;

public class PosMarketPage : BaseView
{
    Bitmap show = null;
    Bitmap show2 = null;
    int frame = 0;
    bool started = false;
    public PosMarketPage()
    {
        show = Bitmap.FromFile("Img/showtana.jpg") as Bitmap;
        show2 = Bitmap.FromFile("Img/show2.png") as Bitmap;
    }
    
    protected override void draw(Bitmap bmp, Graphics g)
    {
        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        var wid = bmp.Height * show.Width / show.Height;
        g.DrawImage(show, new RectangleF((bmp.Width - wid) / 2, 0f, wid, bmp.Height),
            new RectangleF(0, 0, show.Width, show.Height), 
            GraphicsUnit.Pixel);
        
        var hei = bmp.Width * show2.Height / show2.Width;
        var start = (bmp.Height - hei) / 2 + 40f;
        if (started)
        {
            g.DrawImage(show2, new RectangleF(0f, start, 
                bmp.Width, hei),
                new RectangleF(0, 0, show2.Width, show2.Height), 
                GraphicsUnit.Pixel);
            
            foreach (var team in Game.Current.Others.Append(Game.Current.Team))
            {
                if (team.Organization.Name == "Los Grandes")
                    writeTeam(team, 1);
                
                if (team.Organization.Name == "Furia")
                    writeTeam(team, 2);
                
                if (team.Organization.Name == "INTZ")
                    writeTeam(team, 3);
                
                if (team.Organization.Name == "Kabum")
                    writeTeam(team, 4);
                
                if (team.Organization.Name == "Loud")
                    writeTeam(team, 5);
                
                if (team.Organization.Name == "Vivo Keyd Stars")
                    writeTeam(team, 6);
                
                if (team.Organization.Name == "Pain Gamming")
                    writeTeam(team, 7);
                
                if (team.Organization.Name == "Red Canids Kalunga")
                    writeTeam(team, 8);
                
                if (team.Organization.Name == "Fluxo")
                    writeTeam(team, 9);
                
                if (team.Organization.Name == "Liberty")
                    writeTeam(team, 10);
            }
        }

        void writeTeam(Team team, int column)
        {
            g.DrawString(team.TopLaner?.Nickname ?? "?", font, Brushes.Black, 
                new RectangleF(column * bmp.Width / 11, 
                start + 3 * hei / 12 - 20f, 
                bmp.Width / 11, 20f), format);
            g.DrawString(team.Jungler?.Nickname ?? "?", font, Brushes.Black, 
                new RectangleF(column * bmp.Width / 11, 
                start + 4 * hei / 12 - 20f, 
                bmp.Width / 11, 20f), format);
            g.DrawString(team.MidLaner?.Nickname ?? "?", font, Brushes.Black, 
                new RectangleF(column * bmp.Width / 11, 
                start + 5 * hei / 12 - 20f, 
                bmp.Width / 11, 20f), format);
            g.DrawString(team.AdCarry?.Nickname ?? "?", font, Brushes.Black, 
                new RectangleF(column * bmp.Width / 11, 
                start + 6 * hei / 12 - 20f, 
                bmp.Width / 11, 20f), format);
            g.DrawString(team.Support?.Nickname ?? "?", font, Brushes.Black, 
                new RectangleF(column * bmp.Width / 11, 
                start + 7 * hei / 12 - 20f, 
                bmp.Width / 11, 20f), format);
        }
    }

    bool isdown = false;
    public override void MouseMove(PointF cursor, bool down)
    {
        if (down)
            isdown = true;
        
        if (!down && isdown)
        {
            if (!started)
                started = true;
            else if (Exit != null)
                Exit();
            isdown = false;
        }   
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
    }

    public event Action Exit;
}