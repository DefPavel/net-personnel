namespace AlphaPersonel.ViewModels;

internal class AddPersonVeiwModel : BaseViewModel
{
    #region Свойства
    private readonly int _idDepartment;
    private readonly Users _user;

    private decimal? _countBudget = 1;
    public decimal? CountBudget
    {
        get => _countBudget;
        set => Set(ref _countBudget, value);
    }

    private decimal? _countNoBudget = 0;
    public decimal? CountNoBudget
    {
        get => _countNoBudget;
        set => Set(ref _countNoBudget, value);
    }

    private string? _firstName;
    public string? FirstName
    {
        get => _firstName;
        set => Set(ref _firstName, value);
    }

    private string? _middleName;
    public string? MidlleName
    {
        get => _middleName;
        set => Set(ref _middleName, value);
    }

    private string? _lastName;
    public string? LastName
    {
        get => _lastName;
        set => Set(ref _lastName, value);
    }

    private string? _mtcPhone;
    public string? MtcPhone
    {
        get => _mtcPhone;
        set => Set(ref _mtcPhone, value);
    }

    private string? _lugPhone;
    public string? LugPhone
    {
        get => _lugPhone;
        set => Set(ref _lugPhone, value);
    }

    private DateTime? _birthday;
    public DateTime? Birthday
    {
        get => _birthday;
        set => Set(ref _birthday, value);
    }

    private DateTime? _dateWorking;
    public DateTime? DateWorking
    {
        get => _dateWorking;
        set => Set(ref _dateWorking, value);
    }
    private bool _isMain;
    public bool IsMain
    {
        get => _isMain;
        set => Set(ref _isMain, value);
    }
    private bool _isOter;
    public bool IsOter
    {
        get => _isOter;
        set => Set(ref _isOter, value);
    }
    private DateTime? _dateContract;
    public DateTime? DateContract
    {
        get => _dateContract;
        set => Set(ref _dateContract, value);
    }
    private DateTime? _dateEndContract;
    public DateTime? DateEndContract
    {
        get => _dateEndContract;
        set => Set(ref _dateEndContract, value);
    }

    private bool _gender;
    public bool Gender
    {
        get => _gender;
        set => Set(ref _gender, value);
    }
    // Массив Должностей
    private ObservableCollection<Position>? _positions;
    public ObservableCollection<Position>? Positions
    {
        get => _positions;
        private set => Set(ref _positions, value);
    }

    // Место работы
    private ObservableCollection<PlaceOfWork>? _places;
    public ObservableCollection<PlaceOfWork>? Places
    {
        get => _places;
        private set => Set(ref _places, value);
    }

    private PlaceOfWork? _selectedPlace;
    public PlaceOfWork? SelectedPlace
    {
        get => _selectedPlace;
        set => Set(ref _selectedPlace, value);
    }

    // Тип контракта
    private ObservableCollection<TypeContract>? _contracts;
    public ObservableCollection<TypeContract>? Contracts
    {
        get => _contracts;
        private set => Set(ref _contracts, value);
    }

    // Массив Приказов
    private ObservableCollection<Order>? _orders;
    public ObservableCollection<Order>? Orders
    {
        get => _orders;
        private set => Set(ref _orders, value);
    }

    // Выбранный приказ
    private Order? _selectedOrders;
    public Order? SelectedOrders
    {
        get => _selectedOrders;
        set => Set(ref _selectedOrders, value);
    }

    // Выбранная должность
    private Position? _selectedPositions;
    public Position? SelectedPositions
    {
        get => _selectedPositions;
        set => Set(ref _selectedPositions, value);
    }

    private TypeContract? _selectedContract;
    public TypeContract? SelectedContract
    {
        get => _selectedContract;
        set => Set(ref _selectedContract, value);
    }

    #endregion

    public AddPersonVeiwModel(Users user, int idDepartment)
    {
        _idDepartment = idDepartment;
        _user = user;
    }

    #region Команды

    private ICommand? _getData;
    public ICommand GetData => _getData ??= new LambdaCommand(LoadedApi);

    private ICommand? _closeWin;
    public ICommand CloseWin => _closeWin ??= new LambdaCommand(CloseWindow, _ => SelectedPositions != null && SelectedOrders != null && SelectedContract!= null && !string.IsNullOrEmpty(FirstName));

    #endregion

    #region Логика

    // Закрытие окна
    private async void CloseWindow(object win)
    {
        if (win is not Window w) return;
        try
        {
            
            object person = new
            {
                firstname = FirstName,
                name = MidlleName,
                lastname = LastName,
                birthday = Birthday!.Value.ToString("yyyy-MM-dd"),
                type_passport = 1,
                count_budget = CountBudget,
                count_nobudget = CountNoBudget,
                data_to_working = DateWorking!.Value.ToString("yyyy-MM-dd"),
                data_start_contract = DateContract!,
                date_end_contract = DateEndContract,
                id_position = SelectedPositions!.Id,
                id_order = SelectedOrders!.Id,
                id_contract = SelectedContract!.Id,
                phone_ua = MtcPhone,
                phone_lug = LugPhone,
                gender = Gender ? "male" : "female",
                is_main = IsOter != true && IsMain,
                is_pluralism_oter = IsOter,
            };

            // Создать персону
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/add", "POST", person);

            w.DialogResult = true;
            w.Close();
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

    // Загрузить все справочники
    private async void LoadedApi(object p)
    {
        try
        {
            // Загрузка место работы
            Places = await QueryService.JsonDeserializeWithToken<PlaceOfWork>(_user.Token, "/pers/position/type/place", "GET");
            // Загрузка приказов
            var idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_user.Token, "/pers/order/type/name", "POST", new TypeOrder { Name = "Приём" });
            Orders = await QueryService.JsonDeserializeWithToken<Order>(_user.Token, "/pers/order/get/" + idTypeOrder.Id, "GET");
            //Загрузка должностей
            Positions = await QueryService.JsonDeserializeWithToken<Position>(_user.Token, "/pers/position/get/" + _idDepartment, "GET");
            // загрузка типов контракта
            Contracts = await QueryService.JsonDeserializeWithToken<TypeContract>(_user.Token, "/pers/position/type/contract", "GET");
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

