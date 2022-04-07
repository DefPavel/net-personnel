using System;
using System.Text.Json.Serialization;

namespace AlphaPersonel.Models.Home
{
    internal class Vacation
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("period")]
        public string Period { get; set; } = string.Empty;

        [JsonPropertyName("order")]
        public string Order { get; set; } = string.Empty;

        [JsonPropertyName("length")]
        public int LengthVacation { get; set; } = 0;

        [JsonPropertyName("residue")]
        public int Residue { get; set; } = 0;

        [JsonPropertyName("date_start")]
        public DateTime? DateBegin { get; set; }

        [JsonPropertyName("date_end")]
        public DateTime? DateEnd { get; set;}

    }
}
