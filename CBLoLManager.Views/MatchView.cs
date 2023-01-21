using System;
using System.Drawing;

namespace CBLoLManager.Views;

using Model;
using GameRule;

public class MatchView : BaseView
{
    GameSimulationSystem sys;
    DraftResult draft = null;
    Image game = null;
    Image[] champThumbs = new Image[10];
    Image teamAThumb = null;
    Image teamBThumb = null;

    int time = 0;

    public MatchView(DraftResult draft)
    {
        this.draft = draft;
        this.sys = new GameSimulationSystem(draft);

        sys.NextStep();
        sys.NextStep();
        sys.NextStep();
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        if (time != sys.Time)
        {
            time += (int)MathF.Ceiling((sys.Time - time) / 10f);
        }

        int wid = bmp.Width;
        int hei = bmp.Height;

        g.DrawImage(game, new Rectangle(0, 0, bmp.Width, bmp.Height),
            new RectangleF(0, 0, game.Width, game.Height),
            GraphicsUnit.Pixel);
        
        Font font = new Font(FontFamily.GenericMonospace, 16f);
        Font font2 = new Font(FontFamily.GenericMonospace, 12f);
        var format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;
        
        g.DrawImage(teamAThumb, new RectangleF(0.28f * wid, 5, wid * 0.035f, wid * 0.035f),
            new RectangleF(0, 0, 60, 60),
            GraphicsUnit.Pixel);
        g.DrawString(draft.TeamA.Organization.Sigla, font,
            Brushes.White, new RectangleF(0.22f * wid, 5, wid * 0.06f, hei * 0.025f),
            format);
        g.DrawString(sys.TeamATowers.ToString(), font,
            Brushes.White, new RectangleF(0.33f * wid, 5, wid * 0.06f, hei * 0.05f),
            format);
        g.DrawString($"{sys.TeamAGold:#0.0}k", font,
            Brushes.White, new RectangleF(0.395f * wid, 5, wid * 0.06f, hei * 0.05f),
            format);
        g.DrawString(sys.TeamAKills.ToString(), font,
            Brushes.White, new RectangleF(0.45f * wid, 5, wid * 0.06f, hei * 0.05f),
            format);
        
        g.DrawImage(teamBThumb, new RectangleF(0.70f * wid, 5, wid * 0.035f, wid * 0.035f),
            new RectangleF(0, 0, 60, 60),
            GraphicsUnit.Pixel);
        g.DrawString(draft.TeamB.Organization.Sigla, font,
            Brushes.White, new RectangleF(0.73f * wid, 5, wid * 0.06f, hei * 0.025f),
            format);
        g.DrawString(sys.TeamBTowers.ToString(), font,
            Brushes.White, new RectangleF(0.64f * wid, 5, wid * 0.06f, hei * 0.05f),
            format);
        g.DrawString($"{sys.TeamBGold:#0.0}k", font,
            Brushes.White, new RectangleF(0.57f * wid, 5, wid * 0.06f, hei * 0.05f),
            format);
        g.DrawString(sys.TeamBKills.ToString(), font,
            Brushes.White, new RectangleF(0.49f * wid, 5, wid * 0.06f, hei * 0.05f),
            format);

        g.DrawString($"{time / 60:00}:{time % 60:00}", font2,
            Brushes.White, new RectangleF(0.47f * wid, hei * 0.05f, wid * 0.06f, hei * 0.05f),
            format);
        
        for (int i = 0; i < 5; i++)
        {
            g.DrawImage(champThumbs[i], 
                new RectangleF(0, .14f * hei + i * 0.095f * hei, wid * 0.045f, wid * 0.045f),
                new RectangleF(0, 0, 60, 60),
                GraphicsUnit.Pixel);

            g.DrawImage(champThumbs[i + 5],
                new RectangleF(wid * 0.955f, .14f * hei + i * 0.095f * hei, wid * 0.045f, wid * 0.045f),
                new RectangleF(0, 0, 60, 60),
                GraphicsUnit.Pixel);
        }
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);

        game = Bitmap.FromFile("Img/game.png");

        for (int i = 0; i < 5; i++)
            champThumbs[i] = Bitmap.FromFile(
                "Img/" + draft.TeamADraft[i].Photo)
                    .GetThumbnailImage(60, 60, null, IntPtr.Zero);

        for (int i = 0; i < 5; i++)
            champThumbs[i + 5] = Bitmap.FromFile(
                "Img/" + draft.TeamBDraft[i].Photo)
                    .GetThumbnailImage(60, 60, null, IntPtr.Zero);
        
        teamAThumb = Bitmap.FromFile(
            "Img/w" + draft.TeamA.Organization.Photo)
            .GetThumbnailImage(60, 60, null, IntPtr.Zero);

        teamBThumb = Bitmap.FromFile(
            "Img/w" + draft.TeamB.Organization.Photo)
            .GetThumbnailImage(60, 60, null, IntPtr.Zero);
    }

    public event Action Exit;
}