using System;
using System.Text.Json.Serialization;

namespace AlphaPersonel.Models.Home
{
    internal class Qualification
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name_course")]
        public string NameCourse { get; set; } = string.Empty;

        [JsonPropertyName("date_start")]
        public DateTime? DateBegin { get; set; }

        [JsonPropertyName("date_end")]
        public DateTime? DateEnd { get; set; }

        [JsonPropertyName("place")]
        public string Place { get; set; } = string.Empty;

        [JsonPropertyName("certificate")]
        public string Certificate { get; set; } = string.Empty;

        [JsonPropertyName("date_issue")]
        public DateTime? DateIssue { get; set; }

        [JsonPropertyName("id_person")]
        public int IdPerson { get; set; } = 0;
    }
}
