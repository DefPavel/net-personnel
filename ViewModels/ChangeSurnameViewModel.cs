namespace AlphaPersonel.ViewModels;
internal class ChangeSurnameViewModel : BaseViewModel
{

    #region Переменнные

    private readonly Users _user;
    private readonly Persons _person;

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
    // Дата изменения
    private DateTime? _DateChange;
    public DateTime? DateChange
    {
        get => _DateChange;
        set => Set(ref _DateChange, value);
    }
    // Новая фамилия
    private string? _NewSurname;
    public string? NewSurname
    {
        get => _NewSurname;
        set => Set(ref _NewSurname, value);
    }

    #endregion

    public ChangeSurnameViewModel(Users user , Persons persons)
    {
        _user = user;
        _person = persons;
    }

    #region Команды

    private ICommand? _GetData;
    public ICommand GetData => _GetData ??= new LambdaCommand(LoadedApi);

    private ICommand? _CloseWin;
    public ICommand CloseWin => _CloseWin ??= new LambdaCommand(CloseWindow , _ => NewSurname != null);
    #endregion

    #region Логика
    private async void CloseWindow(object win)
    {
        if (win is Window w)
        {
            try
            {
                object paylod = new
                {
                    id_person = _person.Id,
                    created_at = DateChange,
                    id_order = SelectedOrders!.Id,
                    old_surname = _person.FirstName.Trim(),
                    new_surname = NewSurname!.Trim(),
                };

                // Удалить персону
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/rename/firstname", "POST", paylod);

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

    private async void LoadedApi(object p)
    {
        try
        {
            if (_user.Token == null)
            {
                return;
            }
            // Загрузка приказов смены фамилии
            TypeOrder idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_user.Token, "/pers/order/type/name", "POST", new TypeOrder { Name = "Смена фамилии" });
            Orders = await QueryService.JsonDeserializeWithToken<Order>(_user.Token, "/pers/order/get/" + idTypeOrder.Id, "GET");

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
                        // Получаем всю информацию которая пришла с response
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


}

