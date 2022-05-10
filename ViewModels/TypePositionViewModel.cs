using System.Linq;

namespace AlphaPersonel.ViewModels;

internal class TypePositionViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Users _user;

    public TypePositionViewModel(NavigationStore navigationStore, Users user)
    {
        this._navigationStore = navigationStore;
        _user = user;
    }

    private bool _radioIsPed;
    public bool RadioIsPed
    {
        get => _radioIsPed;
        set => Set(ref _radioIsPed, value);
    }

    private bool _radioIsNoPed;
    public bool RadioIsNoPed
    {
        get => _radioIsNoPed;
        set => Set(ref _radioIsNoPed, value);
    }
    // Выбранные отдел
    private TypePosition? _selectedPosition;
    public TypePosition? SelectedPosition
    {
        get => _selectedPosition;
        set
        {
            Set(ref _selectedPosition, value);

            if (_selectedPosition == null) return;
            if (_selectedPosition.IsPed)
            {
                RadioIsPed = true;
            }
            else
            {
                RadioIsNoPed = true;
            }
        }
    }

    // Поисковая строка для поиска по ФИО сотрудника
    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            _ = Set(ref _filter, value);
            if (TypePosition != null)
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
        return string.IsNullOrEmpty(Filter) || (emp is TypePosition dep && dep.Name!.ToUpper().Contains(value: Filter.ToUpper()));
    }

    private ObservableCollection<TypePosition>? _typePosition;
    public ObservableCollection<TypePosition>? TypePosition
    {
        get => _typePosition;
        set
        {
            _ = Set(ref _typePosition, value);
            if (TypePosition != null) CollectionDepart = CollectionViewSource.GetDefaultView(TypePosition);
            if (CollectionDepart != null) CollectionDepart.Filter = FilterToDepart;
        }
    }

    #region Команды
    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _loadedType;
    public ICommand LoadedType => _loadedType ??= new LambdaCommand(ApiGetTypeAsync);

    private ICommand? _add;
    public ICommand Add => _add ??= new LambdaCommand(AddPosition);

    private ICommand? _save;
    public ICommand Save => _save ??= new LambdaCommand(SaveTypePosition , _ => SelectedPosition is not null && !string.IsNullOrEmpty(SelectedPosition.Name));

    private ICommand? _delete;
    public ICommand Delete => _delete ??= new LambdaCommand(DeleteTypePosition , _ => SelectedPosition is not null && TypePosition!.Count > 0);
    #endregion

    private async void ApiGetTypeAsync(object obj)
    {
        try
        {
            TypePosition = await QueryService.JsonDeserializeWithToken<TypePosition>(_user!.Token, "/pers/position/type/position", "GET");
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

    private void GetBack(object obj)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }

    private async void SaveTypePosition(object p)
    {
        var newSlectedItem = SelectedPosition!;
        try
        {
            /*if(string.IsNullOrWhiteSpace(SelectedPosition!.Name))
            {
                MessageBox.Show("Введите наименование должности!", "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                return;
            }
            */
            if (SelectedPosition!.Id > 0)
            {
                SelectedPosition.IsPed = RadioIsPed;
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/position/type/position/rename", "POST", SelectedPosition);
            }
            else
            {
                SelectedPosition.IsPed = RadioIsPed;
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/position/type/position/add", "POST", SelectedPosition);
            }
            TypePosition = await QueryService.JsonDeserializeWithToken<TypePosition>(_user.Token, "/pers/position/type/position", "GET");
            SelectedPosition = TypePosition.FirstOrDefault(x => x.Name == newSlectedItem!.Name);
           // _ = MessageBox.Show("Данные успешно сохраненны");


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
    private void AddPosition(object p)
    {

        TypePosition type = new()
        {
            Name = "Новая Должность",
            LimitHoliday = 28, 
            NameGenitive = "Не указано"
                
                
        };
        _typePosition!.Insert(0, type);
        SelectedPosition = type;

    }

    private async void DeleteTypePosition(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данную запись?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/position/type/del/" + SelectedPosition!.Id, "DELETE", SelectedPosition);

                
            // Удаляем из массива
            _ = _typePosition!.Remove(SelectedPosition);
            // Очищаем фильтр
            Filter = string.Empty;
            // Выбираем первый элемент из списка
            if (TypePosition != null) SelectedPosition = TypePosition.FirstOrDefault();
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

    public override void Dispose()
    {
        base.Dispose();
    }
}

