namespace AlphaPersonel.Models;

internal class OldSurname
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("old_surname")] public string Surname { get; set; } = string.Empty;
    [JsonPropertyName("id_order")] public int IdOrder { get; set; } = 0;
    [JsonPropertyName("order")] public string Order { get; set; } = string.Empty;
    [JsonPropertyName("date_order")] public DateTime DateOrder { get; set; }
    [JsonPropertyName("created_at")] public DateTime? DateChange { get; set; }
    public int IdPerson { get; internal set; }
}

