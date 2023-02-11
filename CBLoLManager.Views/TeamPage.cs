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
            g.Clear(Color.Black);
            teamStatus = new PlayerStatusView();
            teamStatus.Player = team.AveragePlayer();
            teamStatus.Location = new PointF(4 * unity, 40f);
            teamStatus.Size = 3 * unity;
        }

        if (playerView == null)
        {
            g.Clear(Color.Black);
            playerView = new PlayerTeamView(team,
                new PointF(10f + mapStart, 10f + maphei + 2 * margin), 10 * unity - 5f
            );
        }

        if (map == null)
        {
            g.Clear(Color.Black);
            map = new GameMap(team.GetAll(), new RectangleF(
                margin + 10f + mapStart,
                margin + 5f,
                mapwid,
                maphei
            ));
        }

        if (options == null)
        {
            g.Clear(Color.Black);
            options = new OptionsView(
                "Salvar",
                "Próxima Semana"
            );

            options.OnOptionClick += s =>
            {
                switch (s)
                {
                    case "Salvar":
                        Game.Save();
                        break;
                    
                    case "Próxima Semana":
                        var weekEvent = Game.Current.CurrentWeekEvent;
                        
                        if (weekEvent == WeekEvent.MarketWeek)
                            OnOpenMarket();
                        else if (weekEvent == WeekEvent.GameEvent)
                            NextGame();
                        else if (weekEvent == WeekEvent.Sponsorship)
                            Sponsorship();
                        else if (weekEvent == WeekEvent.AdministrativeWeek)
                            AdministrativeWeek();
                        else if (weekEvent == WeekEvent.None)
                            g.Clear(Color.Black);
                        
                        Game.Current.Week++;
                        
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
        g.DrawString($"Recursos: {Formatter.FormatMoney(Game.Current.Team.Money)}", font, 
            Game.Current.Team.Money < 0 ? Brushes.Red : Brushes.White,
            new RectangleF(5f, 3 * unity + 10f, 3 * unity, unity), format);
        g.DrawString($"Semana {Game.Current.Week}", font, Brushes.White,
            new RectangleF(5f, 3 * unity + 30f, 3 * unity, unity), format);

        var shirt = Game.Current.Team.Shirt;
        if (shirt != null)
        {
            img(7 * unity, .5f * unity, shirt.Photo.Width * 0.8f,
                shirt.Photo.Height * 0.8f, () => shirt.Photo as Bitmap, "shirt");
        }
        
        var weekEvent = Game.Current.CurrentWeekEvent;
        string nextEvent = string.Empty;
        
        if (weekEvent == WeekEvent.MarketWeek)
            nextEvent = "Janela de Transferência";
        else if (weekEvent == WeekEvent.GameEvent)
            nextEvent = "Jogo do CBLoL";
        else if (weekEvent == WeekEvent.Sponsorship)
            nextEvent = "Negociação com os Patrocinadores";
        else if (weekEvent == WeekEvent.None)
            nextEvent = "Nenhum evento";
        else if (weekEvent == WeekEvent.AdministrativeWeek)
            nextEvent = "Semana Administrativa";
        else if (weekEvent == WeekEvent.ContractWeek)
            nextEvent = "Semana dos Contratos e Acordos";
        
        g.DrawString($"Evento da Próxima semana: {nextEvent}", 
            font, Brushes.White,
            new Rectangle(5, bmp.Height - 80, bmp.Width, 40),
            format);
        
        playerView.Draw(bmp, g);
        teamStatus.Draw(bmp, g);
        map.Draw(bmp, g);
        options.Draw(bmp, g);
    }

    public async void Reopen()
    {
        playerView = null;
        teamStatus = null;
        map = null;
        options = null;
        await Audio.ConfiaNaCall();
    }

    public override async void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
        await Audio.ConfiaNaCall();
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        playerView?.MouseMove(cursor, down);
        options?.MouseMove(cursor, down);
    }

    public event Action OnOpenMarket;
    public event Action AdministrativeWeek;
    public event Action NextGame;
    public event Action Sponsorship;
}