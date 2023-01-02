using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace CBLoLManager.Views;

using Util;
using Model;
using GameRule;

public class PlayerMarket : BaseView
{
    int round = 1;

    PointF cursor = PointF.Empty;
    bool down = false;

    PlayerCarrousel freeAgent = null;
    OptionsView options = null;

    public PlayerMarket()
    {
        
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        var font = new Font(FontFamily.GenericMonospace, 15f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        g.DrawString($"Rodada: {round}", font, Brushes.White, PointF.Empty);
        g.DrawString($"Recursos: {Formatter.FormatMoney(Game.Current.Team.Money)}",
            font, Brushes.White, new PointF(0, 25f));

        if (freeAgent == null)
        {
            freeAgent = new PlayerCarrousel(
                new PointF(5, 200),
                1500f,
                Game.Current.FreeAgent
            );
        }

        if (options == null)
        {
            options = new OptionsView(
                "Ver Jogadores Free Agent",
                "Ver Jogadores Ouvindo Propostas",
                "Ver Jogadores em Fim de Contrato",
                "Ver TopLaners",
                "Ver Junglers",
                "Ver MidLaners",
                "Ver AdCarries",
                "Ver Supportes"
            );
            options.SetCheckBox("Ver Jogadores Free Agent");
            options.SetCheckBox("Ver Jogadores Ouvindo Propostas");
            options.SetCheckBox("Ver Jogadores em Fim de Contrato");
            options.SetCheckBox("Ver TopLaners");
            options.SetCheckBox("Ver Junglers");
            options.SetCheckBox("Ver MidLaners");
            options.SetCheckBox("Ver AdCarries");
            options.SetCheckBox("Ver Supportes");

            options.OnOptionClick += delegate
            {
                var list = options.Checked;
                IEnumerable<Player> players = null;
                
                bool fa = list.Contains("Ver Jogadores Free Agent");
                bool op = list.Contains("Ver Jogadores Ouvindo Propostas");
                bool ec = list.Contains("Ver Jogadores em Fim de Contrato");

                bool pos = list.Contains("Ver TopLaners") ||
                    list.Contains("Ver Junglers") ||
                    list.Contains("Ver MidLaners") ||
                    list.Contains("Ver AdCarries") ||
                    list.Contains("Ver Supportes");

                if (fa && op)
                {
                    players = Game.Current.FreeAgent.Union(
                        Game.Current.SeeingProposes
                    );
                }
                else if (fa && ec)
                {
                    players = Game.Current.FreeAgent.Union(
                        Game.Current.EndContract
                    );
                }
                else if (op && ec)
                {
                    players = Game.Current.SeeingProposes.Union(
                        Game.Current.EndContract
                    );
                }
                else if (fa)
                    players = Game.Current.FreeAgent;
                else if (op)
                    players = Game.Current.SeeingProposes;
                else if (ec)
                    players = Game.Current.EndContract;
                else
                {
                    players = Game.Current.FreeAgent.Union(
                        Game.Current.EndContract
                    ).Union(
                        Game.Current.SeeingProposes
                    );
                }

                if (pos)
                {
                    if (!list.Contains("Ver TopLaners"))
                        players = players.Where(p => p.Role != Position.TopLaner);
                        
                    if (!list.Contains("Ver Junglers"))
                        players = players.Where(p => p.Role != Position.Jungler);
                        
                    if (!list.Contains("Ver MidLaners"))
                        players = players.Where(p => p.Role != Position.MidLaner);
                        
                    if (!list.Contains("Ver AdCarries"))
                        players = players.Where(p => p.Role != Position.AdCarry);
                        
                    if (!list.Contains("Ver Supportes"))
                        players = players.Where(p => p.Role != Position.Support);
                }

                freeAgent = new PlayerCarrousel(
                    new PointF(5, 200),
                    1500f,
                    players
                );
            };
        }

        freeAgent.Draw(bmp, g);
        freeAgent.MoseMove(cursor, down);

        options.Draw(bmp, g);
        options.MoseMove(cursor, down);
    }

    public override void MoseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
    }
}