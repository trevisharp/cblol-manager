using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace CBLoLManager.Views;

using Model;
using GameRule;

public class MatchView : BaseView
{
    GameSimulationSystem sys;
    DraftResult draft = null;
    Image game = null;
    Image[] champThumbs = new Image[10];
    Player[] players = new Player[10];
    Image teamAThumb = null;
    Image teamBThumb = null;
    Queue<string> messages = new Queue<string>();
    bool paused = false;

    Image blueTower;
    Image redTower;
    Image blueInibitor;
    Image redInibitor;
    Image blueNexus;
    Image redNexus;

    int time = 0;
    int messageAnimation = 0;
    DateTime updateControl;

    public MatchView(DraftResult draft)
    {
        this.draft = draft;
        this.sys = new GameSimulationSystem(draft);
        updateControl = DateTime.Now;
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        if (paused)
        {
            g.FillRectangle(Brushes.White, 
                new Rectangle(50, 50, 50, 200));
            g.FillRectangle(Brushes.White, 
                new Rectangle(125, 50, 50, 200));
            return;
        }

        var controlTime = DateTime.Now - updateControl;
        if (controlTime.TotalSeconds > 0.25)
        {
            updateControl = DateTime.Now;
            if (!sys.GameEnded)
                sys.NextStep();
        }
        
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

        var format2 = new StringFormat();
        format2.Alignment = StringAlignment.Near;
        format2.LineAlignment = StringAlignment.Center;

        var format3 = new StringFormat();
        format3.Alignment = StringAlignment.Far;
        format3.LineAlignment = StringAlignment.Center;

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

        messageAnimations(g, wid, hei, font, format);
        drawTowers(g, wid, hei);

        for (int i = 0; i < 5; i++)
        {
            g.DrawImage(champThumbs[i], 
                new RectangleF(0, .14f * hei + i * 0.095f * hei, wid * 0.045f, wid * 0.045f),
                new RectangleF(0, 0, 60, 60),
                GraphicsUnit.Pixel);
            g.FillRectangle(Brushes.Black, 
                new RectangleF(0, 
                    .14f * hei + 0.04f * wid + i * 0.095f * hei,
                    wid * 0.045f, wid * 0.005f));
            g.FillRectangle(Brushes.Green, 
                new RectangleF(0, 
                    .14f * hei + 0.04f * wid + i * 0.095f * hei,
                    (wid * 0.045f) * sys.GetLife(players[i]), wid * 0.005f));

            g.DrawImage(champThumbs[i + 5],
                new RectangleF(wid * 0.955f, .14f * hei + i * 0.095f * hei, wid * 0.045f, wid * 0.045f),
                new RectangleF(0, 0, 60, 60),
                GraphicsUnit.Pixel);
            g.FillRectangle(Brushes.Black, 
                new RectangleF(wid * 0.955f,
                    .14f * hei + 0.04f * wid + i * 0.095f * hei, 
                    wid * 0.045f, wid * 0.005f));
            g.FillRectangle(Brushes.Green, 
                new RectangleF(wid * 0.955f, 
                    .14f * hei + 0.04f * wid + i * 0.095f * hei,
                    (wid * 0.045f) * sys.GetLife(players[i + 5]), wid * 0.005f));
        }
        
        for (int i = 0; i < 5; i++)
        {
            g.DrawImage(champThumbs[i], 
                new RectangleF(
                    0.47f * wid,
                    .8f * hei + i * 0.04f * hei, 
                    wid * 0.02f, 
                    wid * 0.02f
                ), new RectangleF(0, 0, 60, 60),
                GraphicsUnit.Pixel);
            g.DrawString(sys.GetFrag(players[i]),
                SystemFonts.CaptionFont, Brushes.White,
                new RectangleF(
                    0.36f * wid,
                    .8f * hei + i * 0.04f * hei, 
                    wid * 0.1f, 
                    wid * 0.02f), format3);
            g.DrawString($"{sys.GetGold(players[i]):0.00} k",
                SystemFonts.CaptionFont, Brushes.White,
                new RectangleF(
                    0.245f * wid,
                    .8f * hei + i * 0.04f * hei, 
                    wid * 0.1f, 
                    wid * 0.02f), format3);

            g.DrawImage(champThumbs[i + 5],
                new RectangleF(
                    wid * 0.51f, 
                    .8f * hei + i * 0.04f * hei, 
                    wid * 0.02f, 
                    wid * 0.02f
                ), new RectangleF(0, 0, 60, 60),
                GraphicsUnit.Pixel);
            g.DrawString(sys.GetFrag(players[i + 5]),
                SystemFonts.CaptionFont, Brushes.White,
                new RectangleF(
                    0.54f * wid,
                    .8f * hei + i * 0.04f * hei, 
                    wid * 0.1f, 
                    wid * 0.02f), format2);
            g.DrawString($"{sys.GetGold(players[i + 5]):0.00} k",
                SystemFonts.CaptionFont, Brushes.White,
                new RectangleF(
                    0.66f * wid,
                    .8f * hei + i * 0.04f * hei, 
                    wid * 0.1f, 
                    wid * 0.02f), format2);
        }
    }

