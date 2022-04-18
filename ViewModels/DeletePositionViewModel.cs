using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaPersonel.ViewModels;

internal class DeletePositionViewModel : BaseViewModel
{
    private readonly Persons _Person;
    private readonly Users _User;
    private readonly Departments _Department;
    private readonly Position _Position;

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
    private DateTime? _Date_Delete;
    public DateTime? DateDelete
    {
        get => _Date_Delete;
        set => Set(ref _Date_Delete, value);
    }

    private string? _Title;
    public string? Title
    {
        get => _Title;
        set => Set(ref _Title, value);
    }

    public DeletePositionViewModel(string title, Users users, Persons persons, Position position)
    {
        _Title = title;
        _User = users;
        _Person = persons;
        _Position = position;
    }


    private ICommand? _GetData;
    public ICommand GetData => _GetData ??= new LambdaCommand(LoadedApi);

    private ICommand? _CloseWin;
    public ICommand CloseWin => _CloseWin ??= new LambdaCommand(CloseWindow);

    private async void CloseWindow(object win)
    {
        if (win is Window w)
        {
            try
            {
                object personMove = new
                {
                    id = _Position.Id,
                    id_person = _Person.Id,
                    created_at = DateDelete,
                    name_departament = _Position.DepartmentName,
                    name_position = _Position.Name,
                    id_order = SelectedOrders!.Id,
                    order_drop = SelectedOrders!.Name,
                    date_drop = _SelectedOrders!.DateOrder,
                    is_main = _Person.IsMain,
                    kolvo_b = _Person.StavkaBudget,
                    kolvo_nb = _Person.StavkaNoBudget,
                    id_contract = _Position.IdContract,
                    date_begin = _Person.StartDateContract,
                    date_end = _Person.EndDateContract,
                    id_person_position = _Person.IdPersonPosition

                };

                // Удалить персону
                await QueryService.JsonSerializeWithToken(_User!.Token, "/pers/position/dropByPerson", "POST", personMove);

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



}

