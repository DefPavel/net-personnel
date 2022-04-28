namespace AlphaPersonel.ViewModels;

internal class MasterReportViewModel : BaseViewModel
{
    public MasterReportViewModel(NavigationStore navigationStore, Users user)
    {
        _navigationStore = navigationStore;
        _user = user;

        IsPedagogical = new ObservableCollection<PedagogicalPosition> 
        { 
            new()
            {
                IdPed = 1,
                IsPed = "Педагогическая",
                Query = "typ_pos.is_ped = true",
            },
            new()
            {
                IdPed = 2,
                IsPed = "Не Педагогическая",
                Query = "typ_pos.is_ped = false",
            },
            new()
            {
                IdPed = -1,
                IsPed = "Все",
                Query = "",                
            },
        };

        TypeGender = new ObservableCollection<string>
        {
            "Мужской",
            "Женский",
        };

        TypeStatus = new ObservableCollection<string>
        {
            "Студент",
            "Аспирант",
            "Доктор наук",
            "Мать одиночка",
            "Мать двоих и более детей",
            "Предоставил справку о не судимости",
            "Имеет русский паспорт",
            "Имеет СНИСЛ",
            "Материально ответственный",
            "Внутренний совместитель",
            "Внешний совместитель"

        };
        TypeAge = new ObservableCollection<string>
        {
          "От 18 до 21",
          "От 25 до 76",
          "Старше 65"
        };
    }

    #region Переменные

    private readonly NavigationStore _navigationStore;
    private readonly Users _user;

    private ObservableCollection<PedagogicalPosition>? _isPedagogical;
    public ObservableCollection<PedagogicalPosition>? IsPedagogical
    {
        get => _isPedagogical;
        private set => Set(ref _isPedagogical, value);
    }
    private ObservableCollection<Position>? _positions;
    public ObservableCollection<Position>? Positions
    {
        get => _positions;
        private set => Set(ref _positions, value);
    }
    private PedagogicalPosition? _selectedIsPed;
    public PedagogicalPosition? SelectedIsPed
    {
        get => _selectedIsPed;
        set => Set(ref _selectedIsPed, value);
    }

    private ObservableCollection<TypeContract>? _typeContracts;
    public ObservableCollection<TypeContract>? TypeContracts
    {
        get => _typeContracts;
        private set => Set(ref _typeContracts, value);
    }
    private TypeContract? _selectedContract;
    public TypeContract? SelectedContract
    {
        get => _selectedContract;
        set => Set(ref _selectedContract, value);
    }

    private Position? _selectedPosition;
    public Position? SelectedPosition
    {
        get => _selectedPosition;
        set => Set(ref _selectedPosition, value);
    }

    private DateTime? _dateStart;
    public DateTime? DateStart
    {
        get => _dateStart;
        set => Set(ref _dateStart, value);
    }

    private DateTime? _dateEnd;
    public DateTime? DateEnd
    {
        get => _dateEnd;
        set => Set(ref _dateEnd, value);
    }

    private ObservableCollection<string>? _typeGender;
    public ObservableCollection<string>? TypeGender
    {
        get => _typeGender;
        private set => Set(ref _typeGender, value);
    }
    private string? _selectedGender;
    public string? SelectedGender
    {
        get => _selectedGender;
        set => Set(ref _selectedGender, value);
    }
    private ObservableCollection<string>? _typeStatus;
    public ObservableCollection<string>? TypeStatus
    {
        get => _typeStatus;
        private set => Set(ref _typeStatus, value);
    }
    private string? _selectedStatus;
    public string? SelectedStatus
    {
        get => _selectedStatus;
        set => Set(ref _selectedStatus, value);
    }

    private ObservableCollection<string>? _typeAge;
    public ObservableCollection<string>? TypeAge
    {
        get => _typeAge;
        private set => Set(ref _typeAge, value);
    }
    private string? _selectedAge;
    public string? SelectedAge
    {
        get => _selectedAge;
        set => Set(ref _selectedAge, value);
    }


    #endregion

    #region Команды

    private ICommand? _getData;
    public ICommand GetData => _getData ??= new LambdaCommand(LoadedContract);

    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _getPosition;
    public ICommand GetPosition => _getPosition ??= new LambdaCommand(LoadedPositions , _ => SelectedIsPed != null);

