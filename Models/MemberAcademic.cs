
namespace AlphaPersonel.Models.Home;

    // Наследуем свойства Типа академиком
    internal class MemberAcademic : TypeRank 
    {
        [JsonPropertyName("id")]
        public int IdMember { get; set; }

        [JsonPropertyName("name_academy")]
        public string NameAcademic { get; set; } = string.Empty;

        [JsonPropertyName("name_diplom")]
        public string Document { get; set; } = string.Empty;

        [JsonPropertyName("date_begin")]
        public DateTime Datebegin { get; set; }

        [JsonPropertyName("id_person")]
        public int IdPerson { get; set; } = 0;


    }

