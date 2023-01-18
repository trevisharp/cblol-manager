using System;

namespace CBLoLManager.Model;

[Serializable]
public class Player
{
    public string Nickname { get; set; }
    public string Name { get; set; }
    public Position Role { get; set; }
    public Nationality Nationality { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Photo { get; set; }

    public int LanePhase { get; set; }
    public int Mentality { get; set; }
    public int GameVision { get; set; }
    public int MechanicSkill { get; set; }
    public int Leadership { get; set; }
    public int TeamFigth { get; set; }
}