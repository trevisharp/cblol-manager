using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class UpdateSystem
{
    private UpdateSystem() { }

    private ReleaseDownloader downloader = new ReleaseDownloader();
    private string instalationPath = "App";
    private Patch crr = null;
    private List<Resource> toDownload { get; set; }

    private volatile int bytesToDownload;
    public int BytesToDownload => bytesToDownload;

    private volatile int filesToDownload;
    public int FilesToDownload => filesToDownload;

    private volatile int findingFilesProgress;
    public int FindingFilesProgress => findingFilesProgress;

    private volatile int bytesDownloaded;
    public int BytesDownloaded => bytesDownloaded;

    private volatile int bytesDownloadProgress;
    public int DownloadProgress => bytesDownloadProgress;

    public bool Completed { get; private set; } = false;
    public bool Started { get; private set; } = false;

    private void createInstalationFolderIfNotExist()
    {
        try
        {
            if (Directory.Exists(instalationPath))
                return;
            
            Directory.CreateDirectory(instalationPath);    
        }
        catch
        {
            throw new Exception($"O sistema não teve permissão de criar/ler a pasta {instalationPath}.\n"
                + "Tente executar a aplicação como administrador.");
        }
    }

    private Patch getCurrentPatch()
    {
        createInstalationFolderIfNotExist();

        try
        {
            return Patch.Open(instalationPath);
        }
        catch
        {
            throw new Exception("O sistema não pode criar/ler o arquivo de informações de patch."
                + "Tente executar a aplicação como administrador.");
        }
    }

    public async Task FindFiles()
    {
        toDownload = new List<Resource>();
        var releases = await downloader.GetReleases();

        releases = releases
            .Where(r => r.Version > crr.Version)
            .OrderByDescending(r => r.Version)
            .ToList();
        
        int duration = releases.Count + 1;
        int progress = 1;
        findingFilesProgress = 100 * progress / duration;

        if (releases.Count == 0)
        {
            findingFilesProgress = 
            bytesDownloadProgress = 100;
            Completed = true;
            return;
        }

        Started = true;
        foreach (var release in releases)
        {
            if (!release.Name.Contains("cblolmanager"))
                continue;
            
            var resources = await downloader.GetResources(release);
            foreach (var resource in resources)
            {
                if (toDownload.Exists(r => r.Name == resource.Name))
                    continue;
                
                filesToDownload++;
                bytesToDownload += resource.Size;
                toDownload.Add(resource);
            }
            progress++;
            findingFilesProgress = 100 * progress / duration;
        }
    }

    public async Task DownloadFiles()
    {
        foreach (var file in toDownload)
        {
            await downloader.DownloadResource(file, instalationPath);
            bytesDownloaded += file.Size;
            bytesDownloadProgress = (int)(100 * (float)bytesDownloaded / bytesToDownload);
        }
        Completed = true;
    }

    public static UpdateSystem Start()
    {
        UpdateSystem sys = new UpdateSystem();

        sys.crr = sys.getCurrentPatch();

        return sys;
    }
}