using System;
using System.Text.Json.Serialization;


namespace AlphaPersonel.Models.Home
{
    internal class ScientificDegree
    {
        [JsonPropertyName("id")]  public long Id { get; set; }

        [JsonPropertyName("id_type")] public long IdType { get; set; }

        [JsonPropertyName("type_scientific")] public string TypeScientific { get; set; } = string.Empty;

        [JsonPropertyName("document")] public string Document { get; set; } = string.Empty;

        [JsonPropertyName("scientific_specialty")] public string ScientificSpecialty { get; set; } = string.Empty;

        [JsonPropertyName("scientific_branch")] public string ScientificBranch { get; set; } = string.Empty;

        [JsonPropertyName("count_scientific")]  public int CountScientific { get; set; }

        [JsonPropertyName("dissertation")] public string Dissertation { get; set; } = string.Empty;

        [JsonPropertyName("date_of_issue")] public DateTime DateOfIssue { get; set; }

        [JsonPropertyName("city")] public string City { get; set; } = string.Empty;

        [JsonPropertyName("job_after")] public string JobAfter { get; set; } = string.Empty;

        [JsonPropertyName("place")] public string Place { get; set; } = string.Empty;


    }
}
