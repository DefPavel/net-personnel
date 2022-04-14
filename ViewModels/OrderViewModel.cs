using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Input;

namespace AlphaPersonel.ViewModels;

internal class OrderViewModel : BaseViewModel
{
    private readonly NavigationStore navigationStore;
    private readonly Users _User;

    public OrderViewModel(NavigationStore navigationStore, Users user)
    {
        this.navigationStore = navigationStore;
        _User = user;
    }

    // Массив отделов
    private ObservableCollection<Order>? _Orders;
    public ObservableCollection<Order>? Orders
    {
        get => _Orders;
        set
        {
            _ = Set(ref _Orders, value);
            CollectionOrder = CollectionViewSource.GetDefaultView(Orders);
            CollectionOrder.Filter = FilterToOrder;
        }
    }

    private ObservableCollection<TypeOrder>? _TypeOrders;
    public ObservableCollection<TypeOrder>? TypeOrders
    {
        get => _TypeOrders;
        private set => Set(ref _TypeOrders, value);
    }

    // Выбранные отдел
    private Order? _SelectedOrder;
    public Order? SelectedOrder
    {
        get => _SelectedOrder;
        set => Set(ref _SelectedOrder, value);
    }

    // Поисковая строка для поиска по ФИО сотрудника
    private string? _Filter;
    public string? Filter
    {
        get => _Filter;
        set
        {
            _ = Set(ref _Filter, value);
            if (Orders != null)
            {
                _CollectionOrder!.Refresh();
            }

        }
    }
    // Специальная колекция для фильтров
    private ICollectionView? _CollectionOrder;
    public ICollectionView? CollectionOrder
    {
        get => _CollectionOrder;
        set => Set(ref _CollectionOrder, value);
    }

    private bool FilterToOrder(object emp) => string.IsNullOrEmpty(Filter) || (emp is Order dep && dep.Name!.ToUpper().Contains(value: Filter.ToUpper()));


    #region Команды

    private ICommand? _GetToMain;
    public ICommand GetToMain => _GetToMain ??= new LambdaCommand(GetBack);

    private ICommand? _LoadedOrder;
    public ICommand? LoadedOrder => _LoadedOrder ??= new LambdaCommand(ApiGetOrders);

    private ICommand? _AddNew;
    public ICommand? AddNew => _AddNew ??= new LambdaCommand(AddOrderAsync);

    private ICommand? _Delete;
    public ICommand? Delete => _Delete ??= new LambdaCommand(DeleteOrder, _ => SelectedOrder != null);

    private ICommand? _Save;
    public ICommand? Save => _Save ??= new LambdaCommand(UpdateOrder, _ => SelectedOrder != null);

    #endregion

    #region Логика

    private void GetBack(object p)
    {
        navigationStore.CurrentViewModel = new HomeViewModel(_User, navigationStore);
    }

    private async void ApiGetOrders(object p)
    {
        try
        {
            if (_User.Token == null) return;
            // Загрузить сами приказы
            Orders = await QueryService.JsonDeserializeWithToken<Order>(_User!.Token, "/pers/order/get", "GET");
            // Загрузить массив типов приказов
            TypeOrders = await QueryService.JsonDeserializeWithToken<TypeOrder>(_User!.Token, "/pers/order/type/get", "GET");
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

    private async void UpdateOrder(object p)
    {
        try
        {
            if (_User.Token == null)
            {
                return;
            }

            if (SelectedOrder!.Id > 0)
            {
                //Изменить уже текущие данные
                await QueryService.JsonSerializeWithToken(token: _User!.Token, "/pers/order/add", "POST", SelectedOrder);
            }
            else
            {
                // Создать новую запись отдела 
                await QueryService.JsonSerializeWithToken(token: _User!.Token, "/pers/order/add", "POST", SelectedOrder);
                // Обновить данные
                Orders = await QueryService.JsonDeserializeWithToken<Order>(_User.Token, "/pers/order/get", "GET");
            }
            _ = MessageBox.Show("Данные успешно сохраненны");
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

    private async void AddOrderAsync(object p)
    {
        try
        {
            Order order = new()
            {
                Name = "000-ОК",
                Type = "Приём",
                DateOrder = DateTime.Now.Date

            };
            _Orders?.Insert(0, order);
            SelectedOrder = order;

        }
        catch (System.Net.WebException ex)
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

    private async void DeleteOrder(object p)
    {
        try
        {
            if (_User.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/order/del/" + SelectedOrder!.Id, "DELETE", SelectedOrder);
                //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

                _ = Orders!.Remove(SelectedOrder);
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


    #endregion

    public override void Dispose()
    {

        base.Dispose();
    }

}

