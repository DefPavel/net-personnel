using System;
using System.Text.Json.Serialization;


namespace AlphaPersonel.Models
{
    internal class HistoryEmployment
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("number_record")]
        public int Record { get; set; }

        [JsonPropertyName("information")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("is_pedagogical")]
        public bool IsPedagogical { get; set; }

        [JsonPropertyName("is_science")]
        public bool IsScience { get; set; }

        [JsonPropertyName("is_univer")]
        public bool IsUniver { get; set; }

        [JsonPropertyName("is_library")]
        public bool IsLibrary { get; set; }

        [JsonPropertyName("is_medical")]
        public bool IsMedical { get; set; }

        [JsonPropertyName("is_museum")]
        public bool IsMuseum { get; set; }

        [JsonPropertyName("is_over")]
        public bool IsOver { get; set; }

        [JsonPropertyName("order_name")]
        public string Order { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public DateTime CreateAt { get; set; }



    }
}
