using System;
using System.Text.Json.Serialization;


namespace AlphaPersonel.Models.Home
{
    internal class Invalid
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("document")]
        public string Document { get; set; } = string.Empty;
        [JsonPropertyName("date_begin")]
        public DateTime? DateStart { get; set; }
        [JsonPropertyName("date_end")]
        public DateTime? DateEnd { get; set; }
        [JsonPropertyName("group")]
        public string Group { get; set; } = string.Empty;
        [JsonPropertyName("for_life")]
        public bool IsForLife { get; set; }
        [JsonPropertyName("id_person")]
        public int IdPerson { get; internal set; }
    }
}
