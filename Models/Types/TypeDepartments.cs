namespace AlphaPersonel.Models;
internal class TypeDepartments
{
    [JsonPropertyName("id_type")]
    public int IdType { get; set; }
    [JsonPropertyName("type")]
    public string TypeName { get; set; } = string.Empty;

}

