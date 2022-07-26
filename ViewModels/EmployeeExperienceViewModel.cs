namespace AlphaPersonel.ViewModels;

internal class EmployeeExperienceViewModel : BaseViewModel
{
    #region Свойства 
    private readonly Persons _persons;

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
    public ICommand GetInfo => _getInfo ??= new LambdaCommand(GetInformationToPerson , _ => DateSelected != null);
    #endregion

    #region Логика
    private void GetInformationToPerson(object p)
    {
        try
        {
            if (!(_persons!.HistoryEmployment?.Count > 0)) return;
            TextWrapResul = string.Empty;
            TextWrapResul = $"ФИО: {_persons.FirstName} {_persons.MidlleName} {_persons.LastName};\r\nДолжность: {_persons.ArrayPosition?[0].Name}; \r\n";
            if (StageIsOver)
            {
                TextWrapResul += $"Общий стаж: {ServiceWorkingExperience.GetStageIsOver(_persons.HistoryEmployment, DateSelected)}; \r\n";
            }
            if (StageIsUniver)
            {
                TextWrapResul += $"Стаж в ЛГПУ: {ServiceWorkingExperience.GetStageIsUniver(_persons.HistoryEmployment, DateSelected)}; \r\n";
            }
            if (StageIsPedagogical)
            {
                TextWrapResul += $"Науч-пед стаж: {ServiceWorkingExperience.GetStageIsPedagogical(_persons.HistoryEmployment , DateSelected)}; \r\n";
            }
            if (StageIsScience)
            {
                TextWrapResul += $"Научый стаж: {ServiceWorkingExperience.GetStageIsScience(_persons.HistoryEmployment, DateSelected)}; \r\n";
            }
            if (StageIsMedical)
            {
                TextWrapResul += $"Медицинский стаж: {ServiceWorkingExperience.GetStageIsMedical(_persons.HistoryEmployment, DateSelected)}; \r\n";
            }
            if (StageIsMuseum)
            {
                TextWrapResul += $"Стаж в музеи: {ServiceWorkingExperience.GetStageIsMuseum(_persons.HistoryEmployment, DateSelected)}; \r\n";
            }
            if (StageIsLibrary)
            {
                TextWrapResul += $"Библиотечный стаж: {ServiceWorkingExperience.GetStageIsLibrary(_persons.HistoryEmployment, DateSelected)}; \r\n";
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
        this._persons = persons;
    }

}

