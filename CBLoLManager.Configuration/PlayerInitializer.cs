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

        #region TopLaners (11)

        addLaner("Felipe Boal", "Boal", "boal.png", 
            Position.TopLaner, 85,
            Nationality.BR,
            4, 12, 2002
        );

        AddTeamFigther("Leonardo Souza", "Robo", "robo.png", 
            Position.TopLaner, 81,
            Nationality.BR,
            22, 4, 1998
        );

        addLaner("Choi Eui-seok", "Wizer", "wizer.png", 
            Position.TopLaner, 88,
            Nationality.KR,
            29, 11, 1998
        );

        addLaner("Shin Tae-min", "HiRit", "hirit.png", 
            Position.TopLaner, 83,
            Nationality.KR,
            24, 7, 1998
        );

        addLaner("Francisco Natanael Braz do Espirito Santo Miranda", "fNb", "fnb.png", 
            Position.TopLaner, 77,
            Nationality.BR,
            3, 2, 2000
        );

        addLaner("Han Gyu-joon", "Lonely", "lonely.png", 
            Position.TopLaner, 79,
            Nationality.KR,
            29, 8, 2000
        );

        addLaner("Guilherme Ruiz", "Guigo", "guigo.png", 
            Position.TopLaner, 82,
            Nationality.BR,
            29, 1, 2002
        );

        AddTeamFigther("Rodrigo Panisa", "Tay", "tay.png", 
            Position.TopLaner, 82,
            Nationality.BR,
            22, 11, 1996
        );

        addLaner("Kwon Hee-won", "Zzk", "zzk.png", 
            Position.TopLaner, 67,
            Nationality.KR,
            18, 2, 2001
        );

        addLaner("Thiago Luiz", "Kiari", "kiari.png", 
            Position.TopLaner, 73,
            Nationality.BR
        );

        AddTeamFigther("Norberto Motta", "Betão", "betao.png", 
            Position.TopLaner, 65,
            Nationality.BR
        );

        #endregion

        #region Junglers (11)

        addLaner("Gabriel Vinicius Saes de Lemos", "Aegis", "aegis.png", 
            Position.Jungler, 88,
            Nationality.BR,
            9, 9, 1999
        );

        AddTeamFigther("Marcos Santos de Oliveira Junior", "CarioK", "cariok.png", 
            Position.Jungler, 85,
            Nationality.BR,
            31, 1, 2000
        );

        addLaner("Park Jong-hoon", "Croc", "croc.png", 
            Position.Jungler, 80,
            Nationality.KR
        );

        AddLeader("Filipe Brombilla de Barrios", "Ranger", "ranger.png", 
            Position.Jungler, 86,
            Nationality.BR
        );
        
        addLaner("Gabriel Goot", "Goot", "goot.png", 
            Position.Jungler, 79,
            Nationality.BR
        );

        addLaner("Yan Petermann", "Yampi", "yampi.png", 
            Position.Jungler, 74,
            Nationality.BR,
            7, 11, 1998
        );

        addLaner("Jesús Alberto Loya Trujillo", "Grell", "grell.png", 
            Position.Jungler, 86,
            Nationality.MX,
            9, 4, 2001
        );

        addLaner("Pedro Gonçalves", "Disamis", "disamis.png", 
            Position.Jungler, 83,
            Nationality.BR
        );

        addLaner("Lucas Miranda", "Accez", "accez.png", 
            Position.Jungler, 73,
            Nationality.BR
        );
        
        AddTeamFigther("Artur Queiroz", "scary", "scary.png", 
            Position.Jungler, 72,
            Nationality.BR
        );
        
        AddTeamFigther("Luís Gustavo Dirami Martins", "Sting", "sting.png", 
            Position.Jungler, 65,
            Nationality.BR,
            19, 7, 2001
        );

        #endregion

        #region MidLaners (11)

        addLaner("Kim Tae-hoon", "Lava", "lava.png", 
            Position.MidLaner, 79,
            Nationality.KR,
            14, 7, 1999
        );

        AddTeamFigther("Thiago Sartori", "tinowns", "tinowns.png", 
            Position.MidLaner, 88,
            Nationality.BR,
            6, 5, 1997
        );

        addLaner("Bruno Augusto Felberge Ferreira", "Hauz", "hauz.png", 
            Position.MidLaner, 85,
            Nationality.BR,
            29, 4, 2001
        );

        addLaner("Daniel Xavier", "Grevthar", "grevthar.png", 
            Position.MidLaner, 78,
            Nationality.BR,
            29, 8, 1999
        );

        AddTeamFigther("Matheus Rossini Miranda", "dyNquedo", "dynkedo.png", 
            Position.MidLaner, 90,
            Nationality.BR,
            14, 10, 1997
        );

        AddLeader("Bruno Farias", "Envy", "envy.png", 
            Position.MidLaner, 81,
            Nationality.BR
        );

        addLaner("Cha Hee-min", "Yuri", "yuri.png", 
            Position.MidLaner, 72,
            Nationality.KR,
            19, 12, 2000
        );

        addLaner("Adriano Perassoli", "Avenger", "avenger.png", 
            Position.MidLaner, 69,
            Nationality.BR,
            14, 5, 2001
        );
        
        addLaner("Elvis Vergara", "Piloto", "piloto.png", 
            Position.MidLaner, 65,
            Nationality.BR
        );
        
        addLaner("Júlio César Cruz", "NOsFerus", "nosferus.png", 
            Position.MidLaner, 75,
            Nationality.BR,
            14, 9, 1999
        );

        AddLeader("Marcos Henrique Ferraz", "Krastyel", "krastyel.png", 
            Position.MidLaner, 70,
            Nationality.BR,
            17, 3, 1999
        );

        #endregion

        #region AdCarries (11)

        AddLeader("Felipe Gonçalves", "brTT", "brtt.png", 
            Position.AdCarry, 85,
            Nationality.BR,
            19, 2, 1991
        );
        
        addLaner("Ju Yeong-hoon", "Bvoy", "bvoy.png", 
            Position.AdCarry, 90,
            Nationality.KR,
            18, 12, 1997
        );

        AddTeamFigther("Moon Geom-su", "Route", "route.png", 
            Position.AdCarry, 85,
            Nationality.KR,
            5, 1, 1999
        );
        
        AddTeamFigther("Alexandre Lima dos Santos", "TitaN", "titan.png", 
            Position.AdCarry, 87,
            Nationality.BR,
            8, 7, 2000
        );

        AddLeader("Matheus Trigo Nobrega", "Trigo", "trigo.png", 
            Position.AdCarry, 80,
            Nationality.BR,
            24, 9, 1999
        );

        addLaner("Diego Amaral", "Brance", "brance.png", 
            Position.AdCarry, 86,
            Nationality.BR,
            25, 2, 2004
        );
        
        AddTeamFigther("Lucas Flores", "Netuno", "netuno.png", 
            Position.AdCarry, 92,
            Nationality.BR
        );

        addLaner("Igor Lima Homem", "DudsTheBoy", "dudstheboy.png", 
            Position.AdCarry, 70,
            Nationality.BR,
            30, 3, 1996
        );

        AddLeader("Micael Rodrigues", "micaO", "micao.png", 
            Position.AdCarry, 70,
            Nationality.BR,
            5, 9, 1996
        );

        addLaner("Yudi Leonardo Miyashiro", "NinjaKiwi", "ninjakiwi.png", 
            Position.AdCarry, 80,
            Nationality.BR,
            21, 9, 2003
        );

        addLaner("Julio Henrique", "Juliera", "juliera.png", 
            Position.AdCarry, 72,
            Nationality.BR
        );

        addLaner("André Eidi Yanagimachi Pavezi", "esA", "esa.png", 
            Position.AdCarry, 80,
            Nationality.BR,
            9, 3, 1994
        );

        #endregion

        #region Supports (11)

        addLaner("Denilson Oliveira Gonçalves", "Ceos", "ceos.png", 
            Position.Support, 84,
            Nationality.BR,
            8, 8, 1998
        );

        AddTeamFigther("Ygor Freitas", "RedBert", "redbert.png", 
            Position.Support, 75,
            Nationality.BR,
            26, 8, 1998
        );

        AddTeamFigther("Gabriel Dzelme de Oliveira", "Jojo", "jojo.png", 
            Position.Support, 84,
            Nationality.BR,
            11, 11, 1998
        );
        
        AddTeamFigther("Yan Sales Neves", "Damage", "damage.png", 
            Position.Support, 80,
            Nationality.BR,
            20, 6, 1997
        );

        AddLeader("Gustavo Gomes", "Baiano", "baiano.png", 
            Position.Support, 85,
            Nationality.BR,
            9, 4, 1994
        );

        addLaner("Choi Won-yeong", "Kuri", "kuri.png", 
            Position.Support, 75,
            Nationality.KR,
            8, 8, 2000
        );

        addLaner("Gabriel Scuro", "Scuro", "scuro.png", 
            Position.Support, 70,
            Nationality.BR
        );

        addLaner("Alexandre Fernandes", "Cavalo", "cavalo.png", 
            Position.Support, 69,
            Nationality.BR
        );

        addLaner("Woo Jun-sung", "Nia", "nia.png", 
            Position.Support, 71,
            Nationality.KR,
            2, 1, 2000
        );

        addLaner("Fábio Luis Bezerra Marques", "ProDelta", "prodelta.png", 
            Position.Support, 70,
            Nationality.BR,
            8, 5, 2001
        );

        addLaner("Vinicius Argolo Viana", "zay", "zay.png", 
            Position.Support, 85,
            Nationality.BR
        );

        #endregion

        void addLaner(
            string name, string nickname, string photo, Position role, int avarage, 
            Nationality nationality, int day = -1, int month = -1, int year = -1
        )
        {
            int pts = 6 * avarage;
            int[] data = randomGen(pts, 100, .5f, .25f, 0f, .25f, 0f, 0f);
            add(name, nickname, photo, role, 
                data[0], data[1], data[2], data[3], data[4], data[5],
                nationality, day, month, year
            );
        }

        void AddTeamFigther(
            string name, string nickname, string photo, Position role, int avarage, 
            Nationality nationality, int day = -1, int month = -1, int year = -1
        )
        {
            int pts = 6 * avarage;
            int[] data = randomGen(pts, 100, 0f, 0f, .5f, .25f, .25f, 0f);
            add(name, nickname, photo, role, 
                data[0], data[1], data[2], data[3], data[4], data[5],
                nationality, day, month, year
            );
        }

        void AddLeader(
            string name, string nickname, string photo, Position role, int avarage, 
            Nationality nationality, int day = -1, int month = -1, int year = -1
        )
        {
            int pts = 6 * avarage;
            int[] data = randomGen(pts, 100, 0f, 0f, .0f, .25f, .5f, .25f);
            add(name, nickname, photo, role, 
                data[0], data[1], data[2], data[3], data[4], data[5],
                nationality, day, month, year
            );
        }

        int[] randomGen(int pts, int max, params float[] pounds)
        {
            int N = pounds.Length;
            int[] data = new int[N];
            int totalSpace = max * N;
            var rand = Random.Shared;
            int i;

            while (pts > 0)
            {
                var prob = rand.NextSingle();

                i = -1;
                while (prob > 0)
                {
                    i++;
                    int j = i % N;

                    if (data[j] >= max)
                        continue;
                    
                    var space = (max - data[j]) / (float)max;
                    var extra = pounds[j] / 4;

                    prob -= (space + extra) / 10f;
                }

                data[i % N]++;

                pts--;
                totalSpace--;

                if (totalSpace <= 0)
                    break;
            }

            return data;
        }

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