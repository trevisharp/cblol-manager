using System.Drawing;

namespace CBLoLManager.Views;

using Model;

public class PlayerPage : BaseView
{
    private PlayerStatusView status = null;

    public PlayerPage(Player player)
    {
        status = new PlayerStatusView();
        status.Player = player;
    }

    public override void draw(Bitmap bmp, Graphics g)
    {
        
    }
}