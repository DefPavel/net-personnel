using System.Collections.Generic;

namespace AlphaPersonel.ViewModels;
public enum TypeReport
{
    IsPed = 1,
    IsNoPed = 2,
    IsAll = 3,
}
internal class ReportsViewModel : BaseViewModel
{
    #region Переменные

    // Процесс загрузки
    private VisualBoolean _isLoading = false;
    public VisualBoolean IsLoading
    {
        get => _isLoading;
        private set => Set(ref _isLoading, value);
    }

    // Ссылка на User для того чтобы забрать token
    private Users _user;
    public Users User
    {
        get => _user;
        set => Set(ref _user, value);
    }

    private readonly NavigationStore _navigationStore;

    #region Штат
    // Выбранные отчет
    private Report? _selectedReport;
    public Report? SelectedReport
    {
        get => _selectedReport;
        set => Set(ref _selectedReport, value);
    }

    private bool _selectedIsPed = true;
    public bool SelectedIsPed
    {
        get => _selectedIsPed;
        set => Set(ref _selectedIsPed, value);
    }

    private bool _selectedIsNoPed;
    public bool SelectedIsNoPed
    {
        get => _selectedIsNoPed;
        set => Set(ref _selectedIsNoPed, value);
    }

    private bool _selectedIsAll;
    public bool SelectedIsAll
    {
        get => _selectedIsAll;
        set => Set(ref _selectedIsAll, value);
    }
    #endregion

    #region Совместители
    private Report? _selectedReportPluralist;
    public Report? SelectedReportPluralist
    {
        get => _selectedReportPluralist;
        set => Set(ref _selectedReportPluralist, value);
    }

    private bool _selectedIsPedPluralist = true;
    public bool SelectedIsPedPluralist
    {
        get => _selectedIsPedPluralist;
        set => Set(ref _selectedIsPedPluralist, value);
    }

    private bool _selectedIsNoPedPluralist;
    public bool SelectedIsNoPedPluralist
    {
        get => _selectedIsNoPedPluralist;
        set => Set(ref _selectedIsNoPedPluralist, value);
    }

    private bool _selectedIsAllPluralist;
    public bool SelectedIsAllPluralist
    {
        get => _selectedIsAllPluralist;
        set => Set(ref _selectedIsAllPluralist, value);
    }

    #endregion

    // Массив отчетов
    private IEnumerable<Report>? _reports;

    private IEnumerable<Report>? Reports
    {
        get => _reports;
        set
        {
            _ = Set(ref _reports, value);
            if (Reports != null) CollectionDepart = CollectionViewSource.GetDefaultView(Reports);
            CollectionDepart.Filter = FilterToDepart;
        }
    }

    private IEnumerable<Report>? _reportsPluralist;

    private IEnumerable<Report>? ReportsPluralist
    {
        get => _reportsPluralist;
        set
        {
            _ = Set(ref _reportsPluralist, value);
            if (ReportsPluralist != null) CollectionPluralist = CollectionViewSource.GetDefaultView(ReportsPluralist);
            CollectionPluralist.Filter = FilterToPluralist;
        }
    }

