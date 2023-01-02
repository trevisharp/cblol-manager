using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace CBLoLManager.Views;

using Model;

public class PlayerCarrousel : BaseView
{
    public Player Current => players[crr];

    private Player[] players;
    private PlayerPage page;
    private PointF cursor = PointF.Empty;
    private bool down = false;
    private int crr = 0;
    private float size = 0f;
    private bool indown = false;
    private int dir = 0;

    public PlayerCarrousel(PointF location, float size, IEnumerable<Player> players)
    {
        this.players = players.ToArray();
        this.Location = location;
        this.size = size;

        if (players.Count() == 0)
            return;
        
        page = new PlayerPage(this.players[crr], 
            new PointF(location.X + 70f, location.Y), size);
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        if (!Location.HasValue)
            return;
        var p = Location.Value;

        g.FillRectangle(Brushes.Black, 
            new RectangleF(p.X, p.Y, 1.5f * size, size / 3));

        lines(Pens.White, () => new PointF[]{
            new PointF(p.X + 60f, p.Y + 3 * size / 27),
            new PointF(p.X + 10f, p.Y + 4.5f * size / 27),
            new PointF(p.X + 60f, p.Y + 6f * size / 27) 
        }, "left");
        lines(Pens.White, () => new PointF[]{
            new PointF(p.X + size + 70f + 10f, p.Y + 3 * size / 27),
            new PointF(p.X + size + 70f + 60f, p.Y + 4.5f * size / 27),
            new PointF(p.X + size + 70f + 10f, p.Y + 6f * size / 27) 
        }, "rigth");

        if (players.Count() == 0)
            return;
        
        page.Draw(bmp, g);

        if (!new RectangleF(p.X, p.Y, size + 140f, size / 3).Contains(cursor))
            return;
        
        if (cursor.X < p.X + 70f)
            dir = -1;
        else if (cursor.X > p.X + size - 70f)
            dir = 1;
        else dir = 0;

        if (dir == 0)
            return;

        if (down)
            indown = true;
        
        if (!down && indown)
        {
            indown = false;
            crr += dir;
            if (crr == players.Length)
                crr = 0;
            else if (crr == -1)
                crr = players.Length - 1;
            page.Reset();
            page.Player = players[crr];
        }
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
    }
}