    private ICommand? _getReport;
    public ICommand GetReport => _getReport ??= new LambdaCommand(Reports);

    #endregion

    #region Логика

    private void GetBack(object obj)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }

    private async void Reports(object win)
    {
        try
        {

            var status = string.Empty;
            var age = string.Empty;

            if(SelectedStatus != null)
            {
                status = SelectedStatus == "Студент" ? " pp.is_main = true and p.is_student = true"
                    : SelectedStatus == "Аспирант" ? " pp.is_main = true and p.is_graduate = true"
                    : SelectedStatus == "Доктор наук" ? "pp.is_main = true and p.is_doctor = true"
                    : SelectedStatus == "Мать одиночка" ? "pp.is_main = true and p.is_single_mother = true"
                    : SelectedStatus == "Мать двоих и более детей" ? "pp.is_main = true and p.is_two_child_mother = true"
                    : SelectedStatus == "Предоставил справку о не судимости" ? "pp.is_main = true and p.is_previos_convition = true"
                    : SelectedStatus == "Имеет русский паспорт" ? "pp.is_main = true and p.is_rus = true"
                    : SelectedStatus == "Имеет СНИСЛ" ? "pp.is_main = true and p.is_snils = true"
                    : SelectedStatus == "Материально ответственный" ? "pp.is_main = true and p.is_responsible = true"
                    : SelectedStatus == "Внутренний совместитель" ? " pp.is_main = false and pp.is_pluralism_inner = true"
                    : SelectedStatus == "Внешний совместитель" ? " pp.is_main = false and pp.is_pluralism_oter = true"
                    : null;



            }
            if(SelectedAge != null)
            {
                age = SelectedAge == "От 18 до 21" ? " (EXTRACT( YEAR FROM age(p.birthday))) >= 18 and (EXTRACT( YEAR FROM age(p.birthday))) < 22 " :
                    SelectedAge == "От 25 до 76" ? " (EXTRACT( YEAR FROM age(p.birthday))) >= 25 and (EXTRACT( YEAR FROM age(p.birthday))) < 77 " :
                    SelectedAge == "Старше 65" ? " (EXTRACT( YEAR FROM age(p.birthday))) >= 65 " :
                    null;
            }

            object person = new
            {
                is_ped = SelectedIsPed?.IdPed,
                query_ped = SelectedIsPed?.Query,
                query_position = SelectedPosition != null ? $"{(SelectedStatus is "Внутренний совместитель" or "Внешний совместитель" ? " pp.is_main = false" : " pp.is_main = true")} and typ_pos.name_position = '{SelectedPosition?.Name}'" : null,
                query_contract = SelectedContract != null ? $"{(SelectedStatus is "Внутренний совместитель" or "Внешний совместитель" ? " pp.is_main = false" : " pp.is_main = true")} and contract.id = {SelectedContract?.Id}" : null,
                query_date_start = DateStart != null ?
                $" pp.is_main = true and p.date_to_working >= '{DateStart.Value:yyyy-MM-dd}' and p.date_to_working < '{DateEnd.Value:yyyy-MM-dd}'" : null,
                query_gender = SelectedGender != null ? $"{(SelectedStatus is "Внутренний совместитель" or "Внешний совместитель" ? " pp.is_main = false" : " pp.is_main = true")} and p.gender = {(SelectedGender == "Мужской" ? "'male'" : "'female'")}" : null,
                query_status = status,
                query_age = age

            };
            // Отправить запрос
            await ReportService.JsonPostWithToken(person, _user!.Token, "/reports/pers/master", "POST", "Отчет с параметрами");

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void LoadedContract(object p)
    {
        try
        {
           TypeContracts = await QueryService.JsonDeserializeWithToken<TypeContract>(_user.Token, "/pers/position/type/contract/", "GET");

        }
        catch (WebException ex)
        {

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void LoadedPositions(object p)
    {
        try
        {
            if (p is PedagogicalPosition ped)
            {
                // Выдать все обещежития по Институту
                Positions = await QueryService.JsonDeserializeWithToken<Position>(_user.Token, "/pers/position/get/ped/" + ped.IdPed, "GET");
            }

        }
        catch (WebException ex)
        {

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    #endregion

    public override void Dispose()
    {
        base.Dispose();
    }
}