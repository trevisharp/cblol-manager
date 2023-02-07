using System;

namespace CBLoLManager.Model;

[Serializable]
public class PlayOffs
{
    public Team WinnerBracketA { get; set; }
    public Team WinnerBracketB { get; set; }
    public Team WinnerBracketC { get; set; }
    public Team WinnerBracketD { get; set; }

    public Team WinnerBracketFinalA { get; set; }
    public Team WinnerBracketFinalB { get; set; }

    public Team LoserBracketA { get; set; }
    public Team LoserBracketB { get; set; }

    public Team LoserBracketSecondPhaseA { get; set; }
    public Team LoserBracketSecondPhaseB { get; set; }

    public Team LoserBracketThirdPhaseA { get; set; }
    public Team LoserBracketThirdPhaseB { get; set; }

    public Team LoserBracketFinalA { get; set; }
    public Team LoserBracketFinalB { get; set; }

    public Team FinalA { get; set; }
    public Team FinalB { get; set; }
    
    public Team Champion { get; set; }
}