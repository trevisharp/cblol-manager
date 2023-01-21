using System.Linq;
using System.Drawing;

namespace CBLoLManager.Views;

using System;
using CBLoLManager.Util;
using GameRule;
using Model;

public class ShirtSponsorPage : BaseView
{
    Shirt shirt = new Shirt();
    Sponsorship main = new Sponsorship();
    Sponsorship second = new Sponsorship();
    Team team = null;
    Image org = null;
    Image iorg = null;

    PointF[] shirtpts = null;
    PointF[] collar = null;
    PointF[] leftSleeve = null;
    PointF[] rigthSleeve = null;
    PointF[] shortLeftSleeve = null;
    PointF[] shortRigthSleeve = null;
    PointF[] hemA = null;
    PointF[] hemB = null;
    RectangleF orgLogo;
    RectangleF optOrg;
    RectangleF optCollar;
    RectangleF optCollarColor;
    RectangleF optColor;
    RectangleF optSleeve;
    RectangleF optSleeveColor;
    RectangleF optHem;
    RectangleF optHemColor;
    RectangleF optMainSponsor;
    RectangleF optSecondSponsor;
    int ms = 0;
    Sponsorship[] mainSponsorships;
    Image[] mainThumbs;
    RectangleF mainRect;
    int ss = 0;
    Sponsorship[] secondSponsorships;
    Image[] secondThumbs;
    RectangleF secondRect;
    
    ButtonView end;

    protected override void draw(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);

        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;
        
        var wid = (float)bmp.Width;
        var hei = bmp.Height * 0.8f;
        var size = 1.35f * hei;
        var ymargin = bmp.Height * 0.05f;
        var xmargin = (wid - size) / 2;

        drawOpt(optOrg, "Logo");
        drawOpt(optCollar, "Estilo da Gola");
        drawOpt(optCollarColor, "Cor da Gola");
        drawOpt(optColor, "Cor da Camisa");
        drawOpt(optSleeve, "Estilo da Manga");
        drawOpt(optSleeveColor, "Cor da Manga");
        drawOpt(optHem, "Estilo Principal");
        drawOpt(optHemColor, "Cor Principal");
        drawOpt(optMainSponsor, "Patrocínio Principal");
        drawOpt(optSecondSponsor, "Patrocínio Secundário");

        g.FillPolygon(new SolidBrush(getColor(this.shirt.Color)), shirtpts);
        
        switch (this.shirt.HemStyle)
        {
            case 0:
                break;
            case 1:
                g.FillPolygon(new SolidBrush(getColor(this.shirt.HemColor)), hemA);
                break;
            case 2:
                g.FillPolygon(new SolidBrush(getColor(this.shirt.HemColor)), hemB);
                break;
        }

        switch (this.shirt.CollarStyle)
        {
            case 0:
                break;
            case 1:
                g.FillPolygon(new SolidBrush(getColor(this.shirt.CollarColor)), collar);
                break;
        }
        
        switch (this.shirt.SleeveStyle)
        {
            case 0:
                break;
            case 1:
                g.FillPolygon(new SolidBrush(getColor(this.shirt.SleeveColor)), leftSleeve);
                g.FillPolygon(new SolidBrush(getColor(this.shirt.SleeveColor)), rigthSleeve);
                break;
            case 2:
                g.FillPolygon(new SolidBrush(getColor(this.shirt.SleeveColor)), shortLeftSleeve);
                g.FillPolygon(new SolidBrush(getColor(this.shirt.SleeveColor)), shortRigthSleeve);
                break;
        }

        var img = this.mainThumbs[ms % mainSponsorships.Length];
        mainRect = new RectangleF(mainRect.X,
            mainRect.Y, mainRect.Width, mainRect.Width / img.Width * img.Height);
        g.DrawImage(img, mainRect);

        img = this.secondThumbs[ss % secondSponsorships.Length];
        secondRect = new RectangleF(secondRect.X,
            secondRect.Y, secondRect.Width, secondRect.Width / img.Width * img.Height);
        g.DrawImage(this.secondThumbs[ss % secondSponsorships.Length], secondRect);

        g.DrawPolygon(Pens.White, shirtpts);
        g.DrawImage(this.shirt.Alternative ? iorg : org, orgLogo);

