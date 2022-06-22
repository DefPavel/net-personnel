using AlphaPersonel.Models.Home;
using AlphaPersonel.Models.PersonCard;
using System.Configuration;

namespace AlphaPersonel.Models;

internal class Persons
{
    #region Свойства персоны

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("id_pers_pos")]
    public int IdPersPos { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("firstname")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string MidlleName { get; set; } = string.Empty;

    [JsonPropertyName("lastname")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonPropertyName("phone_ua")]
    public string PhoneUkraine { get; set; } = string.Empty;

    [JsonPropertyName("phone_lug")]
    public string PhoneLugakom { get; set; } = string.Empty;

    public string ShortDate => Birthday.ToShortDateString();

    [JsonPropertyName("birthday")]
    public DateTime Birthday { get; set; }

    [JsonPropertyName("adress")]
    public string Adress { get; set; } = string.Empty;

    [JsonPropertyName("data_start_contract")]
    public DateTime? StartDateContract { get; set; } 
    [JsonPropertyName("data_end_contract")]
    public DateTime? EndDateContract { get; set; } 

    private string? _Photo;
    [JsonPropertyName("photo")]
    public string Photo
    {
        get => _UrlStorage + _Photo;
        set => _Photo = value;
    }

    [JsonPropertyName("searial_passport")]
    public string SerialPassport { get; set; } = string.Empty;

    [JsonPropertyName("number_passport")]
        public string NumberPassport { get; set; } = string.Empty;

    [JsonPropertyName("organization_passport")]
        public string OrganizationPassport { get; set; } = string.Empty;

    [JsonPropertyName("date_passport")]
        public DateTime DatePassport { get; set; }

    [JsonPropertyName("type_passport")]
        public string TypePassport { get; set; } = string.Empty;

    #endregion

    #region Bool 

    [JsonPropertyName("is_ped")]
    public bool IsPed { get; set; }

    [JsonPropertyName("is_student")]
    public bool IsStudent { get; set; }

    [JsonPropertyName("is_graduate")]
    public bool IsGraduate { get; set; }

    [JsonPropertyName("is_doctor")]
    public bool IsDoctor { get; set; }

    
    [JsonPropertyName("is_pluralism_inner")]
    public bool IsPluralismInner { get; set; }
    
    [JsonPropertyName("is_pluralism_oter")]
    public bool IsPluralismOter { get; set; }

    [JsonPropertyName("is_single_mother")]
    public bool IssingleMother { get; set; }
    [JsonPropertyName("is_two_child_mother")]
    public bool IsTwoChildMother { get; set; }
    [JsonPropertyName("is_previos_convition")]
    public bool IsPreviosConvition { get; set; }

    [JsonPropertyName("is_responsible")]
    public bool IsResponsible { get; set; }

    [JsonPropertyName("is_main")]
    public bool IsMain { get; set; }

    [JsonPropertyName("is_rus")]
    public bool IsRussion { get; set; }

    [JsonPropertyName("is_snils")]
    public bool IsSnils { get; set; }

    #endregion

    [JsonPropertyName("stavka_budget")]
    public decimal StavkaBudget { get; set; }

    [JsonPropertyName("stavka_nobudget")]
    public decimal StavkaNoBudget { get; set; }

    [JsonPropertyName("identification_code")]
    public string IdentificationCode { get; set; } = string.Empty;

    [JsonPropertyName("full_age")]
    public FullAge? FullAge { get; set; }

    [JsonPropertyName("date_working")]
    public DateTime? DateWorking { get; set; }

    [JsonPropertyName("date_order")]
    public DateTime? DateOrder { get; set; }

    [JsonPropertyName("order")]
    public string OrderName { get; set; } = string.Empty;

    [JsonPropertyName("date_insert")]
    public DateTime? OrderDate { get; set; }
    [JsonPropertyName("position")]
    public string PersonPosition { get; set; } = string.Empty;

    #region Массивы
    [JsonPropertyName("positionsOfDepartment")]
    public ObservableCollection<Position>? ArrayPosition { get; set; }

    [JsonPropertyName("vacations")]
    public ObservableCollection<Vacation>? ArrayVacation { get; set; }

    [JsonPropertyName("historyEmployment")]
    public ObservableCollection<HistoryEmployment>? HistoryEmployment { get; set; }

    [JsonPropertyName("moving")]
    public ObservableCollection<Move>? ArrayMove { get; set; }

    [JsonPropertyName("pensioner")]
    public ObservableCollection<Pensioner>? ArrayPensioner { get; set; }

    [JsonPropertyName("ivalid")]
    public ObservableCollection<Invalid>? ArrayInvalid { get; set; }

    [JsonPropertyName("familyPerson")]
    public ObservableCollection<Family>? ArrayFamily { get; set; }

    [JsonPropertyName("education")]
    public ObservableCollection<Education>? ArrayEducation { get; set; }

    [JsonPropertyName("rewarding")]
    public ObservableCollection<Rewarding>? ArrayRewarding { get; set; }

    [JsonPropertyName("qualification")]
    public ObservableCollection<Qualification>? ArrayQualification { get; set; }

    [JsonPropertyName("medicalEducation")]
    public ObservableCollection<Medical>? ArrayMedical { get; set; }

    [JsonPropertyName("scientific_degree")]
    public ObservableCollection<ScientificDegree>? ArrayScientificDegree { get; set; }

    [JsonPropertyName("academic_title")]
    public ObservableCollection<AcademicTitle>? ArrayAcademicTitle { get; set; }

    [JsonPropertyName("accademic")]
    public ObservableCollection<MemberAcademic>? ArrayMeberAcademic { get; set; }

    [JsonPropertyName("changeSurname")]
    public ObservableCollection<OldSurname>? ArrayChangeSurname { get; set; }

    [JsonPropertyName("documents")]
    public ObservableCollection<Documents>? ArrayDocuments { get; set; }



    #endregion

    private string _FullName = string.Empty;
    public string FullName
    {
        get => _FullName = $"{FirstName} {MidlleName} {LastName}";
        set => _FullName = value;
    }

    //[JsonPropertyName("title_department")]
    //public string TitleDepartment { get; set; } = string.Empty;

    [JsonPropertyName("id_dep")]
    public int IdDepartment { get; set; }

    [JsonPropertyName("name_depart")]
    public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("id_contract")]
    public int IdContract { get; set; }

    [JsonPropertyName("id_person_position")]
    public int IdPersonPosition { get; set; }

    [JsonPropertyName("type_contract")]
    public string Contract { get; set; } = string.Empty;

    private readonly string _UrlStorage = ConfigurationManager.AppSettings["storage"] 
        ?? throw new NullReferenceException("Uninitialized property: " + nameof(_UrlStorage));
}

