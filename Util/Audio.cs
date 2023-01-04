using System.Media;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CBLoLManager.Util;

public static class Audio
{
    static Queue<string> queue = new Queue<string>();
    static SoundPlayer player = null;
    static bool playing = false;

    static async Task Play()
    {
        if (playing)
            return;
        
        playing = true;
        while (queue.Count > 0)
        {
            player = new SoundPlayer(queue.Dequeue());
            player.Load();
            await Task.Run(() => player.PlaySync());
            player.Stop();
        }
        playing = false;
    }

    public static async Task Instalock()
    {
        queue.Enqueue(@"Sound/Instalock.wav");
        await Play();
    }

    public static async Task PicksBans()
    {
        queue.Enqueue(@"Sound/shaepiDraft.wav");
        await Play();
    }
}