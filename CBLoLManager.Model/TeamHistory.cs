using System;

namespace CBLoLManager.Model;

[Serializable]
public class TeamHistory
{
    public int Year { get; set; }
    public int Split { get; set; }
    public int ShirtSale { get; set; }
    public int ShirtSaleTotal { get; set; }
    public float PlayerWage { get; set; }
    public float InitialGold { get; set; }
    public int Followers { get; set; }
    public int CBLoLAward { get; set; }
    public int PlayOffsPlayed { get; set; }
    public int Vices { get; set; }
    public int Titles { get; set; }
    public int RegularPhaseTitles { get; set; }
}