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

    Bitmap bg = null;

    public TorunamentView(bool hasNext = true)
        => this.hasNext = hasNext;

    protected override void draw(Bitmap bmp, Graphics g)
    {
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
        var tournament = Game.Current.Tournament;

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
        
        if (Game.Current.Tournament.Round < 19)
        {
            bg = Bitmap.FromFile("Img/class.png") as Bitmap;
            g.DrawImage(bg, new Rectangle(0, 0, bmp.Width, bmp.Height),
                new Rectangle(0, 0, bg.Width, bg.Height),
                GraphicsUnit.Pixel);
            drawTorunament(bmp, g);
        }
        else
        {
            bg = Bitmap.FromFile("Img/playoffs.png") as Bitmap;
            drawPlayOffs(bmp, g);
        }
    }

    void drawTorunament(Bitmap bmp, Graphics g)
    {
        var torunament = Game.Current.Tournament;

        int wid = bmp.Width;
        int hei = bmp.Height;

        float tableWid = wid * .9f;
        float tableHei = hei * 2 / 3f - 60f;
        float rowHei = tableHei / 10f;

        var font = new Font(FontFamily.GenericMonospace, 16f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        float y = 20 + .3f * hei;
        int t = 0;
        foreach (var team in teams)
        {
            RectangleF thumbRect = new RectangleF(
                20 + .125f * wid, 
                y + rowHei * .05f,
                0.025f * wid, 
                rowHei * .9f
            );
            g.FillRectangle(Brushes.Gray, thumbRect);
            g.DrawImage(thumbs[t], thumbRect);

            RectangleF textRect = new RectangleF(
                thumbRect.X + thumbRect.Width + .04f * wid, 
                y, tableWid * .20f, rowHei);
            g.DrawString(team.team.Organization.Name, font,
                Brushes.White, textRect, format);
            
            g.DrawString(team.wins.ToString(), font, Brushes.White, 
                new RectangleF(textRect.X + textRect.Width + .06f * tableWid, y, rowHei, rowHei),
                format);
            g.DrawString((torunament.Round - team.wins).ToString(), font, Brushes.White, 
                new RectangleF(textRect.X + textRect.Width + .175f * tableWid, y, rowHei, rowHei),
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