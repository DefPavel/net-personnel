using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlphaPersonel.Models.Home
{
    internal class AcademicTitle
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("document")]
        public string Document { get; set; } = string.Empty;

        [JsonPropertyName("type_rank")]
        public string TypeRank { get; set; } = string.Empty;

        [JsonPropertyName("date_issue")]
        public DateTime DateIssue { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; } = string.Empty;

        [JsonPropertyName("place")]
        public string Place { get; set; } = string.Empty;

        [JsonPropertyName("id_person")]
        public int IdPerson { get; set; } = 0;
    }
}
