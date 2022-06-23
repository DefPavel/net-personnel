namespace AlphaPersonel.Models;

internal class Report
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("id_type")]
    public int IdType { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

