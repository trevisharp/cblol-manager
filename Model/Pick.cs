namespace CBLoLManager.Model;

public class Pick
{
    private Champion pick = null;

    public Champion OptionA { get; set; }
    public Champion OptionB { get; set; }

    public Champion Selected => pick;

    public void MakeAPick(bool chooseOptionA)
        => pick = chooseOptionA ? OptionA : OptionB;
}