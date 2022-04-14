namespace AlphaPersonel.ViewModels;

internal class TypeRewardingViewModel : BaseViewModel 
{
    private readonly NavigationStore navigationStore;
    private readonly Users _User;

    public TypeRewardingViewModel(NavigationStore navigationStore, Users user)
    {
        this.navigationStore = navigationStore;
        _User = user;
    }

    private string? _Filter;
    public string? Filter
    {
        get => _Filter;
        set
        {
            _ = Set(ref _Filter, value);
            if (TypeRewardings != null)
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
        return string.IsNullOrEmpty(Filter) || (emp is TypeRewarding dep && dep.Name!.ToUpper().Contains(Filter.ToUpper()));
    }


    private ObservableCollection<TypeRewarding>? _TypeRewardings;
    public ObservableCollection<TypeRewarding>? TypeRewardings
    {
        get => _TypeRewardings;
        private set
        {
            _ = Set(ref _TypeRewardings, value);
            CollectionDepart = CollectionViewSource.GetDefaultView(TypeRewardings);
            CollectionDepart.Filter = FilterToType;

        }
    }

    // Выбранные отдел
    private TypeRewarding? _SelectedType;
    public TypeRewarding? SelectedType
    {
        get => _SelectedType;
        set => Set(ref _SelectedType, value);
    }

    #region Команды


    private ICommand? _GetToMain;
    public ICommand GetToMain => _GetToMain ??= new LambdaCommand(GetBack);

    private ICommand? _LoadedType;
    public ICommand? LoadedType => _LoadedType ??= new LambdaCommand(ApiGetTypeRewarding);

    private ICommand? _Add;
    public ICommand? Add => _Add ??= new LambdaCommand(AddTypeRewardingAsync);

    private ICommand? _Save;
    public ICommand? Save => _Save ??= new LambdaCommand(SaveTypeRewarding);

    private ICommand? _Delete;
    public ICommand? Delete => _Delete ??= new LambdaCommand(DeleteTypeRewarding);

    #endregion

    #region Логика

    private void GetBack(object p)
    {
        navigationStore.CurrentViewModel = new HomeViewModel(_User, navigationStore);
    }

    private async void AddTypeRewardingAsync(object p)
    {
        try
        {
            TypeRewarding type = new()
            {
                Name = "Новый тип награждения"
            };
            _TypeRewardings!.Insert(0, type);
            SelectedType = type;

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

    private async void SaveTypeRewarding(object p)
    {
        try
        {
            if (_User.Token == null) return;

            if (SelectedType!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/rewarding/type/update/", "POST", SelectedType);
                // MessageBox.Show("Изменить");
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/rewarding/type/add", "POST", SelectedType);
            }
            TypeRewardings = await QueryService.JsonDeserializeWithToken<TypeRewarding>(_User.Token, "/pers/rewarding/type/get", "GET");
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

    private async void DeleteTypeRewarding(object p)
    {
        try
        {
            if (_User.Token == null) return;
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/rewarding/type/del/" + SelectedType!.Id, "DELETE", SelectedType);

                _ = _TypeRewardings!.Remove(SelectedType);
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


    private async void ApiGetTypeRewarding(object p)
    {
        try
        {
            if (_User.Token == null) return;
            // Загрузить сами приказы
            TypeRewardings = await QueryService.JsonDeserializeWithToken<TypeRewarding>(_User!.Token, "/pers/rewarding/type/get", "GET");
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

