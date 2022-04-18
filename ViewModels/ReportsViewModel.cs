using System.Linq;

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
    private VisualBoolean? _IsLoading = false;
    public VisualBoolean? IsLoading
    {
        get => _IsLoading;
        set => Set(ref _IsLoading, value);
    }

    // Ссылка на User для того чтобы забрать token
    private Users _User;
    public Users User
    {
        get => _User;
        set => Set(ref _User, value);
    }

    private readonly NavigationStore navigationStore;

    // Выбранные отчет
    private Report? _SelectedReport;
    public Report? SelectedReport
    {
        get => _SelectedReport;
        set => Set(ref _SelectedReport, value);
    }

    private bool _SelectedIsPed;
    public bool SelectedIsPed
    {
        get => _SelectedIsPed;
        set => Set(ref _SelectedIsPed, value);
    }

    private bool _SelectedIsNoPed;
    public bool SelectedIsNoPed
    {
        get => _SelectedIsNoPed;
        set => Set(ref _SelectedIsNoPed, value);
    }

    private bool _SelectedIsAll = true;
    public bool SelectedIsAll
    {
        get => _SelectedIsAll;
        set => Set(ref _SelectedIsAll, value);
    }

    // Массив отчетов
    private ObservableCollection<Report>? _Reports;
    public ObservableCollection<Report>? Reports
    {
        get => _Reports;
        set
        {
            _ = Set(ref _Reports, value);
            CollectionDepart = CollectionViewSource.GetDefaultView(Reports);
            CollectionDepart.Filter = FilterToDepart;
        }
    }

    // Поисковая строка 
    private string? _Filter;
    public string? Filter
    {
        get => _Filter;
        set
        {
            _ = Set(ref _Filter, value);
            if (Reports != null)
            {
                _CollectionDepart!.Refresh();
            }

        }
    }
    // Специальная колекция для фильтров
    private ICollectionView? _CollectionDepart;
    public ICollectionView? CollectionDepart
    {
        get => _CollectionDepart;
        set => Set(ref _CollectionDepart, value);
    }

    private bool FilterToDepart(object emp)
    {
        return string.IsNullOrEmpty(Filter) || (emp is Report dep && dep.Name!.ToUpper().Contains(value: Filter.ToUpper()));
    }

    private bool CanCommandExecute(object p)
    {
        return p is Report && p is not null;
    }
    #endregion

    #region Команды

    private ICommand? _OpenReport;
    public ICommand OpenReport => _OpenReport ??= new LambdaCommand(ApiGetReport, CanCommandExecute);

    private ICommand? _GetToMain;
    public ICommand GetToMain => _GetToMain ??= new LambdaCommand(GetBack);

    #endregion

    #region Логика

    private void GetBack(object p)
    {
        navigationStore.CurrentViewModel = new HomeViewModel(_User, navigationStore);
    }

    private async void ApiGetReport(object obj)
    {
        try
        {

            if (_User!.Token == null) return;

            IsLoading = true;

            if (SelectedIsPed)
            {
                await ReportService.JsonDeserializeWithToken(
                    token: _User!.Token,
                    queryUrl: SelectedReport!.Url + TypeReport.IsPed,
                    HttpMethod: "GET",
                    ReportName: SelectedReport!.Name
                );
            }
            else if (SelectedIsNoPed)
            {
                await ReportService.JsonDeserializeWithToken(
                    token: _User!.Token,
                    queryUrl: SelectedReport!.Url + TypeReport.IsNoPed.ToString(),
                    HttpMethod: "GET",
                    ReportName: SelectedReport!.Name
                );

            }
            else
            {

                await ReportService.JsonDeserializeWithToken(
                   token: _User!.Token,
                   queryUrl: SelectedReport!.Url + TypeReport.IsAll.ToString(),
                   HttpMethod: "GET",
                   ReportName: SelectedReport!.Name
               );
            }
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
    #endregion


    public ReportsViewModel(NavigationStore navigationStore, Users user)
    {
        _User = user;
        this.navigationStore = navigationStore;

        Reports = new ObservableCollection<Report>
            {
                new Report
                {
                    Name = "Список отделов",
                    Url = "/reports/pers/departments/"
                },
                new Report
                {
                    Name = "Список всех сотрудников",
                    Url = "/reports/pers/persons/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Только Мужчины)",
                    Url = "/reports/pers/persons/male/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Только Женщины)",
                    Url = "/reports/pers/persons/female/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Студенты)",
                    Url = "/reports/pers/persons/student/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Аспиранты)",
                    Url = "/reports/pers/persons/graduate/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Пенсионеры)",
                    Url = "/reports/pers/persons/persioner"
                },
                new Report
                {
                    Name = "Список сотрудников-инвалидов",
                    Url = "/reports/pers/persons/invalids/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Не ЛГПУ)",
                    Url = "/reports/pers/persons/no_lgpu/"
                },
                new Report
                {
                    Name = "Список академиков и членов корреспондентов",
                    Url = "/reports/pers/persons/academic/"
                },
                new Report
                {
                    Name = "Список Принятых Сотрудников",
                    Url = "/reports/pers/persons/insert/"
                },
                new Report
                {
                    Name = "Список Сотрудников(Юбиляров)",
                    Url = "/reports/pers/persons/jubilee/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Имеющих русский паспорт)",
                    Url = "/reports/pers/persons/passport_rus/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Не имеющих паспорт ЛНР)",
                    Url = "/reports/pers/persons/passport_no_lnr/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Имеющие паспорт ЛНР)",
                    Url = "/reports/pers/persons/passport_lnr/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(У которорых отсутствует справка о несудимости)",
                    Url = "/reports/pers/persons/reference_no/"
                },
                new Report
                {
                    Name = "Список Награжденных",
                    Url = "/reports/pers/persons/rewarding/"
                },
                new Report
                {
                    Name = "Список деканов и директоров",
                    Url = "/reports/pers/persons/director/"
                },
                new Report
                {
                    Name = "Повышение квалификации(Методистов)",
                    Url = "/reports/pers/persons/metodist/"
                },
                new Report
                {
                    Name = "Список Профессоров и Докторов",
                    Url = "/reports/pers/persons/doc_prof/"
                },
                new Report
                {
                    Name = "Дни рождения руководства",
                    Url = "/reports/pers/persons/birthdays/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Мать одиночка)",
                    Url = "/reports/pers/persons/mother_is_one/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Мать двоих и более детей)",
                    Url = "/reports/pers/persons/mother_is_two/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Материально ответственные)",
                    Url = "/reports/pers/persons/is_responsible/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(больше 1 ставки)",
                    Url = "/reports/pers/persons/responsible/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Внешние совместители)",
                    Url = "/reports/pers/persons/is_pluralism_ot/"
                },
                new Report
                {
                    Name = "Список всех сотрудников(Внутренние совместители)",
                    Url = "/reports/pers/persons/is_pluralism_in/"
                },
            };
    }

    public override void Dispose()
    {
        base.Dispose();
    }

}

