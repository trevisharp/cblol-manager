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
    IEnumerator<Champion> picks = null;
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
    DateTime timer = DateTime.Now;
    protected override void draw(Bitmap bmp, Graphics g)
    {   
        float hei = bmp.Width / (float)draft.Width * draft.Height;

        if (frame > delay)
        {
            switch(pickCount)
            {
                case 0:
                case 3:
                case 4:
                case 7:
                case 8:
                    bluePick();
                    break;
                
                default:
                    redPick();
                    break;
            }
            pickCount++;
            if (pickCount == 10)
                step++;
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

            var timeSub = DateTime.Now - timer;
            var crrTime = 30 - (int)timeSub.TotalSeconds;
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

        void bluePick()
        {
            picks.MoveNext();
            var pick = picks.Current;
            this.champs[(int)pick.Role] = pick;
            delay += 40 + Random.Shared.Next(80);
            timer = DateTime.Now;
        }

        void redPick()
        {
            picks.MoveNext();
            var pick = picks.Current;
            this.champs[(int)pick.Role + 5] = pick;
            delay += 40 + Random.Shared.Next(80);
            timer = DateTime.Now;
        }
    }

    public override async void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
        Audio.PicksBans();
        Audio.Instalock();
    }
}
