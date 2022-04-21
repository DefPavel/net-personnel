namespace AlphaPersonel.Models;

internal class PedagogicalPosition
{
    [JsonPropertyName("is_ped")]
    public int IdPed { get; set; } = 0;
    [JsonIgnore]
    public string IsPed { get; set; } = string.Empty;
    [JsonPropertyName("query_ped")]
    public string Query { get; set; } = string.Empty; 
}

