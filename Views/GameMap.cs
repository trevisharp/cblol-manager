using System.Linq;
using System.Drawing;

namespace CBLoLManager.Views;

using System.Collections.Generic;
using Model;

public class GameMap : BaseView
{
    RectangleF rectangle;
    
    Player top;
    Player jg;
    Player mid;
    Player adc;
    Player sup;

    public GameMap(
        IEnumerable<Player> players,
        RectangleF rect)
    {
        top = players.FirstOrDefault(p => p.Role == Position.TopLaner);
        jg = players.FirstOrDefault(p => p.Role == Position.Jungler);
        mid = players.FirstOrDefault(p => p.Role == Position.MidLaner);
        adc = players.FirstOrDefault(p => p.Role == Position.AdCarry);
        sup = players.FirstOrDefault(p => p.Role == Position.Support);
        this.rectangle = rect;
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        img(this.rectangle.X, this.rectangle.Y,
            this.rectangle.Width, this.rectangle.Height,
            () => Bitmap.FromFile("Img/map.png") as Bitmap, "map");

        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;
        
        draw(top, .26f, .05f);
        draw(jg, .27f, .33f);
        draw(mid, .47f, .38f);
        draw(sup, .70f, .65f);
        draw(adc, .76f, .73f);

        void draw(Player player, float pX, float pY)
        {
            if (player == null)
                return;
            
            g.DrawString(player.Nickname, font, Brushes.White, new RectangleF(
                this.rectangle.X + this.rectangle.Width * pX, 
                this.rectangle.Y + this.rectangle.Height * pY + 80f,
                80f, 20f), format);
            
            img(this.rectangle.X + this.rectangle.Width * pX,
                this.rectangle.Y + this.rectangle.Height * pY, 80f, 80f,
                () => Bitmap.FromFile("Img/" + player.Photo) as Bitmap, 
                player.Nickname + "photo");
        }
    }
}