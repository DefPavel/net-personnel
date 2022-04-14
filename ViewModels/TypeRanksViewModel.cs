namespace AlphaPersonel.ViewModels;

internal class TypeRanksViewModel : BaseViewModel
{
    private readonly NavigationStore navigationStore;
    private readonly Users _User;

    public TypeRanksViewModel(NavigationStore navigationStore, Users user)
    {
        this.navigationStore = navigationStore;
        _User = user;
    }

    #region Переменные

    private ObservableCollection<TypeRank>? _TypeRank;
    public ObservableCollection<TypeRank>? TypeRank
    {
        get => _TypeRank;
        set
        {
            _ = Set(ref _TypeRank, value);
            CollectionDepart = CollectionViewSource.GetDefaultView(TypeRank);
            CollectionDepart.Filter = FilterToDepart;
        }
    }

    // Выбранные отдел
    private TypeRank? _SelectedRank;
    public TypeRank? SelectedRank
    {
        get => _SelectedRank;
        set => Set(ref _SelectedRank, value);
    }

    // Поисковая строка для поиска по ФИО сотрудника
    private string? _Filter;
    public string? Filter
    {
        get => _Filter;
        set
        {
            _ = Set(ref _Filter, value);
            if (TypeRank != null)
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

        navigationStore.CurrentViewModel = new HomeViewModel(_User, navigationStore);
    }


    private async void AddTypeOrderAsync(object p)
    {
        try
        {
            TypeRank type = new()
            {
                Name = "Новое звание"
            };
            _TypeRank!.Insert(0, type);
            SelectedRank = type;

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

    private async void DeleteTypeRank(object p)
    {
        try
        {
            if (_User.Token == null) return;
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/rank/type/del/" + SelectedRank!.Id, "DELETE", SelectedRank);

                _ = _TypeRank!.Remove(SelectedRank);
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
    private async void SaveRank(object p)
    {
        try
        {
            if (_User.Token == null) return;

            if (SelectedRank!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/rank/type/rename/", "POST", SelectedRank);
                // MessageBox.Show("Изменить");
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/rank/type/add", "POST", SelectedRank);
            }
            TypeRank = await QueryService.JsonDeserializeWithToken<TypeRank>(_User.Token, "/pers/rank/type/all", "GET");
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

    private async void ApiGetType(object p)
    {
        try
        {
            if (_User.Token != null)
            {
                TypeRank = await QueryService.JsonDeserializeWithToken<TypeRank>(_User!.Token, "/pers/rank/type/all", "GET");

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

    #endregion


    public override void Dispose()
    {
        base.Dispose();
    }
}
