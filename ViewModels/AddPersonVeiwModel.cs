namespace AlphaPersonel.ViewModels;

internal class AddPersonVeiwModel : BaseViewModel
{
    #region Свойства
    private readonly int _idDepartment = 0;
    private readonly Users _User;

    private decimal? _CountBudget = 1;
    public decimal? CountBudget
    {
        get => _CountBudget;
        set => Set(ref _CountBudget, value);
    }

    private decimal? _CountNoBudget = 0;
    public decimal? CountNoBudget
    {
        get => _CountNoBudget;
        set => Set(ref _CountNoBudget, value);
    }

    private string? _FirstName;
    public string? FirstName
    {
        get => _FirstName;
        set => Set(ref _FirstName, value);
    }

    private string? _MiddleName;
    public string? MidlleName
    {
        get => _MiddleName;
        set => Set(ref _MiddleName, value);
    }

    private string? _LastName;
    public string? LastName
    {
        get => _LastName;
        set => Set(ref _LastName, value);
    }

    private string? _MTCPhone;
    public string? MTCPhone
    {
        get => _MTCPhone;
        set => Set(ref _MTCPhone, value);
    }

    private string? _LugPhone;
    public string? LugPhone
    {
        get => _LugPhone;
        set => Set(ref _LugPhone, value);
    }

    private DateTime? _Birthday;
    public DateTime? Birthday
    {
        get => _Birthday;
        set => Set(ref _Birthday, value);
    }

    private DateTime? _DateWorking;
    public DateTime? DateWorking
    {
        get => _DateWorking;
        set => Set(ref _DateWorking, value);
    }
    private bool _IsMain = false;
    public bool IsMain
    {
        get => _IsMain;
        set => Set(ref _IsMain, value);
    }
    private DateTime? _DateContract;
    public DateTime? DateContract
    {
        get => _DateContract;
        set => Set(ref _DateContract, value);
    }

    private bool _Gender;
    public bool Gender
    {
        get => _Gender;
        set => Set(ref _Gender, value);
    }
    // Массив Должностей
    private ObservableCollection<Position>? _Positions;
    public ObservableCollection<Position>? Positions
    {
        get => _Positions;
        private set => Set(ref _Positions, value);
    }

    // Место работы
    private ObservableCollection<PlaceOfWork>? _Places;
    public ObservableCollection<PlaceOfWork>? Places
    {
        get => _Places;
        private set => Set(ref _Places, value);
    }

    private PlaceOfWork? _SelectedPlace;
    public PlaceOfWork? SelectedPlace
    {
        get => _SelectedPlace;
        set => Set(ref _SelectedPlace, value);
    }

    // Тип контракта
    private ObservableCollection<TypeContract>? _Contracts;
    public ObservableCollection<TypeContract>? Contracts
    {
        get => _Contracts;
        private set => Set(ref _Contracts, value);
    }

    // Массив Приказов
    private ObservableCollection<Order>? _Orders;
    public ObservableCollection<Order>? Orders
    {
        get => _Orders;
        private set => Set(ref _Orders, value);
    }

    // Выбранный приказ
    private Order? _SelectedOrders;
    public Order? SelectedOrders
    {
        get => _SelectedOrders;
        set => Set(ref _SelectedOrders, value);
    }

    // Выбранная должность
    private Position? _SelectedPositions;
    public Position? SelectedPositions
    {
        get => _SelectedPositions;
        set => Set(ref _SelectedPositions, value);
    }

    private TypeContract? _SelectedContract;
    public TypeContract? SelectedContract
    {
        get => _SelectedContract;
        set => Set(ref _SelectedContract, value);
    }

    private Persons? _Person;
    public Persons? Person
    {
        get => _Person;
        private set => Set(ref _Person, value);
    }

    #endregion

    public AddPersonVeiwModel(Users user, int idDepartment)
    {
        _idDepartment = idDepartment;
        _User = user;
    }

    #region Команды

    private ICommand? _GetData;
    public ICommand GetData => _GetData ??= new LambdaCommand(LoadedApi);

    private ICommand? _CloseWin;
    public ICommand CloseWin => _CloseWin ??= new LambdaCommand(CloseWindow, _ => SelectedPositions != null && !string.IsNullOrEmpty(FirstName));

    #endregion

    #region Логика

    // Закрытие окна
    private async void CloseWindow(object win)
    {
        if (win is Window w)
        {
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
                    date_to_working = DateWorking!.Value.ToString("yyyy-MM-dd"),
                    data_start_contract = DateContract!.Value.ToString("yyyy-MM-dd"),
                    id_position = SelectedPositions!.Id,
                    id_order = SelectedOrders!.Id,
                    id_contract = SelectedContract!.Id,
                    phone_ua = MTCPhone,
                    phone_lug = LugPhone,
                    gender = Gender == true ? "male" : "female",
                    is_main = IsMain,
                };

                // Создать персону
                await QueryService.JsonSerializeWithToken(_User!.Token, "/pers/person/add", "POST", person);

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

                        if (reader != null)
                        {
                            _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    // Загрузить все справочники
    private async void LoadedApi(object p)
    {
        try
        {

            if (_User.Token == null)
            {
                return;
            }
            // Загрузка место работы
            Places = await QueryService.JsonDeserializeWithToken<PlaceOfWork>(_User.Token, "/pers/position/type/place", "GET");
            // Загрузка приказов
            TypeOrder idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_User.Token, "/pers/order/type/name", "POST", new TypeOrder { Name = "Приём" });
            Orders = await QueryService.JsonDeserializeWithToken<Order>(_User.Token, "/pers/order/get/" + idTypeOrder.Id, "GET");

            //Загрузка должностей
            Positions = await QueryService.JsonDeserializeWithToken<Position>(_User.Token, "/pers/position/get/" + _idDepartment, "GET");

            // загрузка типов контракта
            Contracts = await QueryService.JsonDeserializeWithToken<TypeContract>(_User.Token, "/pers/position/type/contract", "GET");

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
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

