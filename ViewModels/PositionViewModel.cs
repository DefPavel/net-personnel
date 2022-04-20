﻿namespace AlphaPersonel.ViewModels;

internal class PositionViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    private readonly Users _user;

    public PositionViewModel(NavigationStore navigationStore, Users user)
    {
        this._navigationStore = navigationStore;
        _user = user;
    }

    #region Переменные

    // Поисковая строка для поиска по ФИО сотрудника
    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            _ = Set(ref _filter, value);
            if (Positions != null)
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

    private bool FilterToPos(object emp) => string.IsNullOrEmpty(Filter) || (emp is Position pos && pos.Name!.ToUpper().Contains(value: Filter.ToUpper()));


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


    private ObservableCollection<Position>? _position;
    public ObservableCollection<Position>? Positions
    {
        get => _position;
        set
        {
            _ = Set(ref _position, value);
            if (Positions != null) CollectionDepart = CollectionViewSource.GetDefaultView(Positions);
            if (CollectionDepart != null) CollectionDepart.Filter = FilterToPos;
        }
    }
    // Выбранные отдел
    private Position? _selectedPosition;
    public Position? SelectedPosition
    {
        get => _selectedPosition;
        set
        {
            _ = Set(ref _selectedPosition, value);

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

    private ObservableCollection<Departments>? _department;
    public ObservableCollection<Departments>? Department
    {
        get => _department;
        set => Set(ref _department, value);
    }

    #endregion

    #region Команды

    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _loadedPosition;
    public ICommand LoadedPosition => _loadedPosition ??= new LambdaCommand(ApiGetPosition);

    private ICommand? _addNew;
    public ICommand AddNew => _addNew ??= new LambdaCommand(AddPosition);

    private ICommand? _delete;
    public ICommand Delete => _delete ??= new LambdaCommand(DeletePosition, CanUpdate);

    private ICommand? _save;

    public ICommand Save => _save ??= new LambdaCommand(UpdatePosition, CanUpdate);


    #endregion


    #region Логика
    private void GetBack(object p)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }
    private bool CanUpdate(object p) => SelectedPosition != null;

    private async void ApiGetPosition(object p)
    {
        try
        {
            Positions = await QueryService.JsonDeserializeWithToken<Position>(_user!.Token, "/pers/position/all", "GET");

            Department = await QueryService.JsonDeserializeWithToken<Departments>(_user!.Token, "/pers/tree/all", "GET");
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


    private void AddPosition(object p)
    {
        try
        {
            Position position = new()
            {
                Name = "Новая Должность",
                HolidayLimit = 28,
                IsPed = true,

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
            if (SelectedPosition!.Id > 0)
            {
                SelectedPosition.IsPed = RadioIsPed;
                //Изменить уже текущие данные
                await QueryService.JsonSerializeWithToken(token: _user!.Token, "/pers/position/rename", "POST", SelectedPosition);
            }
            else
            {
                SelectedPosition.IsPed = RadioIsPed;
                // Создать новую запись  
                await QueryService.JsonSerializeWithToken(token: _user!.Token, "/pers/position/add", "POST", SelectedPosition);
                // Обновить данные
                Positions = await QueryService.JsonDeserializeWithToken<Position>(_user!.Token, "/pers/position/all", "GET");
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

    private async void DeletePosition(object p)
    {
        try
        {
            if (_user.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/position/del/" + SelectedPosition!.Id, "DELETE", SelectedPosition);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _position!.Remove(SelectedPosition);
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

