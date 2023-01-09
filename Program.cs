using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

using CBLoLManager.Model;
using CBLoLManager.Views;
using CBLoLManager.GameRule;
using CBLoLManager.Configuration;

// TODO: Move Game initializations to GameRule or Configuration Namespace

Graphics g = null;

// App Config

// App Logic

ProposeSystem sys = new ProposeSystem();

BaseView crrPage = null;

TeamSelectorPage teamSelectorPage = null;
TeamPage teamPage = null;
MarketPlayer market = null;
MarketRoundSumary sumary = null;
PosMarketPage posmarket = null;
ShirtSponsorPage sponsorPage = null;

bool firstTimeInMarket = true;
bool firstTimeInSponsorship = true;

// Team Selector Page Logic
teamSelectorPage = new TeamSelectorPage();
crrPage = teamSelectorPage;
teamSelectorPage.OnSelect += org =>
{
    Team team = new Team();
    team.Organization = org;

    Game.Current.Team = team;
    team.Money = 200000;
    List<float> moneys = new List<float>()
    {
        200000, 200000,
        300000, 300000, 300000,
        500000, 500000, 500000,
        1000000
    }
    .OrderBy(x => Random.Shared.Next())
    .ToList();
    foreach (var x in Organizations.All
        .Where(o => o.Name != org.Name))
    {
        Game.Current.Others.Add(new Team()
        {
            Organization = x,
            Money = moneys[0]
        });
        moneys.RemoveAt(0);
    }
    Game.Current.FreeAgent.AddRange(Players.All);

    teamPage = new TeamPage(team);
    teamPage.OnOpenMarket += () =>
    {
        if (firstTimeInMarket)
        {
            firstTimeInMarket = false;
            var tutorial = new MarketTutorial();
            tutorial.Exit += delegate
            {
                crrPage = market;
                g.Clear(Color.Black);
            };
            crrPage = tutorial;
            return;
        }
        
        crrPage = market;
    };
    teamPage.NextGame += () =>
    {
        crrPage = new MatchView(
            Game.Current.Team, 
            Game.Current.Others.First());
    };
    teamPage.Sponsorship += () =>
    {
        if (firstTimeInSponsorship)
        {
            firstTimeInSponsorship = false;
            var tutorial = new ShirtSponsorTutorialPage();
            tutorial.Exit += delegate
            {
                crrPage = sponsorPage;
                g.Clear(Color.Black);
            };
            crrPage = tutorial;
            return;
        }
        
        crrPage = sponsorPage;
    };
    
    crrPage = teamPage;
};

market = new MarketPlayer();
market.ProposeMaked += p =>
{
    var result = sys.RunRound(p);

    sumary = new MarketRoundSumary();
    sumary.Events = result.Item1;
    sumary.Contract = result.Item2;
    sumary.Exit += delegate
    {
        market.Reopen();
        crrPage = market;
    };

    crrPage = sumary;
};
market.CloseMarket += () =>
{
    sys.Complete();

    posmarket = new PosMarketPage();
    crrPage = posmarket;

    posmarket.Exit += delegate
    {
        crrPage = teamPage;
        teamPage.Reopen();
    };
};

sponsorPage = new ShirtSponsorPage();
sponsorPage.Exit += delegate
{
    teamPage.Reopen();
    crrPage = teamPage;
};

// View Logic

Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
ApplicationConfiguration.Initialize();

Bitmap bmp = null;
PointF cursor = PointF.Empty;
bool down = false;

var form = new Form();

form.WindowState = FormWindowState.Maximized;
form.FormBorderStyle = FormBorderStyle.None;

PictureBox pb = new PictureBox();
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

var tm = new System.Windows.Forms.Timer();
tm.Interval = 20;

tm.Tick += delegate
{
    crrPage.Draw(bmp, g);
    crrPage.MouseMove(cursor, down);
    pb.Refresh();
};

form.Load += delegate
{
    Initializer.InitializeAll();
    bmp = new Bitmap(pb.Width, pb.Height);
    g = Graphics.FromImage(bmp);
    g.Clear(Color.Black);
    pb.Image = bmp;
    tm.Start();
};

pb.MouseMove += (o, e) =>
{
    cursor = e.Location;
};

pb.MouseDown += (o, e) =>
{
    down = true;
};

pb.MouseUp += (o, e) =>
{
    down = false;
};

form.KeyDown += (o, e) =>
{
    switch (e.KeyCode)
    {
        case Keys.Escape:
            Application.Exit();
            break;
    }
};

Application.Run(form);