using System.ComponentModel;

namespace AlphaPersonel.ViewModels;

internal class PeriodVacationViewModel : BaseViewModel
{
    #region Переменные

    // Ссылка на User для того чтобы забрать token
    private readonly Users _User;

    private readonly NavigationStore navigationStore;
    public PeriodVacationViewModel(NavigationStore navigationStore, Users user)
    {
        _User = user;
        this.navigationStore = navigationStore;

    }

    private ObservableCollection<PeriodVacation>? _PeriodVacation;
    public ObservableCollection<PeriodVacation>? PeriodVacations
    {
        get => _PeriodVacation;
        private set
        {
            Set(ref _PeriodVacation, value);
            CollectionDepart = CollectionViewSource.GetDefaultView(PeriodVacations);
            CollectionDepart.Filter = FilterToDepart;
        }
    }

    // Выбранные отдел
    private PeriodVacation? _SelectedPeriod;
    public PeriodVacation? SelectedPeriod
    {
        get => _SelectedPeriod;
        set => Set(ref _SelectedPeriod, value);
    }


    // Поисковая строка для поиска по ФИО сотрудника
    private string? _Filter;
    public string? Filter
    {
        get => _Filter;
        set
        {
            _ = Set(ref _Filter, value);
            if (PeriodVacations != null)
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
        return string.IsNullOrEmpty(Filter) || (emp is PeriodVacation dep && dep.Name!.ToUpper().Contains(value: Filter.ToUpper()));
    }

    #endregion

    #region Команды

    private ICommand? _GetToMain;
    public ICommand GetToMain => _GetToMain ??= new LambdaCommand(GetBack);

    private ICommand? _LoadedPeriod;
    public ICommand LoadedPeriod => _LoadedPeriod ??= new LambdaCommand(ApiGetPeriod);

    private ICommand? _AddNew;
    public ICommand AddNew => _AddNew ??= new LambdaCommand(AddPeriod);

    private ICommand? _Save;
    public ICommand Save => _Save ??= new LambdaCommand(SavePeriod);

    private ICommand? _Delete;
    public ICommand Delete => _Delete ??= new LambdaCommand(DeletePeriod);

    #endregion

    #region Логика
    private void GetBack(object p)
    {
        navigationStore.CurrentViewModel = new HomeViewModel(_User, navigationStore);
    }

    private async void ApiGetPeriod(object p)
    {
        try
        {
            if (_User.Token != null)
            {

                PeriodVacations = await QueryService.JsonDeserializeWithToken<PeriodVacation>(_User!.Token, "/pers/vacation/period/get", "GET");
            }
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

    private void AddPeriod(object p)
    {
        try
        {
            DateTime toDay = DateTime.Now;

            PeriodVacation dep = new()
            {
                Name = $"{toDay.Year}-{toDay.AddYears(1).Year}",
            };
            _PeriodVacation!.Insert(0, dep);
            SelectedPeriod = dep;

        }
        catch
        {
            throw;
        }

    }

    private async void DeletePeriod(object p)
    {
        try
        {
            if (_User.Token == null) return;
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/vacation/period/del/" + SelectedPeriod!.Id, "DELETE", SelectedPeriod);

                _ = _PeriodVacation!.Remove(SelectedPeriod);
            }

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

    private async void SavePeriod(object p)
    {
        try
        {
            if (_User.Token == null) return;

            if (SelectedPeriod!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/vacation/period/rename/", "POST", SelectedPeriod);
                // MessageBox.Show("Изменить");
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/vacation/period/add", "POST", SelectedPeriod);
            }
            PeriodVacations = await QueryService.JsonDeserializeWithToken<PeriodVacation>(_User.Token, "/pers/vacation/period/get", "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

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

