using System.Text.Json.Serialization;

public class Resource
{
    public string Name { get; set; }
    
    [JsonPropertyName("browser_download_url")]
    public string BrowserDownloadUrl { get; set; }

    public int Size { get; set; }
}