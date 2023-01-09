using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace CBLoLManager.Views;

using Util;
using Model;
using GameRule;

public class MatchView : BaseView
{
    int frame = 0;
    int step = 0;
    Team a = null;
    Team b = null;
    Bitmap draft = null;
    Bitmap arena = null;
    DraftSystem sys = null;
    Champion[] champs = null;
    IEnumerator<Pick> picks = null;
    Image[] champsThumbs = null;

    public MatchView(Team A, Team B)
    {
        this.a = A;
        this.b = B;

        this.draft = Bitmap.FromFile("Img/draft.png") as Bitmap;

        this.champs = new Champion[10];
        this.champsThumbs = new Image[10];

        if (A == null || B == null)
            return;

        this.sys = new DraftSystem
        {
            Blue = A,
            Red = B
        };
        this.picks = this.sys.Simulate().GetEnumerator();
    }

    int delay = 40;
    int pickCount = 0;
    Pick currentPick = null;
    Image optA = null;
    Image optB = null;
    bool waitingPlayer = false;
    DateTime timer = DateTime.Now;
    protected override void draw(Bitmap bmp, Graphics g)
    {   
        float hei = bmp.Width / (float)draft.Width * draft.Height;

        var timeSub = DateTime.Now - timer;
        var crrTime = 30 - (int)timeSub.TotalSeconds;
        switch(pickCount)
        {
            case 0:
            case 3:
            case 4:
            case 7:
            case 8:
                if (!waitingPlayer)
                    chooseOptions();
                if (crrTime <= 0 || currentPick?.Selected != null)
                {
                    bluePick();
                    pickCount++;
                    if (pickCount == 10)
                        step++;
                }
            break;
            
            default:
                if (frame > delay)
                {
                    redPick();
                    pickCount++;
                    if (pickCount == 10)
                        step++;
                }
                break;
        }
        frame++;
        
        var font = new Font(FontFamily.GenericMonospace, 20f);
        var font2 = new Font(FontFamily.GenericMonospace, 15f);
        var font3 = new Font(FontFamily.GenericMonospace, 40f);
        StringFormat format = new StringFormat();
        format.LineAlignment = StringAlignment.Center;

        // Draft
        if (step < 2)
        {
            if (arena == null)
            {
                this.arena = Bitmap.FromFile("Animations/arena.gif") as Bitmap;
                ImageAnimator.Animate(this.arena, delegate { });
            }
            
            ImageAnimator.UpdateFrames(this.arena);
            g.DrawImage(this.arena, new Rectangle(0, 0, bmp.Width, bmp.Height));
            g.DrawImage(draft,
                new RectangleF(0, bmp.Height - hei, bmp.Width, hei),
                new RectangleF(0, 0, draft.Width, draft.Height),
                GraphicsUnit.Pixel);

            format.Alignment = StringAlignment.Far;
            g.DrawString(a?.Organization?.Name ?? "?", font, Brushes.White,
                new RectangleF(bmp.Width * .26f, bmp.Height - hei + 10f, 350f, 30f), format);

            drawChamp(0);
            g.DrawString(a?.TopLaner?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * -.09f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
                
            drawChamp(1);
            g.DrawString(a?.Jungler?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .0f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            drawChamp(2);
            g.DrawString(a?.MidLaner?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .09f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            drawChamp(3);
            g.DrawString(a?.AdCarry?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .18f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            drawChamp(4);
            g.DrawString(a?.Support?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .27f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);

            if (step == 0)
            {
                g.DrawString(crrTime.ToString(), font3, Brushes.White,
                    new RectangleF(bmp.Width * .42f, bmp.Height - hei * .9f + 10f, 200f, 200f), format);
            }
            else if (crrTime < 25)
            {
                step++;
                Audio.Stop();
            }
  
            format.Alignment = StringAlignment.Near;
            g.DrawString(b?.Organization?.Name ?? "?", font, Brushes.White,
                new RectangleF(bmp.Width * .56f, bmp.Height - hei + 10f, 350f, 30f), format);

            drawChamp(5);
            g.DrawString(b?.TopLaner?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .55f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
                
            drawChamp(6);
            g.DrawString(b?.Jungler?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .64f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            drawChamp(7);
            g.DrawString(b?.MidLaner?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .73f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            drawChamp(8);
            g.DrawString(b?.AdCarry?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .82f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            drawChamp(9);
            g.DrawString(b?.Support?.Nickname ?? "?", font2, Brushes.White, new RectangleF(
                bmp.Width * .91f, 
                bmp.Height - hei + hei * .6f,
                350f, 30f), format);
            
            if (waitingPlayer)
            {
                if (optA == null)
                {
                    optA = Bitmap.FromFile("Img/" + currentPick.OptionA.Photo);
                    optA = optA.GetThumbnailImage(optA.Width / 2, optA.Height / 2, null, IntPtr.Zero) as Bitmap;

                }
                if (optB == null)
                {
                    optB = Bitmap.FromFile("Img/" + currentPick.OptionB.Photo);
                    optB = optB.GetThumbnailImage(optB.Width / 2, optB.Height / 2, null, IntPtr.Zero) as Bitmap;
                }

                float optHei = bmp.Height - hei - 40;
                float ra = optHei / optA.Height;
                float rb = optHei / optB.Height;
                float optWidA = ra * optA.Width;
                float optWidB = rb * optB.Width;
                float margin = (bmp.Width - optWidA - optWidB) / 3;
                var optARect = new RectangleF(margin, 20, optHei, optWidA);
                var optBRect = new RectangleF(2 * margin + optWidA, 20, optHei, optWidB);

                g.DrawImage(optA,
                    optARect,
                    new RectangleF(0, 0, optA.Width, optA.Height),
                    GraphicsUnit.Pixel);
                g.DrawImage(optB, 
                    optBRect,
                    new RectangleF(0, 0, optB.Width, optB.Height),
                    GraphicsUnit.Pixel);
                
                if (optARect.Contains(cursor))
                {
                    g.DrawRectangle(Pens.Yellow, Rectangle.Round(optARect));

                    if (down)
                        currentPick.MakeAPick(true);
                }
                else if (optBRect.Contains(cursor))
                {
                    g.DrawRectangle(Pens.Yellow, Rectangle.Round(optBRect));

                    if (down)
                        currentPick.MakeAPick(false);
                }
            }
        }
        
        void drawChamp(int i)
        {
            if (champs[i] != null)
            {
                if (champsThumbs[i] == null)
                {
                    var bmp = Bitmap.FromFile("Img/" + champs[i].Photo);
                    champsThumbs[i] = bmp.GetThumbnailImage(
                        5 * (int)(bmp.Width * .09f), 5 * (int)(bmp.Width * .13f),
                        null, IntPtr.Zero) as Bitmap;
                }
                
                g.DrawImage(champsThumbs[i], 
                    new RectangleF(
                        5 + i * bmp.Width * 0.092f + (i > 4 ? bmp.Width * 0.078f : 0), 
                        bmp.Height - hei * (1f - 0.12f), 
                        bmp.Width * .09f, bmp.Width * .13f),
                    new RectangleF(0, 0, champsThumbs[i].Width, champsThumbs[i].Height),
                    GraphicsUnit.Pixel);
            }
        }

        void chooseOptions()
        {
            picks.MoveNext();
            currentPick = picks.Current;
            waitingPlayer = true;
        }

        void bluePick()
        {
            if (currentPick.Selected == null)
                currentPick.MakeAPick(true);

            var champ = currentPick.Selected;
            this.champs[(int)champ.Role] = champ;

            delay = frame + 40 + Random.Shared.Next(80);
            timer = DateTime.Now;

            waitingPlayer = false;
            optA = null;
            optB = null;
            currentPick = null;
        }

        void redPick()
        {
            picks.MoveNext();
            var pick = picks.Current;

            pick.MakeAPick(true);
            var champ = pick.Selected;

            this.champs[(int)champ.Role + 5] = champ;

            delay = frame + 40 + Random.Shared.Next(80);
            timer = DateTime.Now;
        }
    }

    public override async void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
        Audio.Clear();
        Audio.Stop();
        await Audio.PicksBans();
        await Audio.Instalock();
    }

    PointF cursor;
    bool down;
    public override void MouseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
    }
}
