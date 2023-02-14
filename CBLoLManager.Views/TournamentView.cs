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
        
        if (Game.Current.Tournament.Round < 18)
        {
            bg = Bitmap.FromFile("Img/class.png") as Bitmap;
            g.DrawImage(bg, 
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                new Rectangle(0, 0, bg.Width, bg.Height),
                GraphicsUnit.Pixel);
            drawTorunament(bmp, g);
        }
        else
        {
            bg = Bitmap.FromFile("Img/playoffs.png") as Bitmap;
            g.DrawImage(bg, 
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                new Rectangle(0, 0, bg.Width, bg.Height),
                GraphicsUnit.Pixel);
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
        var tournament = Game.Current.Tournament;
        bool inPlayOffs = tournament.StartPlayOffs();
        var play = tournament.PlayOffs;

        int wid = bmp.Width;
        int hei = bmp.Height;

        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        if (play.Champion == null)
        {
            options = new OptionsView(
                "Iniciar jogos MD5"
            );
            options.OnOptionClick += s =>
            {
                if (s == "Iniciar jogos MD5")
                {
                    var oponent = tournament.SimulatePlayOffRound();
                    if (oponent == null)
                    {
                        while (tournament.PlayOffs.Champion == null && oponent == null)
                            oponent = tournament.SimulatePlayOffRound();
                        
                        drawPlayOffs(bmp, g);
                    }
                    else PlayMatch(oponent);
                }
            };
        }
        else
        {
            options = new OptionsView(
                "Sair"
            );
            options.OnOptionClick += s =>
            {
                if (s == "Sair")
                {
                    Game.Current.EndSeason();
                    Exit();
                }
            };
        }

        g.DrawString(play.LoserBracketA?.Organization?.Name, font, 
            play.LoserBracketSecondPhaseB == play.LoserBracketA ? Brushes.Green : Brushes.White, 
            new RectangleF(0.13f * wid, 0.78f * hei, 200, 40), format);

        g.DrawString(play.LoserBracketB?.Organization?.Name, font,
            play.LoserBracketSecondPhaseB == play.LoserBracketB ? Brushes.Green : Brushes.White, 
            new RectangleF(0.13f * wid, 0.84f * hei, 200, 40), format);

        g.DrawString(play.LoserBracketSecondPhaseA?.Organization?.Name, font,
            play.LoserBracketThirdPhaseB == play.LoserBracketSecondPhaseA ? Brushes.Green : Brushes.White, 
            new RectangleF(0.28f * wid, 0.78f * hei, 200, 40), format);

        g.DrawString(play.LoserBracketSecondPhaseB?.Organization?.Name, font,
            play.LoserBracketThirdPhaseB == play.LoserBracketSecondPhaseB ? Brushes.Green : Brushes.White, 
            new RectangleF(0.28f * wid, 0.84f * hei, 200, 40), format);

        g.DrawString(play.WinnerBracketA?.Organization?.Name, font,
            play.WinnerBracketFinalA == play.WinnerBracketA ? Brushes.Green : Brushes.White, 
            new RectangleF(0.28f * wid, 0.39f * hei, 200, 40), format);

        g.DrawString(play.WinnerBracketB?.Organization?.Name, font, 
            play.WinnerBracketFinalA == play.WinnerBracketB ? Brushes.Green : Brushes.White,
            new RectangleF(0.28f * wid, 0.45f * hei, 200, 40), format);

        g.DrawString(play.WinnerBracketC?.Organization?.Name, font, 
            play.WinnerBracketFinalB == play.WinnerBracketC ? Brushes.Green : Brushes.White,
            new RectangleF(0.28f * wid, 0.52f * hei, 200, 40), format);

        g.DrawString(play.WinnerBracketD?.Organization?.Name, font,
            play.WinnerBracketFinalB == play.WinnerBracketD ? Brushes.Green : Brushes.White,
            new RectangleF(0.28f * wid, 0.58f * hei, 200, 40), format);

        g.DrawString(play.WinnerBracketFinalA?.Organization?.Name, font, 
            play.FinalA == play.WinnerBracketFinalA ? Brushes.Green : Brushes.White, 
            new RectangleF(0.58f * wid, 0.46f * hei, 200, 40), format);

        g.DrawString(play.WinnerBracketFinalB?.Organization?.Name, font, 
            play.FinalA == play.WinnerBracketFinalB ? Brushes.Green : Brushes.White,
            new RectangleF(0.58f * wid, 0.52f * hei, 200, 40), format);

        g.DrawString(play.LoserBracketThirdPhaseA?.Organization?.Name, font, 
            play.LoserBracketFinalB == play.LoserBracketThirdPhaseA ? Brushes.Green : Brushes.White, 
            new RectangleF(0.43f * wid, 0.78f * hei, 200, 40), format);

        g.DrawString(play.LoserBracketThirdPhaseB?.Organization?.Name, font, 
            play.LoserBracketFinalB == play.LoserBracketThirdPhaseB ? Brushes.Green : Brushes.White, 
            new RectangleF(0.43f * wid, 0.84f * hei, 200, 40), format);         

        g.DrawString(play.LoserBracketFinalA?.Organization?.Name, font, 
            play.FinalB == play.LoserBracketFinalA ? Brushes.Green : Brushes.White, 
            new RectangleF(0.58f * wid, 0.67f * hei, 200, 40), format);

        g.DrawString(play.LoserBracketFinalB?.Organization?.Name, font,
            play.FinalB == play.LoserBracketFinalB ? Brushes.Green : Brushes.White, 
            new RectangleF(0.58f * wid, 0.73f * hei, 200, 40), format);

        g.DrawString(play.FinalA?.Organization?.Name, font, 
            play.Champion == play.FinalA ? Brushes.Green : Brushes.White, 
            new RectangleF(0.77f * wid, 0.56f * hei, 200, 40), format);

        g.DrawString(play.FinalB?.Organization?.Name, font, 
            play.Champion == play.FinalB ? Brushes.Green : Brushes.White, 
            new RectangleF(0.77f * wid, 0.62f * hei, 200, 40), format);
    }

    public event Action<Team> PlayNext;
    public event Action<Team> PlayMatch;
    public event Action Exit;
}