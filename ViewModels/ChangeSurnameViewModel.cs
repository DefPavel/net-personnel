using System.Collections.Generic;
using System.Linq;

namespace AlphaPersonel.ViewModels;
internal class ChangeSurnameViewModel : BaseViewModel
{

    #region Переменнные

    private readonly Users _user;
    private readonly Persons _person;

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
    // Дата изменения
    private DateTime? _dateChange;
    public DateTime? DateChange
    {
        get => _dateChange;
        set => Set(ref _dateChange, value);
    }
    // Новая фамилия
    private string? _newSurname;
    public string? NewSurname
    {
        get => _newSurname;
        set => Set(ref _newSurname, value);
    }

    #endregion

    public ChangeSurnameViewModel(Users user , Persons persons)
    {
        _user = user;
        _person = persons;
    }

    #region Команды

    private ICommand? _getData;
    public ICommand GetData => _getData ??= new LambdaAsyncCommand(LoadedApi);

    private ICommand? _closeWin;
    public ICommand CloseWin => _closeWin ??= new LambdaAsyncCommand(CloseWindow , _ => NewSurname != null);
    #endregion

    #region Логика
    private async Task CloseWindow(object win)
    {
        if (win is not Window w) return;
        if(DateChange == null)
        {
            _ = MessageBox.Show("Необходимо выбрать дату!");
            return;
        }
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

            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/rename/firstname", "POST", paylod);

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
            // Загрузка приказов смены фамилии
            var idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_user.Token,
                "/pers/order/type/name", "POST", new TypeOrder { Name = "Смена фамилии" });
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

                    // Получаем всю информацию которая пришла с response
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

