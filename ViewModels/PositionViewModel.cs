namespace AlphaPersonel.ViewModels;

internal class PositionViewModel : BaseViewModel
{
    private readonly NavigationStore navigationStore;

    private readonly Users _User;

    public PositionViewModel(NavigationStore navigationStore, Users user)
    {
        this.navigationStore = navigationStore;
        _User = user;
    }

    #region Переменные

    // Поисковая строка для поиска по ФИО сотрудника
    private string? _Filter;
    public string? Filter
    {
        get => _Filter;
        set
        {
            _ = Set(ref _Filter, value);
            if (Positions != null)
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

    private bool FilterToPos(object emp) => string.IsNullOrEmpty(Filter) || (emp is Position pos && pos.Name!.ToUpper().Contains(value: Filter.ToUpper()));


    private bool _RadioIsPed;
    public bool RadioIsPed
    {
        get => _RadioIsPed;
        set => Set(ref _RadioIsPed, value);
    }

    private bool _RadioIsNoPed;
    public bool RadioIsNoPed
    {
        get => _RadioIsNoPed;
        set => Set(ref _RadioIsNoPed, value);
    }


    private ObservableCollection<Position>? _Position;
    public ObservableCollection<Position>? Positions
    {
        get => _Position;
        set
        {
            _ = Set(ref _Position, value);
            CollectionDepart = CollectionViewSource.GetDefaultView(Positions);
            CollectionDepart.Filter = FilterToPos;
        }
    }
    // Выбранные отдел
    private Position? _SelectedPosition;
    public Position? SelectedPosition
    {
        get => _SelectedPosition;
        set
        {
            _ = Set(ref _SelectedPosition, value);

            if (_SelectedPosition != null)
            {
                if (_SelectedPosition.IsPed)
                {
                    RadioIsPed = true;
                }
                else
                {
                    RadioIsNoPed = true;
                }
            }

        }
    }

    private ObservableCollection<Departments>? _Department;
    public ObservableCollection<Departments>? Department
    {
        get => _Department;
        set => Set(ref _Department, value);
    }

    #endregion

    #region Команды

    private ICommand? _GetToMain;
    public ICommand GetToMain => _GetToMain ??= new LambdaCommand(GetBack);

    private ICommand? _LoadedPosition;
    public ICommand LoadedPosition => _LoadedPosition ??= new LambdaCommand(ApiGetPosition);

    private ICommand? _AddNew;
    public ICommand AddNew => _AddNew ??= new LambdaCommand(AddPosition);

    private ICommand? _Delete;
    public ICommand Delete => _Delete ??= new LambdaCommand(UpdatePosition, CanUpdate);

    private ICommand? _Save;

    public ICommand Save => _Save ??= new LambdaCommand(UpdatePosition, CanUpdate);


    #endregion


    #region Логика
    private void GetBack(object p)
    {
        navigationStore.CurrentViewModel = new HomeViewModel(_User, navigationStore);
    }
    private bool CanUpdate(object p) => SelectedPosition != null;

    private async void ApiGetPosition(object p)
    {
        if (_User.Token == null) return;

        Positions = await QueryService.JsonDeserializeWithToken<Position>(_User!.Token, "/pers/position/all", "GET");

        Department = await QueryService.JsonDeserializeWithToken<Departments>(_User!.Token, "/pers/tree/all", "GET");
    }


    private void AddPosition(object p)
    {
        try
        {
            Position position = new()
            {
                Name = "Новая Должность",

            };
            Positions!.Insert(0, position);
            SelectedPosition = position;

        }
        catch (System.Net.WebException ex)
        {
            if (ex.Response != null)
            {
                using StreamReader reader = new(ex.Response.GetResponseStream());
                _ = MessageBox.Show(reader.ReadToEnd(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

    }

    private async void UpdatePosition(object p)
    {
        try
        {
            if (_User.Token == null) return;
            if (SelectedPosition!.Id > 0)
            {
                //await _Api.UpdatePosition(_User.Token, SelectedPosition);
            }
            else
            {
                // Создать новую запись отдела 
                //await _Api.AddPosition(_User.Token, SelectedPosition);
            }
            _ = MessageBox.Show("Данные успешно сохраненны");
        }
        catch (System.Net.WebException ex)
        {
            if (ex.Response != null)
            {
                using StreamReader reader = new(ex.Response.GetResponseStream());
                _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
    #endregion

    public override void Dispose()
    {

        base.Dispose();
    }

}

