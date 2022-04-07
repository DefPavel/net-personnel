namespace AlphaPersonel.Models.PersonCard;
internal class FullAge
{
    [JsonPropertyName("years")]
    public int Years { get; set; }

    [JsonPropertyName("months")]
    public int Months { get; set; } 

    [JsonPropertyName("days")]
    public int Days { get; set; }
}

