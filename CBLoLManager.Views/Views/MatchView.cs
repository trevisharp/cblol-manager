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
        Exit();
    }

    public event Action Exit;
}