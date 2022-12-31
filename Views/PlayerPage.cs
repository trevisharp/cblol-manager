using System.Drawing;

namespace CBLoLManager.Views;

using Model;

public class PlayerPage : BaseView
{
    private PlayerStatusView status = null;
    private Player player;
    private float size;

    public PlayerPage(Player player, PointF point, float size)
    {
        this.status = new PlayerStatusView();
        this.status.Player = player;
        this.Location = point;
        this.player = player;
        this.size = size;
    }

    public override void draw(Bitmap bmp, Graphics g)
    {
        if (!Location.HasValue)
            return;
        var p = this.Location.Value;

        img(new RectangleF(p.X, p.Y, 100, 100),
            () => Bitmap.FromFile("Img/" + player.Photo) as Bitmap, "photo");
        // status.draw(bmp, g);
    }
}