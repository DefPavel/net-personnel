using System.Linq;

namespace AlphaPersonel.ViewModels;

internal class TypeOrderViewModel : BaseViewModel 
{
    private readonly NavigationStore navigationStore;
    private readonly Users _User;

    public TypeOrderViewModel(NavigationStore navigationStore, Users user)
    {
        this.navigationStore = navigationStore;
        _User = user;
    }

    private string? _Filter;
    public string? Filter
    {
        get => _Filter;
        set
        {
            _ = Set(ref _Filter, value);
            if (TypeOrders != null)
            {
                _CollectionDepart!.Refresh();
            }

        }
    }
    // Специальная колекция для фильтров
    private ICollectionView? _CollectionDepart;
    public ICollectionView? CollectionDepart
    {
        get => _CollectionDepart;
        private set => Set(ref _CollectionDepart, value);
    }
    private bool FilterToType(object emp)
    {
        return string.IsNullOrEmpty(Filter) || (emp is TypeOrder dep && dep.Name!.ToUpper().Contains(Filter.ToUpper()));
    }


    private ObservableCollection<TypeOrder>? _TypeOrders;
    public ObservableCollection<TypeOrder>? TypeOrders
    {
        get => _TypeOrders;
        private set
        {
            _ = Set(ref _TypeOrders, value);
            CollectionDepart = CollectionViewSource.GetDefaultView(TypeOrders);
            CollectionDepart.Filter = FilterToType;

        }
    }

    // Выбранные отдел
    private TypeOrder? _SelectedOrder;
    public TypeOrder? SelectedOrder
    {
        get => _SelectedOrder;
        set => Set(ref _SelectedOrder, value);
    }


    #region Команды

    private ICommand? _GetToMain;
    public ICommand GetToMain => _GetToMain ??= new LambdaCommand(GetBack);

    private ICommand? _AddType;
    public ICommand AddType => _AddType ??= new LambdaCommand(AddTypeOrderAsync);

    private ICommand? _SaveType;
    public ICommand SaveType => _SaveType ??= new LambdaCommand(SaveTypeOrder, _ => SelectedOrder is not null && !string.IsNullOrWhiteSpace(SelectedOrder.Name));

    private ICommand? _DeleteType;
    public ICommand DeleteType => _DeleteType ??= new LambdaCommand(DeleteTypeOrder, _ => SelectedOrder is not null && TypeOrders!.Count > 0);

    private ICommand? _LoadedOrder;

    public ICommand LoadedOrder => _LoadedOrder ??= new LambdaCommand(ApiGetOrders);
    #endregion


    #region Логика

    private void GetBack(object p)
    {
        navigationStore.CurrentViewModel = new HomeViewModel(_User, navigationStore);
    }

    // Создать в коллекции новый тип приказа
    private async void AddTypeOrderAsync(object p)
    {
        try
        {
            TypeOrder typeOrder = new()
            {
                Name = "Новый вид приказа"
            };
            _TypeOrders!.Insert(0, typeOrder);
            SelectedOrder = typeOrder;

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

    private async void DeleteTypeOrder(object p)
    {
        try
        {
            if (_User.Token == null) return;
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/order/del/type/" + SelectedOrder!.Id, "DELETE", SelectedOrder);

                _ = _TypeOrders!.Remove(SelectedOrder);
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

    // Сохранить новую запись типа приказа
    private async void SaveTypeOrder(object p)
    {
        try
        {
            if (_User.Token == null) return;
            var newSlectedItem = SelectedOrder;
            if (SelectedOrder!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/order/rename/type/" + SelectedOrder.Id, "POST", SelectedOrder);
                // MessageBox.Show("Изменить");
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/order/add/type", "POST", SelectedOrder);
            }
            TypeOrders = await QueryService.JsonDeserializeWithToken<TypeOrder>(_User.Token, "/pers/order/type/get", "GET");
            SelectedOrder = TypeOrders.FirstOrDefault(x => x.Name == newSlectedItem!.Name);

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
    // Вернуть список типов приказов
    private async void ApiGetOrders(object p)
    {
        try
        {
            if (_User.Token == null) return;
            // Загрузить массив типов приказов
            TypeOrders = await QueryService.JsonDeserializeWithToken<TypeOrder>(_User.Token, "/pers/order/type/get", "GET");


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

