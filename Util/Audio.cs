using System;
using System.Media;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CBLoLManager.Util;

public static class Audio
{
    static Queue<(string path, int time)> queue = new Queue<(string, int)>();
    static SoundPlayer player = null;
    static bool playing;

    static async Task Play(string file, int time, bool inLoop = false)
    {
        queue.Enqueue((file, time));
        if (playing)
            return;
        await Play(inLoop);
    }

    static async Task Play(bool inLoop = false)
    {
        playing = false;
        if (queue.Count == 0)
            return;
        playing = true;

        var next = queue.Dequeue();
        player = new SoundPlayer(next.path);
        if (inLoop)
            player.PlayLooping();
        else
        {
            player.Play();
            await Await(next.time);
        }
    }

    public static void Stop()
    {
        player.Stop();
        playing = false;
    }

    public static async Task Next()
    {
        player.Stop();
        await Play();
    }

    public static async Task Instalock(bool inLoop = false)
        => await Play("Sound/Instalock.wav", 161, inLoop);

    public static async Task PicksBans()
        => await Play("Sound/shaepiDraft.wav", 3);

    public static async Task BoraInvadir(bool inLoop = false)
        => await Play("Sound/Bora Invadir.wav", 64, inLoop);

    public static async Task BraboDemais(bool inLoop = false)
        => await Play("Sound/Brabo Demais.wav", 129, inLoop);

    public static async Task ConfiaNaCall(bool inLoop = false)
        => await Play("Sound/Confia na Call.wav", 99, inLoop);

    public static async Task FoiDeBase(bool inLoop = false)
        => await Play("Sound/Foi de Base.wav", 63, inLoop);

    public static async Task GGWP(bool inLoop = false)
        => await Play("Sound/GGWP.wav", 97, inLoop);

    static async Task Await(int sec)
    {
        var end = DateTime.Now.AddSeconds(sec);
        while (playing)
        {
            await Task.Delay(500);
            if (DateTime.Now > end)
            {
                await Play();
                return;
            }
        }
    }
}