using System;
using System.Text.Json.Serialization;


namespace AlphaPersonel.Models.Home
{
    internal class Medical 
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("date_start")]
        public DateTime? DateStart { get; set; }

        [JsonPropertyName("date_end")]
        public DateTime? DateEnd { get; set; }

        [JsonPropertyName("id_person")]
        public int IdPerson { get; set; } = 0;
    }
}
