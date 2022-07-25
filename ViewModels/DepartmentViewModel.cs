using System.Collections.Generic;
using System.Linq;
namespace AlphaPersonel.ViewModels;
internal class DepartmentViewModel : BaseViewModel
{
    #region Переменные
    // Ссылка на User для того чтобы забрать token
    private readonly Users _user;

    private readonly NavigationStore _navigationStore;
    public DepartmentViewModel(NavigationStore navigationStore, Users user)
    {
        _user = user;
        _navigationStore = navigationStore;

    }

    // Массив отделов
    private ObservableCollection<Departments>? _departments;
    public ObservableCollection<Departments>? Departments
    {
        get => _departments;
        private set
        {
            _ = Set(ref _departments, value);
            if (Departments != null) CollectionDepart = CollectionViewSource.GetDefaultView(Departments);
            if (CollectionDepart != null) CollectionDepart.Filter = FilterToDepart;
        }
    }
    // Тип отдела
    private IEnumerable<TypeDepartments>? _typeDepartments;
    public IEnumerable<TypeDepartments>? TypeDepartments
    {
        get => _typeDepartments;
        set => Set(ref _typeDepartments, value);
    }
    // Выбранные отдел
    private Departments? _selectedDepartment;
    public Departments? SelectedDepartment
    {
        get => _selectedDepartment;
        set => Set(ref _selectedDepartment, value);
    }

    // Поисковая строка для поиска по ФИО сотрудника
    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            _ = Set(ref _filter, value);
            if (Departments != null)
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
        return string.IsNullOrEmpty(Filter) || (emp is Departments dep && dep.Name.ToUpper().Contains(value: Filter.ToUpper()));
    }

    #endregion

    #region Комадны

    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _loadedDepartment;
    public ICommand LoadedDepartment => _loadedDepartment ??= new LambdaCommand(ApiGetDepartments);

    private ICommand? _addNewDepartment;
    public ICommand AddNewDepartment => _addNewDepartment ??= new LambdaCommand(AddDepartmentAsync);

    private ICommand? _deleteDepartment;
    public ICommand DeleteDepartment => _deleteDepartment ??= new LambdaCommand(DeleteDepartments, _ => SelectedDepartment is not null);

    private ICommand? _saveDepartment;
    public ICommand SaveDepartment => _saveDepartment ??= new LambdaCommand(UpdateDepartment, _ => SelectedDepartment is not null && Departments!.Count > 0 && !string.IsNullOrWhiteSpace(SelectedDepartment.Name));
    #endregion

    #region Логика

    private void GetBack(object p)
    {

        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }

    // Выгрузить данные отделов ввиде списка
    private async void ApiGetDepartments(object p)
    {
        try
        {
            TypeDepartments = await QueryService.JsonDeserializeWithToken<TypeDepartments>(_user.Token, "/pers/tree/type/get", "GET");

            Departments = await QueryService.JsonDeserializeWithToken<Departments>(_user.Token, "/pers/tree/all", "GET");

            if(Departments.Count > 0)
            {
                SelectedDepartment = Departments[0];
            }
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
    // Создать новую запись
    private void AddDepartmentAsync(object p)
    {

        var count = Departments!.Where(x => x.Id == 0).ToList().Count;
        if(count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        Departments dep = new()
        {
            Name = "Новый отдел",
            Short = "Новый отдел",
            Phone = "Не указан",
            TypeName = "Не указано",
            ParentId = 0,
        };
        _departments!.Insert(0, dep);
        SelectedDepartment = dep;


    }
    // Изменить или сохранить новые данные
    private async void UpdateDepartment(object p)
    {
        try
        {
            var newSelectedDepartment = SelectedDepartment!;
            if (SelectedDepartment!.Id > 0)
            {
                //Изменить уже текущие данные
                await QueryService.JsonSerializeWithToken(token: _user.Token, "/pers/tree/rename/" + SelectedDepartment.Id, "POST", SelectedDepartment);
            }
            else
            {
                // Создать новую запись отдела 
                await QueryService.JsonSerializeWithToken(token: _user.Token, "/pers/tree/add", "POST", SelectedDepartment);
                // Обновить данные
                //Departments = await QueryService.JsonDeserializeWithToken<Departments>(_user.Token, "/pers/tree/all", "GET");
            }
            //ApiGetDepartments(p);
            Departments = await QueryService.JsonDeserializeWithToken<Departments>(_user.Token, "/pers/tree/all", "GET");
            SelectedDepartment = Departments.FirstOrDefault(x => x.Name == newSelectedDepartment.Name);

            //_ = MessageBox.Show("Данные успешно сохраненны");
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
    // Удалить текущую запись 
    private async void DeleteDepartments(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/tree/del/" + SelectedDepartment!.Id, "DELETE", SelectedDepartment);
            // Удаляем из массива
            _ = _departments!.Remove(SelectedDepartment);
            // Очищаем фильтр
            Filter = string.Empty;
            // Выбираем первый элемент из списка
            SelectedDepartment = Departments!.FirstOrDefault();
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

