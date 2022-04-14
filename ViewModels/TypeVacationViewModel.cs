namespace AlphaPersonel.ViewModels;

internal class TypeVacationViewModel : BaseViewModel 
{
    private readonly NavigationStore navigationStore;
    private readonly Users _User;

    public TypeVacationViewModel(NavigationStore navigationStore, Users user)
    {
        this.navigationStore = navigationStore;
        _User = user;
    }


    private ObservableCollection<Models.TypeVacation>? _TypeVacation;
    public ObservableCollection<Models.TypeVacation>? TypeVacations
    {
        get => _TypeVacation;
        private set
        {
            _ = Set(ref _TypeVacation, value);
            CollectionDepart = CollectionViewSource.GetDefaultView(TypeVacations);
            CollectionDepart.Filter = FilterToType;

        }
    }

    // Выбранные отдел
    private Models.TypeVacation? _SelectedTypeVacation;
    public Models.TypeVacation? SelectedTypeVacation
    {
        get => _SelectedTypeVacation;
        set => Set(ref _SelectedTypeVacation, value);
    }


    // Поисковая строка для поиска по ФИО сотрудника
    private string? _Filter;
    public string? Filter
    {
        get => _Filter;
        set
        {
            _ = Set(ref _Filter, value);
            if (TypeVacations != null)
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
        private set => Set(ref _CollectionDepart, value);
    }

    private bool FilterToType(object emp)
    {
        return string.IsNullOrEmpty(Filter) || (emp is Models.TypeVacation dep && dep.Name!.ToUpper().Contains(Filter.ToUpper()));
    }

    #region Команды
    private ICommand? _GetToMain;
    public ICommand GetToMain => _GetToMain ??= new LambdaCommand(GetBack);

    private ICommand? _LoadedType;
    public ICommand LoadedType => _LoadedType ??= new LambdaCommand(ApiGetType);

    private ICommand? _Add;
    public ICommand Add => _Add ??= new LambdaCommand(AddTypeVacationAsync);

    private ICommand? _Save;
    public ICommand Save => _Save ??= new LambdaCommand(SaveTypeVacation, _ => SelectedTypeVacation != null);

    private ICommand? _Delete;
    public ICommand Delete => _Delete ??= new LambdaCommand(DeleteTypeVacation, _ => SelectedTypeVacation != null);

    #endregion

    #region Логика

    private void GetBack(object p)
    {
        navigationStore.CurrentViewModel = new HomeViewModel(_User, navigationStore);
    }


    private async void AddTypeVacationAsync(object p)
    {
        try
        {
            Models.TypeVacation type = new()
            {
                Name = "Новый тип отпуска"
            };
            _TypeVacation!.Insert(0, type);
            SelectedTypeVacation = type;

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

    private async void SaveTypeVacation(object p)
    {
        try
        {
            if (_User.Token == null) return;

            if (SelectedTypeVacation!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/vacation/type/rename", "POST", SelectedTypeVacation);
                // MessageBox.Show("Изменить");
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/vacation/type/add", "POST", SelectedTypeVacation);
            }
            TypeVacations = await QueryService.JsonDeserializeWithToken<Models.TypeVacation>(_User.Token, "/pers/vacation/type/get", "GET");
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

    private async void DeleteTypeVacation(object p)
    {
        try
        {
            if (_User.Token == null) return;
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/vacation/type/del/" + SelectedTypeVacation!.Id, "DELETE", SelectedTypeVacation);

                _ = TypeVacations!.Remove(SelectedTypeVacation);
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

    private async void ApiGetType(object p)
    {
        try
        {
            if (_User.Token == null) return;
            // Загрузить массив типов приказов
            TypeVacations = await QueryService.JsonDeserializeWithToken<Models.TypeVacation>(_User.Token, "/pers/vacation/type/get", "GET");


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
}

