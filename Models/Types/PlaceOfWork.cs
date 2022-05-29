namespace AlphaPersonel.Models;

internal class PlaceOfWork
{

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name_city_job")]
    public string Name { get; set; } = string.Empty;
}

