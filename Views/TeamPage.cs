using System.Collections.Generic;
using System.Drawing;

namespace CBLoLManager.Views;

using Model;
using GameRule;

public class TeamPage : BaseView
{
    private Team team;
    private PlayerTeamView playerView = null;
    private PlayerStatusView teamStatus = null;
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

        if (teamStatus == null)
        {
            teamStatus = new PlayerStatusView();
            teamStatus.Player = team.AveragePlayer();
            teamStatus.Location = new PointF(4 * unity, 40f);
            teamStatus.Size = 3 * unity;
        }

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
        
        playerView.Draw(bmp, g);
        teamStatus.Draw(bmp, g);
        map.Draw(bmp, g);
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
    }

    public override void MoseMove(PointF cursor, bool down)
    {
        playerView?.MoseMove(cursor, down);
    }
}