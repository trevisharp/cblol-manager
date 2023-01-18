using System;

namespace CBLoLManager.Configuration;

using Model;

public class PlayerInitializer : Initializer
{
    public override void Initialize()
    {
        var players = Players.All;

        #region TopLaners (3)

        add("Choi Eui-seok", "Wizer", "wizer.png", 
            Position.TopLaner,
            3, 2, 1, 0, 0, 0,
            Nationality.KR,
            29, 11, 1998
        );

        add("Norberto Motta", "Betão", "betao.png", 
            Position.TopLaner,
            2, 1, 1, 0, -1, 0,
            Nationality.BR
        );

        add("Felipe Boal", "Boal", "boal.png", 
            Position.TopLaner,
            2, 1, 1, 0, 0, 0,
            Nationality.BR,
            4, 12, 2002
        );

        #endregion

        #region Junglers (2)

        add("Marcos Santos de Oliveira Junior", "CarioK", "cariok.png", 
            Position.Jungler,
            2, 2, 1, 1, -1, 1,
            Nationality.BR,
            31, 1, 2000
        );

        add("Gabriel Vinicius Saes de Lemos", "Aegis", "aegis.png", 
            Position.Jungler,
            2, 2, 1, 0, 1, 1,
            Nationality.BR,
            9, 9, 1999
        );

        #endregion

        #region MidLaners (14)

        // +7
        add("Kim Tae-hoon", "Lava", "lava.png", 
            Position.MidLaner,
            2, 3, 1, 1, -1, 1,
            Nationality.KR,
            14, 7, 1999
        );

        // +6
        add("Thiago Sartori", "tinowns", "tinowns.png", 
            Position.MidLaner,
            2, 2, 0, 2, 1, -1,
            Nationality.BR,
            6, 5, 1997
        );

        add("Bruno Augusto Felberge Ferreira", "Hauz", "hauz.png", 
            Position.MidLaner,
            2, 2, 2, 0, -1, 1,
            Nationality.BR,
            29, 4, 2001
        );

        add("Daniel Xavier", "Grevthar", "grevthar.png", 
            Position.MidLaner,
            1, 2, 0, 1, 0, 2,
            Nationality.BR,
            29, 8, 1999
        );

        // +5
        add("Matheus Rossini Miranda", "dyNquedo", "dynkedo.png", 
            Position.MidLaner,
            2, 1, 1, 1, 0, 0,
            Nationality.BR,
            14, 10, 1997
        );

        add("Bruno Farias", "Envy", "envy.png", 
            Position.MidLaner,
            2, 1, 0, 1, 0, 1,
            Nationality.BR
        );

        add("Cha Hee-min", "Yuri", "yuri.png", 
            Position.MidLaner,
            3, 2, 1, 0, 0, -1,
            Nationality.KR,
            19, 12, 2000
        );

        // +4
        add("Adriano Perassoli", "Avenger", "avenger.png", 
            Position.MidLaner,
            2, 2, 1, -1, 0, 0,
            Nationality.BR,
            14, 5, 2001
        );

        add("Marcos Henrique Ferraz", "Krastyel", "krastyel.png", 
            Position.MidLaner,
            1, 0, 1, 1, 0, 1,
            Nationality.BR,
            17, 3, 1999
        );

        add("Bruno Miyaguchi", "Goku", ".png", 
            Position.MidLaner,
            1, 1, 1, 1, 0, 0,
            Nationality.BR,
            23, 8, 1996
        );

        add("Arthur Peixoto Machado", "Tutsz", "tutsz.png", 
            Position.MidLaner,
            1, 0, 2, 1, 0, 0,
            Nationality.BR,
            16, 12, 2002
        );

        // +3
        add("Elvis Vergara", "Piloto", "piloto.png", 
            Position.MidLaner,
            1, 1, 1, 0, 0, 0,
            Nationality.BR
        );
        
        add("Júlio César Cruz", "NOsFerus", "nosferus.png", 
            Position.MidLaner,
            1, 1, 0, 0, 0, 1,
            Nationality.BR,
            14, 9, 1999
        );

        // +2
        add("", "Qats", ".png", 
            Position.MidLaner,
            1, 1, 1, 1, -1, -1,
            Nationality.BR
        );

        #endregion

        #region AdCarries (14)

        // +7
        add("Felipe Gonçalves", "brTT", "brtt.png", 
            Position.AdCarry,
            1, 0, 1, 0, 3, 2,
            Nationality.BR,
            19, 2, 1991
        );
        
        // +6
        add("Ju Yeong-hoon", "Bvoy", "bvoy.png", 
            Position.AdCarry,
            3, 2, 2, 0, -1, 0,
            Nationality.KR,
            18, 12, 1997
        );

        add("Moon Geom-su", "Route", "route.png", 
            Position.AdCarry,
            3, 3, 2, -1, -1, 0,
            Nationality.KR,
            5, 1, 1999
        );
        
        add("Alexandre Lima dos Santos", "TitaN", "titan.png", 
            Position.AdCarry,
            2, 2, 1, 0, 1, 0,
            Nationality.BR,
            8, 7, 2000
        );

        // +5
        add("Matheus Trigo Nobrega", "Trigo", "trigo.png", 
            Position.AdCarry,
            2, 2, 1, 0, 0, 0,
            Nationality.BR,
            24, 9, 1999
        );

        add("Pedro Gama", "Matsukaze", "matsukaze.png", 
            Position.AdCarry,
            1, 1, 1, 0, 1, 1,
            Nationality.BR,
            1, 2, 1997
        );

        add("Diego Amaral", "Brance", "brance.png", 
            Position.AdCarry,
            2, 2, 2, -1, 0, 0,
            Nationality.BR,
            25, 2, 2004
        );
        
        // +4
        add("Lucas Flores", "Netuno", "netuno.png", 
            Position.AdCarry,
            2, 2, 0, 0, 0, 0,
            Nationality.BR
        );

        add("Igor Lima Homem", "DudsTheBoy", "dudstheboy.png", 
            Position.AdCarry,
            1, 2, 1, 0, 0, 0,
            Nationality.BR,
            30, 3, 1996
        );

        add("Micael Rodrigues", "micaO", "micao.png", 
            Position.AdCarry,
            2, 0, 1, -1, 1, 1,
            Nationality.BR,
            5, 9, 1996
        );

        // +3
        add("Yudi Leonardo Miyashiro", "NinjaKiwi", "ninjakiwi.png", 
            Position.AdCarry,
            1, 2, 1, 0, 0, -1,
            Nationality.BR,
            21, 9, 2003
        );

        add("Julio Henrique", "Juliera", "juliera.png", 
            Position.AdCarry,
            2, 2, 1, -1, 0, -1,
            Nationality.BR
        );

        add("André Eidi Yanagimachi Pavezi", "esA", "esa.png", 
            Position.AdCarry,
            1, 0, 1, 0, -1, 2,
            Nationality.BR,
            9, 3, 1994
        );

        add("Matheus Herdy", "Drop", "drop.png", 
            Position.AdCarry,
            1, 1, 1, 1, 0, -1,
            Nationality.BR,
            1, 9, 1999
        );

        #endregion

        #region Supports (14)

        // +7
        add("Han Chang-hoon", "Luci", "luci.png", 
            Position.Support,
            2, 1, 1, 1, 2, 0,
            Nationality.BR,
            8, 7, 1997
        );

        // +6
        add("Denilson Oliveira Gonçalves", "Ceos", "ceos.png", 
            Position.Support,
            1, 2, 1, 1, 0, 1,
            Nationality.BR,
            8, 8, 1998
        );

        add("Ygor Freitas", "RedBert", "redbert.png", 
            Position.Support,
            1, 1, 1, 1, 1, 1,
            Nationality.BR,
            26, 8, 1998
        );

        // +5
        add("Gabriel Dzelme de Oliveira", "Jojo", "jojo.png", 
            Position.Support,
            1, 1, 1, 2, 0, 0,
            Nationality.BR,
            11, 11, 1998
        );
        
        add("Yan Sales Neves", "Damage", "damage.png", 
            Position.Support,
            1, 1, 1, 1, 0, 1,
            Nationality.BR,
            20, 6, 1997
        );

        add("Gustavo Gomes", "Baiano", "baiano.png", 
            Position.Support,
            1, 0, 0, 1, 0, 3,
            Nationality.BR,
            9, 4, 1994
        );

        // +4
        add("Choi Won-yeong", "Kuri", "kuri.png", 
            Position.Support,
            1, 2, 0, 2, -1, 0,
            Nationality.KR,
            8, 8, 2000
        );

        add("Gabriel Scuro", "Scuro", "scuro.png", 
            Position.Support,
            0, 1, 1, 1, 0, 1,
            Nationality.BR
        );

        add("Alexandre Fernandes", "Cavalo", "cavalo.png", 
            Position.Support,
            1, 2, 0, 1, 0, 0,
            Nationality.BR
        );
        
        // +3
        add("Gabriel Vieira Donner", "Bounty", "bounty.png", 
            Position.Support,
            1, 1, 0, 1, 0, 0,
            Nationality.BR,
            -1, -1, -1
        );

        add("Luan Cardoso", "Jockster", "jockster.png", 
            Position.Support,
            1, 0, 1, 1, 0, 0,
            Nationality.BR,
            18, 2, 1996
        );

        add("Willyan Bonpam", "Wos", "wos.png", 
            Position.Support,
            2, 0, 0, 1, 0, 0,
            Nationality.BR,
            14, 5, 1995
        );

        // +2
        add("Fábio Luis Bezerra Marques", "ProDelta", "prodelta.png", 
            Position.Support,
            1, 1, 1, 0, -1, 0,
            Nationality.BR,
            8, 5, 2001
        );

        add("Vinicius Argolo Viana", "zay", "zay.png", 
            Position.Support,
            1, 1, 0, 1, -1, 0,
            Nationality.BR
        );

        #endregion

        add("", "", ".png", 
            Position.Couch,
            0, 0, 0, 0, 0, 0,
            Nationality.UN,
            -1, -1, -1
        );

        add("", "", ".png", 
            Position.Couch,
            0, 0, 0, 0, 0, 0,
            Nationality.UN,
            -1, -1, -1
        );



        void add(
            string name, string nickname, string photo, Position role,
            int lanePhase, int mechanicSkill, int teamFigth,
            int gameVision, int leadership, int mentality,
            Nationality nationality, int day = -1, int month = -1, int year = -1)
        {
            players.Add(new Player()
            {
                Name = name,
                Nickname = nickname,
                BirthDate = day == -1 ? null : new DateTime(year, month, day),
                Nationality = nationality,
                Photo = photo,
                Role = role,
                GameVision = 75 + 5 * gameVision,
                LanePhase = 75 + 5 * lanePhase,
                Leadership = 75 + 5 * leadership,
                MechanicSkill  = 75 + 5 * mechanicSkill,
                Mentality = 75 + 5 * mechanicSkill,
                TeamFigth = 75 + 5 * teamFigth
            });
        }
    }
}