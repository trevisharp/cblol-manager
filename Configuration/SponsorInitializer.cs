using System.Linq;
using System.Drawing;

namespace CBLoLManager.Configuration;

using Model;

public class SponsorInitializer : Initializer
{
    public override void Initialize()
    {
        var sponsors = Sponsors.All;

        if (sponsors.Count() > 0)
            return;
        
        sponsors.Add(new Sponsor()
        {
            Name = "Coca-Cola",
            Photo = "coca.png",
            Strong = 4,
            isMain = true
        });
        
        sponsors.Add(new Sponsor()
        {
            Name = "Kalunga",
            Photo = "kalunga.png",
            Strong = 3,
            isMain = true
        });
        
        sponsors.Add(new Sponsor()
        {
            Name = "Subway",
            Photo = "subway.png",
            Strong = 2,
            isMain = true
        });
        
        sponsors.Add(new Sponsor()
        {
            Name = "Hyper X",
            Photo = "hyperx.png",
            Strong = 1,
            isMain = true
        });

        sponsors.Add(new Sponsor()
        {
            Name = "BMW",
            Photo = "bmw.png",
            Strong = 4,
            isMain = false
        });

        sponsors.Add(new Sponsor()
        {
            Name = "Adidas",
            Photo = "adidas.png",
            Strong = 3,
            isMain = false
        });
        
        sponsors.Add(new Sponsor()
        {
            Name = "JBL",
            Photo = "jbl.png",
            Strong = 2,
            isMain = false
        });
        
        sponsors.Add(new Sponsor()
        {
            Name = "LG",
            Photo = "lg.png",
            Strong = 1,
            isMain = false
        });
            
    }
}