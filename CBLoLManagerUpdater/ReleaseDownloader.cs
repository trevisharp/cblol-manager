using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

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

    private async Task<Stream> DownloadResource(Resource resource)
    {
        string path = resource.BrowserDownloadUrl;
        var response = await client.GetAsync(path);
        var content = response.Content;
        var stream = await content.ReadAsStreamAsync();
        return stream;
    }
}