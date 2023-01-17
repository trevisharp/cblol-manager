using System;
using System.Linq;

namespace CBLoLManager.Model;

[Serializable]
public class PlayOffs
{
    public Team[] Teams { get; private set; }

    public int WinnerBracketA { get; private set; }
    public int WinnerBracketB { get; private set; }
    public int WinnerBracketC { get; private set; }
    public int WinnerBracketD { get; private set; }

    public int WinnerBracketFinalA { get; private set; }
    public int WinnerBracketFinalB { get; private set; }

    public int LoserBracketA { get; private set; }
    public int LoserBracketB { get; private set; }

    public int LoserBracketSecondPhaseA { get; private set; }
    public int LoserBracketSecondPhaseB { get; private set; }

    public int LoserBracketThirdPhaseA { get; private set; }
    public int LoserBracketThirdPhaseB { get; private set; }

    public int LoserBracketFinalA { get; private set; }
    public int LoserBracketFinalB { get; private set; }

    public int FinalA { get; private set; }
    public int FinalB { get; private set; }
    
    public int Champion { get; private set; }
}