    private void messageAnimations(Graphics g,
        int wid, int hei, Font font, StringFormat format)
    {
        float m = 0f;
        const int N = 11;
        if (messageAnimation == 0 && messages.Count < N)
        {
            if (sys.MessageQueue.Count > 0)
                messages.Enqueue(
                    sys.MessageQueue.Dequeue()
                );
        }
        else if (messageAnimation == 0 && messages.Count == N)
            messageAnimation = 100;
        else if (messageAnimation > 0)
        {
            messageAnimation -= 25;
            m = -(1 - messageAnimation / 100f);
            if (messageAnimation == 0)
            {
                messages.Dequeue();
                messageAnimation = 0;
                m = 0;
            }
        }

        bool frist = true;
        foreach (var message in messages)
        {
            Brush brush = Brushes.White;
            if (messageAnimation > 0 && frist)
            {
                Color color = Color.FromArgb(
                    255 * messageAnimation / 100 , 255, 255, 255
                );
                brush = new SolidBrush(color);
            }
            g.DrawString(message, font, brush,
                new PointF(0.5f * wid, (.15f + m * 0.05f) * hei));
            m++;
            frist = false;
        }
    }

    private void drawTowers(Graphics g, int wid, int hei)
    {
        var towers = sys.TowersUpA.ToArray();

        if (towers[9])
            g.DrawImage(blueInibitor,
                new PointF(0.09f * wid, 0.52f * hei));

        if (towers[10])
            g.DrawImage(blueInibitor,
                new PointF(0.15f * wid, 0.54f * hei));

        if (towers[11])
            g.DrawImage(blueInibitor,
                new PointF(0.17f * wid, 0.66f * hei));

        if (towers[0])
            g.DrawImage(blueTower,
                new PointF(0.09f * wid, 0.18f * hei));

        if (towers[1])
            g.DrawImage(blueTower,
                new PointF(0.22f * wid, 0.40f * hei));

        if (towers[2])
            g.DrawImage(blueTower,
                new PointF(0.32f * wid, 0.65f * hei));
        
        if (towers[3])
            g.DrawImage(blueTower,
                new PointF(0.09f * wid, 0.35f * hei));

        if (towers[4])
            g.DrawImage(blueTower,
                new PointF(0.19f * wid, 0.46f * hei));

        if (towers[5])
            g.DrawImage(blueTower,
                new PointF(0.25f * wid, 0.65f * hei));
        
        if (towers[6])
            g.DrawImage(blueTower,
                new PointF(0.09f * wid, 0.50f * hei));

        if (towers[7])
            g.DrawImage(blueTower,
                new PointF(0.16f * wid, 0.52f * hei));

        if (towers[8])
            g.DrawImage(blueTower,
                new PointF(0.18f * wid, 0.65f * hei));

        if (towers[14])
            g.DrawImage(blueNexus,
                new PointF(0.09f * wid, 0.65f * hei));

        if (towers[12])
            g.DrawImage(blueTower,
                new PointF(0.10f * wid, 0.63f * hei));

        if (towers[13])
            g.DrawImage(blueTower,
                new PointF(0.11f * wid, 0.64f * hei));

        towers = sys.TowersUpB.ToArray();

        if (towers[9])
            g.DrawImage(redInibitor,
                new PointF(0.31f * wid, 0.12f * hei));

        if (towers[10])
            g.DrawImage(redInibitor, 
                new PointF(0.33f * wid, 0.23f * hei));

        if (towers[11])
            g.DrawImage(redInibitor,
                new PointF(0.395f * wid, 0.27f * hei));

        if (towers[0])
            g.DrawImage(redTower,
                new PointF(0.16f * wid, 0.1f * hei));

        if (towers[1])
            g.DrawImage(redTower,
                new PointF(0.26f * wid, 0.32f * hei));

        if (towers[2])
            g.DrawImage(redTower,
                new PointF(0.4f * wid, 0.55f * hei));
        
        if (towers[3])
            g.DrawImage(redTower,
                new PointF(0.24f * wid, 0.1f * hei));

        if (towers[4])
            g.DrawImage(redTower,
                new PointF(0.30f * wid, 0.26f * hei));

        if (towers[5])
            g.DrawImage(redTower,
                new PointF(0.39f * wid, 0.37f * hei));
        
        if (towers[6])
            g.DrawImage(redTower,
                new PointF(0.30f * wid, 0.1f * hei));

        if (towers[7])
            g.DrawImage(redTower, 
                new PointF(0.32f * wid, 0.23f * hei));

        if (towers[8])
            g.DrawImage(redTower,
                new PointF(0.395f * wid, 0.28f * hei));

        if (towers[14])
            g.DrawImage(redNexus,
                new PointF(0.38f * wid, 0.13f * hei));

        if (towers[12])
            g.DrawImage(redTower,
                new PointF(0.38f * wid, 0.16f * hei));

        if (towers[13])
            g.DrawImage(redTower,
                new PointF(0.36f * wid, 0.14f * hei));
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

        int j = 0;
        foreach (var x in draft.TeamA.GetAll())
            players[j++] = x;
        foreach (var x in draft.TeamB.GetAll())
            players[j++] = x;
        
        blueTower = Bitmap.FromFile("Img/bluetower.png");
        redTower = Bitmap.FromFile("Img/redtower.png");
        blueInibitor = Bitmap.FromFile("Img/blueinibitor.png");
        redInibitor = Bitmap.FromFile("Img/redinibitor.png");
        blueNexus = Bitmap.FromFile("Img/bluenexus.png");
        redNexus = Bitmap.FromFile("Img/rednexus.png");

        blueTower = reduce(blueTower);
        redTower = reduce(redTower);
        blueInibitor = reduce(blueInibitor);
        redInibitor = reduce(redInibitor);
        blueNexus = reduce(blueNexus);
        redNexus = reduce(redNexus);

        Image reduce(Image img)
            => img.GetThumbnailImage(img.Width / 10, img.Height / 10, null, IntPtr.Zero);
    }

    bool isdown = false;
    public override void MouseMove(PointF cursor, bool down)
    {
        if (down && !isdown)
        {
            paused = !paused;
            isdown = true;
        }
        if (!down)
            isdown = false;
    }

    public event Action Exit;
}