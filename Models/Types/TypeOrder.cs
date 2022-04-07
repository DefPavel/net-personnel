namespace AlphaPersonel.Models;
internal class TypeOrder
{
    [JsonPropertyName("id_type")]
    public int Id { get; set; }
    [JsonPropertyName("type_order")]
    public string Name { get; set; } = string.Empty;
}

