namespace AlphaPersonel.Models;
internal class TypeFamily
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

