using System.Drawing;

namespace CBLoLManager.Views;

using Model;

public class PlayerPage : BaseView
{
    private PlayerStatusView status = null;
    private float size;
    private int frame = 0;
    
    private Player player = null;
    public Player Player
    {
        get => this.player;
        set
        {
            this.player = value;
            this.status.Player = value;
        }
    }

    public PlayerPage(Player player, PointF point, float size)
    {
        this.status = new PlayerStatusView();
        this.status.Player = player;
        this.Location = point;
        this.Player = player;
        this.size = size;

        status.Location = new PointF(point.X + size / 3 - 60f, point.Y + 50f);
        status.Size = size / 4;
    }

    public override void draw(Bitmap bmp, Graphics g)
    {
        frame++;
        if (!Location.HasValue)
            return;
        var p = this.Location.Value;

        img(p.X + 10f, p.Y + 10f, size / 3 - 20f,
            () => Bitmap.FromFile("Img/" + Player.Photo) as Bitmap, "photo");
        
        Font font = new Font(FontFamily.GenericMonospace, 20f);
        g.DrawString(applyEffect(Player.Nickname), font, Brushes.White, 
            p.X + 2 * size / 3 - 60f, p.Y + 10f);

        font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        g.DrawString(applyEffect($"{Player.Name}"), 
            font, Brushes.White, p.X + 2 * size / 3 - 60f, p.Y + 60f);

        g.DrawString(applyEffect($"Data de Nascimento: {Player.BirthDate.ToShortDateString()}"), 
            font, Brushes.White, p.X + 2 * size / 3 - 60f, p.Y + 80f);
        
        g.DrawString(applyEffect($"Rota: {Player.Role}"), 
            font, Brushes.White, p.X + 2 * size / 3 - 60f, p.Y + 100f);
        
        g.DrawString(applyEffect($"Nacionalidade: {Player.Nationality}"), 
            font, Brushes.White, p.X + 2 * size / 3 - 60f, p.Y + 120f);
        
        g.DrawString(applyEffect($"Fase de Rota: {Player.LanePhase}"), 
            font, getBrush(Player.LanePhase), p.X + 2 * size / 3 - 60f, p.Y + 160f);
        
        g.DrawString(applyEffect($"Mecânica: {Player.MechanicSkill}"), 
            font, getBrush(Player.MechanicSkill), p.X + 2 * size / 3 - 60f, p.Y + 180f);
        
        g.DrawString(applyEffect($"Team Figth: {Player.TeamFigth}"), 
            font, getBrush(Player.TeamFigth), p.X + 2 * size / 3 - 60f, p.Y + 200f);
        
        g.DrawString(applyEffect($"Visão de Jogo: {Player.GameVision}"), 
            font, getBrush(Player.GameVision), p.X + 2 * size / 3 - 60f, p.Y + 220f);
        
        g.DrawString(applyEffect($"Mentalidade: {Player.Mentality}"), 
            font, getBrush(Player.Mentality), p.X + 2 * size / 3 - 60f, p.Y + 240f);
        
        g.DrawString(applyEffect($"Liderança: {Player.Leadership}"), 
            font, getBrush(Player.Leadership), p.X + 2 * size / 3 - 60f, p.Y + 260f);
            
        int media = (Player.LanePhase + Player.MechanicSkill + Player.TeamFigth +
            Player.GameVision + Player.Mentality + Player.Leadership) / 6;
        g.DrawString(applyEffect($"Media: {media}"), 
            font, getBrush(media), p.X + 2 * size / 3 - 60f, p.Y + 300f);

        status.Draw(bmp, g);

        g.DrawRectangle(Pens.White, p.X, p.Y, size, size / 3);
        // status.draw(bmp, g);

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

    public override void Reset()
    {
        this.frame = 0;
        this.status.Reset();
        base.Reset();
    }
}