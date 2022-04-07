namespace AlphaPersonel.Models;
internal class TypeVacation
{
    [JsonPropertyName("id_type")]
    public int Id { get; set; }
    [JsonPropertyName("type_vacation")]
    public string? Name { get; set; }
}

