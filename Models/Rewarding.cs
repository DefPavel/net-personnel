namespace AlphaPersonel.Models;

    internal class Rewarding
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("id_person")]
        public int IdPerson { get; set; } = 0;

        [JsonPropertyName("id_type")]
        public int IdType { get; set; } = 0;

        [JsonPropertyName("type_rewarding")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("date_issuing")]
        public DateTime? DateIssue { get; set; }

        // Внешние приказы
        [JsonPropertyName("notes")]
        public string NumberDocumet { get; set; } = string.Empty;

        [JsonPropertyName("id_order")]
        public int IdOrder { get; set; } = 0;

        [JsonPropertyName("order")]
        public string Order { get; set; } = string.Empty;

        [JsonPropertyName("date_order")]
        public DateTime DateOrder { get; set; }
}


