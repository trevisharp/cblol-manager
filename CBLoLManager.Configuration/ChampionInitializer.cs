using System.Linq;
using System.Drawing;

namespace CBLoLManager.Configuration;

using Model;

public class ChampionInitializer : Initializer
{
    public override void Initialize()
    {
        var champs = Champions.All;

        if (champs.Count() > 0)
            return;

        #region Top

        ad("Gnar", Position.TopLaner,
            control: 1, damage: 1, defence: 1, mobility: 0, range: 0, support: 0);
            
        ad("Ornn", Position.TopLaner,
            control: 1, damage: 0, defence: 2, mobility: 0, range: 0, support: 0);
            
        ad("Shen", Position.TopLaner,
            control: 0, damage: 0, defence: 2, mobility: 1, range: 0, support: 0);
            
        ad("Olaf", Position.TopLaner,
            control: 0, damage: 2, defence: 1, mobility: 0, range: 0, support: 0);
            
        ad("Jayce", Position.TopLaner,
            control: 0, damage: 2, defence: 0, mobility: 0, range: 1, support: 0);
            
        ap("Kennen", Position.TopLaner,
            control: 1, damage: 2, defence: 0, mobility: 0, range: 0, support: 0);
            
        ap("Gwen", Position.TopLaner,
            control: 0, damage: 3, defence: 0, mobility: 0, range: 0, support: 0);
            
        ap("Mordekaiser", Position.TopLaner,
            control: 0, damage: 1, defence: 2, mobility: 0, range: 0, support: 0);
        
        #endregion

        #region Jg
        
        ap("Diana", Position.Jungler,
            control: 1, damage: 2, defence: 0, mobility: 0, range: 0, support: 0);

        ap("Elise", Position.Jungler,
            control: 0, damage: 3, defence: 0, mobility: 0, range: 0, support: 0);

        ap("Maokai", Position.Jungler,
            control: 1, damage: 1, defence: 0, mobility: 0, range: 1, support: 0);

        ap("Gragas", Position.Jungler,
            control: 2, damage: 0, defence: 1, mobility: 0, range: 0, support: 0);
            
        ad("Xin Zhao", Position.Jungler,
            control: 1, damage: 0, defence: 2, mobility: 0, range: 0, support: 0);
            
        ad("Wukong", Position.Jungler,
            control: 1, damage: 1, defence: 1, mobility: 0, range: 0, support: 0);
            
        ad("Viego", Position.Jungler,
            control: 0, damage: 2, defence: 1, mobility: 0, range: 0, support: 0);
            
        ad("Hecarim", Position.Jungler,
            control: 1, damage: 0, defence: 1, mobility: 1, range: 0, support: 0);

        #endregion

        #region Mid

        ap("Azir", Position.MidLaner,
            control: 1, damage: 2, defence: 0, mobility: 0, range: 0, support: 0);
            
        ap("Akali", Position.MidLaner,
            control: 0, damage: 3, defence: 0, mobility: 0, range: 0, support: 0);
            
        ap("Galio", Position.MidLaner,
            control: 1, damage: 0, defence: 1, mobility: 1, range: 0, support: 0);
            
        ap("Swain", Position.MidLaner,
            control: 1, damage: 1, defence: 1, mobility: 0, range: 0, support: 0);
            
        ap("Syndra", Position.MidLaner,
            control: 0, damage: 2, defence: 0, mobility: 0, range: 1, support: 0);
            
        ad("Irelia", Position.MidLaner,
            control: 0, damage: 2, defence: 1, mobility: 0, range: 0, support: 0);
            
        ad("Yone", Position.MidLaner,
            control: 1, damage: 2, defence: 0, mobility: 0, range: 0, support: 0);
            
        ad("Talon", Position.MidLaner,
            control: 0, damage: 2, defence: 0, mobility: 1, range: 0, support: 0);
        
        #endregion
        
        #region Adc

        ad("Kai'Sa", Position.AdCarry,
            control: 0, damage: 1, defence: 0, mobility: 1, range: 1, support: 0);
            
        ad("Varus", Position.AdCarry,
            control: 1, damage: 1, defence: 0, mobility: 0, range: 1, support: 0);
            
        ad("Samira", Position.AdCarry,
            control: 0, damage: 3, defence: 0, mobility: 0, range: 0, support: 0);
            
        ad("Ashe", Position.AdCarry,
            control: 1, damage: 2, defence: 0, mobility: 0, range: 0, support: 0);
            
        ad("Caitlyn", Position.AdCarry,
            control: 0, damage: 1, defence: 0, mobility: 0, range: 2, support: 0);
            
        ap("Seraphine", Position.AdCarry,
            control: 1, damage: 0, defence: 0, mobility: 0, range: 1, support: 1);
            
        ap("Ziggs", Position.AdCarry,
            control: 0, damage: 2, defence: 0, mobility: 0, range: 1, support: 0);
        
        #endregion

        #region Sup

        ap("Nami", Position.Support,
            control: 1, damage: 0, defence: 0, mobility: 0, range: 1, support: 1);
            
        ap("Karma", Position.Support,
            control: 0, damage: 2, defence: 0, mobility: 0, range: 0, support: 1);
            
        ap("Lulu", Position.Support,
            control: 1, damage: 0, defence: 0, mobility: 0, range: 0, support: 2);
            
        ap("Yuumi", Position.Support,
            control: 0, damage: 0, defence: 0, mobility: 0, range: 0, support: 3);
            
        ap("Nautilus", Position.Support,
            control: 1, damage: 0, defence: 2, mobility: 0, range: 0, support: 0);
            
        ap("Leona", Position.Support,
            control: 0, damage: 0, defence: 3, mobility: 0, range: 0, support: 0);
        
        ad("Tahm Kench", Position.Support,
            control: 1, damage: 0, defence: 1, mobility: 1, range: 0, support: 0);
        
        ad("Senna", Position.Support,
            control: 0, damage: 0, defence: 0, mobility: 0, range: 1, support: 2);

        #endregion

        void ad(string name, Position role, 
            int control, int damage, int defence, int mobility, int range, int support)
            => add(name, role, true, control, damage, defence, mobility, range, support);
        
        void ap(string name, Position role, 
            int control, int damage, int defence, int mobility, int range, int support)
            => add(name, role, false, control, damage, defence, mobility, range, support);

        void add(string name, Position role, bool ad,
            int control, int damage, int defence, int mobility, int range, int support)
        {
            champs.Add(new Champion()
            {
                Name = name,
                Role = role,
                AD = ad,
                Photo = name
                    .Replace(" ", "")
                    .Replace("'", "")
                    .ToLower() + ".jpg",
                Control = control,
                Damage = damage,
                Defence = defence,
                Mobility = mobility,
                Range = range,
                Support = support
            });
        }
    }
}