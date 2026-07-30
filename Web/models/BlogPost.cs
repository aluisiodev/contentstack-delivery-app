using Newtonsoft.Json;

namespace Web.Models;

public class BlogPost
{
    [JsonProperty("uid")]
    public string Uid { get; set; } = string.Empty;

    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    [JsonProperty("body")]
    public string Body { get; set; } = string.Empty;

    [JsonProperty("author")]
    public List<Author> Authors { get; set; } = new();

    [JsonProperty("seo")]
    public Seo? Seo { get; set; }
}