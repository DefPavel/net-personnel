
namespace AlphaPersonel.Models;

internal class Pensioner
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("date_document")]
    public DateTime? DateDocument { get; set; }
    [JsonPropertyName("document")] public string Document { get; set; } = string.Empty;
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
    [JsonPropertyName("id_person")] public int IdPerson { get; internal set; }
}

