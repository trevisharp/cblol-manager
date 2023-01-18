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

        #region MidLaners (2)

        add("Matheus Rossini Miranda", "dyNquedo", "dynquedo.png", 
            Position.MidLaner,
            1, 1, 1, 1, 1, 1,
            Nationality.BR,
            14, 10, 1997
        );

        add("Adriano Perassoli", "Avenger", "avenger.png", 
            Position.MidLaner,
            2, 2, 1, -1, 0, 1,
            Nationality.BR,
            14, 5, 2001
        );

        #endregion

        #region AdCarries (4)

        add("Ju Yeong-hoon", "Bvoy", "bvoy.png", 
            Position.AdCarry,
            2, 2, 2, 1, 0, 1,
            Nationality.KR,
            18, 12, 1997
        );

        add("Felipe Gonçalves", "brTT", "brtt.png", 
            Position.AdCarry,
            1, 0, 1, 0, 3, 2,
            Nationality.BR,
            19, 2, 1991
        );

        add("André Eidi Yanagimachi Pavezi", "esA", "esa.png", 
            Position.AdCarry,
            1, 1, 1, 0, 0, 1,
            Nationality.BR,
            9, 3, 1994
        );
        
        add("Alexandre Lima dos Santos", "TitaN", "titan.png", 
            Position.AdCarry,
            2, 2, 1, 0, 2, 0,
            Nationality.BR,
            8, 7, 2000
        );

        #endregion

        #region Supports (4)

        add("Yan Sales Neves", "Damage", "damage.png", 
            Position.Support,
            1, 1, 1, 1, 0, 1,
            Nationality.BR,
            20, 6, 1997
        );

        add("Fábio Luis Bezerra Marques", "ProDelta", "prodelta.png", 
            Position.Support,
            1, 1, 1, 0, -1, 0,
            Nationality.BR,
            8, 5, 2001
        );

        add("Han Chang-hoon", "Luci", "luci.png", 
            Position.Support,
            2, 1, 1, 0, 2, 0,
            Nationality.BR,
            8, 7, 1997
        );
        
        add("Choi Won-yeong", "Kuri", "kuri.png", 
            Position.Support,
            1, 2, 0, 2, -1, -1,
            Nationality.KR,
            8, 8, 2000
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