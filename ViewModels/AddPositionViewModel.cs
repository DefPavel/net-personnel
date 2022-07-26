using System.Collections.Generic;
using System.Linq;

namespace AlphaPersonel.ViewModels;

internal class AddPositionViewModel : BaseViewModel
{
    private readonly Users _user;
    private readonly Persons _person;


    private decimal? _countBudget = 1;
    public decimal? CountBudget
    {
        get => _countBudget;
        set => Set(ref _countBudget, value);
    }

    private bool _isMain;
    public bool IsMain
    {
        get => _isMain;
        set => Set(ref _isMain, value);
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

    private decimal? _countNoBudget = 0;
    public decimal? CountNoBudget
    {
        get => _countNoBudget;
        set => Set(ref _countNoBudget, value);
    }

    // Массив Должностей
    private IEnumerable<Position>? _positions;
    public IEnumerable<Position>? Positions
    {
        get => _positions;
        private set => Set(ref _positions, value);
    }

    // Тип контракта
    private IEnumerable<TypeContract>? _contracts;
    public IEnumerable<TypeContract>? Contracts
    {
        get => _contracts;
        private set => Set(ref _contracts, value);
    }

    private IEnumerable<Departments>? _departments;
    public IEnumerable<Departments>? Departments
    {
        get => _departments;
        private set => Set(ref _departments, value);
    }

    // Массив Приказов
    private IEnumerable<Order>? _orders;
    public IEnumerable<Order>? Orders
    {
        get => _orders;
        private set => Set(ref _orders, value);
    }
    // Место работы
    private IEnumerable<PlaceOfWork>? _places;
    public IEnumerable<PlaceOfWork>? Places
    {
        get => _places;
        private set => Set(ref _places, value);
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
    // Выбранная отдел
    private Departments? _selectedDepartments;
    public Departments? SelectedDepartments
    {
        get => _selectedDepartments;
        set => Set(ref _selectedDepartments, value);
    }

    private PlaceOfWork? _selectedPlace;
    public PlaceOfWork? SelectedPlace
    {
        get => _selectedPlace;
        set => Set(ref _selectedPlace, value);
    }

    private TypeContract? _selectedContract;
    public TypeContract? SelectedContract
    {
        get => _selectedContract;
        set => Set(ref _selectedContract, value);
    }


    public AddPositionViewModel(Users user, Persons person)
    {
        _user = user;
        _person = person;
    }

    private ICommand? _getData;
    public ICommand GetData => _getData ??= new LambdaAsyncCommand(LoadedApi);

    private ICommand? _getPosition;
    public ICommand GetPosition => _getPosition ??= new LambdaAsyncCommand(LoadedPositions);

    private ICommand? _closeWin;
    public ICommand CloseWin => _closeWin ??= new LambdaAsyncCommand(CloseWindow, _ => 
    SelectedOrders != null 
    && SelectedDepartments != null 
    && SelectedContract != null
    && SelectedPositions != null
    );


    private async Task LoadedPositions(object p)
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

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async Task CloseWindow(object win)
    {
        if (win is not Window w) return;
        try
        {
            if (DateContract != null)
            {
                object person = new
                {
                    id_person = _person.Id,
                    id_place = SelectedPlace?.Id ?? 1,
                    id_position = SelectedPositions!.Id,
                    id_order = SelectedOrders!.Id,
                    id_contract = SelectedContract!.Id,
                    count_budget = CountBudget,
                    count_nobudget = CountNoBudget,
                    date_start_contract = DateContract!,
                    date_end_contract = DateEndContract,
                    is_pluralism_inner = true,
                    is_main = IsMain,
                    created_at = SelectedOrders.DateOrder,
                    name_departament = SelectedDepartments!.Name,
                    name_position = SelectedPositions.Name,
                };

                // Создать персону
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/position/addByPerson", "POST", person);
            }
            else
            {
                _ = MessageBox.Show("Не выбрана дата контракта", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
    private async Task LoadedApi(object p)
    {
        try
        {
            var currentYear = DateTime.Today.Year;
            var prevYear = DateTime.Today.AddYears(-1);
            // Загрузка место работы
            Places = await QueryService.JsonDeserializeWithToken<PlaceOfWork>(_user.Token, "/pers/position/type/place", "GET");
            // загрузка типов контракта
            Contracts = await QueryService.JsonDeserializeWithToken<TypeContract>(_user.Token, "/pers/position/type/contract", "GET");
            // Загрузка приказов
            var idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_user.Token, "/pers/order/type/name", "POST", new TypeOrder { Name = "Перевод" });
            var orders = await QueryService.JsonDeserializeWithToken<Order>(_user.Token, "/pers/order/get/" + idTypeOrder.Id, "GET");
            // Берем за последние два года , чтобы Combobox сильно не тупил от количества элементов
            Orders = orders.Where(x => x.DateOrder.Date.Year == currentYear || x.DateOrder.Date.Year == prevYear.Year);
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

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

