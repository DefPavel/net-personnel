using AlphaPersonel.Views.Models;
using System.Collections.Generic;
using System.Linq;
namespace AlphaPersonel.ViewModels;

internal class HomeViewModel : BaseViewModel
{
    public HomeViewModel(Users account, NavigationStore navigationStore)
    {
        _user = account;
        this._navigationStore = navigationStore;
    }
    // При возврате с личной карты,чтобы оставались на том же отделе 
    public HomeViewModel(Users account, NavigationStore navigationStore, Departments departments)
    {
        _user = account;
        this._navigationStore = navigationStore;
        _selectedItem = departments;
        ApiGetPersons(departments);
    }

    #region Свойства

    private readonly NavigationStore _navigationStore;

    private bool _radioIsPed;
    public bool RadioIsPed
    {
        get => _radioIsPed;
        set => Set(ref _radioIsPed, value);
    }

    private bool _radioIsNoPed;
    public bool RadioIsNoPed
    {
        get => _radioIsNoPed;
        set => Set(ref _radioIsNoPed, value);
    }

    // Процесс загрузки
    private VisualBoolean? _isLoading = false;
    public VisualBoolean? IsLoading
    {
        get => _isLoading;
        set => Set(ref _isLoading, value);
    }
    // Вывести информацию о пользователе который авторизировался 
    private Users _user;
    public Users User
    {
        get => _user;
        set => Set(ref _user, value);
    }
    // Права на изменение данных 
    public bool IsAdministrator => _user.Grants!.FirstOrDefault(x => x.Name.Contains("изменение"))?.Id == 2; 
    public bool IsReadeOnly => !IsAdministrator; // Маленький костыль для свойства только чтения
                                                 // Массив Отделов
    private ObservableCollection<Departments>? _departments;
    public ObservableCollection<Departments>? Departments
    {
        get => _departments;
        private set => Set(ref _departments, value);
    }
    // Массив Должностей
    private ObservableCollection<Position>? _positions;
    public ObservableCollection<Position>? Positions
    {
        get => _positions;
        private set => Set(ref _positions, value);
    }
    // Список всех должностей
    private ObservableCollection<TypePosition>? _typePosition;
    public ObservableCollection<TypePosition>? TypePosition
    {
        get => _typePosition;
        private set => Set(ref _typePosition, value);
    }
    // Выбранный элемент из TreeView
    private Position? _selectedPosition;
    public Position? SelectedPosition
    {
        get => _selectedPosition;
        set
        {
            _ = Set(ref _selectedPosition, value);
            if (_selectedPosition == null) return;
            if (_selectedPosition.IsPed)
            {
                RadioIsPed = true;
            }
            else
            {
                RadioIsNoPed = true;
            }
        }
    }
    // Выбранный элемент из TreeView
    private Departments? _selectedItem;
    public Departments? SelectedItem
    {
        get => _selectedItem;
        set => Set(ref _selectedItem, value);
    }
    // Массив Сотрудников
    private ObservableCollection<Persons>? _persons;
    public ObservableCollection<Persons>? Persons
    {
        get => _persons;
        private set
        {
            _ = Set(ref _persons, value);
            // Для фильтрации полей 
            if (Persons == null) return;
            CollectionPerson = CollectionViewSource.GetDefaultView(Persons);
            CollectionPerson.Filter = FilterToPerson;

            // Считаем сколько НПП
            CountIsPedPerson = Persons!.Count(x => x.IsPed);
            // Считаем сколько не НПП
            CountNotIsPedPerson = Persons!.Count(x => x.IsPed == false);

            // Совместителей Внутренних НПП
            CountIsPluralismInnerIsPed = Persons!.Count(x => x.IsPluralismInner && x.IsPed);
            // Совместителей Внутренних не НПП
            CountIsPluralismInnerNotIsPed = Persons!.Count(x => x.IsPluralismInner && x.IsPed == false);

            // Совместителей Внешних НПП
            CountIsPluralismInnerIsPed = Persons!.Count(x => x.IsPluralismOter && x.IsPed);
            // Совместителей Внешних не НПП
            CountIsPluralismInnerNotIsPed = Persons!.Count(x => x.IsPluralismOter && x.IsPed == false);

            // Считаем свободных бюджетных ставки
            CountFreeBudget = Positions!.Sum(x => x.Free_B);
            // Считаем свободных внебюджетных ставки
            CountFreeNotBudget = Positions!.Sum(x => x.Free_NB);

            // итоги бюджетных ставок
            CountBudget = Persons!.Sum(x => x.StavkaBudget);
            // итоги внебюджетных ставок
            CountNotBudget = Persons!.Sum(x => x.StavkaNoBudget);
        }
    }
    // расчет совместителей Внутренних НПП
    private int _countIsPluralismInnerIsPed;
    public int CountIsPluralismInnerIsPed
    {
        get => _countIsPluralismInnerIsPed;
        private set => Set(ref _countIsPluralismInnerIsPed, value);
    }

