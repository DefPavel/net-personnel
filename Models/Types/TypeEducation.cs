namespace AlphaPersonel.Models;
internal class TypeEducation
{
    [JsonPropertyName("id_type")]
    public int IdType { get; set; }
    [JsonPropertyName("type_name")]
    public string TypeName { get; set; } = string.Empty;
}

