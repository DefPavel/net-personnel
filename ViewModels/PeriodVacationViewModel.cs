using System.Linq;
namespace AlphaPersonel.ViewModels;
internal class PeriodVacationViewModel : BaseViewModel
{
    #region Переменные

    // Ссылка на User для того чтобы забрать token
    private readonly Users _user;

    private readonly NavigationStore _navigationStore;
    public PeriodVacationViewModel(NavigationStore navigationStore, Users user)
    {
        _user = user;
        this._navigationStore = navigationStore;

    }

    private ObservableCollection<PeriodVacation>? _periodVacation;
    public ObservableCollection<PeriodVacation>? PeriodVacations
    {
        get => _periodVacation;
        private set
        {
            Set(ref _periodVacation, value);
            if (PeriodVacations != null) CollectionDepart = CollectionViewSource.GetDefaultView(PeriodVacations);
            if (CollectionDepart != null) CollectionDepart.Filter = FilterToDepart;
        }
    }

    // Выбранные отдел
    private PeriodVacation? _selectedPeriod;
    public PeriodVacation? SelectedPeriod
    {
        get => _selectedPeriod;
        set => Set(ref _selectedPeriod, value);
    }


    // Поисковая строка для поиска по ФИО сотрудника
    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            _ = Set(ref _filter, value);
            if (PeriodVacations != null)
            {
                _collectionDepart!.Refresh();
            }

        }
    }
    // Специальная колекция для фильтров
    private ICollectionView? _collectionDepart;
    public ICollectionView? CollectionDepart
    {
        get => _collectionDepart;
        private set => Set(ref _collectionDepart, value);
    }

    private bool FilterToDepart(object emp)
    {
        return string.IsNullOrEmpty(Filter) || (emp is PeriodVacation dep && dep.Name.ToUpper().Contains(value: Filter.ToUpper()));
    }

    #endregion

    #region Команды

    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _loadedPeriod;
    public ICommand LoadedPeriod => _loadedPeriod ??= new LambdaAsyncCommand(ApiGetPeriod);

    private ICommand? _addNew;
    public ICommand AddNew => _addNew ??= new LambdaCommand(AddPeriod);

    private ICommand? _save;
    public ICommand Save => _save ??= new LambdaAsyncCommand(SavePeriod,_ => SelectedPeriod is not null);

    private ICommand? _delete;
    public ICommand Delete => _delete ??= new LambdaAsyncCommand(DeletePeriod, _ => PeriodVacations != null && SelectedPeriod is not null);

    #endregion

    #region Логика
    private void GetBack(object p)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }
    private async Task ApiGetPeriod(object p)
    {
        try
        {
            PeriodVacations = await QueryService.JsonDeserializeWithToken<PeriodVacation>(_user.Token, "/pers/vacation/period/get", "GET");

            if (PeriodVacations.Count > 0)
            {
                SelectedPeriod = PeriodVacations[0];
            }
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
    private void AddPeriod(object p)
    {

        var count = PeriodVacations!.Where(x => x.Id == 0).ToList().Count;
        if(count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        var toDay = DateTime.Now;

        PeriodVacation dep = new()
        {
            Name = $"{toDay.Year}-{toDay.AddYears(1).Year}",
        };
        _periodVacation!.Insert(0, dep);
        SelectedPeriod = dep;



    }
    private async Task DeletePeriod(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/vacation/period/del/" + SelectedPeriod!.Id, "DELETE", SelectedPeriod);

            _ = _periodVacation!.Remove(SelectedPeriod);

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
    private async Task SavePeriod(object p)
    {
        try
        {
            if (SelectedPeriod!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/vacation/period/rename/", "POST", SelectedPeriod);
                // MessageBox.Show("Изменить");
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/vacation/period/add", "POST", SelectedPeriod);
            }
            PeriodVacations = await QueryService.JsonDeserializeWithToken<PeriodVacation>(_user.Token, "/pers/vacation/period/get", "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

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
}

