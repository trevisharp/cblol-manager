using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using CBLoLManager.Model;
using CBLoLManager.Views;

Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
ApplicationConfiguration.Initialize();

Bitmap bmp = null;
Graphics g = null;
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

PlayerCarrousel page = null;

tm.Tick += delegate
{
    g.Clear(Color.Black);
    page.MoseMove(cursor, down);
    page.Draw(bmp, g);
    pb.Refresh();
};

form.Load += delegate
{
    bmp = new Bitmap(pb.Width, pb.Height);
    g = Graphics.FromImage(bmp);
    g.Clear(Color.Black);
    pb.Image = bmp;
    tm.Start();
    
    // Player player = new Player();

    // player.BirthDate = new DateTime(1997, 8, 23);
    // player.Name = "Bruno Miyaguchi";
    // player.Nickname = "Goku";
    // player.Photo = "goku.png";
    // player.Nationality = Nationality.BR;
    // player.Role = Position.MidLaner;
    // player.LanePhase = 85;
    // player.Mentality = 80;
    // player.GameVision = 85;
    // player.MechanicSkill = 85;
    // player.Leadership = 80;
    // player.TeamFigth = 85;
    
    // Players.All.Add(player);
    
    page = new PlayerCarrousel(new PointF(20, 20), 1400f, Players.All);
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