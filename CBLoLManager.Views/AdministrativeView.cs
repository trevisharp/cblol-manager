using System;
using System.Linq;
using System.Drawing;

namespace CBLoLManager.Views;

using Util;
using Model;
using GameRule;

public class AdministrativeView : BaseView
{
    OptionsView options = null;
    PointF cursor = PointF.Empty;
    TeamHistory hist;
    bool down = false;

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
        this.hist = Game.Current.Team.History.LastOrDefault();
        
        g.Clear(Color.Black);

        float y = 10;
        var font1 = new Font(FontFamily.GenericMonospace, 36f);
        var font2 = new Font(FontFamily.GenericMonospace, 24f);
        var font3 = new Font(FontFamily.GenericMonospace, 12f);

        title("Estatísticas");
        subtitle("Veja suas conquistas até aqui...");
        
        subtitle("Geral");
        text($"Ano atual: {hist.Year}");
        text($"Split atual: {hist.Split}");

        subtitle("Conquistas Esportivas");
        text($"PlayOffs disputados: {hist.PlayOffsPlayed}");
        text($"Titulos de fases regulares alcançados: {hist.RegularPhaseTitles}");
        text($"Vices alcançados: {hist.Titles}");
        text($"Titulos alcançados: {hist.Titles}");

        subtitle("Conquistas Financeiras");
        text($"Recurso inicial: {hist.InitialGold}");
        text($"Salário pago aos jogadores: {hist.PlayerWage}");
        text($"Premiações ganhas no CBLOL: {hist.CBLoLAward}");
        text($"Camisetas Vendidas: {hist.ShirtSale} ({hist.ShirtSaleTotal} no total)");
        text($"Ganho em vendas de camisetas: {Formatter.FormatMoney(200 * hist.ShirtSale)}");
        
        subtitle("Popularidade");
        text($"Seguidores no Twitter: {hist.Followers}");

        void title(string message)
        {
            g.DrawString(message, font1, Brushes.White, 10, y);
            y += 60;
        }

        void subtitle(string message)
        {
            y += 20;
            g.DrawString(message, font2, Brushes.White, 10, y);
            y += 40;
        }

        void text(string message)
        {
            g.DrawString(message, font3, Brushes.White, 10, y);
            y += 20;
        }

        options = new OptionsView("Sair");
        options.OnOptionClick += delegate
        {
            Exit();
        };
    }

    public event Action Exit;
}