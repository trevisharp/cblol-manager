using System;
using System.Drawing;

namespace CBLoLManager.Views;

using System.Collections.Generic;
using Model;

public class PlayerStatusView : BaseView
{
    private int frame = 0;
    private int oldLanePhase = 0;
    private int oldTeamFigth = 0;
    private int oldMechanicalSkill = 0;
    private int oldGameVision = 0;
    private int oldMentality = 0;
    private int oldLeadership = 0;

    public Player Player { get; set; }
    public float Size { get; set; }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        frame++;

        var position = this.Location ?? Point.Empty;
        float raddius = this.Size / 2;
        float cx = position.X + raddius,
              cy = position.Y + raddius;
        const float a = MathF.Tau / 6;

        var back1 = new SolidBrush(Color.FromArgb(30, 30, 45));
        var back2 = new SolidBrush(Color.FromArgb(60, 60, 75));

        for (int k = 0; k < 5; k++)
        {
            var brush = k % 2 == 0 ? back1 : back2;

            poly(brush, i => new PointF(
                    cx + (1f - .2f * k) * raddius * MathF.Sin(i * a),
                    cy - (1f - .2f * k) * raddius * MathF.Cos(i * a)
                ), 6, $"back{k}"
            );
        }

        Pen pen = new Pen(Color.Yellow, 3f);
        float moment = frame < 20 ? frame / 20f : 1f;
        List<PointF> pts = new List<PointF>();
        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        Font font = SystemFonts.CaptionFont;

        float ratio = (1f - moment) * (oldLanePhase / 100f) + 
            moment * (this.Player.LanePhase / 100f);
        pts.Add(new PointF(
            cx + ratio * raddius * MathF.Sin(0 * a),
            cy - ratio * raddius * MathF.Cos(0 * a)
        ));
        g.DrawString("Lan.Pha.", font, Brushes.White, new RectangleF(
            cx + 1.2f * raddius * MathF.Sin(0 * a) - 40f,
            cy - 1.2f * raddius * MathF.Cos(0 * a),
            80f, 12f
        ), format);

        ratio = (1f - moment) * (oldMechanicalSkill / 100f) + 
            moment * (this.Player.MechanicSkill / 100f);
        pts.Add(new PointF(
            cx + ratio * raddius * MathF.Sin(1 * a),
            cy - ratio * raddius * MathF.Cos(1 * a)
        ));
        g.DrawString("Mec.Ski.", font, Brushes.White, new RectangleF(
            cx + 1.2f * raddius * MathF.Sin(1 * a) - 30f,
            cy - 1.2f * raddius * MathF.Cos(1 * a),
            80f, 12f
        ), format);

        ratio = (1f - moment) * (oldLeadership / 100f) + 
            moment * (this.Player.Leadership / 100f);
        pts.Add(new PointF(
            cx + ratio * raddius * MathF.Sin(2 * a),
            cy - ratio * raddius * MathF.Cos(2 * a)
        ));
        g.DrawString("Leaders.", font, Brushes.White, new RectangleF(
            cx + 1.2f * raddius * MathF.Sin(2 * a) - 30f,
            cy - 1.2f * raddius * MathF.Cos(2 * a),
            80f, 12f
        ), format);

        ratio = (1f - moment) * (oldMentality / 100f) + 
            moment * (this.Player.Mentality / 100f);
        pts.Add(new PointF(
            cx + ratio * raddius * MathF.Sin(3 * a),
            cy - ratio * raddius * MathF.Cos(3 * a)
        ));
        g.DrawString("Mentali.", font, Brushes.White, new RectangleF(
            cx + 1.2f * raddius * MathF.Sin(3 * a) - 40f,
            cy - 1.2f * raddius * MathF.Cos(3 * a) - 10f,
            80f, 12f
        ), format);

        ratio = (1f - moment) * (oldGameVision / 100f) + 
            moment * (this.Player.GameVision / 100f);
        pts.Add(new PointF(
            cx + ratio * raddius * MathF.Sin(4 * a),
            cy - ratio * raddius * MathF.Cos(4 * a)
        ));
        g.DrawString("Gam.Vis.", font, Brushes.White, new RectangleF(
            cx + 1.2f * raddius * MathF.Sin(4 * a) - 50f,
            cy - 1.2f * raddius * MathF.Cos(4 * a),
            80f, 12f
        ), format);

        ratio = (1f - moment) * (oldTeamFigth / 100f) + 
            moment * (this.Player.TeamFigth / 100f);
        pts.Add(new PointF(
            cx + ratio * raddius * MathF.Sin(5 * a),
            cy - ratio * raddius * MathF.Cos(5 * a)
        ));
        g.DrawString("Tea.Fig.", font, Brushes.White, new RectangleF(
            cx + 1.2f * raddius * MathF.Sin(5 * a) - 50f,
            cy - 1.2f * raddius * MathF.Cos(5 * a),
            80f, 12f
        ), format);
        g.DrawPolygon(pen, pts.ToArray());

        poly(Pens.White, i => new PointF(
                cx + raddius * MathF.Sin(i * a),
                cy - raddius * MathF.Cos(i * a)
            ), 6, "cont"
        );
    }

    public override void Reset()
    {
        frame = 0;
        oldGameVision = this.Player.GameVision;
        oldLeadership = this.Player.Leadership;
        oldMechanicalSkill = this.Player.MechanicSkill;
        oldMentality = this.Player.Mentality;
        oldTeamFigth = this.Player.TeamFigth;
        oldLanePhase = this.Player.LanePhase;
        base.Reset();
    }
}