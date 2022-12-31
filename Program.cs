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

var form = new Form();

form.WindowState = FormWindowState.Maximized;
form.FormBorderStyle = FormBorderStyle.None;

PictureBox pb = new PictureBox();
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

var tm = new System.Windows.Forms.Timer();
tm.Interval = 20;

Player player = null;
PlayerPage page = null;

tm.Tick += delegate
{
    g.Clear(Color.Black);
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
    
    player = new Player();

    player.BirthDate = new DateTime(1999, 9, 9);
    player.Name = "Gabriel Vinicius Saes de Lemos";
    player.Nickname = "Aegis";
    player.Photo = "aegis.png";
    player.Nationality = Nationality.BR;
    player.Role = Position.Support;
    player.LanePhase = 90;
    player.Mentality = 80;
    player.GameVision = 85;
    player.MechanicSkill = 85;
    player.Leadership = 80;
    player.TeamFigth = 85;

    Players.All.Add(player);
    
    page = new PlayerPage(player, new PointF(100, 100), 1000f);
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