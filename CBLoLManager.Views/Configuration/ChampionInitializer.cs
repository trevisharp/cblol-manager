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

        // TOP

        champs.Add(new Champion()
        {
            Name = "Aatrox",
            Role = Position.TopLaner,
            AD = true,
            Photo = "aatrox.jpg",

            Control = 0,
            Damage = 2,
            Defence = 1,
            Mobility = 0,
            Range = 0,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Kennen",
            Role = Position.TopLaner,
            AD = false,
            Photo = "kennen.jpg",

            Control = 2,
            Damage = 1,
            Defence = 0,
            Mobility = 0,
            Range = 0,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Gnar",
            Role = Position.TopLaner,
            AD = true,
            Photo = "gnar.jpg",

            Control = 1,
            Damage = 0,
            Defence = 1,
            Mobility = 0,
            Range = 1,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Mordekaiser",
            Role = Position.TopLaner,
            AD = false,
            Photo = "mordekaiser.jpg",

            Control = 1,
            Damage = 1,
            Defence = 1,
            Mobility = 0,
            Range = 0,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Ornn",
            Role = Position.TopLaner,
            AD = true,
            Photo = "ornn.jpg",

            Control = 1,
            Damage = 0,
            Defence = 2,
            Mobility = 0,
            Range = 0,
            Support = 0
        });
    
        champs.Add(new Champion()
        {
            Name = "Shen",
            Role = Position.TopLaner,
            AD = true,
            Photo = "shen.jpg",

            Control = 0,
            Damage = 0,
            Defence = 2,
            Mobility = 1,
            Range = 0,
            Support = 0
        });

        // MID
        
        champs.Add(new Champion()
        {
            Name = "Swain",
            Role = Position.MidLaner,
            AD = false,
            Photo = "swain.jpg",

            Control = 1,
            Damage = 1,
            Defence = 1,
            Mobility = 0,
            Range = 0,
            Support = 0
        });
        
        champs.Add(new Champion()
        {
            Name = "Taliyah",
            Role = Position.MidLaner,
            AD = false,
            Photo = "taliyah.jpg",

            Control = 1,
            Damage = 1,
            Defence = 0,
            Mobility = 1,
            Range = 0,
            Support = 0
        });
        
        champs.Add(new Champion()
        {
            Name = "Azir",
            Role = Position.MidLaner,
            AD = false,
            Photo = "azir.jpg",

            Control = 1,
            Damage = 1,
            Defence = 0,
            Mobility = 0,
            Range = 1,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Ryze",
            Role = Position.MidLaner,
            AD = false,
            Photo = "ryze.jpg",

            Control = 0,
            Damage = 2,
            Defence = 0,
            Mobility = 1,
            Range = 0,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Zed",
            Role = Position.MidLaner,
            AD = true,
            Photo = "zed.jpg",

            Control = 0,
            Damage = 3,
            Defence = 0,
            Mobility = 0,
            Range = 0,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Syndra",
            Role = Position.MidLaner,
            AD = false,
            Photo = "syndra.jpg",

            Control = 1,
            Damage = 1,
            Defence = 0,
            Mobility = 0,
            Range = 1,
            Support = 0
        });

        // JG

        champs.Add(new Champion()
        {
            Name = "Wukong",
            Role = Position.Jungler,
            AD = true,
            Photo = "wukong.jpg",

            Control = 1,
            Damage = 1,
            Defence = 1,
            Mobility = 0,
            Range = 0,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Xin Zhao",
            Role = Position.Jungler,
            AD = true,
            Photo = "xinzhao.jpg",

            Control = 1,
            Damage = 0,
            Defence = 2,
            Mobility = 0,
            Range = 0,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Hecarim",
            Role = Position.Jungler,
            AD = true,
            Photo = "hecarim.jpg",

            Control = 1,
            Damage = 0,
            Defence = 1,
            Mobility = 1,
            Range = 0,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Lillia",
            Role = Position.Jungler,
            AD = false,
            Photo = "lillia.jpg",

            Control = 1,
            Damage = 1,
            Defence = 0,
            Mobility = 0,
            Range = 1,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Fiddlesticks",
            Role = Position.Jungler,
            AD = false,
            Photo = "fiddlesticks.jpg",

            Control = 2,
            Damage = 1,
            Defence = 0,
            Mobility = 0,
            Range = 0,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Diana",
            Role = Position.Jungler,
            AD = false,
            Photo = "diana.jpg",

            Control = 1,
            Damage = 2,
            Defence = 0,
            Mobility = 0,
            Range = 0,
            Support = 0
        });

        // ADC

        champs.Add(new Champion()
        {
            Name = "Caitlyn",
            Role = Position.AdCarry,
            AD = true,
            Photo = "caitlyn.jpg",

            Control = 0,
            Damage = 1,
            Defence = 0,
            Mobility = 0,
            Range = 2,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Jinx",
            Role = Position.AdCarry,
            AD = true,
            Photo = "jinx.jpg",

            Control = 0,
            Damage = 2,
            Defence = 0,
            Mobility = 0,
            Range = 1,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Kai'Sa",
            Role = Position.AdCarry,
            AD = true,
            Photo = "kaisa.jpg",

            Control = 0,
            Damage = 1,
            Defence = 0,
            Mobility = 1,
            Range = 1,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Ashe",
            Role = Position.AdCarry,
            AD = true,
            Photo = "ashe.jpg",

            Control = 1,
            Damage = 1,
            Defence = 0,
            Mobility = 0,
            Range = 1,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Seraphine",
            Role = Position.AdCarry,
            AD = false,
            Photo = "seraphine.jpg",

            Control = 2,
            Damage = 1,
            Defence = 0,
            Mobility = 0,
            Range = 0,
            Support = 0
        });
        
        champs.Add(new Champion()
        {
            Name = "Samira",
            Role = Position.AdCarry,
            AD = true,
            Photo = "samira.jpg",

            Control = 0,
            Damage = 3,
            Defence = 0,
            Mobility = 0,
            Range = 0,
            Support = 0
        });

        // SUP

        champs.Add(new Champion()
        {
            Name = "Leona",
            Role = Position.Support,
            AD = false,
            Photo = "leona.jpg",

            Control = 1,
            Damage = 0,
            Defence = 2,
            Mobility = 0,
            Range = 0,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Senna",
            Role = Position.Support,
            AD = true,
            Photo = "senna.jpg",

            Control = 0,
            Damage = 1,
            Defence = 0,
            Mobility = 0,
            Range = 1,
            Support = 1
        });

        champs.Add(new Champion()
        {
            Name = "Tahm Kench",
            Role = Position.Support,
            AD = true,
            Photo = "tahmkench.jpg",

            Control = 1,
            Damage = 0,
            Defence = 1,
            Mobility = 1,
            Range = 0,
            Support = 0
        });

        champs.Add(new Champion()
        {
            Name = "Lulu",
            Role = Position.Support,
            AD = false,
            Photo = "lulu.jpg",

            Control = 1,
            Damage = 0,
            Defence = 0,
            Mobility = 0,
            Range = 0,
            Support = 2
        });

        champs.Add(new Champion()
        {
            Name = "Nautilus",
            Role = Position.Support,
            AD = false,
            Photo = "nautilus.jpg",

            Control = 1,
            Damage = 0,
            Defence = 2,
            Mobility = 0,
            Range = 0,
            Support = 0
        });
        
        champs.Add(new Champion()
        {
            Name = "Nami",
            Role = Position.Support,
            AD = false,
            Photo = "nami.jpg",

            Control = 1,
            Damage = 1,
            Defence = 0,
            Mobility = 0,
            Range = 0,
            Support = 1
        });
    }
}