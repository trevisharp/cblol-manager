using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using CBLoLManager.Model;
using CBLoLManager.Views;
using CBLoLManager.GameRule;

Graphics g = null;

// App Login

BaseView crrPage = null;

TeamSelectorPage teamSelectorPage = null;
TeamPage teamPage = null;
PlayerMarket market = null;

bool firstTimeInMarket = true;

// Team Selector Page Logic
teamSelectorPage = new TeamSelectorPage();
crrPage = teamSelectorPage;
teamSelectorPage.OnSelect += org =>
{
    Team team = new Team();
    team.Organization = org;

    Game.Current.Team = team;
    foreach (var x in Organizations.All
        .Where(o => o.Name != org.Name))
    {
        Game.Current.Others.Add(new Team()
        {
            Organization = x
        });
    }
    Game.Current.FreeAgent.AddRange(Players.All);

    teamPage = new TeamPage(team);
    teamPage.OnOpenMarket += () =>
    {
        if (firstTimeInMarket)
        {
            var tutorial = new MarketTutorial();
            tutorial.Exit += delegate
            {
                market = new PlayerMarket();
                crrPage = market;
            };
            crrPage = tutorial;
            return;
        }
        
        market = new PlayerMarket();
        crrPage = market;
    };

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
    crrPage.MoseMove(cursor, down);
    pb.Refresh();
};

form.Load += delegate
{
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