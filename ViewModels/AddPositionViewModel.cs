namespace AlphaPersonel.ViewModels;

internal class AddPositionViewModel : BaseViewModel
{
    private readonly Users _user;
    private readonly Persons _person;


    private decimal? _CountBudget = 1;
    public decimal? CountBudget
    {
        get => _CountBudget;
        set => Set(ref _CountBudget, value);
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

    private decimal? _CountNoBudget = 0;
    public decimal? CountNoBudget
    {
        get => _CountNoBudget;
        set => Set(ref _CountNoBudget, value);
    }

    // Массив Должностей
    private ObservableCollection<Position>? _Positions;
    public ObservableCollection<Position>? Positions
    {
        get => _Positions;
        private set => Set(ref _Positions, value);
    }

    // Тип контракта
    private ObservableCollection<TypeContract>? _Contracts;
    public ObservableCollection<TypeContract>? Contracts
    {
        get => _Contracts;
        private set => Set(ref _Contracts, value);
    }

    private ObservableCollection<Departments>? _Departments;
    public ObservableCollection<Departments>? Departments
    {
        get => _Departments;
        private set => Set(ref _Departments, value);
    }

    // Массив Приказов
    private ObservableCollection<Order>? _Orders;
    public ObservableCollection<Order>? Orders
    {
        get => _Orders;
        private set => Set(ref _Orders, value);
    }
    // Место работы
    private ObservableCollection<PlaceOfWork>? _Places;
    public ObservableCollection<PlaceOfWork>? Places
    {
        get => _Places;
        private set => Set(ref _Places, value);
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
    // Выбранная отдел
    private Departments? _SelectedDepartments;
    public Departments? SelectedDepartments
    {
        get => _SelectedDepartments;
        set => Set(ref _SelectedDepartments, value);
    }

    private PlaceOfWork? _SelectedPlace;
    public PlaceOfWork? SelectedPlace
    {
        get => _SelectedPlace;
        set => Set(ref _SelectedPlace, value);
    }

    private TypeContract? _SelectedContract;
    public TypeContract? SelectedContract
    {
        get => _SelectedContract;
        set => Set(ref _SelectedContract, value);
    }


    public AddPositionViewModel(Users user, Persons person)
    {
        _user = user;
        _person = person;
    }

    private ICommand? _GetData;
    public ICommand GetData => _GetData ??= new LambdaCommand(LoadedApi);

    private ICommand? _GetPosition;
    public ICommand GetPosition => _GetPosition ??= new LambdaCommand(LoadedPositions);

    private ICommand? _CloseWin;
    public ICommand CloseWin => _CloseWin ??= new LambdaCommand(CloseWindow, _ => SelectedOrders != null);


    private async void LoadedPositions(object p)
    {
        try
        {
            if (p is Departments dep)
            {
                // Выдать все обещежития по Институту
                Positions = await QueryService.JsonDeserializeWithToken<Position>(_user.Token, "/pers/position/get/" + dep.Id, "GET");
            }

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

    private async void CloseWindow(object win)
    {
        if (win is Window w)
        {
            try
            {
                object person = new
                {
                    id_person = _person.Id,
                    id_place = SelectedPlace == null ? 1 : SelectedPlace.Id,
                    id_position = SelectedPositions!.Id,
                    id_order = SelectedOrders!.Id,
                    id_contract = SelectedContract!.Id,
                    count_budget = CountBudget,
                    count_nobudget = CountNoBudget,
                    data_start_contract = DateContract!.Value.ToString("yyyy-MM-dd"),
                    is_pluralism_inner = true,
                    is_main = IsMain,
                    created_at = SelectedOrders.DateOrder,
                    name_departament = SelectedDepartments.Name,
                    name_position = SelectedPositions.Name,
                };

                // Создать персону
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/position/addByPerson", "POST", person);

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

            if (_user.Token == null)
            {
                return;
            }
            // Загрузка место работы
            Places = await QueryService.JsonDeserializeWithToken<PlaceOfWork>(_user.Token, "/pers/position/type/place", "GET");
            // загрузка типов контракта
            Contracts = await QueryService.JsonDeserializeWithToken<TypeContract>(_user.Token, "/pers/position/type/contract", "GET");
            // Загрузка приказов
            TypeOrder idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_user.Token, "/pers/order/type/name", "POST", new TypeOrder { Name = "Перевод" });
            Orders = await QueryService.JsonDeserializeWithToken<Order>(_user.Token, "/pers/order/get/" + idTypeOrder.Id, "GET");
            // Загрузка отделов
            Departments = await QueryService.JsonDeserializeWithToken<Departments>(_user.Token, "/pers/tree/all" , "GET");
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