    // расчет совместителей Внутренних не НПП
    private int _countIsPluralismInnerNotIsPed;
    public int CountIsPluralismInnerNotIsPed
    {
        get => _countIsPluralismInnerNotIsPed;
        private set => Set(ref _countIsPluralismInnerNotIsPed, value);
    }


    // расчет совместителей Внешних 
    private int _countIsPluralismOterIsPed;
    public int CountIsPluralismOterIsPed
    {
        get => _countIsPluralismOterIsPed;
        private set => Set(ref _countIsPluralismOterIsPed, value);
    }

    // Подсчет количества людей имеющие педагогическую должность
    private int _countIsPedPerson;
    public int CountIsPedPerson
    {
        get => _countIsPedPerson;
        private set => Set(ref _countIsPedPerson, value);
    }
    // Подсчет количества людей имеющие не педагогическую должность
    private int _countNotIsPedPerson;
    public int CountNotIsPedPerson
    {
        get => _countNotIsPedPerson;
        private set => Set(ref _countNotIsPedPerson, value);
    }

    // Подсчет количество свободных бюджетных ставок 
    private decimal _countFreeBudget;
    public decimal CountFreeBudget
    {
        get => _countFreeBudget;
        private set => Set(ref _countFreeBudget, value);
    }
    // Подсчет количество свободных внебюджетных ставок 
    private decimal _countFreeNotBudget;
    public decimal CountFreeNotBudget
    {
        get => _countFreeBudget;
        private set => Set(ref _countFreeNotBudget, value);
    }

    // Подсчет количество  бюджетных ставок 
    private decimal _countBudget;
    public decimal CountBudget
    {
        get => _countBudget;
        private set => Set(ref _countBudget, value);
    }
    // Подсчет количество  внебюджетных ставок 
    private decimal _countNotBudget;
    public decimal CountNotBudget
    {
        get => _countFreeBudget;
        private set => Set(ref _countNotBudget, value);
    }

    // Выбранный должность 
    private TypePosition? _selectedTypePosition;
    public TypePosition? SelectedTypePosition
    {
        get => _selectedTypePosition;
        set => Set(ref _selectedTypePosition, value);
    }

    // Выбранный сотрудник 
    private Persons? _selectedPerson;
    public Persons? SelectedPerson
    {
        get => _selectedPerson;
        set => Set(ref _selectedPerson, value);
    }
    // Поисковая строка для поиска по ФИО сотрудника
    private string? _filterPerson;
    public string? FilterPerson
    {
        get => _filterPerson;
        set
        {
            _ = Set(ref _filterPerson, value);
            if (Persons != null)
            {
                CollectionPerson!.Refresh();
            }
        }
    }
    // Специальная колекция для фильтров
    private ICollectionView? _collectionPerson;
    public ICollectionView? CollectionPerson
    {
        get => _collectionPerson;
        private set => Set(ref _collectionPerson, value);
    }
    #endregion

    #region Команды
    // Команда для отображения всех отделов для TreeView
    private ICommand? _getTreeView;
    public ICommand GetTreeView => _getTreeView ??= new LambdaCommand(ApiGetDepartments);

    // Команда для отображения всех сотрудников которые пренадлежат определённому отделу
    private ICommand? _getPersonsToDepartment;
    public ICommand GetPersonsToDepartment => _getPersonsToDepartment ??= new LambdaCommand(ApiGetPersons, _ => SelectedItem is not null);

    // Выход из аккаунта
    private ICommand? _logout;
    public ICommand Logout => _logout ??= new LambdaCommand(Exit);


    // UserControl
    private ICommand? _openDepartment;
    public ICommand OpenDepartment => _openDepartment ??= new LambdaCommand(OpenDepartmentView);

    private ICommand? _openTypeVacation;
    public ICommand OpenVacation => _openTypeVacation ??= new LambdaCommand(OpenTypeVacationView);

    private ICommand? _openTypeRewarding;
    public ICommand OpenRewarding => _openTypeRewarding ??= new LambdaCommand(OpenTypeRewardingView);

    private ICommand? _openSearch;
    public ICommand OpenSearch => _openSearch ??= new LambdaCommand(OpenSearchView);

    private ICommand? _openPosition;
    public ICommand OpenPosition => _openPosition ??= new LambdaCommand(OpenPositionView);

