using System;
using System.Drawing;

namespace CBLoLManager.Views;

using Model;

public class MatchView : BaseView
{
    DraftResult draft = null;

    public MatchView(DraftResult draft)
    {
        this.draft = draft;
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);
    }

    public event Action Exit;
}