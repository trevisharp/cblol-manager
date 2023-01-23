using System.Diagnostics;

UpdateSystem sys = null;

ApplicationConfiguration.Initialize();

var form = new Form();
form.FormBorderStyle = FormBorderStyle.None;

Bitmap bmp = null;
Graphics g = null;
Image logo = null;
StringFormat format = null;
Rectangle logoRect = Rectangle.Empty;
Rectangle infoRect = Rectangle.Empty;
Rectangle progRect = Rectangle.Empty;

PictureBox pb = new PictureBox();
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

var timer = new System.Windows.Forms.Timer();
timer.Interval = 20;

timer.Tick += delegate
{
    tick();
    pb.Refresh();
};

form.Load += async delegate
{
    var screen = Screen.FromControl(form);
    int wid = screen.Bounds.Width;
    int hei = screen.Bounds.Height;
    int formWid = 20 * wid / 100;
    int formHei = 40 * hei / 100;

    form.Left = (wid - formWid) / 2;
    form.Top = (hei - formHei) / 2;
    form.Size = new Size(formWid, formHei);

    format = new StringFormat();
    format.LineAlignment = StringAlignment.Center;
    format.Alignment = StringAlignment.Far;

    bmp = new Bitmap(form.Width, form.Height);
    g = Graphics.FromImage(bmp);
    logo = Image.FromFile("logo1.png");
    pb.Image = bmp;
    logoRect = new Rectangle(0, 0, formWid, formWid);
    infoRect = new Rectangle(0, logoRect.Bottom - 40, logoRect.Width, 40);
    progRect = new Rectangle(0, logoRect.Bottom, logoRect.Width, hei - formHei);
    timer.Start();

    await update();
};

Application.Run(form);

async Task update()
{
    sys = UpdateSystem.Start();
    await sys.FindFiles();
    await sys.DownloadFiles();
    Process process = new Process();
    process.StartInfo.FileName = "App/cblol-manager.exe";
    process.StartInfo.WorkingDirectory = "App";
    process.Start();
    Application.Exit();
}

void tick()
{
    g.Clear(Color.White);
    g.DrawImage(logo, logoRect,
        new Rectangle(0, 0, logo.Width, logo.Height),
        GraphicsUnit.Pixel);
    
    if (sys == null)
        return;
    
    if (!sys.Started)
    {
        g.DrawString($"Buscando atualizações...",
            new Font(FontFamily.GenericMonospace, 20f),
            Brushes.White, infoRect, format);
    }
    else if (sys.FindingFilesProgress < 100)
    {
        g.DrawString(sys.FilesToDownload.ToString(),
            new Font(FontFamily.GenericMonospace, 20f),
            Brushes.White, infoRect, format);
        g.FillRectangle(Brushes.LightGreen, 
            new Rectangle(progRect.Location, new Size(
                progRect.Width * sys.FindingFilesProgress / 100,
                progRect.Height
            )));
    }
    else if (sys.FilesDownloadProgress < 100)
    {
        g.DrawString($"{sys.FilesDownloaded} de {sys.FilesToDownload}",
            new Font(FontFamily.GenericMonospace, 20f),
            Brushes.White, infoRect, format);
        g.FillRectangle(Brushes.LightGreen, 
            new Rectangle(progRect.Location, new Size(
                progRect.Width * sys.FilesDownloadProgress / 100,
                progRect.Height
            )));
    }
    else
    {
        g.DrawString($"Atualizado! Abrindo...",
            new Font(FontFamily.GenericMonospace, 20f),
            Brushes.White, infoRect, format);
    }
}