        var crrmain = mainSponsorships[ms % mainSponsorships.Length];
        var crrseco = secondSponsorships[ss % secondSponsorships.Length];
        string text =
            $"Patrocínio Principal: {crrmain.Sponsor.Name}\n" +
            $"Duração do contrato: {crrmain.Duration / 24} (splits)\n" +
            $"Valor a ganhar: {Formatter.FormatMoney(crrmain.Value)}\n" +
            "\n" +
            $"Patrocínio Secundário: {crrseco.Sponsor.Name}\n" +
            $"Duração do contrato: {crrseco.Duration / 24} (splits)\n" +
            $"Valor a ganhar: {Formatter.FormatMoney(crrseco.Value)}\n";

        g.DrawString(text, font, Brushes.White, 
            new RectangleF(5, bmp.Height / 2 + 5, xmargin, bmp.Height / 2 - 10),
            format);

        end.Draw(bmp, g);

        void drawOpt(RectangleF opt, string text = "")
        {
            g.FillPolygon(Brushes.BlueViolet, new PointF[]
            {
                opt.Location,
                new PointF(opt.Right, (opt.Top + opt.Bottom) / 2),
                new PointF(opt.Left, opt.Bottom),
            });
            g.DrawString(text, font, Brushes.White, new RectangleF(opt.X + 60, opt.Y, 120, 60), format);
        }

