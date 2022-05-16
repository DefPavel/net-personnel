namespace AlphaPersonel.Models.Home
{
    internal class Education : TypeEducation
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name_diplom")]
        public string Name { get; set; } = string.Empty;

        /*[JsonPropertyName("id_type")]
        public int IdType { get; set; } = 0;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        */

        [JsonPropertyName("is_actual")]
        public bool IsActual { get; set; }

        [JsonPropertyName("institution")]
        public string Institution { get; set; } = string.Empty;

        [JsonPropertyName("specialty")]
        public string Specialty { get; set; } = string.Empty;

        [JsonPropertyName("date_issue")]
        public DateTime? DateIssue { get; set; }

        [JsonPropertyName("qualification")]
        public string Qualification { get; set; } = string.Empty;

        [JsonPropertyName("id_person")]
        public int IdPerson { get; set; } = 0;
    }
}
