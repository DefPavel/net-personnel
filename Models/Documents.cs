namespace AlphaPersonel.Models;

internal class Documents
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("url_document")]
    public string Url { get; set; } = string.Empty;
}

