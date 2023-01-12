using System.Drawing;

namespace CBLoLManager.Views;

public class TorunamentView : BaseView
{
    public OptionsView options = null;

    protected override void draw(Bitmap bmp, Graphics g)
    {
        
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        options = new OptionsView(
            "Ir para partida"
        );
        options.OnOptionClick += s =>
        {
            if (s == "Ir para partida")
            {
                
            }
        };
    }
}