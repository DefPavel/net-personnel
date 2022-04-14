namespace AlphaPersonel.ViewModels;

internal class SearchViewModel : BaseViewModel
{
    private readonly NavigationStore navigationStore;
    private Users? _User;
    public Users? User
    {
        get => _User;
        set => Set(ref _User, value);
    }

    public SearchViewModel(NavigationStore navigationStore, Users user)
    {
        this.navigationStore = navigationStore;
        User = user;
    }
    // Выбранный сотрудник 
    private Persons? _SelectedPerson;
    public Persons? SelectedPerson
    {
        get => _SelectedPerson;
        set => Set(ref _SelectedPerson, value);
    }

    // Процесс загрузки
    private VisualBoolean? _IsLoading;
    public VisualBoolean? IsLoading
    {
        get => _IsLoading;
        set => Set(ref _IsLoading, value);
    }

    private string? _queryPerson;

    public string? QueryPerson
    {
        get => _queryPerson;
        set => Set(ref _queryPerson, value);

    }

    private string? _queryDep;
    public string? QueryDepartment
    {
        get => _queryDep;
        set => Set(ref _queryDep, value);
    }

    // Массив отделов
    private ObservableCollection<Departments>? _Departments;
    public ObservableCollection<Departments>? Departments
    {
        get => _Departments;
        set => Set(ref _Departments, value);
    }
    // Массив Персон
    private ObservableCollection<Persons>? _Persons;
    public ObservableCollection<Persons>? Persons
    {
        get => _Persons;
        set => Set(ref _Persons, value);
    }

    #region Команды

    // Команда поиска сотрудника
    private ICommand? _SearchPerson;
    // Лямбда Команда 
    public ICommand SearchPerson => _SearchPerson ??= new LambdaCommand(SearchItemPersonAsync, CanSearchPerson);
    // Команда поиска сотрудника
    private ICommand? _SearchDepartment;
    // Лямбда Команда 
    public ICommand SearchDepartment => _SearchDepartment ??= new LambdaCommand(SearchItemDepartment, CanSearchDepartment);

    private ICommand? _GetToMain;
    public ICommand GetToMain => _GetToMain ??= new LambdaCommand(GetBack);

    private ICommand? _OpenPersonCard;
    public ICommand OpenPersonCard => _OpenPersonCard ??= new LambdaCommand(OpenPersonCardView);

    #endregion

    #region Логика
    private bool CanSearchPerson(object arg) => !string.IsNullOrEmpty(QueryPerson);
    private bool CanSearchDepartment(object arg) => !string.IsNullOrEmpty(QueryDepartment);


    private void OpenPersonCardView(object p)
    {
        navigationStore.CurrentViewModel = new PersonCardViewModel(_User!, navigationStore, SelectedPerson!, SelectedPerson!.IdDepartment);
    }

    private async void SearchItemDepartment(object obj)
    {
        try
        {
            if (_User!.Token == null) return;

            IsLoading = true;

            Departments = await QueryService.JsonDeserializeWithTokenAndParam(_User!.Token, "/pers/tree/find/", "POST", new Departments
            {
                Name = QueryDepartment!.Trim(),
            });

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

    private void GetBack(object p)
    {
        navigationStore.CurrentViewModel = new HomeViewModel(_User!, navigationStore);
    }

    private async void SearchItemPersonAsync(object obj)
    {
        try
        {
            if (_User!.Token == null) return;

            IsLoading = true;

            Persons = await QueryService.JsonDeserializeWithTokenAndParam(_User!.Token, "/pers/person/find/", "POST",
            new Persons
            {
                FirstName = QueryPerson!.Trim(),
            });

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

    public override void Dispose()
    {
        base.Dispose();
    }
}

