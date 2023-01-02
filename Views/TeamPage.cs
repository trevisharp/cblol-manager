using System.Collections.Generic;
using System.Drawing;

namespace CBLoLManager.Views;

using Model;

public class TeamPage : BaseView
{
    private Team team;
    private PlayerTeamView playerView = null;
    private GameMap map = null;

    public TeamPage(Team team)
    {
        this.team = team;
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        float unity = bmp.Width * .05f;
        var org = team.Organization;
        
        float mapStart = 10 * unity, 
            margin = 40f;
        float mapwid = bmp.Width - mapStart - 10f - 2 * margin;
        float maphei = 8 * unity - 10f - 2 * margin;

        if (playerView == null)
        {
            playerView = new PlayerTeamView(team,
                new PointF(10f + mapStart, 10f + maphei + 2 * margin), 10 * unity - 5f
            );
        }

        if (map == null)
        {
            map = new GameMap(team.GetAll(), new RectangleF(
                margin + 10f + mapStart,
                margin + 5f,
                mapwid,
                maphei
            ));
        }
        
        var brush = new SolidBrush(
            Color.FromArgb(32, 42, 68)
        );
        rect(brush, () => new RectangleF(5f, 5f, 3 * unity, 3 * unity),
            org.Name + "back");
        img(5f, 5f, 3 * unity, 3 * unity, 
            () => Bitmap.FromFile("Img/" + org.Photo) as Bitmap,
            "logo");
        
        rect(brush, () => new RectangleF(
            10f + mapStart, 5f, bmp.Width - mapStart - 15f, 8 * unity - 10f),
            "mapback");
        
        playerView.Draw(bmp, g);
        map.Draw(bmp, g);
    }

    public override void MoseMove(PointF cursor, bool down)
    {
        playerView?.MoseMove(cursor, down);
    }
}