using System.Collections.Generic;
using System.Linq;

namespace AlphaPersonel.ViewModels;
internal class DeletePersonViewModel : BaseViewModel
{
    #region Свойства

    private readonly Persons _person;
    private readonly Users _user;
    private readonly Departments _department;

    // Массив Приказов
    private IEnumerable<Order>? _orders;
    public IEnumerable<Order>? Orders
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
    private DateTime? _deleteWorking;
    public DateTime? DaleteWorking
    {
        get => _deleteWorking;
        set => Set(ref _deleteWorking, value);
    }

    private string? _title;
    public string? Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    #endregion
    public DeletePersonViewModel(string title ,Users users, Persons persons ,Departments department)
    {
        _title = title;
        _person = persons;
        _user = users;
        _department = department;
    }

    #region Команды
    private ICommand? _getData;
    public ICommand GetData => _getData ??= new LambdaCommand(LoadedApi);

    private ICommand? _closeWin;
    public ICommand CloseWin => _closeWin ??= new LambdaCommand(CloseWindow);

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
                    id_person = _person.Id,
                    created_at = DaleteWorking,
                    name_departament = _department.Name,
                    name_position = _person.PersonPosition,
                    id_order = SelectedOrders!.Id,
                    order_drop = SelectedOrders!.Name,
                    date_drop = _selectedOrders!.DateOrder,
                    is_main = _person.IsMain,
                    kolvo_b = _person.StavkaBudget,
                    kolvo_nb = _person.StavkaNoBudget,
                    id_type_contract = _person.IdContract,
                    date_begin = _person.StartDateContract,
                    date_end = _person.EndDateContract,
                    id_person_position = _person.IdPersonPosition,
                    is_pluralism_oter = _person.IsPluralismOter,
                    is_ped = _person.IsPed

                };

                // Удалить персону
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/drop", "POST", personMove);

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
    }

    private async void LoadedApi(object p)
    {
        
        try
        {
            var currentYear = DateTime.Today.Year;
            var prevYear = DateTime.Today.AddYears(-1);

            var idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_user.Token, "/pers/order/type/name", "POST", new TypeOrder { Name = "Увольнение" });
            // Загрузка приказов
            var orders = await QueryService.JsonDeserializeWithToken<Order>(_user.Token, "/pers/order/get/" + idTypeOrder.Id, "GET");
            // Берем за последние два года , чтобы Combobox сильно не тупил от количества элементов
            Orders = orders.Where(x => x.DateOrder.Date.Year == currentYear || x.DateOrder.Date.Year == prevYear.Year);

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

