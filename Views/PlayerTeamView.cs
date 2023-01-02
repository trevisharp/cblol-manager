using System.Drawing;

namespace CBLoLManager.Views;

using System.Collections.Generic;
using Model;

public class PlayerTeamView : BaseView
{
    private PlayerStatusView status = null;
    private float size;
    private int frame = 0;
    
    private Team team = null;
    private Player crrPlayer = null;
    private List<Player> players = null;
    private PointF cursor = PointF.Empty;
    private bool down = false;

    public PlayerTeamView(Team team, PointF point, float size)
    {
        this.status = new PlayerStatusView();
        this.team = team;
        this.players = team.GetAll();
        this.crrPlayer = players.Count > 0 ? players[0] : null;
        this.status.Player = this.crrPlayer;
        this.Location = point;
        this.size = size;
        this.status.Location = new PointF(point.X + size / 6 + 50f, point.Y + size / 6 / 6);
        this.status.Size = size / 6;
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        if (crrPlayer == null)
            return;
        
        frame++;
        if (!Location.HasValue)
            return;
        var p = this.Location.Value;

        g.FillRectangle(Brushes.Black, p.X, p.Y, size, size / 5 + 30f);

        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;
        
        g.DrawString(applyEffect($"Rota: {crrPlayer.Role}"), 
            font, Brushes.White, p.X + 3 * size / 6, p.Y + 10f);
        
        g.DrawString(applyEffect($"Fase de Rota: {crrPlayer.LanePhase}"), 
            font, getBrush(crrPlayer.LanePhase), p.X + 3 * size / 6, p.Y + 50f);
        
        g.DrawString(applyEffect($"Mecânica: {crrPlayer.MechanicSkill}"), 
            font, getBrush(crrPlayer.MechanicSkill), p.X + 3 * size / 6, p.Y + 70f);
        
        g.DrawString(applyEffect($"Team Figth: {crrPlayer.TeamFigth}"), 
            font, getBrush(crrPlayer.TeamFigth), p.X + 3 * size / 6, p.Y + 90f);
        
        g.DrawString(applyEffect($"Visão de Jogo: {crrPlayer.GameVision}"), 
            font, getBrush(crrPlayer.GameVision), p.X + 3 * size / 6, p.Y + 110f);
        
        g.DrawString(applyEffect($"Mentalidade: {crrPlayer.Mentality}"), 
            font, getBrush(crrPlayer.Mentality), p.X + 3 * size / 6, p.Y + 130f);
        
        g.DrawString(applyEffect($"Liderança: {crrPlayer.Leadership}"), 
            font, getBrush(crrPlayer.Leadership), p.X + 3 * size / 6, p.Y + 150f);
            
        int media = (crrPlayer.LanePhase + crrPlayer.MechanicSkill + crrPlayer.TeamFigth +
            crrPlayer.GameVision + crrPlayer.Mentality + crrPlayer.Leadership) / 6;
        g.DrawString(applyEffect($"Media: {media}"), 
            font, getBrush(media), p.X + 3 * size / 6, p.Y + 190f);

        img(p.X + 4 * size / 6, p.Y + 15f, size / 5, 
            () => Bitmap.FromFile("Img/" + crrPlayer.Photo) as Bitmap, "photo");

        status.Draw(bmp, g);

        g.DrawRectangle(Pens.White, p.X, p.Y, size, size / 5 + 30f);
        
        float x = p.X + 5f,
              y = p.Y + 5f;
        foreach (var pl in players)
        {
            var plRect = new Rectangle((int)x, (int)y, (int)size / 6, (int)size / 5 / 5);
            bool contains = plRect.Contains(new Point((int)cursor.X, (int)cursor.Y));
            if (contains)
            {
                rect(Brushes.White, () => plRect);
                rect(Pens.Black, () => plRect);
                g.DrawString(pl.Nickname, font, Brushes.Black, plRect, format);

                if (crrPlayer != pl)
                {
                    Reset();
                    crrPlayer = pl;
                    this.status.Player = this.crrPlayer;
                }
            }
            else
            {
                rect(Pens.White, () => plRect);
                g.DrawString(pl.Nickname, font, Brushes.White, plRect, format);
            }
            y += size / 5 / 5 + 5f;
        }

        string applyEffect(string text)
        {
            int moment = frame;
            return text.Substring(0,
                moment > text.Length ? text.Length : moment);
        }

        Brush getBrush(int value)
        {
            return new SolidBrush(Color.FromArgb(
                value < 75 ? 255 : 255 - 51 * (value - 75) / 5,
                value < 50 ? 0 : (value > 75 ? 255 : 51 * (value - 50) / 5),
                0
            ));
        }
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
    }

    public override void Reset()
    {
        this.frame = 0;
        this.status.Reset();
        base.Reset();
    }
}