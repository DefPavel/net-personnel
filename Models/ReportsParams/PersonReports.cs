using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaPersonel.Models
{
    internal class PersonReports
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("firstname")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string MidlleName { get; set; } = string.Empty;

        [JsonPropertyName("lastname")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("birthday")]
        public string Birthday { get; set; } = string.Empty;

        [JsonPropertyName("department")]
        public string Department { get; set; } = string.Empty;

        [JsonPropertyName("full_age")]
        public int FullAge { get; set; } = 0;

        [JsonPropertyName("type_contract")]
        public string TypeContract { get; set; } = string.Empty;

        [JsonPropertyName("position")]
        public string PersonPosition { get; set; } = string.Empty;

        [JsonPropertyName("date_to_working")]
        public string DateWorking { get; set; } = string.Empty;

        [JsonPropertyName("type_rank")]
        public string TypeRank { get; set; } = string.Empty;

        [JsonPropertyName("type_sciens")]
        public string TypeSciens { get; set; } = string.Empty;

        [JsonPropertyName("scientific_branch")]
        public string Branch { get; set; } = string.Empty;

    }
}
