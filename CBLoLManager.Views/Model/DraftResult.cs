using System.Collections.Generic;

namespace CBLoLManager.Model;

public class DraftResult
{
    public Team TeamA { get; set; }
    public Team TeamB { get; set; }

    public List<Champion> TeamADraft { get; private set; } = new List<Champion>();
    public List<Champion> TeamBDraft { get; private set; } = new List<Champion>();
}