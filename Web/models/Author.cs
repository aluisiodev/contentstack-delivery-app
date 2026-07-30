using Newtonsoft.Json;

namespace Web.Models;

public class Author
{
    [JsonProperty("uid")]
    public string Uid { get; set; } = string.Empty;

    [JsonProperty("title")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("bio")]
    public string Bio { get; set; } = string.Empty;
}