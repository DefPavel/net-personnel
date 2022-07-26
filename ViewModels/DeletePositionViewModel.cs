using System.Collections.Generic;
using System.Linq;

namespace AlphaPersonel.ViewModels;

internal class DeletePositionViewModel : BaseViewModel
{
    private readonly Persons _person;
    private readonly Users _user;
    //private readonly Departments _Department;
    private readonly Position _position;

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
    public ICommand GetData => _getData ??= new LambdaAsyncCommand(LoadedApi);

    private ICommand? _closeWin;
    public ICommand CloseWin => _closeWin ??= new LambdaAsyncCommand(CloseWindow , _ => SelectedOrders != null && DateDelete != null);

    private async Task CloseWindow(object win)
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


    private async Task LoadedApi(object p)
    {
        try
        {
            var currentYear = DateTime.Today.Year;
            var prevYear = DateTime.Today.AddYears(-1);

            // Загрузка приказов
            var idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_user.Token, "/pers/order/type/name", "POST", new TypeOrder { Name = "Увольнение" });
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



}

