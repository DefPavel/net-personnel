namespace AlphaPersonel.ViewModels;

internal class TypeRanksViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Users _user;

    public TypeRanksViewModel(NavigationStore navigationStore, Users user)
    {
        this._navigationStore = navigationStore;
        _user = user;
    }

    #region Переменные

    private ObservableCollection<TypeRank>? _typeRank;
    public ObservableCollection<TypeRank>? TypeRank
    {
        get => _typeRank;
        set
        {
            _ = Set(ref _typeRank, value);
            if (TypeRank != null) CollectionDepart = CollectionViewSource.GetDefaultView(TypeRank);
            if (CollectionDepart != null) CollectionDepart.Filter = FilterToDepart;
        }
    }

    // Выбранные отдел
    private TypeRank? _selectedRank;
    public TypeRank? SelectedRank
    {
        get => _selectedRank;
        set => Set(ref _selectedRank, value);
    }

    // Поисковая строка для поиска по ФИО сотрудника
    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            _ = Set(ref _filter, value);
            if (TypeRank != null)
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
        return string.IsNullOrEmpty(Filter) || (emp is TypeRank dep && dep.Name!.ToUpper().Contains(value: Filter.ToUpper()));
    }
    #endregion

    #region Команды

    private ICommand? _GetToMain;
    public ICommand GetToMain => _GetToMain ??= new LambdaCommand(GetBack);

    private ICommand? _LoadedType;
    public ICommand LoadedType => _LoadedType ??= new LambdaCommand(ApiGetType);

    private ICommand? _SaveType;
    public ICommand SaveType => _SaveType ??= new LambdaCommand(SaveRank, _ => SelectedRank is not null);

    private ICommand? _Add;
    public ICommand Add => _Add ??= new LambdaCommand(AddTypeOrderAsync);

    private ICommand? _Delete;
    public ICommand Delete => _Delete ??= new LambdaCommand(DeleteTypeRank);

    #endregion

    #region Логика

    private void GetBack(object p)
    {

        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }


    private async void AddTypeOrderAsync(object p)
    {

        TypeRank type = new()
        {
            Name = "Новое звание"
        };
        _typeRank!.Insert(0, type);
        SelectedRank = type;

       

    }

    private async void DeleteTypeRank(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rank/type/del/" + SelectedRank!.Id, "DELETE", SelectedRank);

            _ = _typeRank!.Remove(SelectedRank);

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
    private async void SaveRank(object p)
    {
        try
        {
            if (SelectedRank!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rank/type/rename/", "POST", SelectedRank);
                // MessageBox.Show("Изменить");
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rank/type/add", "POST", SelectedRank);
            }
            TypeRank = await QueryService.JsonDeserializeWithToken<TypeRank>(_user.Token, "/pers/rank/type/all", "GET");
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

    private async void ApiGetType(object p)
    {
        try
        {
            TypeRank = await QueryService.JsonDeserializeWithToken<TypeRank>(_user!.Token, "/pers/rank/type/all", "GET");
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