    private ICommand? _openTypePosition;
    public ICommand OpenTypePosition => _openTypePosition ??= new LambdaCommand(OpenTypePositionView);

    private ICommand? _openTypeOrder;
    public ICommand OpenTypeOrder => _openTypeOrder ??= new LambdaCommand(OpenTypeOrderView);

    private ICommand? _openTypeRanks;
    public ICommand OpenTypeRanks => _openTypeRanks ??= new LambdaCommand(OpenTypeRankView);

    private ICommand? _openOrder;
    public ICommand OpenOrder => _openOrder ??= new LambdaCommand(OpenOrderView);

    private ICommand? _openPeriod;
    public ICommand OpenPeriod => _openPeriod ??= new LambdaCommand(OpenPeriodView);

    private ICommand? _openPersonCard;
    public ICommand OpenPersonCard => _openPersonCard ??= new LambdaCommand(OpenPersonCardView, _ => SelectedPerson is not null);

    private ICommand? _openReport;
    public ICommand OpenReport => _openReport ??= new LambdaCommand(OpenReportView);

    private ICommand? _openAddPerson;
    public ICommand OpenAddPerson => _openAddPerson ??= new LambdaCommand(AddPerson, _ => SelectedItem is not null);

    private ICommand? _addPosition;
    public ICommand AddPosition => _addPosition ??= new LambdaCommand(AddPositionToDepartmentAsync);

    private ICommand? _savePosition;
    public ICommand SavePosition => _savePosition ??= new LambdaCommand(UpdatePosition , _ => SelectedPosition is not null);

    private ICommand? _deletePosition;
    public ICommand DeletePosition => _deletePosition ??= new LambdaCommand(DeletePositionApi, _ => SelectedPosition is not null);

    private ICommand? _openDeletePerson;
    public ICommand OpenDeletePerson => _openDeletePerson ??= new LambdaCommand(DeletePerson, _ => SelectedItem is not null);

    private ICommand? _renameDepartment;
    public ICommand RenameDepartment => _renameDepartment ??= new LambdaCommand(UpdateDataDepartment, _ => SelectedItem is not null);

    #endregion

    #region Логика

    // Открыть модальное окно на созадние человека
    private void AddPerson(object p)
    {
        AddPersonVeiwModel viewModel = new(_user, _selectedItem!.Id);
        AddPersonView view = new() { DataContext = viewModel };
        view.ShowDialog();
        // Обновить данные
        ApiGetPersons(p);
    }
    // Открыть модальное окно на удаление человека
    private void DeletePerson(object p)
    {
        DeletePersonViewModel viewModel = new($"С должности:'{_selectedPerson!.PersonPosition}'", _user, _selectedPerson!, SelectedItem!);
        DeleteView view = new() { DataContext = viewModel };
        view.ShowDialog();
        // Обновить данные
        ApiGetPersons(p);
    }
    private void OpenTypeVacationView(object p)
    {
        _navigationStore.CurrentViewModel = new TypeVacationViewModel(_navigationStore, _user);
    }

    private void OpenTypeRewardingView(object p)
    {
        _navigationStore.CurrentViewModel = new TypeRewardingViewModel(_navigationStore, _user);
    }

