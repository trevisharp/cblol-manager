using System.Drawing;

namespace CBLoLManager.Views;

using Model;

public class PlayerPage : BaseView
{
    private PlayerStatusView status = null;
    private Player player;
    private float size;
    private int frame = 0;

    public PlayerPage(Player player, PointF point, float size)
    {
        this.status = new PlayerStatusView();
        this.status.Player = player;
        this.Location = point;
        this.player = player;
        this.size = size;

        status.Location = new PointF(point.X + size / 3 - 60f, point.Y + 50f);
        status.Size = size / 4;
    }

    public override void draw(Bitmap bmp, Graphics g)
    {
        frame++;
        if (!Location.HasValue)
            return;
        var p = this.Location.Value;

        img(p.X + 10f, p.Y + 10f, size / 3 - 20f,
            () => Bitmap.FromFile("Img/" + player.Photo) as Bitmap, "photo");
        
        Font font = new Font(FontFamily.GenericMonospace, 20f);
        g.DrawString(applyEffect(player.Nickname), font, Brushes.White, 
            p.X + 2 * size / 3 - 60f, p.Y + 10f);

        font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        g.DrawString(applyEffect($"{player.Name} ({player.BirthDate.ToShortDateString()})"), 
            font, Brushes.White, p.X + 2 * size / 3 - 60f, p.Y + 60f);
        
        g.DrawString(applyEffect($"Rota: {player.Role}"), 
            font, Brushes.White, p.X + 2 * size / 3 - 60f, p.Y + 80f);
        
        g.DrawString(applyEffect($"Nacionalidade: {player.Nationality}"), 
            font, Brushes.White, p.X + 2 * size / 3 - 60f, p.Y + 100f);

        status.Draw(bmp, g);

        g.DrawRectangle(Pens.White, p.X, p.Y, size, size / 3);
        // status.draw(bmp, g);

        string applyEffect(string text)
        {
            int moment = frame / 2;
            return text.Substring(0,
                moment > text.Length ? text.Length : moment);
        }
    }
}