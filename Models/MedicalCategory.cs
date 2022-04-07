﻿using System.Text.Json.Serialization;

namespace AlphaPersonel.Models.Home
{
    internal class MedicalCategory
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
