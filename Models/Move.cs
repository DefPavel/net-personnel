using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlphaPersonel.Models.Home
{
    internal class Move
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name_dep")]
        public string Department { get; set; } = string.Empty;
        [JsonPropertyName("position")]
        public string Position { get; set; } = string.Empty;

        [JsonPropertyName("is_main")]
        public bool IsMain { get; set; }

        [JsonPropertyName("order")]
        public string Order { get; set; } = string.Empty;

        [JsonPropertyName("day_holiday")]
        public int Holiday { get; set; }

        [JsonPropertyName("contract")]
        public string Contract { get; set; } = string.Empty;

        [JsonPropertyName("count_budget")]
        public decimal? CountBudget { get; set; }

        [JsonPropertyName("count_nobudget")]
        public decimal? CountNoBudget { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("date_start")]
        public DateTime? DateStart { get; set; }

        [JsonPropertyName("date_end")]
        public DateTime? DateEnd { get; set; }
    }
}
