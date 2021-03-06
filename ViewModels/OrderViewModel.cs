using System.Linq;
namespace AlphaPersonel.ViewModels;

internal class OrderViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Users _user;

    public OrderViewModel(NavigationStore navigationStore, Users user)
    {
        this._navigationStore = navigationStore;
        _user = user;
    }

    // Массив отделов
    private ObservableCollection<Order>? _orders;
    public ObservableCollection<Order>? Orders
    {
        get => _orders;
        set
        {
            _ = Set(ref _orders, value);
            if (Orders != null) CollectionOrder = CollectionViewSource.GetDefaultView(Orders);
            if (CollectionOrder != null) CollectionOrder.Filter = FilterToOrder;
        }
    }

    private ObservableCollection<TypeOrder>? _typeOrders;
    public ObservableCollection<TypeOrder>? TypeOrders
    {
        get => _typeOrders;
        private set => Set(ref _typeOrders, value);
    }

    // Выбранные отдел
    private Order? _selectedOrder;
    public Order? SelectedOrder
    {
        get => _selectedOrder;
        set => Set(ref _selectedOrder, value);
    }

    // Поисковая строка для поиска по ФИО сотрудника
    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            _ = Set(ref _filter, value);
            if (Orders != null)
            {
                _collectionOrder!.Refresh();
            }

        }
    }
    // Специальная колекция для фильтров
    private ICollectionView? _collectionOrder;
    public ICollectionView? CollectionOrder
    {
        get => _collectionOrder;
        private set => Set(ref _collectionOrder, value);
    }

    private bool FilterToOrder(object emp) => string.IsNullOrEmpty(Filter) || (emp is Order dep && dep.Name.ToUpper().Contains(value: Filter.ToUpper()));


    #region Команды

    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _loadedOrder;
    public ICommand LoadedOrder => _loadedOrder ??= new LambdaAsyncCommand(ApiGetOrders);

    private ICommand? _addNew;
    public ICommand AddNew => _addNew ??= new LambdaCommand(AddOrderAsync);

    private ICommand? _delete;
    public ICommand Delete => _delete ??= new LambdaAsyncCommand(DeleteOrder, _ => SelectedOrder is not null && Orders!.Count > 0);

    private ICommand? _save;
    public ICommand Save => _save ??= new LambdaAsyncCommand(UpdateOrder, _ => SelectedOrder is not null );

    #endregion

    #region Логика
    private void GetBack(object p)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }
    private async Task ApiGetOrders(object p)
    {
        try
        {
            // Загрузить сами приказы
            Orders = await QueryService.JsonDeserializeWithToken<Order>(_user.Token, "/pers/order/get", "GET");
            // Загрузить массив типов приказов
            TypeOrders = await QueryService.JsonDeserializeWithToken<TypeOrder>(_user.Token, "/pers/order/type/get", "GET");

            if (Orders.Count > 0)
            {
                SelectedOrder = Orders[0];
            }
        }
        // Проверка токена
        catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == (HttpStatusCode)403)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
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
    private async Task UpdateOrder(object p)
    {
        try
        {
            if (SelectedOrder!.Id > 0)
            {
                //Изменить уже текущие данные
                await QueryService.JsonSerializeWithToken(token: _user.Token, "/pers/order/rename/" + SelectedOrder.Id, "POST", SelectedOrder);
            }
            else
            {
                // Создать новую запись отдела 
                await QueryService.JsonSerializeWithToken(token: _user.Token, "/pers/order/add", "POST", SelectedOrder);
                
            }
            // Обновить данные
            Orders = await QueryService.JsonDeserializeWithToken<Order>(_user.Token, "/pers/order/get", "GET");

            _ = MessageBox.Show("Данные успешно сохраненны");
        }
        // Проверка токена
        catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == (HttpStatusCode)403)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
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
    private void AddOrderAsync(object p)
    {
        var count = Orders!.Where(x => x.Id == 0).ToList().Count;
        if(count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        Order order = new()
        {
            Name = $"000-ОК(от {DateTime.Now.ToShortDateString()})",
            Type = "Приём",
            DateOrder = DateTime.Now.Date

        };
        _orders?.Insert(0, order);
        SelectedOrder = order;

      
    }
    private async Task DeleteOrder(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/order/del/" + SelectedOrder!.Id, "DELETE", SelectedOrder);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = Orders!.Remove(SelectedOrder);
        }
        // Проверка токена
        catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == (HttpStatusCode)403)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
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
}

