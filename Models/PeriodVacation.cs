﻿using System.Text.Json.Serialization;

namespace AlphaPersonel.Models.Home
{
    internal class PeriodVacation
    {
        [JsonPropertyName("id_period")]
        public int Id { get; set; }
        [JsonPropertyName("period")] public string Name { get; set; } = string.Empty;
    }
}
