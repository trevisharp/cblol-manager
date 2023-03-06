using System;
using System.Drawing;
using System.Collections.Generic;

namespace CBLoLManager.Views;

using Util;
using Model;
using GameRule;

public class ContractView : BaseView
{
    ContractSystem sys;
    OptionsView opt = null;
    PlayerCarrousel pla = null;
    Dictionary<Player, Contract> contracts = null;
    Dictionary<Player, Contract> newContracts = null;
    PointF cursor = PointF.Empty;
    List<Player> players = null;
    bool down = false;
    Contract lastContract = null;
    PointF carLoc;
    float carWid;

    protected override void draw(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);

        var player = pla.Current;
        if (player == null)
            return;
        
        var contract = contracts[player];
        if (contract != lastContract)
            setContractOptions(contract);
        lastContract = contract;

        pla.Draw(bmp, g);
        opt.Draw(bmp, g);

        int split = Game.Current.Week / 26;

        float y = (pla.Location?.Y ?? 0) + bmp.Height / 2;
        g.DrawString("Conrtato Atual:", SystemFonts.CaptionFont,
            Brushes.White, new PointF(20, y));
        y += 40;
        g.DrawString($"Salário: {Formatter.FormatMoney(contract.Wage)}", 
            SystemFonts.CaptionFont, Brushes.White, new PointF(20, y));
        y += 40;
        g.DrawString($"Splits Restantes: {contract.End - split}",
            SystemFonts.CaptionFont, Brushes.White, new PointF(20, y));
        y += 40;
        g.DrawString($"Multa Recisória: {Formatter.FormatMoney(contract.RescissionFee)}",
            SystemFonts.CaptionFont, Brushes.White, new PointF(20, y));

        if (contract.End >= split)
            return;
        
        var newContract = newContracts[player];
        y = (pla.Location?.Y ?? 0) + bmp.Height / 2;
        g.DrawString("Nova Proposta:", SystemFonts.CaptionFont,
            Brushes.White, new PointF(bmp.Width / 2, y));
        y += 40;
        g.DrawString($"Novo Salário: {Formatter.FormatMoney(newContract.Wage)}", 
            SystemFonts.CaptionFont, Brushes.White, new PointF(bmp.Width / 2, y));
        y += 40;
        g.DrawString($"Nova Duração: {newContract.End - split}",
            SystemFonts.CaptionFont, Brushes.White, new PointF(bmp.Width / 2, y));
        y += 40;
        g.DrawString($"Nova Multa: {Formatter.FormatMoney(newContract.RescissionFee)}",
            SystemFonts.CaptionFont, Brushes.White, new PointF(bmp.Width / 2, y));
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
        opt?.MouseMove(cursor, down);
        pla?.MouseMove(cursor, down);
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        this.sys = new ContractSystem();
        this.players = Game.Current.Team.GetAll();
        var contracts = Game.Current.Contracts;

        opt = new OptionsView();

        carLoc = new PointF(5, 5);
        carWid = 8 * (bmp.Width - 10) / 10;
        pla = new PlayerCarrousel(carLoc, carWid, players);

        int split = Game.Current.Week / 26;
        this.contracts = new Dictionary<Player, Contract>();
        this.newContracts = new Dictionary<Player, Contract>();
        foreach (var player in players)
        {
            foreach (var contract in contracts)
            {
                if (!contract.Closed)
                    continue;
                
                if (contract.End < split)
                    continue;
                
                if (contract.Player.Nickname != player.Nickname)
                    continue;
                
                this.contracts.Add(
                    player, contract
                );
                
                this.newContracts.Add(
                    player, sys.UpdateContract(contract)
                );
            }
        }

        g.Clear(Color.Black);
    }

    void setContractOptions(Contract contract)
    {
        int split = Game.Current.Week / 26;
        if (contract.End > split)
        {
            opt = new OptionsView(
                "Liberar para Ouvir Propostas",
                "Manter"
            );

            opt.OnOptionClick += s =>
            {
                if (s == "Liberar para Ouvir Propostas")
                {
                    Game.Current.SeeingProposes
                        .Add(contract.Player);
                    updateCarrousel(contract.Player);
                }
                else if (s == "Manter")
                {
                    updateCarrousel(contract.Player);
                }
            };
        }
        else
        {
            opt = new OptionsView(
                "Recontratar por Nova Proposta",
                "Tentar Manter Proposta Antiga",
                "Dispensar"
            );

            opt.OnOptionClick += s =>
            {
                if (s == "Recontratar por Nova Proposta")
                {
                    var newContract = newContracts[contract.Player];
                    newContract.Closed = true;
                    Game.Current.Contracts.Add(newContract);

                    updateCarrousel(contract.Player);
                }
                else if (s == "Tentar Extender Contrato (2 splits)")
                {
                    Game.Current.EndContract
                        .Add(contract.Player);
                    var newContract = sys.TryKeepContract(contract);
                    Game.Current.Contracts.Add(newContract);
                    
                    updateCarrousel(contract.Player);
                }
                else if (s == "Dispensar")
                {
                    Game.Current.FreeAgent
                        .Add(contract.Player);
                    Game.Current.Team
                        .Remove(contract.Player);
                    
                    updateCarrousel(contract.Player);
                }
            };
        }
    }

    void updateCarrousel(Player removed)
    {
        players.RemoveAll(p => p.Nickname == removed.Nickname);
        if (players.Count == 0)
            exit();
        pla = new PlayerCarrousel(carLoc, carWid, players);
    }

    private void exit()
    {
        sys.Run();
        
        if (Exit != null)
            Exit();
    }

    public event Action Exit;
}