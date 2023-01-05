using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace CBLoLManager.Views;

using Util;
using Model;
using GameRule;
using System;

public class PlayerMarket : BaseView
{
    int round = 1;

    PointF cursor = PointF.Empty;
    bool down = false;

    PlayerCarrousel players = null;
    OptionsView options = null;
    NumericView wage = null;
    NumericView time = null;
    NumericView rescissionFee = null;
    ButtonView propose = null;
    ButtonView counterpropose = null;
    ButtonView contract = null;
    Contract[] contracts = new Contract[0];

    public PlayerMarket()
    {
        
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        if (Game.Current.EndContractStepForPlayer)
        {
            if (CloseMarket != null)
            {
                Audio.Stop();
                CloseMarket();
            }
            return;
        }

        g.Clear(Color.Black);

        var font = new Font(FontFamily.GenericMonospace, 15f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        g.DrawString($"Rodada: {round}", font, Brushes.White, PointF.Empty);
        g.DrawString($"Recursos: {Formatter.FormatMoney(Game.Current.Team.Money)}",
            font, Brushes.White, new PointF(0, 25f));

        if (this.players == null)
        {
            this.players = new PlayerCarrousel(
                new PointF(5, 150),
                1500f,
                Game.Current.PlayersInMarket
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
                update();
            };
        }

        if (wage == null)
        {
            wage = new NumericView();
            wage.IsMoney = true;
            wage.Label = "Salário";
            wage.Rect = new RectangleF(20, 700, 300, 60);
            wage.Value = 1000f;
            wage.Step = 500f;
        }

        if (time == null)
        {
            time = new NumericView();
            time.IsMoney = false;
            time.Label = "Duração em Splits (Semestres):";
            time.Rect = new RectangleF(20, 800, 300, 60);
            time.Value = 2;
            time.Step = 1;
        }

        if (rescissionFee == null)
        {
            rescissionFee = new NumericView();
            rescissionFee.IsMoney = true;
            rescissionFee.Label = "Multa Rescisória:";
            rescissionFee.Rect = new RectangleF(20, 900, 300, 60);
            rescissionFee.Value = 12000;
            rescissionFee.Step = 1000;
        }

        if (propose == null)
        {
            propose = new ButtonView();
            propose.Label = "Realizar Proposta";
            propose.Color = Brushes.Navy;
            propose.SelectedColor = Brushes.White;
            propose.Rect = new RectangleF(350, 800, 300, 60);
            propose.OnClick += delegate
            {
                if (ProposeMaked != null)
                {
                    Propose propose = new Propose();
                    
                    propose.Player = players.Current;
                    propose.RescissionFee = rescissionFee.Value;
                    propose.Time = time.Value;
                    propose.Wage = wage.Value;
                    propose.Team = Game.Current.Team;
                    propose.Round = round;

                    ProposeMaked(propose);
                    g.Clear(Color.Black);
                    round++;
                }
            };
        }

        if (counterpropose == null)
        {
            counterpropose = new ButtonView();
            counterpropose.Label = "Tentar Contra-Proposta";
            counterpropose.Color = Brushes.Navy;
            counterpropose.SelectedColor = Brushes.White;
            counterpropose.Rect = new RectangleF(350, 800, 300, 60);
            counterpropose.OnClick += delegate
            {
                var contractCrr = contracts
                    .LastOrDefault(c => c.Player == this.players.Current);

                Game.Current.Contracts.Remove(contractCrr);
                this.contracts = this.contracts
                    .Where(c => c != contractCrr)
                    .ToArray();
            };
        }

        if (contract == null)
        {
            contract = new ButtonView();
            contract.Label = "Contratar";
            contract.Color = Brushes.Navy;
            contract.SelectedColor = Brushes.White;
            contract.Rect = new RectangleF(700, 800, 300, 60);

            contract.OnClick += delegate
            {
                var contractCrr = contracts
                    .LastOrDefault(c => c.Player == this.players.Current);
                contractCrr.Closed = true;
                Game.Current.Team.Add(contractCrr.Player);
                Game.Current.FreeAgent.Remove(contractCrr.Player);
                Game.Current.SeeingProposes.Remove(contractCrr.Player);
                Game.Current.EndContract.Remove(contractCrr.Player);
                Game.Current.Team.Money -= contractCrr.Wage * 6;
                this.contracts = this.contracts
                    .Where(c => c != contractCrr)
                    .ToArray();
                update();
            };
        }

        var contractCrr = contracts
            .LastOrDefault(c => c.Player == this.players.Current);
        if (contractCrr != null)
        {
            wage.Value = contractCrr.Wage;
            time.Value = contractCrr.End - Game.Current.Week;
            rescissionFee.Value = contractCrr.RescissionFee;
            
            contract.Draw(bmp, g);
            contract.MouseMove(cursor, down);

            counterpropose.Draw(bmp, g);
            counterpropose.MouseMove(cursor, down);
        }
        else
        {
            propose.Draw(bmp, g);
            propose.MouseMove(cursor, down);
        }

        g.DrawString("Proposta:", font, Brushes.White, new PointF(20, 670));

        wage.Draw(bmp, g);
        wage.MouseMove(cursor, down);

        time.Draw(bmp, g);
        time.MouseMove(cursor, down);

        rescissionFee.Draw(bmp, g);
        rescissionFee.MouseMove(cursor, down);

        this.players.Draw(bmp, g);
        this.players.MouseMove(cursor, down);

        options.Draw(bmp, g);
        options.MouseMove(cursor, down);

    }

    public void Reopen()
    {
        update();
        this.contracts = Game.Current.Contracts
            .Where(c => c.Team == Game.Current.Team)
            .ToArray();
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
    }

    public override async void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
        await Audio.BoraInvadir(true);
    }

    void update()
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
        
        if (Game.Current.Team.TopLaner != null)
            players = players.Where(p => p.Role != Position.TopLaner);
            
        if (Game.Current.Team.Jungler != null)
            players = players.Where(p => p.Role != Position.Jungler);
            
        if (Game.Current.Team.MidLaner != null)
            players = players.Where(p => p.Role != Position.MidLaner);
            
        if (Game.Current.Team.AdCarry != null)
            players = players.Where(p => p.Role != Position.AdCarry);
            
        if (Game.Current.Team.Support != null)
            players = players.Where(p => p.Role != Position.Support);

        this.players = new PlayerCarrousel(
            new PointF(5, 150),
            1500f,
            players
        );
    }

    public event Action<Propose> ProposeMaked;
    public event Action CloseMarket;
}