using System.Text.Json.Serialization;

namespace AlphaPersonel.Models.PersonCard
{
    internal class FullAge
    {
        [JsonPropertyName("years")]
        public int Years { get; set; } = 0;

        [JsonPropertyName("months")]
        public int Months { get; set; } = 0;

        [JsonPropertyName("days")]
        public int Days { get; set; } = 0;
    }
}
