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

var player = Players.All
    .Skip(2)
    .FirstOrDefault();

PlayerPage page = new PlayerPage(player, new PointF(100, 100), 1000f);

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
    
    // Player tin = new Player();

    // tin.BirthDate = new DateTime(1997, 10, 14);
    // tin.Name = "Choi Eui-seok";
    // tin.Nickname = "Wizer";
    // tin.Photo = "wizer.png";
    // tin.Nationality = Nationality.KR;
    // tin.Role = Position.TopLaner;
    // tin.LanePhase = 90;
    // tin.Mentality = 80;
    // tin.GameVision = 80;
    // tin.MechanicSkill = 90;
    // tin.Leadership = 70;
    // tin.TeamFigth = 85;

    // Players.All.Add(tin);
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