    // Поисковая строка 
    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            _ = Set(ref _filter, value);
            if (Reports != null)
            {
                _collectionDepart.Refresh();
            }

        }
    }

    // Поисковая строка 
    private string? _filterPluralist;
    public string? FilterPluralist
    {
        get => _filterPluralist;
        set
        {
            _ = Set(ref _filterPluralist, value);
            if (ReportsPluralist != null)
            {
                _collectionPluralist.Refresh();
            }

        }
    }
    // Специальная колекция для фильтров
    private ICollectionView _collectionPluralist;
    public ICollectionView CollectionPluralist
    {
        get => _collectionPluralist;
        private set => Set(ref _collectionPluralist, value);
    }

    private ICollectionView _collectionDepart;
    public ICollectionView CollectionDepart
    {
        get => _collectionDepart;
        private set => Set(ref _collectionDepart, value);
    }

    private bool FilterToDepart(object emp)
    {
        return string.IsNullOrEmpty(Filter) || (emp is Report dep && dep.Name!.ToUpper().Contains(value: Filter.ToUpper()));
    }

    private bool FilterToPluralist(object emp)
    {
        return string.IsNullOrEmpty(FilterPluralist) || (emp is Report dep && dep.Name!.ToUpper().Contains(value: FilterPluralist.ToUpper()));
    }

    private bool CanCommandExecute(object p)
    {
        return p is Report && IsLoading != true;
    }
    #endregion

    #region Команды

    private ICommand? _openReport;
    public ICommand OpenReport => _openReport ??= new LambdaCommand(ApiGetReport, CanCommandExecute);

    private ICommand? _openReportPluralist;
    public ICommand OpenReportPluralist => _openReportPluralist ??= new LambdaCommand(ApiGetReportPluralist, CanCommandExecute);

    private ICommand? _loadedReports;
    public ICommand LoadedReports => _loadedReports ??= new LambdaCommand(LoadedData);

    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack , _ => IsLoading != true);

    #endregion

    #region Логика

    private void GetBack(object p)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }

    private async void LoadedData(object obj)
    {
        try
        {
            Reports = await QueryService.JsonDeserializeWithToken<Report>(_user.Token, "/reports/pers/get/1", "GET");

            ReportsPluralist = await QueryService.JsonDeserializeWithToken<Report>(_user.Token, "/reports/pers/get/2", "GET");
        }
        // Проверка токена
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            // Не удалось получить response
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void ApiGetReportPluralist(object obj)
    {
        try
        {
            IsLoading = true;
            var reportName = SelectedReportPluralist!.Name;
            if (SelectedIsPedPluralist)
            {

                if (reportName != null)
                    await ReportService.JsonDeserializeWithToken(
                        token: _user!.Token,
                        queryUrl: SelectedReportPluralist!.Url + TypeReport.IsPed,
                        httpMethod: "GET",
                        reportName: reportName
                    );
            }
            else if (SelectedIsNoPedPluralist)
            {
                if (reportName != null)
                    await ReportService.JsonDeserializeWithToken(
                        token: _user!.Token,
                        queryUrl: SelectedReportPluralist!.Url + TypeReport.IsNoPed.ToString(),
                        httpMethod: "GET",
                        reportName: reportName
                    );
            }
            else
            {
                if (reportName != null)
                    await ReportService.JsonDeserializeWithToken(
                        token: _user!.Token,
                        queryUrl: SelectedReportPluralist!.Url + TypeReport.IsAll.ToString(),
                        httpMethod: "GET",
                        reportName: reportName
                    );
            }
            IsLoading = false;
        }
        // Проверка токена
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            IsLoading = false;
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            IsLoading = false;
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            // Не удалось получить response
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void ApiGetReport(object obj)
    {
        try
        {
            IsLoading = true;
            var reportName = SelectedReport!.Name;
            if (SelectedIsPed)
            {
                
                if (reportName != null)
                    await ReportService.JsonDeserializeWithToken(
                        token: _user!.Token,
                        queryUrl: SelectedReport!.Url + TypeReport.IsPed,
                        httpMethod: "GET",
                        reportName: reportName
                    );
            }
            else if (SelectedIsNoPed)
            {
                if (reportName != null)
                    await ReportService.JsonDeserializeWithToken(
                        token: _user!.Token,
                        queryUrl: SelectedReport!.Url + TypeReport.IsNoPed.ToString(),
                        httpMethod: "GET",
                        reportName: reportName
                    );
            }
            else
            {
                if (reportName != null)
                    await ReportService.JsonDeserializeWithToken(
                        token: _user!.Token,
                        queryUrl: SelectedReport!.Url + TypeReport.IsAll.ToString(),
                        httpMethod: "GET",
                        reportName: reportName
                    );
            }
            IsLoading = false;
        }
        // Проверка токена
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            IsLoading = false;
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            IsLoading = false;
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            // Не удалось получить response
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    #endregion


#pragma warning disable CS8618
    public ReportsViewModel(NavigationStore navigationStore, Users user)
#pragma warning restore CS8618
    {
        _user = user;
        _navigationStore = navigationStore;

       /* Reports = new ObservableCollection<Report>
            {
                new ()
                {
                    Name = "Список отделов",
                    Url = "/reports/pers/departments/"
                },
                new ()
                {
                    Name = "Сотрудники в отпуске(по уходу за ребенком)",
                    Url = "/reports/pers/persons/decret/"
                },
                new ()
                {
                    Name = "Список всех сотрудников",
                    Url = "/reports/pers/persons/"
                },
                new ()
                {
                    Name = "Список всех сотрудников(Только Мужчины)",
                    Url = "/reports/pers/persons/male/"
                },
                new ()
                {
                    Name = "Список всех сотрудников(Только Женщины)",
                    Url = "/reports/pers/persons/female/"
                },
                new ()
                {
                    Name = "Список всех сотрудников(Студенты)",
                    Url = "/reports/pers/persons/student/"
                },
                new ()
                {
                    Name = "Список всех сотрудников(Аспиранты)",
                    Url = "/reports/pers/persons/graduate/"
                },
                new ()
                {
                    Name = "Список всех сотрудников(Пенсионеры)",
                    Url = "/reports/pers/persons/persioner"
                },
                new ()
                {
                    Name = "Список сотрудников-инвалидов",
                    Url = "/reports/pers/persons/invalids/"
                },
                new ()
                {
                    Name = "Список всех сотрудников(Не ЛГПУ)",
                    Url = "/reports/pers/persons/no_lgpu/"
                },
                new ()
                {
                    Name = "Список академиков и членов корреспондентов",
                    Url = "/reports/pers/persons/academic/"
                },
                /*new ()
                {
                    Name = "Список Принятых Сотрудников",
                    Url = "/reports/pers/persons/insert/"
                },
                
                new ()
                {
                    Name = "Список Сотрудников(Юбиляров)",
                    Url = "/reports/pers/persons/jubilee/"
                },
                new ()
                {
                    Name = "Список всех сотрудников(Имеющие РФ паспорт)",
                    Url = "/reports/pers/persons/passport_rus/"
                },
                new ()
                {
                    Name = "Список всех сотрудников(Не имеющие РФ паспорт)",
                    Url = "/reports/pers/persons/passport_no_rus/"
                },
                new ()
                {
                    Name = "Список всех сотрудников(Не имеющих паспорт ЛНР)",
                    Url = "/reports/pers/persons/passport_no_lnr/"
                },
                new ()
                {
                    Name = "Список всех сотрудников(Имеющие паспорт ЛНР)",
                    Url = "/reports/pers/persons/passport_lnr/"
                },
                new ()
                {
                    Name = "Список всех сотрудников(У которорых отсутствует справка о несудимости)",
                    Url = "/reports/pers/persons/reference_no/"
                },
                new ()
                {
                    Name = "Список деканов и директоров",
                    Url = "/reports/pers/persons/director/"
                },
                new ()
                {
                    Name = "Список доцентов и кандидатов",
                    Url = "/reports/pers/persons/scince/"
                },
                new ()
                {
                    Name = "Реестер Докторов",
                    Url = "/reports/pers/persons/register_dok/"
                },
                new ()
                {
                    Name = "Реестер Кандидатов",
                    Url = "/reports/pers/persons/register_kan/"
                },
                new()
                {
                    Name = "Повышение квалификации(Методистов)",
                    Url = "/reports/pers/persons/metodist/"
                },
                new()
                {
                    Name = "Повышение квалификации Совместители(Методистов)",
                    Url = "/reports/pers/persons/metodist/is_pluralism/"
                },
                new()
                {
                    Name = "Список Профессоров и Докторов",
                    Url = "/reports/pers/persons/doc_prof/"
                },
                 new()
                {
                    Name = "Список Профессоров и Докторов(Совместители)",
                    Url = "/reports/pers/persons/doc_prof/is_pluralism/"
                },
                new()
                {
                    Name = "Дни рождения руководства",
                    Url = "/reports/pers/persons/birthdays/"
                },
                new()
                {
                    Name = "Список всех сотрудников(Мать одиночка)",
                    Url = "/reports/pers/persons/mother_is_one/"
                },
                new()
                {
                    Name = "Список всех сотрудников(Мать двоих и более детей)",
                    Url = "/reports/pers/persons/mother_is_two/"
                },
                new()
                {
                    Name = "Список всех сотрудников(Материально ответственные)",
                    Url = "/reports/pers/persons/is_responsible/"
                },
                new()
                {
                    Name = "Список всех сотрудников(больше 1 ставки)",
                    Url = "/reports/pers/persons/stavka/"
                },
                new()
                {
                    Name = "Список всех сотрудников(Внешние совместители)",
                    Url = "/reports/pers/persons/is_pluralism_ot/"
                },
                new()
                {
                    Name = "Список всех сотрудников(Внутренние совместители)",
                    Url = "/reports/pers/persons/is_pluralism_in/"
                },
            };*/
    }

    public override void Dispose()
    {
        base.Dispose();
    }

}

