namespace AlphaPersonel.Models
{
    internal class Family
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("fullname")]
        public string FullName { get; set; } = string.Empty;
        [JsonPropertyName("birthday")]
        public DateTime? Birthday { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("id_person")]
        public int IdPerson { get; set; } = 0;
    }
}
