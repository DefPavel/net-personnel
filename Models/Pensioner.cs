using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlphaPersonel.Models.Home
{
    internal class Pensioner
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("date_document")]
        public DateTime? DateDocument { get; set; }
        [JsonPropertyName("document")] public string Document { get; set; } = string.Empty;
        [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;

    }
}
