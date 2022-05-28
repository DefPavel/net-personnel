namespace AlphaPersonel.ViewModels;

internal class DeletePositionViewModel : BaseViewModel
{
    private readonly Persons _person;
    private readonly Users _user;
    //private readonly Departments _Department;
    private readonly Position _position;

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
    private DateTime? _dateDelete;
    public DateTime? DateDelete
    {
        get => _dateDelete;
        set => Set(ref _dateDelete, value);
    }

    private string? _title;
    public string? Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    public DeletePositionViewModel(string title, Users users, Persons persons, Position position)
    {
        _title = title;
        _user = users;
        _person = persons;
        _position = position;
    }


    private ICommand? _getData;
    public ICommand GetData => _getData ??= new LambdaCommand(LoadedApi);

    private ICommand? _closeWin;
    public ICommand CloseWin => _closeWin ??= new LambdaCommand(CloseWindow , _ => SelectedOrders != null && DateDelete != null);

    private async void CloseWindow(object win)
    {
        if (win is not Window w) return;
        try
        {
            object personMove = new
            {
                id = _position.Id,
                id_person = _person.Id,
                created_at = DateDelete,
                name_departament = _position.DepartmentName,
                name_position = _position.Name,
                id_order = SelectedOrders!.Id,
                order_drop = SelectedOrders!.Name,
                date_drop = _selectedOrders!.DateOrder,
                is_main = _person.IsMain,
                kolvo_b = _person.StavkaBudget,
                kolvo_nb = _person.StavkaNoBudget,
                id_contract = _position.IdContract,
                date_begin = _person.StartDateContract,
                date_end = _person.EndDateContract,
                id_person_position = _person.IdPersonPosition

            };

            // Удалить персону
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/position/dropByPerson", "POST", personMove);

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


    private async void LoadedApi(object p)
    {
        try
        {
            // Загрузка приказов
            var idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_user.Token, "/pers/order/type/name", "POST", new TypeOrder { Name = "Увольнение" });
            Orders = await QueryService.JsonDeserializeWithToken<Order>(_user.Token, "/pers/order/get/" + idTypeOrder.Id, "GET");

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

