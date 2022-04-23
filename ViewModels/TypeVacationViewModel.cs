namespace AlphaPersonel.ViewModels;

internal class TypeVacationViewModel : BaseViewModel 
{
    private readonly NavigationStore _navigationStore;
    private readonly Users _user;

    public TypeVacationViewModel(NavigationStore navigationStore, Users user)
    {
        this._navigationStore = navigationStore;
        _user = user;
    }


    private ObservableCollection<Models.TypeVacation>? _typeVacation;
    public ObservableCollection<Models.TypeVacation>? TypeVacations
    {
        get => _typeVacation;
        private set
        {
            _ = Set(ref _typeVacation, value);
            if (TypeVacations != null) CollectionDepart = CollectionViewSource.GetDefaultView(TypeVacations);
            if (CollectionDepart != null) CollectionDepart.Filter = FilterToType;
        }
    }

    // Выбранные отдел
    private Models.TypeVacation? _selectedTypeVacation;
    public Models.TypeVacation? SelectedTypeVacation
    {
        get => _selectedTypeVacation;
        set => Set(ref _selectedTypeVacation, value);
    }


    // Поисковая строка для поиска по ФИО сотрудника
    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            _ = Set(ref _filter, value);
            if (TypeVacations != null)
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

    private bool FilterToType(object emp)
    {
        return string.IsNullOrEmpty(Filter) || (emp is Models.TypeVacation dep && dep.Name!.ToUpper().Contains(Filter.ToUpper()));
    }

    #region Команды
    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _loadedType;
    public ICommand LoadedType => _loadedType ??= new LambdaCommand(ApiGetType);

    private ICommand? _add;
    public ICommand Add => _add ??= new LambdaCommand(AddTypeVacationAsync);

    private ICommand? _save;
    public ICommand Save => _save ??= new LambdaCommand(SaveTypeVacation, _ => SelectedTypeVacation != null);

    private ICommand? _delete;
    public ICommand Delete => _delete ??= new LambdaCommand(DeleteTypeVacation, _ => SelectedTypeVacation != null);

    #endregion

    #region Логика

    private void GetBack(object p)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }


    private void AddTypeVacationAsync(object p)
    {

        Models.TypeVacation type = new()
        {
            Name = "Новый тип отпуска"
        };
        _typeVacation!.Insert(0, type);
        SelectedTypeVacation = type;



    }

    private async void SaveTypeVacation(object p)
    {
        try
        {
            if (SelectedTypeVacation!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/vacation/type/rename", "POST", SelectedTypeVacation);
                // MessageBox.Show("Изменить");
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/vacation/type/add", "POST", SelectedTypeVacation);
            }
            TypeVacations = await QueryService.JsonDeserializeWithToken<Models.TypeVacation>(_user.Token, "/pers/vacation/type/get", "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

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

    private async void DeleteTypeVacation(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/vacation/type/del/" + SelectedTypeVacation!.Id, "DELETE", SelectedTypeVacation);

            _ = TypeVacations!.Remove(SelectedTypeVacation);

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

    private async void ApiGetType(object p)
    {
        try
        {
            // Загрузить массив типов приказов
            TypeVacations = await QueryService.JsonDeserializeWithToken<Models.TypeVacation>(_user.Token, "/pers/vacation/type/get", "GET");


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

