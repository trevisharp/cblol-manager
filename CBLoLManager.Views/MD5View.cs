using System;
using System.Drawing;

namespace CBLoLManager.Views;

using GameRule;
using Model;

public class MD5View : BaseView
{
    public Team TeamA { get; set; }
    public Team TeamB { get; set; }
    public int ScoreA { get; set; }
    public int ScoreB { get; set; }
    
    private OptionsView options = null;
    private PointF cursor = PointF.Empty;
    private bool down = false;
    private Image aImg = null;
    private Image bImg = null;

    protected override void draw(Bitmap bmp, Graphics g)
    {
        options.MouseMove(cursor, down);
        options.Draw(bmp, g);
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.DarkCyan);

        var font = new Font(FontFamily.GenericMonospace, 320f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;
        if (ScoreA == 3 || ScoreB == 3)
        {
            if (ScoreA == 3)
            {
                Game.Current
                    .Tournament
                    .SetWinner(TeamA, TeamB);
            }
            else
            {
                Game.Current
                    .Tournament
                    .SetWinner(TeamB, TeamA);
            }

            options = new OptionsView(
                "Sair"
            );
            options.OnOptionClick += s =>
            {
                if (s == "Sair")
                {
                    Exit();
                }
            };
        }
        else
        {
            options = new OptionsView(
                "Ir para próximo Jogo"
            );
            options.OnOptionClick += s =>
            {
                if (s == "Ir para próximo Jogo")
                {
                    MakeGame(TeamB);
                }
            };
        }

        float wid = bmp.Width;
        float hei = bmp.Height;

        aImg = Image.FromFile("Img/" + TeamA.Organization.Photo);
        bImg = Image.FromFile("Img/" + TeamB.Organization.Photo);

        g.DrawImage(aImg, new RectangleF(
            25, 25, hei - 100, hei - 100
        ));
        g.DrawImage(bImg, new RectangleF(
            wid - (hei - 100), 25, hei - 100, hei - 100
        ));

        g.DrawString(ScoreA.ToString(), font, Brushes.White,
            new RectangleF(0.5f * wid - 0.4f * hei, 0.1f * hei, 0.4f * hei, 0.8f * hei),
            format);
            
        g.DrawString(ScoreB.ToString(), font, Brushes.White,
            new RectangleF(0.5f * wid, 0.1f * hei, 0.4f * hei, 0.8f * hei),
            format);
    }

    public event Action<Team> MakeGame;
    public event Action Exit;
}