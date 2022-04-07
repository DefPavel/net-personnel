namespace AlphaPersonel.Models;
internal class TypeRewarding
{
    [JsonPropertyName("id_type")]
    public int Id { get; set; }
    [JsonPropertyName("type_rewarding")]
    public string Name { get; set; } = string.Empty;
}

