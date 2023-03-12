using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

using CBLoLManager.Model;
using CBLoLManager.Views;
using CBLoLManager.GameRule;
using CBLoLManager.RandomGen;
using CBLoLManager.Configuration;

Graphics g = null;

// App Logic

ProposeSystem sys = new ProposeSystem();

BaseView crrPage = null;

MainScreen main = new MainScreen();
TeamSelectorPage teamSelectorPage = null;
TeamPage teamPage = null;
MarketPlayer market = null;
MarketRoundSumary sumary = null;
PosMarketPage posmarket = null;
ShirtSponsorPage sponsorPage = null;
DraftView draft = null;
MatchView match = null;
TorunamentView torunament = null;

bool firstTimeInMarket = true;
bool firstTimeInSponsorship = true;
int playCount = 0;

crrPage = main;

main.OnPlay += delegate
{
    selectTeam();
};
main.OnLoad += delegate
{
    Game.Load();
    openTeamPage();
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
    Game.Current.NewTournament();

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

void openTornament()
{
    torunament = new TorunamentView(playCount < 2);
    if (playCount > 1)
        playCount = 0;
    torunament.PlayMatch += op =>
    {
        makeMD5(op);
    };
    torunament.PlayNext += op =>
    {
        playCount++;
        makeDraft(op);
    };
    torunament.Exit += delegate
    {
        teamPage.Reopen();
        crrPage = teamPage;
    };
    crrPage = torunament;
}

void makeMD5(Team oponent, int scoreA = 0, int scoreB = 0)
{
    MD5View md5 = new MD5View();

    md5.TeamA = Game.Current.Team;
    md5.TeamB = oponent;
    md5.ScoreA = scoreA;
    md5.ScoreB = scoreB;

    md5.MakeGame += x =>
    {
        makeMD5Draft(oponent, scoreA, scoreB);
    };
    md5.Exit += () =>
    {
        teamPage.Reopen();
        crrPage = teamPage;
    };

    crrPage = md5;
}

void makeMD5Draft(Team oponent, int scoreA, int scoreB)
{
    draft = new DraftView(
        Game.Current.Team, 
        oponent);
    draft.Exit += draft =>
    {
        makeMD5Match(draft, scoreA, scoreB);
    };
    crrPage = draft;
}

void makeMD5Match(DraftResult draft, int scoreA, int scoreB)
{
    match = new MatchView(draft);
    match.Exit += (d, w, s) =>
    {  
        if (s.AWin)
        {
            PopularitySystem.OnWin(draft.TeamA);
            scoreA++;
        }
        else 
        {
            PopularitySystem.OnWin(draft.TeamB);
            scoreB++;
        }

        var posGame = new PosGameView(d, w, s);

        posGame.Exit += delegate
        {
            makeMD5(draft.TeamB, scoreA, scoreB);
        };

        crrPage = posGame;
    };
    crrPage = match;
}

void makeDraft(Team oponent)
{
    draft = new DraftView(
        Game.Current.Team, 
        oponent);
    draft.Exit += draft =>
    {
        makeMatch(draft);
    };
    crrPage = draft;
}

void makeMatch(DraftResult draft)
{
    match = new MatchView(draft);
    match.Exit += (d, w, s) =>
    {  
        if (s.AWin)
        {
            Game.Current?
                .Tournament?
                .AddWin(draft.TeamA);
            PopularitySystem.OnWin(draft.TeamA);
        }
        else
        {
            Game.Current?
                .Tournament?
                .AddWin(draft.TeamB);
            PopularitySystem.OnWin(draft.TeamB);
        }

        var posGame = new PosGameView(d, w, s);

        posGame.Exit += delegate
        {
            openTornament();
        };

        crrPage = posGame;
    };
    crrPage = match;
}

void selectTeam()
{
    teamSelectorPage = new TeamSelectorPage();
    teamSelectorPage.OnSelect += org =>
    {
        Game.New();
        Game.Current.Init(org);
        openTeamPage();
    };
    crrPage = teamSelectorPage;
}

void openTeamPage()
{
    teamPage = new TeamPage(Game.Current.Team);
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
        openTornament();
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
    teamPage.AdministrativeWeek += () =>
    {
        var adminisrative = new AdministrativeView();
        adminisrative.Exit += delegate
        {
            crrPage = teamPage;
            teamPage.Reopen();
        };
        crrPage = adminisrative;
    };
    teamPage.OpenContracts += () =>
    {
        var contractView = new ContractView();
        contractView.Exit += delegate
        {
            crrPage = teamPage;
            teamPage.Reopen();
        };
        crrPage = contractView;
    };
    
    crrPage = teamPage;
}

// View Logic

Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
ApplicationConfiguration.Initialize();

Bitmap bmp = null;
PointF cursor = PointF.Empty;
Queue<bool> downQueue = new Queue<bool>();
downQueue.Enqueue(false);

var form = new Form();

form.Icon = new Icon("icon.ico");
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
    crrPage.MouseMove(cursor, 
        downQueue.Count == 1 ? 
        downQueue.Peek() :
        downQueue.Dequeue()
    );
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
    if (downQueue.Count < 4)
        downQueue.Enqueue(true);
};

pb.MouseUp += (o, e) =>
{
    if (downQueue.Count < 4)
        downQueue.Enqueue(false);
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