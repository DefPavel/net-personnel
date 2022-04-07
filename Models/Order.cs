using System;
using System.Text.Json.Serialization;

namespace AlphaPersonel.Models
{
    internal class Order
    {
        [JsonPropertyName("id_order")]
        public int Id { get; set; }
        [JsonPropertyName("order")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("date_order")]
        public DateTime DateOrder { get; set; }
        [JsonPropertyName("type_order")]
        public string Type { get; set; } = string.Empty;
    }
}
