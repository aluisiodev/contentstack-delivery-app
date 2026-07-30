using Newtonsoft.Json;

namespace Web.Models;

public class Seo
{
    [JsonProperty("meta_title")]
    public string MetaTitle { get; set; } = string.Empty;

    [JsonProperty("meta_description")]
    public string MetaDescription { get; set; } = string.Empty;
}