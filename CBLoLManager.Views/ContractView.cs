using System;
using System.Drawing;
using System.Collections.Generic;

namespace CBLoLManager.Views;

using GameRule;
using Model;

public class ContractView : BaseView
{
    OptionsView opt = null;
    PlayerCarrousel pla = null;
    Dictionary<Player, Contract> contracts = null;
    PointF cursor = PointF.Empty;
    List<Player> players = null;
    bool down = false;
    Contract lastContract = null;
    PointF carLoc;
    float carWid;

    protected override void draw(Bitmap bmp, Graphics g)
    {
        var player = pla.Current;
        if (player == null)
            return;
        
        var contract = contracts[player];
        if (contract != lastContract)
            setContractOptions(contract);
        lastContract = contract;

        pla.Draw(bmp, g);
        opt.Draw(bmp, g);
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
        this.players = Game.Current.Team.GetAll();
        var contracts = Game.Current.Contracts;

        opt = new OptionsView();

        carLoc = new PointF(5, 5);
        carWid = 8 * (bmp.Width - 10) / 10;
        pla = new PlayerCarrousel(carLoc, carWid, players);

        this.contracts = new Dictionary<Player, Contract>();
        foreach (var player in players)
        {
            foreach (var contract in contracts)
            {
                if (!contract.Closed)
                    continue;
                
                if (contract.Player.Nickname != player.Nickname)
                    continue;
                
                this.contracts.Add(
                    player, contract
                );
            }
        }

        g.Clear(Color.Black);
    }

    void setContractOptions(Contract contract)
    {
        int week = Game.Current.Week / 26;
        if (contract.End > week)
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
                    players.RemoveAll(p => p.Nickname == contract.Player.Nickname);
                    if (players.Count == 0)
                        exit();
                    pla = new PlayerCarrousel(carLoc, carWid, players);
                }
                else if (s == "Manter")
                {
                    players.RemoveAll(p => p.Nickname == contract.Player.Nickname);
                    if (players.Count == 0)
                        exit();
                    pla = new PlayerCarrousel(carLoc, carWid, players);
                }
            };
        }
        else
        {
            opt = new OptionsView(
                "Oferecer Proposta para Recontratar",
                "Dispensar"
            );

            opt.OnOptionClick += s =>
            {
                if (s == "Oferecer Proposta para Recontratar")
                {
                    Game.Current.FreeAgent
                        .Add(contract.Player);
                    players.RemoveAll(p => p.Nickname == contract.Player.Nickname);
                    if (players.Count == 0)
                        exit();
                    pla = new PlayerCarrousel(carLoc, carWid, players);
                }
                else if (s == "Dispensar")
                {
                    Game.Current.FreeAgent
                        .Add(contract.Player);
                    players.RemoveAll(p => p.Nickname == contract.Player.Nickname);
                    if (players.Count == 0)
                        exit();
                    pla = new PlayerCarrousel(carLoc, carWid, players);
                }
            };
        }
    }

    private void exit()
    {
        if (Exit != null)
            Exit();
    }

    public event Action Exit;
}