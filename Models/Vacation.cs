using System;
using System.Text.Json.Serialization;

namespace AlphaPersonel.Models;

internal class Vacation
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("period")]
    public string Period { get; set; } = string.Empty;
    
    [JsonPropertyName("id_order")]
    public int IdOrder { get; set; } = 0;

    [JsonPropertyName("order")]
    public string Order { get; set; } = string.Empty;

    [JsonPropertyName("date_order")]
    public DateTime DateOrder { get; set; }

    [JsonPropertyName("length")]
    public int LengthVacation { get; set; } = 0;

    [JsonPropertyName("residue")]
    public int Residue { get; set; } = 0;

    [JsonPropertyName("date_start")]
    public DateTime? DateBegin { get; set; }

    [JsonPropertyName("date_end")]
    public DateTime? DateEnd { get; set;}

    [JsonPropertyName("id_person")]
    public int IdPerson { get; set; } = 0;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

}

