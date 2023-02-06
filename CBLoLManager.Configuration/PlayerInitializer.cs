using System;
using System.Linq;

namespace CBLoLManager.Configuration;

using Model;

public class PlayerInitializer : Initializer
{
    public override void Initialize()
    {
        var players = Players.All;

        if (players.Count() > 0)
            return;

        #region TopLaners (12)

        add("Felipe Boal", "Boal", "boal.png", 
            Position.TopLaner,
            8, 8, 8, 6, 5, 5,
            Nationality.BR,
            4, 12, 2002
        );

        add("Leonardo Souza", "Robo", "robo.png", 
            Position.TopLaner,
            10, 5, 10, 1, 10, 10,
            Nationality.BR,
            22, 4, 1998
        );

        add("Choi Eui-seok", "Wizer", "wizer.png", 
            Position.TopLaner,
            5, 5, 5, 5, 0, 0,
            Nationality.KR,
            29, 11, 1998
        );

        add("Shin Tae-min", "HiRit", "hirit.png", 
            Position.TopLaner,
            12, 10, 10, 10, 0, 0,
            Nationality.KR,
            24, 7, 1998
        );

        add("Francisco Natanael Braz do Espirito Santo Miranda", "fNb", "fnb.png", 
            Position.TopLaner,
            5, 5, 5, 0, 10, 0,
            Nationality.BR,
            3, 2, 2000
        );

        add("Han Gyu-joon", "Lonely", "lonely.png", 
            Position.TopLaner,
            14, 10, 14, 10, 0, 0,
            Nationality.KR,
            29, 8, 2000
        );

        add("Guilherme Ruiz", "Guigo", "guigo.png", 
            Position.TopLaner,
            0, 5, 5, 5, 0, 5,
            Nationality.BR,
            29, 1, 2002
        );

        add("Rodrigo Panisa", "Tay", "tay.png", 
            Position.TopLaner,
            5, 4, 5, 4, 5, 5,
            Nationality.BR,
            22, 11, 1996
        );

        add("Kwon Hee-won", "Zzk", "zzk.png", 
            Position.TopLaner,
            10, 2, 10, 2, 0, 0,
            Nationality.KR,
            18, 2, 2001
        );

        add("Thiago Luiz", "Kiari", "kiari.png", 
            Position.TopLaner,
            5, 3, 4, 3, 0, 0,
            Nationality.BR
        );

        add("Norberto Motta", "Betão", "betao.png", 
            Position.TopLaner,
            4, 0, 4, 2, -2, 0,
            Nationality.BR
        );

        add("William Portugal", "Tyrin", "tyrin.png", 
            Position.TopLaner,
            4, 2, 4, 0, -1, -1,
            Nationality.BR,
            11, 12, 2001
        );

        #endregion

        #region Junglers (12)

        add("Gabriel Vinicius Saes de Lemos", "Aegis", "aegis.png", 
            Position.Jungler,
            12, 12, 6, 12, 10, 10,
            Nationality.BR,
            9, 9, 1999
        );

        add("Marcos Santos de Oliveira Junior", "CarioK", "cariok.png", 
            Position.Jungler,
            6, 6, 3, 0, -2, 0,
            Nationality.BR,
            31, 1, 2000
        );

        add("Park Jong-hoon", "Croc", "croc.png", 
            Position.Jungler,
            6, 6, 6, 6, 0, -2,
            Nationality.KR
        );

        add("Filipe Brombilla de Barrios", "Ranger", "ranger.png", 
            Position.Jungler,
            8, 8, 8, 8, 5, 10,
            Nationality.BR
        );
        
        add("Gabriel Goot", "Goot", "goot.png", 
            Position.Jungler,
            4, 3, 4, 4, -2, 0,
            Nationality.BR
        );

        add("Yan Petermann", "Yampi", "yampi.png", 
            Position.Jungler,
            6, 0, 6, 6, 0, 0,
            Nationality.BR,
            7, 11, 1998
        );

        add("Hugo Dias de Faria", "Hugato", "hugato.png", 
            Position.Jungler,
            1, 0, 1, 1, 1, 0,
            Nationality.BR,
            4, 9, 2003
        );

        add("Pedro Gonçalves", "Disamis", "disamis.png", 
            Position.Jungler,
            3, 3, 3, 3, 0, 0,
            Nationality.BR
        );

        add("Lucas Miranda", "Accez", "accez.png", 
            Position.Jungler,
            10, 4, 4, 1, 2, 0,
            Nationality.BR
        );
        
        add("Artur Queiroz", "scary", "scary.png", 
            Position.Jungler,
            5, 5, 5, 6, 0, 0,
            Nationality.BR
        );
        
        add("Luís Gustavo Dirami Martins", "Sting", "sting.png", 
            Position.Jungler,
            5, 0, 5, 0, -2, -2,
            Nationality.BR,
            19, 7, 2001
        );

        add("Gustavo Hayashi", "enel1", "enel.png", 
            Position.Jungler,
            5, 5, 0, 0, -2, -2,
            Nationality.BR
        );

        #endregion

        #region MidLaners (12)

        add("Kim Tae-hoon", "Lava", "lava.png", 
            Position.MidLaner,
            11, 11, 10, 8, 5, 0,
            Nationality.KR,
            14, 7, 1999
        );

        add("Thiago Sartori", "tinowns", "tinowns.png", 
            Position.MidLaner,
            10, 7, 7, 10, 10, 10,
            Nationality.BR,
            6, 5, 1997
        );

        add("Bruno Augusto Felberge Ferreira", "Hauz", "hauz.png", 
            Position.MidLaner,
            6, 6, 6, 6, 5, 0,
            Nationality.BR,
            29, 4, 2001
        );

        add("Daniel Xavier", "Grevthar", "grevthar.png", 
            Position.MidLaner,
            4, 4, 4, 4, 5, 5,
            Nationality.BR,
            29, 8, 1999
        );

        add("Matheus Rossini Miranda", "dyNquedo", "dynkedo.png", 
            Position.MidLaner,
            8, 8, 6, 0, 10, 5,
            Nationality.BR,
            14, 10, 1997
        );

        add("Bruno Farias", "Envy", "envy.png", 
            Position.MidLaner,
            6, 6, 6, 4, 5, 10,
            Nationality.BR
        );

        add("Cha Hee-min", "Yuri", "yuri.png", 
            Position.MidLaner,
            10, 10, 7, 5, 0, -5,
            Nationality.KR,
            19, 12, 2000
        );

        add("Adriano Perassoli", "Avenger", "avenger.png", 
            Position.MidLaner,
            8, 8, 6, 6, 0, 0,
            Nationality.BR,
            14, 5, 2001
        );
        
        add("Elvis Vergara", "Piloto", "piloto.png", 
            Position.MidLaner,
            4, 4, 4, 2, 0, 0,
            Nationality.BR
        );
        
        add("Júlio César Cruz", "NOsFerus", "nosferus.png", 
            Position.MidLaner,
            6, 8, 6, 6, 0, 0,
            Nationality.BR,
            14, 9, 1999
        );

        add("Marcos Henrique Ferraz", "Krastyel", "krastyel.png", 
            Position.MidLaner,
            4, 4, 4, 4, 5, 0,
            Nationality.BR,
            17, 3, 1999
        );

        add("Arthur Peixoto Machado", "Tutsz", "tutsz.png", 
            Position.MidLaner,
            3, 3, 3, 3, 5, 0,
            Nationality.BR,
            16, 12, 2002
        );

        #endregion

        #region AdCarries (12)

        add("Felipe Gonçalves", "brTT", "brtt.png", 
            Position.AdCarry,
            3, 2, 4, 5, 14, 8,
            Nationality.BR,
            19, 2, 1991
        );
        
        add("Ju Yeong-hoon", "Bvoy", "bvoy.png", 
            Position.AdCarry,
            7, 7, 4, 7, 5, 0,
            Nationality.KR,
            18, 12, 1997
        );

        add("Moon Geom-su", "Route", "route.png", 
            Position.AdCarry,
            8, 8, 8, 8, 0, -5,
            Nationality.KR,
            5, 1, 1999
        );
        
        add("Alexandre Lima dos Santos", "TitaN", "titan.png", 
            Position.AdCarry,
            13, 13, 10, 10, 10, 10,
            Nationality.BR,
            8, 7, 2000
        );

        add("Matheus Trigo Nobrega", "Trigo", "trigo.png", 
            Position.AdCarry,
            5, 5, 5, 5, 5, 5,
            Nationality.BR,
            24, 9, 1999
        );

        add("Diego Amaral", "Brance", "brance.png", 
            Position.AdCarry,
            7, 7, 7, 1, 0, 0,
            Nationality.BR,
            25, 2, 2004
        );
        
        add("Lucas Flores", "Netuno", "netuno.png", 
            Position.AdCarry,
            10, 10, 8, 8, 0, 0,
            Nationality.BR
        );

        add("Igor Lima Homem", "DudsTheBoy", "dudstheboy.png", 
            Position.AdCarry,
            7, 5, 5, 4, 5, 0,
            Nationality.BR,
            30, 3, 1996
        );

        add("Micael Rodrigues", "micaO", "micao.png", 
            Position.AdCarry,
            3, 3, 3, 3, 10, 10,
            Nationality.BR,
            5, 9, 1996
        );

        add("Yudi Leonardo Miyashiro", "NinjaKiwi", "ninjakiwi.png", 
            Position.AdCarry,
            5, 5, 5, 0, -2, -2,
            Nationality.BR,
            21, 9, 2003
        );

        add("Julio Henrique", "Juliera", "juliera.png", 
            Position.AdCarry,
            8, 8, 0, 0, 0, 5,
            Nationality.BR
        );

        add("André Eidi Yanagimachi Pavezi", "esA", "esa.png", 
            Position.AdCarry,
            4, 4, 4, 4, 7, 7,
            Nationality.BR,
            9, 3, 1994
        );

        #endregion

        #region Supports (12)

        add("Denilson Oliveira Gonçalves", "Ceos", "ceos.png", 
            Position.Support,
            8, 8, 8, 8, 5, 5,
            Nationality.BR,
            8, 8, 1998
        );

        add("Ygor Freitas", "RedBert", "redbert.png", 
            Position.Support,
            5, 5, 5, 5, 5, 5,
            Nationality.BR,
            26, 8, 1998
        );

        add("Gabriel Dzelme de Oliveira", "Jojo", "jojo.png", 
            Position.Support,
            7, 4, 4, 7, 5, 5,
            Nationality.BR,
            11, 11, 1998
        );
        
        add("Yan Sales Neves", "Damage", "damage.png", 
            Position.Support,
            6, 6, 6, -3, 0, 5,
            Nationality.BR,
            20, 6, 1997
        );

        add("Gustavo Gomes", "Baiano", "baiano.png", 
            Position.Support,
            3, 2, 4, 5, 8, 14,
            Nationality.BR,
            9, 4, 1994
        );

        add("Choi Won-yeong", "Kuri", "kuri.png", 
            Position.Support,
            11, 11, 10, 10, 5, -2,
            Nationality.KR,
            8, 8, 2000
        );

        add("Gabriel Scuro", "Scuro", "scuro.png", 
            Position.Support,
            8, 0, 7, 7, 0, 0,
            Nationality.BR
        );

        add("Alexandre Fernandes", "Cavalo", "cavalo.png", 
            Position.Support,
            5, 0, 5, 5, 0, 0,
            Nationality.BR
        );
        
        add("Gabriel Vieira Donner", "Bounty", "bounty.png", 
            Position.Support,
            5, 5, 5, 0, 0, 0,
            Nationality.BR,
            -1, -1, -1
        );

        add("Woo Jun-sung", "Nia", "nia.png", 
            Position.Support,
            4, 4, 4, 5, 0, 0,
            Nationality.KR,
            2, 1, 2000
        );

        add("Fábio Luis Bezerra Marques", "ProDelta", "prodelta.png", 
            Position.Support,
            3, 2, 3, 4, 0, 5,
            Nationality.BR,
            8, 5, 2001
        );

        add("Vinicius Argolo Viana", "zay", "zay.png", 
            Position.Support,
            8, 6, 2, 8, 0, 0,
            Nationality.BR
        );

        #endregion

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
                GameVision = 70 + 2 * gameVision,
                LanePhase = 70 + 2 * lanePhase,
                Leadership = 70 + 2 * leadership,
                MechanicSkill  = 70 + 2 * mechanicSkill,
                Mentality = 70 + 2 * mentality,
                TeamFigth = 70 + 2 * teamFigth
            });
        }
    }
}