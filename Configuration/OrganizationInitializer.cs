using System.Linq;
using System.Drawing;

namespace CBLoLManager.Configuration;

using Model;

public class OrganizationInitializer : Initializer
{
    public override void Initialize()
    {
        var orgs = Organizations.All;

        if (orgs.Count() > 0)
            return;
        
        orgs.Add(new Organization()
        {
            Name = "paiN Gamming",
            Photo = "pain.png",
            MainColor = Color.Black,
            SecondColor = Color.Red,
            ThirdColor = Color.White
        });
        
        orgs.Add(new Organization()
        {
            Name = "Loud",
            Photo = "loud.png",
            MainColor = Color.Green,
            SecondColor = Color.White,
            ThirdColor = Color.Black
        });
        
        orgs.Add(new Organization()
        {
            Name = "Red Canids Kalunga",
            Photo = "red.png",
            MainColor = Color.Red,
            SecondColor = Color.White,
            ThirdColor = Color.Black
        });
        
        orgs.Add(new Organization()
        {
            Name = "Furia",
            Photo = "furia.png",
            MainColor = Color.Black,
            SecondColor = Color.White,
            ThirdColor = Color.DarkBlue
        });
        
        orgs.Add(new Organization()
        {
            Name = "Fluxo",
            Photo = "fluxo.png",
            MainColor = Color.Black,
            SecondColor = Color.White,
            ThirdColor = Color.Purple
        });
        
        orgs.Add(new Organization()
        {
            Name = "INTZ",
            Photo = "intz.png",
            MainColor = Color.White,
            SecondColor = Color.Black,
            ThirdColor = Color.Pink
        });
        
        orgs.Add(new Organization()
        {
            Name = "Los Grandes",
            Photo = "los.png",
            MainColor = Color.Orange,
            SecondColor = Color.Black,
            ThirdColor = Color.White
        });
        
        orgs.Add(new Organization()
        {
            Name = "Vivo Keyd Stars",
            Photo = "keyd.png",
            MainColor = Color.Purple,
            SecondColor = Color.Black,
            ThirdColor = Color.Yellow
        });
        
        orgs.Add(new Organization()
        {
            Name = "Kabum",
            Photo = "kabum.png",
            MainColor = Color.Orange,
            SecondColor = Color.Black,
            ThirdColor = Color.Blue
        });
        
        orgs.Add(new Organization()
        {
            Name = "Liberty",
            Photo = "liberty.png",
            MainColor = Color.Blue,
            SecondColor = Color.DarkBlue,
            ThirdColor = Color.White
        });
    }
}