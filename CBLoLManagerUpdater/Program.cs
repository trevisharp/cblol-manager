ApplicationConfiguration.Initialize();

var form = new Form();
form.FormBorderStyle = FormBorderStyle.None;

Bitmap bmp = null;
Graphics g = null;
Image logo = null;

PictureBox pb = new PictureBox();
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

form.Load += delegate
{
    var screen = Screen.FromControl(form);
    int wid = screen.Bounds.Width;
    int hei = screen.Bounds.Height;
    int formWid = 20 * wid / 100;
    int formHei = 40 * hei / 100;

    form.Left = (wid - formWid) / 2;
    form.Top = (hei - formHei) / 2;
    form.Size = new Size(formWid, formHei);

    form.TopMost = true;

    bmp = new Bitmap(form.Width, form.Height);
    g = Graphics.FromImage(bmp);
    logo = Image.FromFile("logo1.png");
    pb.Image = bmp;

    g.DrawImage(logo,
        new Rectangle(0, 0, formWid, formWid),
        new Rectangle(0, 0, logo.Width, logo.Height),
        GraphicsUnit.Pixel);
    
    pb.Refresh();
};

Application.Run(form);