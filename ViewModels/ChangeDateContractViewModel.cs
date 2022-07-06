using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaPersonel.ViewModels
{
    internal class ChangeDateContractViewModel : BaseViewModel
    {
        private readonly Users _user;
        private readonly Persons _person;

        private Position _selectedPositions;
        public Position SelectedPositions
        {
            get => _selectedPositions;
            set => Set(ref _selectedPositions, value);
        }

        private string _title = "Продление контрактов";
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
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

        public ChangeDateContractViewModel(Users user, Persons person, Position position)
        {
            _user = user;
            _person = person;
            _selectedPositions = position;

        }

        private ICommand? _getData;
        public ICommand GetData => _getData ??= new LambdaCommand(LoadedApiAsync);

        private ICommand? _closeWin;
        public ICommand CloseWin => _closeWin ??= new LambdaCommand(CloseWindow, _ =>
        SelectedOrders != null &&
        SelectedPositions != null
        );

        private async void CloseWindow(object win)
        {
            if (win is not Window w) return;
            try
            {
                if (_selectedPositions.DateStartContract != null)
                {
                    //2201
                    object person = new
                    {
                        id = _selectedPositions.Id,
                        id_person = _person.Id,
                        id_selectedPositions = _selectedPositions!.Id,
                        id_order = SelectedOrders!.Id,
                        id_contract = _selectedPositions!.IdContract,
                        count_budget = _selectedPositions.Count_B,
                        count_nobudget = _selectedPositions.Count_NB,
                        date_start_contract = _selectedPositions.DateStartContract!,
                        date_end_contract = _selectedPositions.DateEndContract,
                        is_pluralism_inner = _selectedPositions.IsMain != true,
                        is_main = _selectedPositions.IsMain,
                        created_at = SelectedOrders.DateOrder,
                        name_departament = _selectedPositions.DepartmentName,
                        name_selectedPositions = _selectedPositions.Name,
                    };

                    // Создать персону
                    await QueryService.JsonSerializeWithToken(_user.Token, "/pers/position/changeDate", "POST", person);
                }
                else
                {
                    _ = MessageBox.Show("Не выбрана дата контракта", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

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

        private async void LoadedApiAsync(object obj)
        {
            try
            {
                if (_user == null) return;
                var currentYear = DateTime.Today.Year;
                var prevYear = DateTime.Today.AddYears(-1);
                var idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_user!.Token, "/pers/order/type/name", "POST", new TypeOrder { Name = "Перевод" });
                var orders = await QueryService.JsonDeserializeWithToken<Order>(_user!.Token, "/pers/order/get/" + idTypeOrder.Id, "GET");

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
}
