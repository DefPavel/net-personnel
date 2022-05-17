using System.Linq;

namespace AlphaPersonel.ViewModels;
internal class TypeOrderViewModel : BaseViewModel 
{
    private readonly NavigationStore _navigationStore;
    private readonly Users _user;

    public TypeOrderViewModel(NavigationStore navigationStore, Users user)
    {
        this._navigationStore = navigationStore;
        _user = user;
    }

    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            _ = Set(ref _filter, value);
            if (TypeOrders != null)
            {
                _collectionDepart!.Refresh();
            }

        }
    }
    // Специальная колекция для фильтров
    private ICollectionView? _collectionDepart;

    private ICollectionView? CollectionDepart
    {
        get => _collectionDepart;
        set => Set(ref _collectionDepart, value);
    }
    private bool FilterToType(object emp)
    {
        return string.IsNullOrEmpty(Filter) || (emp is TypeOrder dep && dep.Name!.ToUpper().Contains(Filter.ToUpper()));
    }


    private ObservableCollection<TypeOrder>? _typeOrders;
    public ObservableCollection<TypeOrder>? TypeOrders
    {
        get => _typeOrders;
        private set
        {
            _ = Set(ref _typeOrders, value);
            if (TypeOrders != null) CollectionDepart = CollectionViewSource.GetDefaultView(TypeOrders);
            if (CollectionDepart != null) CollectionDepart.Filter = FilterToType;
        }
    }

    // Выбранные отдел
    private TypeOrder? _selectedOrder;
    public TypeOrder? SelectedOrder
    {
        get => _selectedOrder;
        set => Set(ref _selectedOrder, value);
    }


    #region Команды

    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _addType;
    public ICommand AddType => _addType ??= new LambdaCommand(AddTypeOrderAsync);

    private ICommand? _saveType;
    public ICommand SaveType => _saveType ??= new LambdaCommand(SaveTypeOrder, _ => SelectedOrder is not null && !string.IsNullOrWhiteSpace(SelectedOrder.Name));

    private ICommand? _deleteType;
    public ICommand DeleteType => _deleteType ??= new LambdaCommand(DeleteTypeOrder, _ => SelectedOrder is not null && TypeOrders!.Count > 0);

    private ICommand? _loadedOrder;

    public ICommand LoadedOrder => _loadedOrder ??= new LambdaCommand(ApiGetOrders);
    #endregion


    #region Логика

    private void GetBack(object p)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }

    // Создать в коллекции новый тип приказа
    private  void AddTypeOrderAsync(object p)
    {
        var count = TypeOrders!.Where(x => x.Id == 0).ToList().Count;
        if(count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        TypeOrder typeOrder = new()
        {
            Name = "Новый вид приказа"
        };
        _typeOrders!.Insert(0, typeOrder);
        SelectedOrder = typeOrder;

    }

    private async void DeleteTypeOrder(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/order/del/type/" + SelectedOrder!.Id, "DELETE", SelectedOrder);

            _ = _typeOrders!.Remove(SelectedOrder);

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

    // Сохранить новую запись типа приказа
    private async void SaveTypeOrder(object p)
    {
        try
        {
            var newSelectedItem = SelectedOrder;
            if (SelectedOrder!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/order/rename/type/" + SelectedOrder.Id, "POST", SelectedOrder);
                // MessageBox.Show("Изменить");
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/order/add/type", "POST", SelectedOrder);
            }
            TypeOrders = await QueryService.JsonDeserializeWithToken<TypeOrder>(_user.Token, "/pers/order/type/get", "GET");
            SelectedOrder = TypeOrders.FirstOrDefault(x => x.Name == newSelectedItem!.Name);

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
    // Вернуть список типов приказов
    private async void ApiGetOrders(object p)
    {
        try
        {
            // Загрузить массив типов приказов
            TypeOrders = await QueryService.JsonDeserializeWithToken<TypeOrder>(_user.Token, "/pers/order/type/get", "GET");


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


    public override void Dispose()
    {

        base.Dispose();
    }
}

