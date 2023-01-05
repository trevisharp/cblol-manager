using System.Linq;

namespace CBLoLManager.Configuration;

using Model;

public class OrganizationInitializer
{
    public void Initialize()
    {
        var orgs = Organizations.All;

        if (orgs.Count() > 0)
            return;
        
        Organization pain = new Organization();
        pain.Name = "paiN Gamming";
        pain.Photo = "pain.png";
        orgs.Add(pain);
        
        Organization loud = new Organization();
        loud.Name = "Loud";
        loud.Photo = "loud.png";
        orgs.Add(loud);
        
        Organization red = new Organization();
        red.Name = "Red Canids Kalunga";
        red.Photo = "red.png";
        orgs.Add(red);
        
        Organization furia = new Organization();
        furia.Name = "Furia";
        furia.Photo = "furia.png";
        orgs.Add(furia);
        
        Organization fluxo = new Organization();
        fluxo.Name = "Fluxo";
        fluxo.Photo = "fluxo.png";
        orgs.Add(fluxo);
        
        Organization intz = new Organization();
        intz.Name = "INTZ";
        intz.Photo = "intz.png";
        orgs.Add(intz);
        
        Organization los = new Organization();
        los.Name = "Los Grandes";
        los.Photo = "los.png";
        orgs.Add(los);
        
        Organization keyd = new Organization();
        keyd.Name = "Vivo Keyd Stars";
        keyd.Photo = "keyd.png";
        orgs.Add(keyd);
        
        Organization kabum = new Organization();
        kabum.Name = "Kabum";
        kabum.Photo = "kabum.png";
        orgs.Add(kabum);
        
        Organization liberty = new Organization();
        liberty.Name = "Liberty";
        liberty.Photo = "liberty.png";
        orgs.Add(liberty);
    }
}