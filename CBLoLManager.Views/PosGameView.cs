using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace CBLoLManager.Views;

using Model;
using GameRule;
using Animations;

public class PosGameView : BaseView
{
    private DraftResult draft;
    private bool windResult;
    private GameSimulationSystem sys;

    private Image teamA;
    private Image teamB;
    private Image[] champsA = new Image[5];
    private Image[] champsB = new Image[5];
    private Image mvpPhoto;
    private Player mvp;

    private DateTime moment;

    public PosGameView(
        DraftResult draft,
        bool windResult,
        GameSimulationSystem sys
    )
    {
        this.draft = draft;
        this.windResult = windResult;
        this.sys = sys;
        this.moment = DateTime.Now;
    }
    
    protected override void draw(Bitmap bmp, Graphics g)
    {
        int wid = bmp.Width;
        int hei = bmp.Height;
        float size = wid * 0.3f;
        float mvpHei = hei - 160 - size;
        var mvpWid = mvpPhoto.Width * mvpHei / mvpPhoto.Height;

        var prop = (DateTime.Now - moment).TotalSeconds / 5;
        if (windResult)
        {
            g.DrawImage(teamA, new RectangleF(
                20, 20, size, size
            ));
            
            var img = teamB.ToGrayScale(prop);
            if (img != null)
            {
                g.DrawImage(img, new RectangleF(
                    wid - size - 20, 20, size, size
                ));
            }
        }
        else
        {
            g.DrawImage(teamB, new RectangleF(
                bmp.Width - size - 20, 20, size, size
            ));
            
            var img = teamA.ToGrayScale(prop);
            if (img != null)
            {
                g.DrawImage(img, new Rectangle(
                    20, 20, 400, 400
                ));
            }
        }

        float graphHei = hei - size - 40 - 20;
        float graphWid = wid - (mvpWid + 40) - 20;
        float graphX = mvpWid + 40;
        float graphY = size + 40;
        if (prop > 1f)
            prop = 1f;
        
        g.FillRectangle(Brushes.White, new RectangleF(
            graphX, graphY, graphWid, graphHei));
        
        var min = sys.GoldDiff.Min();
        var max = sys.GoldDiff.Max();
        var vpp = graphHei / (max - min);
        var mid = vpp * max;
        var count = sys.GoldDiff.Count();
        var dx = graphWid / count;

        for (int i = 6; i < count; i += 6)
        {
            g.DrawLine(Pens.LightGray,
                graphX + dx * i, graphY, graphX + dx * i, graphY + graphHei);
        }
        
        for (float i = min; i < max; i++)
        {
            var _y = graphY + (max - i) * vpp;
            g.DrawLine(Pens.LightGray,
                graphX, _y, 
                graphX + graphWid, _y);
        }

        var x = graphX;
        int state = 0;
        List<PointF> pts = new List<PointF>();
        pts.Add(new PointF(0, graphY + mid));
        Color color;
        Brush brush;
        foreach (var diff in sys.GoldDiff
            .Take((int)(count * prop)))
        {
            int newState = diff == 0 ? 0 :
                (diff < 0 ? -1 : 1);
            
            if (state != newState)
            {
                pts.Add(new PointF(x, graphY + mid));
                color = state switch
                {
                    -1 => Color.Red,
                    1 => Color.Blue,
                    _ => Color.Gray
                };
                brush = new SolidBrush(color);
                g.FillPolygon(brush, pts.ToArray());
                
                pts.Clear();
                pts.Add(new PointF(x, graphY + mid));
            }

            var y = graphY + (max - diff) * vpp;
            pts.Add(new PointF(x, y));
            x += dx;
            state = newState;
        }
        var last = sys.GoldDiff.Take((int)(count * prop)).Last();
        var lasty = graphY + (max - last) * vpp;
        pts.Add(new PointF(x, lasty));
        pts.Add(new PointF(x, graphY + mid));
        color = state switch
        {
            -1 => Color.Red,
            1 => Color.Blue,
            _ => Color.Gray
        };
        brush = new SolidBrush(color);
        g.FillPolygon(brush, pts.ToArray());
        g.DrawLine(Pens.Black, new PointF(graphX, graphY + mid),
            new PointF(graphX + (float)prop * graphWid, graphY + mid)); 
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        int wid = bmp.Width;
        int hei = bmp.Height;
        float unity = wid * 0.1f;
        float size = 3 * unity;

        Font font = new Font(FontFamily.GenericSerif, 20f);
        StringFormat format = new StringFormat();
        format.LineAlignment = StringAlignment.Center;
        format.Alignment = StringAlignment.Center;

        g.Clear(Color.FromArgb(0, 0, 40));

        teamA = Bitmap.FromFile("Img/" + draft.TeamA.Organization.Photo);
        teamB = Bitmap.FromFile("Img/" + draft.TeamB.Organization.Photo);

        for (int i = 0; i < 5; i++)
            champsA[i] = Bitmap.FromFile("Img/" + draft.TeamADraft[i].Photo);

        for (int i = 0; i < 5; i++)
            champsB[i] = Bitmap.FromFile("Img/" + draft.TeamBDraft[i].Photo);

        var possibleMvps = sys.AWin ? draft.TeamA.GetAll() : draft.TeamB.GetAll();
        var possibleChamps = sys.AWin ? draft.TeamADraft : draft.TeamBDraft;
        var mpvData = possibleMvps
            .Select(p =>
            {
                var frag = sys.GetFrag(p).Split('/');
                int k = int.Parse(frag[0]);
                int d = int.Parse(frag[1]);
                int a = int.Parse(frag[2]);
                float kda = (k + a) / (float)(d + 1);
                float gold = sys.GetGold(p);
                float param = kda / gold;
                return new {
                    player = p,
                    param = param
                };
            });
        var totalParam = mpvData.Sum(x => x.param);
        var sel = Random.Shared.NextSingle() * totalParam;

        foreach (var p in mpvData)
        {
            sel -= p.param;
            if (sel <= 0)
            {
                this.mvp = p.player;
                this.mvpPhoto = Bitmap.FromFile("Img/" + p.player.Photo);
                break;
            }
        }
        
        float mvpHei = hei - 160 - size;
        var mvpWid = mvpPhoto.Width * mvpHei / mvpPhoto.Height;
        g.DrawImage(mvpPhoto, new RectangleF(
            20, size + 40, mvpWid, mvpHei
        ));

        var champ = possibleChamps.FirstOrDefault(c => c.Role == mvp.Role);
        g.DrawString($"{mvp.Nickname} ({champ.Name})", font,
            Brushes.White, new RectangleF(20, hei - 120, mvpWid, 40), format);
        g.DrawString($"{sys.GetFrag(mvp)} - {sys.GetGold(mvp):#0.0}k", font,
            Brushes.White, new RectangleF(20, hei - 80, mvpWid, 40), format);
        g.DrawString(mvp.Role.ToString(), font,
            Brushes.White, new RectangleF(20, hei - 40, mvpWid, 40), format);

        var teamAInfo = 
            $"{sys.TeamAKills}\n{sys.TeamAGold:#0.0}k\n{sys.TeamATowers} esutrturas destruídas";
        var teamBInfo = 
            $"{sys.TeamBKills}\n{sys.TeamBGold:#0.0}k\n{sys.TeamBTowers} esutrturas destruídas";

        g.DrawString(teamAInfo, font, Brushes.White, 
            new RectangleF(20 + size, 20, 2 * unity - 20, size - 40), format);
        g.DrawString(teamBInfo, font, Brushes.White, 
            new RectangleF(wid / 2, 20, 2 * unity - 20, size - 40), format);

    }

    public override void MouseMove(PointF cursor, bool down)
    {
        if (Exit != null)
            Exit();
    }

    public event Action Exit;
}