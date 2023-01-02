using System.Collections.Generic;
using System.Drawing;

namespace CBLoLManager.Views;

using Util;
using Model;
using GameRule;
using System;

public class TeamPage : BaseView
{
    private Team team;
    private PlayerTeamView playerView = null;
    private PlayerStatusView teamStatus = null;
    private GameMap map = null;
    private OptionsView options = null;

    public TeamPage(Team team)
    {
        this.team = team;
        Game.Current.Team = team;
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        float unity = bmp.Width * .05f;
        var org = team.Organization;
        
        float mapStart = 10 * unity, 
            margin = 40f;
        float mapwid = bmp.Width - mapStart - 10f - 2 * margin;
        float maphei = 8 * unity - 10f - 2 * margin;

        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        if (teamStatus == null)
        {
            teamStatus = new PlayerStatusView();
            teamStatus.Player = team.AveragePlayer();
            teamStatus.Location = new PointF(4 * unity, 40f);
            teamStatus.Size = 3 * unity;
        }

        if (playerView == null)
        {
            playerView = new PlayerTeamView(team,
                new PointF(10f + mapStart, 10f + maphei + 2 * margin), 10 * unity - 5f
            );
        }

        if (map == null)
        {
            map = new GameMap(team.GetAll(), new RectangleF(
                margin + 10f + mapStart,
                margin + 5f,
                mapwid,
                maphei
            ));
        }

        if (options == null)
        {
            options = new OptionsView(
                "Salvar",
                "Carregar",
                "Mercado de Jogadores",
                "Próximo Jogo"
            );

            options.OnOptionClick += s =>
            {
                switch (s)
                {
                    case "Salvar":
                        
                        break;

                    case "Carregar":
                        
                        break;
                    
                    case "Mercado de Jogadores":
                        OnOpenMarket();
                        break;
                    
                    case "Próximo Jogo":
                        break;
                }
            };
        }
        
        var brush = new SolidBrush(
            Color.FromArgb(32, 42, 68)
        );
        rect(brush, () => new RectangleF(5f, 5f, 3 * unity, 3 * unity),
            org.Name + "back");
        img(5f, 5f, 3 * unity, 3 * unity, 
            () => Bitmap.FromFile("Img/" + org.Photo) as Bitmap,
            "logo");
        g.DrawString($"Recursos: {Formatter.FormatMoney(Game.Current.Team.Money)}", font, Brushes.White,
            new RectangleF(5f, 3 * unity + 10f, 3 * unity, unity), format);
        
        playerView.Draw(bmp, g);
        teamStatus.Draw(bmp, g);
        map.Draw(bmp, g);
        options.Draw(bmp, g);
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
    }

    public override void MoseMove(PointF cursor, bool down)
    {
        playerView?.MoseMove(cursor, down);
        options?.MoseMove(cursor, down);
    }

    public event Action OnOpenMarket;
}