    private void OpenPeriodView(object p)
    {
        _navigationStore.CurrentViewModel = new PeriodVacationViewModel(_navigationStore, _user);
    }
    // Отчеты
    private void OpenReportView(object p)
    {
        _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user);
    }

    // Отделы
    private void OpenDepartmentView(object p)
    {
        _navigationStore.CurrentViewModel = new DepartmentViewModel(_navigationStore, _user);
    }
    // Типы приказов
    private void OpenTypeOrderView(object p)
    {
        _navigationStore.CurrentViewModel = new TypeOrderViewModel(_navigationStore, _user);
    }
    // Тип Званий
    private void OpenTypeRankView(object p)
    {
        _navigationStore.CurrentViewModel = new TypeRanksViewModel(_navigationStore, _user);
    }
    // Приказы
    private void OpenOrderView(object p)
    {
        _navigationStore.CurrentViewModel = new OrderViewModel(_navigationStore, _user);
    }

    // Поиск
    private void OpenSearchView(object p)
    {
        _navigationStore.CurrentViewModel = new SearchViewModel(_navigationStore, _user);
    }

    private void OpenPositionView(object p)
    {
        _navigationStore.CurrentViewModel = new PositionViewModel(_navigationStore, _user);
    }

    private void OpenTypePositionView(object p)
    {
        _navigationStore.CurrentViewModel = new TypePositionViewModel(_navigationStore, _user);
    }

    private void OpenPersonCardView(object p)
    {
        _navigationStore.CurrentViewModel = new PersonCardViewModel(_user, _navigationStore, SelectedPerson!, SelectedItem!.Id);
    }

    private async void ApiGetPersons(object p)
    {
        try
        {
            // Запуск progress bar
            IsLoading = true;
            // Выдать все должности
            TypePosition = await QueryService.JsonDeserializeWithToken<TypePosition>(token: _user!.Token, "/pers/position/type/position/", "GET");
            // Вывести список штатных должностей данного отдела
            Positions = await QueryService.JsonDeserializeWithToken<Position>(token: _user!.Token, "/pers/position/get/" + SelectedItem!.Id, "GET");
            // Вывести сотрудников данного отдела
            Persons = await QueryService.JsonDeserializeWithToken<Persons>(token: _user!.Token, "/pers/person/get/department/" + SelectedItem.Id, "GET");
            // Завершить progress bar
            IsLoading = false;
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
    // Вернуть api всех отделов 
    private async void ApiGetDepartments(object p)
    {
        try
        {
            Departments = await QueryService.JsonDeserializeWithToken<Departments>(_user.Token, "/pers/tree/get", "GET");
            if(SelectedItem != null)
            {
                ApiGetPersons(p);
            }

            //Departments = new ObservableCollection<Departments>(Departments.OrderBy(x => x.ParentId));

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
    // Создать должность на Отделе
    private async void AddPositionToDepartmentAsync(object p)
    {
        try
        {
            if(SelectedItem != null)
            {
                var name = TypePosition!.FirstOrDefault()?.Name;
                if (name == null) return;
                Position dep = new()
                {
                    Name = name,
                    IsPed = true,
                    HolidayLimit = 28,
                    DepartmentName = SelectedItem.Name,
                    IdDepartment = SelectedItem.Id,
                    Phone = "Не указано",
                };
                Positions!.Insert(0, dep);
                SelectedPosition = dep;
            }
            else
            {
                _ = MessageBox.Show("Чтобы создать должность,необходимо выбрать отдел!","Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
           
            //SelectedPerson.ArrayPosition.Insert(0, dep);
            //SelectedPosition = dep;

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

    private async void UpdateDataDepartment(object p)
    {
        try
        {
            if (SelectedItem!.Id > 0)
            {

                //Изменить уже текущие данные
                await QueryService.JsonSerializeWithToken(token: _user!.Token, "/pers/tree/rename/short/" + SelectedItem.Id, "POST", SelectedItem);
            }
           
           // _ = MessageBox.Show("Данные успешно сохраненны");
           
        }
        catch (System.Net.WebException ex)
        {
            if (ex.Response != null)
            {
                using StreamReader reader = new(ex.Response.GetResponseStream());
                _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
    private async void UpdatePosition(object p)
    {
        try
        {
            if (SelectedPosition!.Id > 0)
            {
                SelectedPosition.IsPed = RadioIsPed;
                //Изменить уже текущие данные
                await QueryService.JsonSerializeWithToken(token: _user!.Token, "/pers/position/rename", "POST", SelectedPosition);
            }
            else
            {
                SelectedPosition.IsPed = RadioIsPed;
                // Создать новую запись  
                await QueryService.JsonSerializeWithToken(token: _user!.Token, "/pers/position/add", "POST", SelectedPosition);
                
            }
            // Обновить данные
            Positions = await QueryService.JsonDeserializeWithToken<Position>(token: _user!.Token, "/pers/position/get/" + SelectedItem!.Id, "GET");
            // Вывести сотрудников данного отдела
            Persons = await QueryService.JsonDeserializeWithToken<Persons>(token: _user!.Token, "/pers/person/get/department/" + SelectedItem.Id, "GET");

            _ = MessageBox.Show("Данные успешно сохраненны");
        }
        catch (System.Net.WebException ex)
        {
            if (ex.Response != null)
            {
                using StreamReader reader = new(ex.Response.GetResponseStream());
                _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
    private async void DeletePositionApi(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/position/del/" + SelectedPosition!.Id, "DELETE", SelectedPosition);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _positions!.Remove(SelectedPosition);
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

    // Логика фильтра
    private bool FilterToPerson(object emp)
    {
        return string.IsNullOrEmpty(FilterPerson) || (emp is Persons pers && pers.FirstName!.ToUpper().Contains(value: FilterPerson.ToUpper()));
    }

    // Удалить Токен
    private async void Exit(object p)
    {
        try
        {
            await QueryService.JsonSerializeWithToken(token: _user!.Token,
                                                    "/logout",
                                                    "POST",
                                                    User);
            // Вернуть на Авторизацию 
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