        Color getColor(int color)
        {
            if (color == 0)
                return team.Organization.MainColor;
            else if (color == 1)
                return team.Organization.SecondColor;
            else
                return team.Organization.ThirdColor;
        }
    }

    bool isdown = false;
    public override void MouseMove(PointF cursor, bool down)
    {
        end.MouseMove(cursor, down);

        if (down)
            isdown = true;
        
        if (!down && isdown)
            isdown = false;
        else return;

        if (optOrg.Contains(cursor))
            this.shirt.Alternative = !this.shirt.Alternative;

        if (optCollar.Contains(cursor))
            this.shirt.CollarStyle = (this.shirt.CollarStyle + 1) % 2;

        if (optCollarColor.Contains(cursor))
            this.shirt.CollarColor = (this.shirt.CollarColor + 1) % 3;

        if (optHem.Contains(cursor))
            this.shirt.HemStyle = (this.shirt.HemStyle + 1) % 3;

        if (optHemColor.Contains(cursor))
            this.shirt.HemColor = (this.shirt.HemColor + 1) % 3;

        if (optSleeve.Contains(cursor))
            this.shirt.SleeveStyle = (this.shirt.SleeveStyle + 1) % 3;

        if (optSleeveColor.Contains(cursor))
            this.shirt.SleeveColor = (this.shirt.SleeveColor + 1) % 3;
        
        if (optColor.Contains(cursor))
            this.shirt.Color = (this.shirt.Color + 1) % 3;

        if (optMainSponsor.Contains(cursor))
            main = mainSponsorships[(++ms % mainSponsorships.Length)];

        if (optSecondSponsor.Contains(cursor))
            second = secondSponsorships[(++ss % secondSponsorships.Length)];
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        team = Game.Current.Team;
        org = Bitmap.FromFile("Img/" + team.Organization.Photo);
        iorg = Bitmap.FromFile("Img/i" + team.Organization.Photo);

        var wid = (float)bmp.Width;
        var hei = bmp.Height * 0.8f;
        var size = 1.35f * hei;
        var ymargin = bmp.Height * 0.05f;
        var xmargin = (wid - size) / 2;

        float r2 = 0.05f * 0.05f;

        collar = new PointF[]
        {
            new PointF(xmargin + size * .4f, ymargin),
            new PointF(xmargin + size * .425f, ymargin + size * MathF.Sqrt(r2 - 0.075f * 0.075f / 3)),
            new PointF(xmargin + size * .45f, ymargin + size * MathF.Sqrt(r2 - 0.05f * 0.05f / 3)),
            new PointF(xmargin + size * .475f, ymargin + size * MathF.Sqrt(r2 - 0.025f * 0.025f / 3)),
            new PointF(xmargin + size * .5f, ymargin + size * MathF.Sqrt(r2)),
            new PointF(xmargin + size * .525f, ymargin + size * MathF.Sqrt(r2 - 0.025f * 0.025f / 3)),
            new PointF(xmargin + size * .55f, ymargin + size * MathF.Sqrt(r2 - 0.05f * 0.05f / 3)),
            new PointF(xmargin + size * .575f, ymargin + size * MathF.Sqrt(r2 - 0.075f * 0.075f / 3)),
            new PointF(xmargin + size * .6f, ymargin),
            new PointF(xmargin + size * .625f, ymargin),
            new PointF(xmargin + size * .6f, ymargin + size * 0.02f),
            new PointF(xmargin + size * .575f, ymargin + size * MathF.Sqrt(r2 - 0.075f * 0.075f / 3) + size * 0.02f),
            new PointF(xmargin + size * .55f, ymargin + size * MathF.Sqrt(r2 - 0.05f * 0.05f / 3) + size * 0.02f),
            new PointF(xmargin + size * .525f, ymargin + size * MathF.Sqrt(r2 - 0.025f * 0.025f / 3) + size * 0.02f),
            new PointF(xmargin + size * .5f, ymargin + size * MathF.Sqrt(r2) + size * 0.02f),
            new PointF(xmargin + size * .475f, ymargin + size * MathF.Sqrt(r2 - 0.025f * 0.025f / 3) + size * 0.02f),
            new PointF(xmargin + size * .45f, ymargin + size * MathF.Sqrt(r2 - 0.05f * 0.05f / 3) + size * 0.02f),
            new PointF(xmargin + size * .425f, ymargin + size * MathF.Sqrt(r2 - 0.075f * 0.075f / 3) + size * 0.02f),
            new PointF(xmargin + size * .375f, ymargin),
        };

        shirtpts = new PointF[]
        {
            new PointF(xmargin + size * .2f, ymargin),
            new PointF(xmargin + size * .4f, ymargin),

            new PointF(xmargin + size * .425f, ymargin + size * MathF.Sqrt(r2 - 0.075f * 0.075f / 3)),
            new PointF(xmargin + size * .45f, ymargin + size * MathF.Sqrt(r2 - 0.05f * 0.05f / 3)),
            new PointF(xmargin + size * .475f, ymargin + size * MathF.Sqrt(r2 - 0.025f * 0.025f / 3)),
            new PointF(xmargin + size * .5f, ymargin + size * MathF.Sqrt(r2)),
            new PointF(xmargin + size * .525f, ymargin + size * MathF.Sqrt(r2 - 0.025f * 0.025f / 3)),
            new PointF(xmargin + size * .55f, ymargin + size * MathF.Sqrt(r2 - 0.05f * 0.05f / 3)),
            new PointF(xmargin + size * .575f, ymargin + size * MathF.Sqrt(r2 - 0.075f * 0.075f / 3)),

            new PointF(xmargin + size * .6f, ymargin),
            new PointF(xmargin + size * .8f, ymargin),
            new PointF(xmargin + size, ymargin + size * .2f),
            new PointF(xmargin + size * .9f, ymargin + size * .3f),
            new PointF(xmargin + size * .8f, ymargin + size * .2f),
            new PointF(xmargin + size * .8f, ymargin + hei),
            new PointF(xmargin + size * .2f, ymargin + hei),
            new PointF(xmargin + size * .2f, ymargin + size * .2f),
            new PointF(xmargin + size * .1f, ymargin + size * .3f),
            new PointF(xmargin + size * .0f, ymargin + size * .2f),
            new PointF(xmargin + size * .2f, ymargin),
        };

        rigthSleeve = new PointF[]
        {
            new PointF(xmargin + size * .8f, ymargin),
            new PointF(xmargin + size, ymargin + size * .2f),
            new PointF(xmargin + size * .9f, ymargin + size * .3f),
            new PointF(xmargin + size * .8f, ymargin + size * .2f),
        };

        leftSleeve = new PointF[]
        {
            new PointF(xmargin + size * .2f, ymargin),
            new PointF(xmargin + size * .0f, ymargin + size * .2f),
            new PointF(xmargin + size * .1f, ymargin + size * .3f),
            new PointF(xmargin + size * .2f, ymargin + size * .2f),
        };

        shortRigthSleeve = new PointF[]
        {
            new PointF(xmargin + size * .95f, ymargin + size * .15f),
            new PointF(xmargin + size, ymargin + size * .2f),
            new PointF(xmargin + size * .9f, ymargin + size * .3f),
            new PointF(xmargin + size * .85f, ymargin + size * .25f),
        };

        shortLeftSleeve = new PointF[]
        {
            new PointF(xmargin + size * .05f, ymargin + size * .15f),
            new PointF(xmargin + size * .0f, ymargin + size * .2f),
            new PointF(xmargin + size * .1f, ymargin + size * .3f),
            new PointF(xmargin + size * .15f, ymargin + size * .25f),
        };

        hemA = new PointF[]
        {
            new PointF(xmargin + size * .3f, ymargin),
            new PointF(xmargin + size * .4f, ymargin),

            new PointF(xmargin + size * .425f, ymargin + size * MathF.Sqrt(r2 - 0.075f * 0.075f / 3)),
            new PointF(xmargin + size * .45f, ymargin + size * MathF.Sqrt(r2 - 0.05f * 0.05f / 3)),
            new PointF(xmargin + size * .475f, ymargin + size * MathF.Sqrt(r2 - 0.025f * 0.025f / 3)),
            new PointF(xmargin + size * .5f, ymargin + size * MathF.Sqrt(r2)),
            new PointF(xmargin + size * .525f, ymargin + size * MathF.Sqrt(r2 - 0.025f * 0.025f / 3)),
            new PointF(xmargin + size * .55f, ymargin + size * MathF.Sqrt(r2 - 0.05f * 0.05f / 3)),
            new PointF(xmargin + size * .575f, ymargin + size * MathF.Sqrt(r2 - 0.075f * 0.075f / 3)),

            new PointF(xmargin + size * .6f, ymargin),
            new PointF(xmargin + size * .7f, ymargin),
            new PointF(xmargin + size * .7f, ymargin + hei),
            new PointF(xmargin + size * .3f, ymargin + hei),
            new PointF(xmargin + size * .3f, ymargin),
        };

        hemB = new PointF[]
        {
            new PointF(xmargin + size * .2f, ymargin + hei * 0.9f),
            new PointF(xmargin + size * .8f, ymargin + hei * 0.9f),
            new PointF(xmargin + size * .8f, ymargin + hei),
            new PointF(xmargin + size * .2f, ymargin + hei),
        };

        SponsorshipSystem sys = new SponsorshipSystem();
        mainSponsorships = sys.GetMain(team);
        secondSponsorships = sys.GetSecond(team);

        mainThumbs = mainSponsorships
            .Select(s => Bitmap.FromFile("Img/" + s.Sponsor.Photo))
            .ToArray();
        secondThumbs = secondSponsorships
            .Select(s => Bitmap.FromFile("Img/" + s.Sponsor.Photo))
            .ToArray();

        mainRect = new RectangleF(
            xmargin + size * .3f,
            ymargin + hei * .6f, 
            size * .4f, size * .4f);

        secondRect = new RectangleF(
            xmargin + size * .25f, 
            ymargin + size * .05f, 
            size * .1f, size * .1f);

        orgLogo = new RectangleF(
            xmargin + size * .65f, 
            ymargin + size * .05f, 
            size * .1f, size * .1f);

        optOrg = new RectangleF(xmargin + size + 40, ymargin + 40, 60, 60);
        optCollar = new RectangleF(xmargin + size + 40, ymargin + 140, 60, 60);
        optCollarColor = new RectangleF(xmargin + size + 40, ymargin + 240, 60, 60);
        optColor = new RectangleF(xmargin + size + 40, ymargin + 340, 60, 60);
        optSleeve  = new RectangleF(xmargin + size + 40, ymargin + 440, 60, 60);
        optSleeveColor = new RectangleF(xmargin + size + 40, ymargin + 540, 60, 60);
        optHem = new RectangleF(xmargin + size + 40, ymargin + 640, 60, 60);
        optHemColor = new RectangleF(xmargin + size + 40, ymargin + 740, 60, 60);
        optMainSponsor = new RectangleF(xmargin + size + 40, ymargin + 840, 60, 60);
        optSecondSponsor = new RectangleF(xmargin + size + 40, ymargin + 940, 60, 60);

        end = new ButtonView();
        end.Rect = new RectangleF(5, bmp.Height - 80, 200, 60);
        end.SelectedColor = Brushes.BlueViolet;
        end.Color = Brushes.White;
        end.Label = "Finalizar";
        end.OnClick += delegate
        {
            var shirtBmp = new Bitmap(405, 300);
            var _g = Graphics.FromImage(shirtBmp);
            _g.DrawImage(bmp, new RectangleF(0, 0, shirtBmp.Width, shirtBmp.Height),
                new RectangleF(xmargin, ymargin, size, hei), GraphicsUnit.Pixel);
            shirt.Photo = shirtBmp;

            team.Shirt = this.shirt;
            team.MainSponsorship = mainSponsorships[ms % mainSponsorships.Length];
            team.SecondSponsorship = secondSponsorships[ss % secondSponsorships.Length];
            team.Money += team.MainSponsorship.Value;
            team.Money += team.SecondSponsorship.Value;
            Exit();
        };
    }

    public event Action Exit;
}