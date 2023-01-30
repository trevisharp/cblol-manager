using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ReleaseDownloader
{
    private const string apiPath = @"https://api.github.com/";
    private const string releasePath = "repos/trevisharp/cblol-manager/releases";
    private HttpClient client;

    public ReleaseDownloader()
    {
        client = new HttpClient();
        client.DefaultRequestHeaders
            .Add("User-Agent", "request");
    }

    public async Task<List<Release>> GetReleases()
    {
        string path = apiPath + releasePath;
        var response = await client.GetAsync(path);
        var content = response.Content;
        var releases = await content.ReadFromJsonAsync<List<Release>>();

        if (releases is null)
            throw new Exception("Releases is null.");

        return releases;
    }

    public async Task<List<Resource>> GetResources(Release release)
    {
        string path = apiPath + releasePath + $"/{release.Id}/assets";
        var response = await client.GetAsync(path);
        var content = response.Content;
        var resources = await content.ReadFromJsonAsync<List<Resource>>();

        if (resources is null)
            throw new Exception("Resources is null.");

        return resources;
    }

    public async Task DownloadResource(Resource resource, string path)
    {
        var response = await client.GetAsync(resource.BrowserDownloadUrl);
        var content = response.Content;
        var stream = await content.ReadAsStreamAsync();
        
        if (resource.Name.Contains(".zip"))
        {
            string folderName = string.Concat(
                resource.Name
                .Reverse()
                .SkipWhile(c => c != '.')
                .Skip(1)
                .Reverse()
            );
            await Task.Run(() => openZip(stream, path + "/" + folderName));
        }
        else await Task.Run(() => saveFile(stream, path + "/" + resource.Name));
    }

    private void openZip(Stream stream, string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        using(var zip = new ZipArchive(stream, ZipArchiveMode.Read))
        {
            foreach(var entry in zip.Entries)
            {
                var entryPath = path + "/" + entry.Name;

                if (entryPath.Contains("/base/"))
                    entryPath = entryPath.Replace("/base/", "/");
                
                if (File.Exists(entryPath))
                    File.Delete(entryPath);
                entry.ExtractToFile(entryPath);
            }
        }
    }

    private void saveFile(Stream stream, string path)
    {
        if (File.Exists(path))
            File.Delete(path);
        
        FileStream file = File.Create(path);
        stream.CopyTo(file);

        file.Close();
    }
}