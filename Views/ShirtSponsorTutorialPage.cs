using System;
using System.Drawing;

namespace CBLoLManager.Views;

public class ShirtSponsorTutorialPage : BaseView
{
    protected override void draw(Bitmap bmp, Graphics g)
    {
        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        string tutorial = @"
            Informações/Tutorial sobre os patrocínios:

            Agora você montará sua camisa e escolherá os patrocínios. Você deve escolher o patrocínio principal e 
            o secundário. Imediatamente após aceitar o patrocínio você receberá uma boa quantidade de dinheiro, mas
            cuidado: Todo patrocínio está atrelado a um tempo de contrato. Caso você feche um contrato muito longo
            demorará a poder fechar um novo patrocínio e assim ganhar mais dinheiro. Cabe a você ponderar e tomar a
            melhor decisão.

            Clique em qualquer lugar para sair...
        ";

        g.DrawString(tutorial, font, Brushes.White, 
            new RectangleF(0, 0, bmp.Width, bmp.Height), format);
    }

    bool isdown = false;
    public override void MouseMove(PointF cursor, bool down)
    {
        if (down)
            isdown = true;
        
        if (!down && isdown)
            Exit();
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
    }

    public event Action Exit;
}