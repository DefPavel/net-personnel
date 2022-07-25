using System.Linq;

namespace AlphaPersonel.ViewModels;

internal class TypeRewardingViewModel : BaseViewModel 
{
    private readonly NavigationStore _navigationStore;
    private readonly Users _user;

    public TypeRewardingViewModel(NavigationStore navigationStore, Users user)
    {
        this._navigationStore = navigationStore;
        _user = user;
    }

    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            _ = Set(ref _filter, value);
            if (TypeRewardings != null)
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
        return string.IsNullOrEmpty(Filter) || (emp is TypeRewarding dep && dep.Name.ToUpper().Contains(Filter.ToUpper()));
    }


    private ObservableCollection<TypeRewarding>? _typeRewardings;

    private ObservableCollection<TypeRewarding>? TypeRewardings
    {
        get => _typeRewardings;
        set
        {
            _ = Set(ref _typeRewardings, value);
            if (TypeRewardings != null) CollectionDepart = CollectionViewSource.GetDefaultView(TypeRewardings);
            if (CollectionDepart != null) CollectionDepart.Filter = FilterToType;
        }
    }

    // Выбранные отдел
    private TypeRewarding? _selectedType;
    public TypeRewarding? SelectedType
    {
        get => _selectedType;
        set => Set(ref _selectedType, value);
    }

    #region Команды


    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _loadedType;
    public ICommand LoadedType => _loadedType ??= new LambdaCommand(ApiGetTypeRewarding);

    private ICommand? _add;
    public ICommand Add => _add ??= new LambdaCommand(AddTypeRewardingAsync);

    private ICommand? _save;
    public ICommand Save => _save ??= new LambdaCommand(SaveTypeRewarding);

    private ICommand? _delete;
    public ICommand Delete => _delete ??= new LambdaCommand(DeleteTypeRewarding);

    #endregion

    #region Логика

    private void GetBack(object p)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }

    private void AddTypeRewardingAsync(object p)
    {
        var count = TypeRewardings!.Where(x => x.Id == 0).ToList().Count;
        if(count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        TypeRewarding type = new()
        {
            Name = "Новый тип награждения"
        };
        _typeRewardings!.Insert(0, type);
        SelectedType = type;
    }

    private async void SaveTypeRewarding(object p)
    {
        try
        {
            if (SelectedType!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rewarding/type/update/", "POST", SelectedType);
                // MessageBox.Show("Изменить");
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rewarding/type/add", "POST", SelectedType);
            }
            TypeRewardings = await QueryService.JsonDeserializeWithToken<TypeRewarding>(_user.Token, "/pers/rewarding/type/get", "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        // Проверка токена
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
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

    private async void DeleteTypeRewarding(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rewarding/type/del/" + SelectedType!.Id, "DELETE", SelectedType);

            _ = _typeRewardings!.Remove(SelectedType);

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


    private async void ApiGetTypeRewarding(object p)
    {
        try
        {
            // Загрузить сами приказы
            TypeRewardings = await QueryService.JsonDeserializeWithToken<TypeRewarding>(_user.Token, "/pers/rewarding/type/get", "GET");

            if(TypeRewardings.Count > 0)
            {
                SelectedType = TypeRewardings[0];
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

    #endregion

}

