using System.Drawing;

namespace CBLoLManager.Views;

using Model;

public class PlayerPage : BaseView
{
    private PlayerStatusView status = null;
    private Player player;
    private float size;
    private int frame = 0;

    public PlayerPage(Player player, PointF point, float size)
    {
        this.status = new PlayerStatusView();
        this.status.Player = player;
        this.Location = point;
        this.player = player;
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
            () => Bitmap.FromFile("Img/" + player.Photo) as Bitmap, "photo");
        
        Font font = new Font(FontFamily.GenericMonospace, 20f);
        g.DrawString(applyEffect(player.Nickname), font, Brushes.White, 
            p.X + 2 * size / 3 - 60f, p.Y + 10f);

        font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        g.DrawString(applyEffect($"{player.Name}"), 
            font, Brushes.White, p.X + 2 * size / 3 - 60f, p.Y + 60f);

        g.DrawString(applyEffect($"Data de Nascimento: {player.BirthDate.ToShortDateString()}"), 
            font, Brushes.White, p.X + 2 * size / 3 - 60f, p.Y + 80f);
        
        g.DrawString(applyEffect($"Rota: {player.Role}"), 
            font, Brushes.White, p.X + 2 * size / 3 - 60f, p.Y + 100f);
        
        g.DrawString(applyEffect($"Nacionalidade: {player.Nationality}"), 
            font, Brushes.White, p.X + 2 * size / 3 - 60f, p.Y + 120f);
        
        g.DrawString(applyEffect($"Fase de Rota: {player.LanePhase}"), 
            font, getBrush(player.LanePhase), p.X + 2 * size / 3 - 60f, p.Y + 160f);
        
        g.DrawString(applyEffect($"Mecânica: {player.MechanicSkill}"), 
            font, getBrush(player.MechanicSkill), p.X + 2 * size / 3 - 60f, p.Y + 180f);
        
        g.DrawString(applyEffect($"Team Figth: {player.TeamFigth}"), 
            font, getBrush(player.TeamFigth), p.X + 2 * size / 3 - 60f, p.Y + 200f);
        
        g.DrawString(applyEffect($"Visão de Jogo: {player.GameVision}"), 
            font, getBrush(player.GameVision), p.X + 2 * size / 3 - 60f, p.Y + 220f);
        
        g.DrawString(applyEffect($"Mentalidade: {player.Mentality}"), 
            font, getBrush(player.Mentality), p.X + 2 * size / 3 - 60f, p.Y + 240f);
        
        g.DrawString(applyEffect($"Liderança: {player.Leadership}"), 
            font, getBrush(player.Leadership), p.X + 2 * size / 3 - 60f, p.Y + 260f);
            
        int media = (player.LanePhase + player.MechanicSkill + player.TeamFigth +
            player.GameVision + player.Mentality + player.Leadership) / 6;
        g.DrawString(applyEffect($"Media: {media}"), 
            font, getBrush(media), p.X + 2 * size / 3 - 60f, p.Y + 300f);

        status.Draw(bmp, g);

        g.DrawRectangle(Pens.White, p.X, p.Y, size, size / 3);
        // status.draw(bmp, g);

        string applyEffect(string text)
        {
            int moment = frame / 2;
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
}