namespace AlphaPersonel.ViewModels;
internal class DeletePersonViewModel : BaseViewModel
{
    #region Свойства

    private readonly Persons _Person;
    private readonly Users _User;
    private readonly Departments _Department;

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
    private DateTime? _DeleteWorking;
    public DateTime? DaleteWorking
    {
        get => _DeleteWorking;
        set => Set(ref _DeleteWorking, value);
    }

    private string? _Title;
    public string? Title
    {
        get => _Title;
        set => Set(ref _Title, value);
    }

    #endregion

    public DeletePersonViewModel(string title ,Users users, Persons persons ,Departments department)
    {
        _Title = title;
        _Person = persons;
        _User = users;
        _Department = department;
    }

    #region Команды
    private ICommand? _GetData;
    public ICommand GetData => _GetData ??= new LambdaCommand(LoadedApi);

    private ICommand? _CloseWin;
    public ICommand CloseWin => _CloseWin ??= new LambdaCommand(CloseWindow);

    #endregion
    #region логика

    private async void CloseWindow(object win)
    {
        if (win is Window w)
        {
            try
            {
                object personMove = new
                {
                    id_person = _Person.Id,
                    created_at = DaleteWorking,
                    name_departament = _Department.Name,
                    name_position = _Person.PersonPosition,
                    id_order = SelectedOrders!.Id,
                    order_drop = SelectedOrders!.Name,
                    date_drop = _SelectedOrders!.DateOrder,
                    is_main = _Person.IsMain,
                    kolvo_b = _Person.StavkaBudget,
                    kolvo_nb = _Person.StavkaNoBudget,
                    id_type_contract = _Person.IdContract,
                    date_begin = _Person.StartDateContract,
                    date_end = _Person.EndDateContract,
                    id_person_position = _Person.IdPersonPosition,
                    is_ped = _Person.IsPed

                };

                // Удалить персону
                await QueryService.JsonSerializeWithToken(_User!.Token, "/pers/person/drop", "POST", personMove);

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
            if (_User.Token == null)
            {
                return;
            }
            // Загрузка приказов
            TypeOrder idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_User.Token, "/pers/order/type/name", "POST", new TypeOrder { Name = "Увольнение" });
            Orders = await QueryService.JsonDeserializeWithToken<Order>(_User.Token, "/pers/order/get/" + idTypeOrder.Id, "GET");

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
}

