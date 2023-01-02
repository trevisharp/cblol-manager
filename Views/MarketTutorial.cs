using System;
using System.Drawing;

namespace CBLoLManager.Views;

public class MarketTutorial : BaseView
{
    protected override void draw(Bitmap bmp, Graphics g)
    {
        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        string tutorial = @"
            O Mercado é o lugar onde você pode comprar novos jogadores para seu time.
            Existem três tipos de jogadores disponíveis:

            -Free Agent: Jogadores em time que não possuem multa e podem ser contratados.

            -Ouvindo Propostas: Jogadores que estão em times e ainda possuem multa, mas foram
            liberados por seus times para escutarem propostas de outros. Você precisa pagar uma
            multa para contratá-los.

            -Fim de contrato: Jogadores que estão no final de seu contrato acertando detalhes para
            a renovação com suas organizações. Porém, você tem a oportunidade de fazer uma oferta
            tentadora e roubá-lo para você.

            Você deve oficializar uma proposta e pode fazer com vários jogadores. Você pode receber
            contra-propostas e no aceite da proposta você ainda pode desistir da negociação, caso
            outra mais interessante de certo.

            A fase de contratação dura 15 rodadas, você pode fazer uma proposta por rodada.

            Na primeira fase de contratação todos os jogadores estão Free Agent.

            Clique em qualquer lugar para sair...
        ";

        g.DrawString(tutorial, font, Brushes.White, 
            new RectangleF(0, 0, bmp.Width, bmp.Height), format);
    }

    bool isdown = false;
    public override void MoseMove(PointF cursor, bool down)
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