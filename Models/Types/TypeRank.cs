namespace AlphaPersonel.Models;
internal class TypeRank
{
    [JsonPropertyName("id_rank")]
    public int Id { get; set; }

    [JsonPropertyName("type_rank")]
    public string Name { get; set; } = string.Empty;
}

