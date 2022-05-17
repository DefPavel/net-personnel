using System.Collections.Generic;

namespace AlphaPersonel.ViewModels;

internal class SearchViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private Users? _user;
    public Users? User
    {
        get => _user;
        set => Set(ref _user, value);
    }

    public SearchViewModel(NavigationStore navigationStore, Users user)
    {
        this._navigationStore = navigationStore;
        User = user;
    }
    // Выбранный сотрудник 
    private Persons? _selectedPerson;
    public Persons? SelectedPerson
    {
        get => _selectedPerson;
        set => Set(ref _selectedPerson, value);
    }

    // Процесс загрузки
    private VisualBoolean? _isLoading;
    public VisualBoolean? IsLoading
    {
        get => _isLoading;
        private set => Set(ref _isLoading, value);
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
    private IEnumerable<Departments>? _departments;
    public IEnumerable<Departments>? Departments
    {
        get => _departments;
        private set => Set(ref _departments, value);
    }
    // Массив Персон
    private IEnumerable<Persons>? _persons;
    public IEnumerable<Persons>? Persons
    {
        get => _persons;
        private set => Set(ref _persons, value);
    }

    #region Команды

    // Команда поиска сотрудника
    private ICommand? _searchPerson;
    // Лямбда Команда 
    public ICommand SearchPerson => _searchPerson ??= new LambdaCommand(SearchItemPersonAsync, CanSearchPerson);
    // Команда поиска сотрудника
    private ICommand? _searchDepartment;
    // Лямбда Команда 
    public ICommand SearchDepartment => _searchDepartment ??= new LambdaCommand(SearchItemDepartment, CanSearchDepartment);

    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _openPersonCard;
    public ICommand OpenPersonCard => _openPersonCard ??= new LambdaCommand(OpenPersonCardView);

    #endregion

    #region Логика
    private bool CanSearchPerson(object arg) => !string.IsNullOrEmpty(QueryPerson);
    private bool CanSearchDepartment(object arg) => !string.IsNullOrEmpty(QueryDepartment);


    private void OpenPersonCardView(object p)
    {
        _navigationStore.CurrentViewModel = new PersonCardViewModel(_user!, _navigationStore, SelectedPerson!, SelectedPerson!.IdDepartment);
    }

    private async void SearchItemDepartment(object obj)
    {
        try
        {
            IsLoading = true;
            Departments = await QueryService.JsonDeserializeWithToken<Departments>(_user!.Token, $"/pers/tree/find/?text={QueryDepartment}", "GET");
            IsLoading = false;
        }
        // Проверка токена
        catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == (HttpStatusCode)403)
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
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void GetBack(object p)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(_user!, _navigationStore);
    }

    private async void SearchItemPersonAsync(object obj)
    {
        try
        {
            IsLoading = true;
            Persons = await QueryService.JsonDeserializeWithToken<Persons>(_user!.Token, $"/pers/person/find/?text={QueryPerson}", "GET");
            IsLoading = false;
        }
        // Проверка токена
        catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == (HttpStatusCode)403)
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

