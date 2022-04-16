using AlphaPersonel.Views.Models;
using System.Linq;
namespace AlphaPersonel.ViewModels;

internal class HomeViewModel : BaseViewModel
{
    public HomeViewModel(Users account, NavigationStore navigationStore)
    {
        _User = account;
        this.navigationStore = navigationStore;
    }
    // При возврате с личной карты,чтобы оставались на том же отделе 
    public HomeViewModel(Users account, NavigationStore navigationStore, Departments departments)
    {
        _User = account;
        this.navigationStore = navigationStore;
        _SelectedItem = departments;
        ApiGetPersons(departments);
    }

    #region Свойства

    private readonly NavigationStore navigationStore;

    private bool _RadioIsPed;
    public bool RadioIsPed
    {
        get => _RadioIsPed;
        set => Set(ref _RadioIsPed, value);
    }

    private bool _RadioIsNoPed;
    public bool RadioIsNoPed
    {
        get => _RadioIsNoPed;
        set => Set(ref _RadioIsNoPed, value);
    }

    // Процесс загрузки
    private VisualBoolean? _IsLoading = false;
    public VisualBoolean? IsLoading
    {
        get => _IsLoading;
        set => Set(ref _IsLoading, value);
    }
    // Вывести информацию о пользователе который авторизировался 
    private Users _User;
    public Users User
    {
        get => _User;
        set => Set(ref _User, value);
    }
    // Права на изменение данных 
    public bool IsAdministrator => _User.Grants!.FirstOrDefault(x => x.Name.Contains("изменение"))?.Id == 2; 
    public bool IsReadeOnly => !IsAdministrator; // Маленький костыль для свойства только чтения
                                                 // Массив Отделов
    private ObservableCollection<Departments>? _Departments;
    public ObservableCollection<Departments>? Departments
    {
        get => _Departments;
        private set => Set(ref _Departments, value);
    }
    // Массив Должностей
    private ObservableCollection<Position>? _Positions;
    public ObservableCollection<Position>? Positions
    {
        get => _Positions;
        private set => Set(ref _Positions, value);
    }
    // Выбранный элемент из TreeView
    private Position? _SelectedPosition;
    public Position? SelectedPosition
    {
        get => _SelectedPosition;
        set
        {
            _ = Set(ref _SelectedPosition, value);
            if (_SelectedPosition != null)
            {
                if (_SelectedPosition.IsPed)
                {
                    RadioIsPed = true;
                }
                else
                {
                    RadioIsNoPed = true;
                }
            }
        }
    }
    // Выбранный элемент из TreeView
    private Departments? _SelectedItem;
    public Departments? SelectedItem
    {
        get => _SelectedItem;
        set => Set(ref _SelectedItem, value);
    }
    // Массив Сотрудников
    private ObservableCollection<Persons>? _Persons;
    public ObservableCollection<Persons>? Persons
    {
        get => _Persons;
        private set
        {
            _ = Set(ref _Persons, value);
            // Для фильтрации полей 
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
    private int _CountIsPluralismInnerIsPed;
    public int CountIsPluralismInnerIsPed
    {
        get => _CountIsPluralismInnerIsPed;
        private set => Set(ref _CountIsPluralismInnerIsPed, value);
    }

    // расчет совместителей Внутренних не НПП
    private int _CountIsPluralismInnerNotIsPed;
    public int CountIsPluralismInnerNotIsPed
    {
        get => _CountIsPluralismInnerNotIsPed;
        private set => Set(ref _CountIsPluralismInnerNotIsPed, value);
    }


    // расчет совместителей Внешних 
    private int _CountIsPluralismOterIsPed;
    public int CountIsPluralismOterIsPed
    {
        get => _CountIsPluralismOterIsPed;
        private set => Set(ref _CountIsPluralismOterIsPed, value);
    }

    // Подсчет количества людей имеющие педагогическую должность
    private int _CountIsPedPerson;
    public int CountIsPedPerson
    {
        get => _CountIsPedPerson;
        private set => Set(ref _CountIsPedPerson, value);
    }
    // Подсчет количества людей имеющие не педагогическую должность
    private int _CountNotIsPedPerson;
    public int CountNotIsPedPerson
    {
        get => _CountNotIsPedPerson;
        private set => Set(ref _CountNotIsPedPerson, value);
    }

    // Подсчет количество свободных бюджетных ставок 
    private decimal _CountFreeBudget;
    public decimal CountFreeBudget
    {
        get => _CountFreeBudget;
        private set => Set(ref _CountFreeBudget, value);
    }
    // Подсчет количество свободных внебюджетных ставок 
    private decimal _CountFreeNotBudget;
    public decimal CountFreeNotBudget
    {
        get => _CountFreeBudget;
        private set => Set(ref _CountFreeNotBudget, value);
    }

    // Подсчет количество  бюджетных ставок 
    private decimal _CountBudget;
    public decimal CountBudget
    {
        get => _CountBudget;
        private set => Set(ref _CountBudget, value);
    }
    // Подсчет количество  внебюджетных ставок 
    private decimal _CountNotBudget;
    public decimal CountNotBudget
    {
        get => _CountFreeBudget;
        private set => Set(ref _CountNotBudget, value);
    }

    // Выбранный сотрудник 
    private Persons? _SelectedPerson;
    public Persons? SelectedPerson
    {
        get => _SelectedPerson;
        set => Set(ref _SelectedPerson, value);
    }
    // Поисковая строка для поиска по ФИО сотрудника
    private string? _FilterPerson;
    public string? FilterPerson
    {
        get => _FilterPerson;
        set
        {
            _ = Set(ref _FilterPerson, value);
            if (Persons != null)
            {
                CollectionPerson!.Refresh();
            }
        }
    }
    // Специальная колекция для фильтров
    private ICollectionView? _CollectionPerson;
    public ICollectionView? CollectionPerson
    {
        get => _CollectionPerson;
        private set => Set(ref _CollectionPerson, value);
    }
    #endregion

    #region Команды
    // Команда для отображения всех отделов для TreeView
    private ICommand? _GetTreeView;
    public ICommand GetTreeView => _GetTreeView ??= new LambdaCommand(ApiGetDepartments);

    // Команда для отображения всех сотрудников которые пренадлежат определённому отделу
    private ICommand? _GetPersonsToDepartment;
    public ICommand GetPersonsToDepartment => _GetPersonsToDepartment ??= new LambdaCommand(ApiGetPersons, _ => SelectedItem is not null);

    // Выход из аккаунта
    private ICommand? _Logout;
    public ICommand Logout => _Logout ??= new LambdaCommand(Exit);


    // UserControl
    private ICommand? _OpenDepartment;
    public ICommand OpenDepartment => _OpenDepartment ??= new LambdaCommand(OpenDepartmentView);

    private ICommand? _OpenTypeVacation;
    public ICommand OpenVacation => _OpenTypeVacation ??= new LambdaCommand(OpenTypeVacationView);

    private ICommand? _OpenTypeRewarding;
    public ICommand OpenRewarding => _OpenTypeRewarding ??= new LambdaCommand(OpenTypeRewardingView);

    private ICommand? _OpenSearch;
    public ICommand OpenSearch => _OpenSearch ??= new LambdaCommand(OpenSearchView);

    private ICommand? _OpenPosition;
    public ICommand OpenPosition => _OpenPosition ??= new LambdaCommand(OpenPositionView);

    private ICommand? _OpenTypeOrder;
    public ICommand OpenTypeOrder => _OpenTypeOrder ??= new LambdaCommand(OpenTypeOrderView);

    private ICommand? _OpenTypeRanks;
    public ICommand OpenTypeRanks => _OpenTypeRanks ??= new LambdaCommand(OpenTypeRankView);

    private ICommand? _OpenOrder;
    public ICommand OpenOrder => _OpenOrder ??= new LambdaCommand(OpenOrderView);

    private ICommand? _OpenPeriod;
    public ICommand OpenPeriod => _OpenPeriod ??= new LambdaCommand(OpenPeriodView);

    private ICommand? _OpenPersonCard;
    public ICommand OpenPersonCard => _OpenPersonCard ??= new LambdaCommand(OpenPersonCardView, _ => SelectedPerson is not null);

    private ICommand? _OpenReport;
    public ICommand OpenReport => _OpenReport ??= new LambdaCommand(OpenReportView);

    private ICommand? _OpenAddPerson;
    public ICommand OpenAddPerson => _OpenAddPerson ??= new LambdaCommand(AddPerson, _ => SelectedItem is not null);

    private ICommand? _OpenDeletePerson;
    public ICommand OpenDeletePerson => _OpenDeletePerson ??= new LambdaCommand(DeletePerson, _ => SelectedItem is not null);

    #endregion

    #region Логика

    // Открыть модальное окно на созадние человека
    private void AddPerson(object p)
    {
        AddPersonVeiwModel viewModel = new(_User, _SelectedItem!.Id);
        AddPersonView view = new() { DataContext = viewModel };
        view.ShowDialog();
        // Обновить данные
        ApiGetPersons(p);
    }
    // Открыть модальное окно на удаление человека
    private void DeletePerson(object p)
    {
        DeletePersonViewModel viewModel = new(_User, _SelectedPerson!, SelectedItem!);
        DeleteView view = new() { DataContext = viewModel };
        view.ShowDialog();
        // Обновить данные
        ApiGetPersons(p);
    }
    private void OpenTypeVacationView(object p)
    {
        navigationStore.CurrentViewModel = new TypeVacationViewModel(navigationStore, _User);
    }

    private void OpenTypeRewardingView(object p)
    {
        navigationStore.CurrentViewModel = new TypeRewardingViewModel(navigationStore, _User);
    }

    private void OpenPeriodView(object p)
    {
        navigationStore.CurrentViewModel = new PeriodVacationViewModel(navigationStore, _User);
    }
    // Отчеты
    private void OpenReportView(object p)
    {
        navigationStore.CurrentViewModel = new ReportsViewModel(navigationStore, _User);
    }

    // Отделы
    private void OpenDepartmentView(object p)
    {
        navigationStore.CurrentViewModel = new DepartmentViewModel(navigationStore, _User);
    }
    // Типы приказов
    private void OpenTypeOrderView(object p)
    {
        navigationStore.CurrentViewModel = new TypeOrderViewModel(navigationStore, _User);
    }
    // Тип Званий
    private void OpenTypeRankView(object p)
    {
        navigationStore.CurrentViewModel = new TypeRanksViewModel(navigationStore, _User);
    }
    // Приказы
    private void OpenOrderView(object p)
    {
        navigationStore.CurrentViewModel = new OrderViewModel(navigationStore, _User);
    }

    // Поиск
    private void OpenSearchView(object p)
    {
        navigationStore.CurrentViewModel = new SearchViewModel(navigationStore, _User);
    }

    private void OpenPositionView(object p)
    {
        navigationStore.CurrentViewModel = new PositionViewModel(navigationStore, _User);
    }

    private void OpenPersonCardView(object p)
    {
        navigationStore.CurrentViewModel = new PersonCardViewModel(_User, navigationStore, SelectedPerson!, SelectedItem!.Id);
    }

    private async void ApiGetPersons(object p)
    {
        try
        {
            if (_User.Token == null)
            {
                return;
            }
            // Запуск progress bar
            IsLoading = true;
            // Вывести список должностей данного отдела

            Positions = await QueryService.JsonDeserializeWithToken<Position>(token: _User!.Token, "/pers/position/get/" + SelectedItem!.Id, "GET");

            // Вывести сотрудников данного отдела
            Persons = await QueryService.JsonDeserializeWithToken<Persons>(token: _User!.Token, "/pers/person/get/department/" + SelectedItem.Id, "GET");
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
    // Вернуть api всех отделов 
    private async void ApiGetDepartments(object p)
    {
        try
        {
            if (_User.Token == null)
            {
                return;
            }

            Departments = await QueryService.JsonDeserializeWithToken<Departments>(_User.Token, "/pers/tree/get", "GET");

            //Departments = new ObservableCollection<Departments>(Departments.OrderBy(x => x.ParentId));

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
            if (_User.Token == null)
            {
                return;
            }

            await QueryService.JsonSerializeWithToken(token: _User!.Token,
                                                    "/logout",
                                                    "POST",
                                                    User);
            // Вернуть на Авторизацию 
            navigationStore.CurrentViewModel = new LoginViewModel(navigationStore);
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

