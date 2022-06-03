namespace AlphaPersonel.ViewModels;

internal class EmployeeExperienceViewModel : BaseViewModel
{

    #region Свойства 
    private readonly Persons persons;

    private DateTime _dateSelect = DateTime.Now;
    public DateTime DateSelected
    {
        get => _dateSelect;
        set => Set(ref _dateSelect, value);

    }

    private string _textWrapResul = string.Empty;
    public string TextWrapResul
    {
        get => _textWrapResul;
        set => Set(ref _textWrapResul, value);
    }
    // Стаж общий
    private bool _stageIsOver;
    public bool StageIsOver
    {
        get => _stageIsOver;
        set => Set(ref _stageIsOver, value);
    }
    // Стаж в Универе
    private bool _stageIsUniver;
    public bool StageIsUniver
    {
        get => _stageIsUniver;
        set => Set(ref _stageIsUniver, value);
    }

    // Стаж Научный
    private bool _stageIsScience;
    public bool StageIsScience
    {
        get => _stageIsScience;
        set => Set(ref _stageIsScience, value);
    }

    // Стаж Научно-Педагогический
    private bool _stageIsPedagogical;
    public bool StageIsPedagogical
    {
        get => _stageIsPedagogical;
        set => Set(ref _stageIsPedagogical, value);
    }

    // Стаж Медицинский
    private bool _stageIsMedical;
    public bool StageIsMedical
    {
        get => _stageIsMedical;
        set => Set(ref _stageIsMedical, value);
    }

    // Стаж Музея
    private bool _stageIsMuseum;
    public bool StageIsMuseum
    {
        get => _stageIsMuseum;
        set => Set(ref _stageIsMuseum, value);
    }

    // Стаж Библиотека
    private bool _stageIsLibrary;
    public bool StageIsLibrary
    {
        get => _stageIsLibrary;
        set => Set(ref _stageIsLibrary, value);
    }
    #endregion

    #region Команды

    private ICommand? _getInfo;
    public ICommand GetInfo => _getInfo ??= new LambdaCommand(GetInformationToPerson , _ => persons != null && DateSelected != null);

    #endregion

    #region Логика
    private void GetInformationToPerson(object p)
    {
        try
        {
            if (persons!.HistoryEmployment?.Count > 0)
            {
                TextWrapResul = string.Empty;
                TextWrapResul = $"ФИО: {persons.FirstName} {persons.MidlleName} {persons.LastName};\r\nДолжность: {persons.ArrayPosition?[0].Name}; \r\n";
                if (StageIsOver == true)
                {
                    TextWrapResul += $"Общий стаж: {ServiceWorkingExperience.GetStageIsOver(persons.HistoryEmployment, DateSelected)}; \r\n";
                }
                if (StageIsUniver == true)
                {
                    TextWrapResul += $"Стаж в ЛГПУ: {ServiceWorkingExperience.GetStageIsUniver(persons.HistoryEmployment, DateSelected)}; \r\n";
                }
                if (StageIsPedagogical == true)
                {
                    TextWrapResul += $"Науч-пед стаж: {ServiceWorkingExperience.GetStageIsPedagogical(persons.HistoryEmployment , DateSelected)}; \r\n";
                }
                if (StageIsScience == true)
                {
                    TextWrapResul += $"Научый стаж: {ServiceWorkingExperience.GetStageIsScience(persons.HistoryEmployment, DateSelected)}; \r\n";
                }
                if (StageIsMedical == true)
                {
                    TextWrapResul += $"Медицинский стаж: {ServiceWorkingExperience.GetStageIsMedical(persons.HistoryEmployment, DateSelected)}; \r\n";
                }
                if (StageIsMuseum == true)
                {
                    TextWrapResul += $"Стаж в музеи: {ServiceWorkingExperience.GetStageIsMuseum(persons.HistoryEmployment, DateSelected)}; \r\n";
                }
                if (StageIsLibrary == true)
                {
                    TextWrapResul += $"Библиотечный стаж: {ServiceWorkingExperience.GetStageIsLibrary(persons.HistoryEmployment, DateSelected)}; \r\n";
                }
            }
        }
        catch (Exception ex)
        {
            _ = MessageBox.Show(ex.Message, "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion

    public EmployeeExperienceViewModel(Persons persons)
    {
        this.persons = persons;
    }

}

