using System.Linq;

namespace AlphaPersonel.ViewModels;
internal class TypeRanksViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Users _user;

    public TypeRanksViewModel(NavigationStore navigationStore, Users user)
    {
        _navigationStore = navigationStore;
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
        return string.IsNullOrEmpty(Filter) || (emp is TypeRank dep && dep.Name.ToUpper().Contains(value: Filter.ToUpper()));
    }
    #endregion

    #region Команды

    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _loadedType;
    public ICommand LoadedType => _loadedType ??= new LambdaAsyncCommand(ApiGetType);

    private ICommand? _saveType;
    public ICommand SaveType => _saveType ??= new LambdaAsyncCommand(SaveRank, _ => SelectedRank is not null);

    private ICommand? _add;
    public ICommand Add => _add ??= new LambdaCommand(AddTypeOrderAsync);

    private ICommand? _delete;
    public ICommand Delete => _delete ??= new LambdaAsyncCommand(DeleteTypeRank);

    #endregion

    #region Логика
    private void GetBack(object p)
    {

        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }
    private void AddTypeOrderAsync(object p)
    {

        var count = TypeRank!.Where(x => x.Id == 0).ToList().Count;
        if(count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        TypeRank type = new()
        {
            Name = "Новое звание"
        };
        _typeRank!.Insert(0, type);
        SelectedRank = type;
    }
    private async Task DeleteTypeRank(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rank/type/del/" + SelectedRank!.Id, "DELETE", SelectedRank);

            _ = _typeRank!.Remove(SelectedRank);

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
    private async Task SaveRank(object p)
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
    private async Task ApiGetType(object p)
    {
        try
        {
            TypeRank = await QueryService.JsonDeserializeWithToken<TypeRank>(_user.Token, "/pers/rank/type/all", "GET");

            if(TypeRank.Count > 0)
            {
                SelectedRank = TypeRank[0];
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
