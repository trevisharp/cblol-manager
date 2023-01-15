using System;
using System.Linq;
using System.Drawing;

namespace CBLoLManager.Views;

using Model;
using GameRule;

public class TorunamentView : BaseView
{
    private OptionsView options = null;
    private PointF cursor = PointF.Empty;
    private bool down  = false;
    private (Team team, int wins)[] teams;
    private Bitmap[] thumbs;
    private bool hasNext;

    public TorunamentView(bool hasNext = true)
        => this.hasNext = hasNext;

    protected override void draw(Bitmap bmp, Graphics g)
    {
        if (Game.Current.CurrentTorunament.Round < 19)
            drawTorunament(bmp, g);
        else
            drawPlayOffs(bmp, g);

        options.Draw(bmp, g);
        options.MouseMove(cursor, down);
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        var tournament = Game.Current.CurrentTorunament;

        g.Clear(Color.Black);
        if (hasNext)
            options = new OptionsView(
                "Ir para partida"
            );
        else options = new OptionsView(
                "Voltar para página do time"
            );
        options.OnOptionClick += s =>
        {
            if (s == "Ir para partida")
            {
                var oponent = tournament.SimulateRound();
                PlayNext(oponent);
            }
            else if (s == "Voltar para página do time")
            {
                Exit();
            }
        };

        this.teams = tournament.Teams
            .Zip(tournament.Wins)
            .OrderByDescending(t => t.Second)
            .ToArray();
        thumbs = this.teams
            .Select(t => Bitmap.FromFile("Img/" + t.team.Organization.Photo)
                .GetThumbnailImage(100, 100, null, IntPtr.Zero) as Bitmap)
            .ToArray();
    }

    void drawTorunament(Bitmap bmp, Graphics g)
    {
        int wid = bmp.Width;
        int hei = bmp.Height;

        float tableWid = wid / 2f - 40f;
        float tableHei = hei - 90f;
        float rowHei = tableHei / 10f;

        var font = new Font(FontFamily.GenericMonospace, 16f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        float y = 20;
        int t = 0;
        foreach (var team in teams)
        {
            g.DrawRectangle(Pens.White, 20, y, tableWid, rowHei);
            g.DrawImage(thumbs[t], 20, y, rowHei, rowHei);
            g.DrawString(team.team.Organization.Name, font, Brushes.White, 
                new RectangleF(20 + rowHei + 10, y, tableWid - 2 * rowHei, rowHei),
                format);
            g.DrawString(team.wins.ToString(), font, Brushes.White, 
                new RectangleF(20 + tableWid - rowHei, y, rowHei, rowHei),
                format);
            t++;
            y += rowHei;
        }
    }

    void drawPlayOffs(Bitmap bmp, Graphics g)
    {

    }

    public event Action<Team> PlayNext;
    public event Action Exit;
}