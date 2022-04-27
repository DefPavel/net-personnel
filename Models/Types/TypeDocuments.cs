namespace AlphaPersonel.Models;

internal class TypeDocuments
{
    [JsonPropertyName("id")]
    public int IdType { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

