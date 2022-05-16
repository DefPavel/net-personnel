namespace AlphaPersonel.Models;
internal class ResponseError
{
    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;
}

