
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Input;

namespace AlphaPersonel.ViewModels;

internal class DepartmentViewModel : BaseViewModel
{
    #region Переменные
    // Ссылка на User для того чтобы забрать token
    private readonly Users _User;

    private readonly NavigationStore navigationStore;
    public DepartmentViewModel(NavigationStore navigationStore, Users user)
    {
        _User = user;
        this.navigationStore = navigationStore;

    }

    // Массив отделов
    private ObservableCollection<Departments>? _Departments;
    public ObservableCollection<Departments>? Departments
    {
        get => _Departments;
        set
        {
            _ = Set(ref _Departments, value);
            CollectionDepart = CollectionViewSource.GetDefaultView(Departments);
            CollectionDepart.Filter = FilterToDepart;
        }
    }
    // Тип отдела
    private ObservableCollection<TypeDepartments>? _TypeDepartments;
    public ObservableCollection<TypeDepartments>? TypeDepartments
    {
        get => _TypeDepartments;
        set => Set(ref _TypeDepartments, value);
    }
    // Выбранные отдел
    private Departments? _SelectedDepartment;
    public Departments? SelectedDepartment
    {
        get => _SelectedDepartment;
        set => Set(ref _SelectedDepartment, value);
    }

    // Поисковая строка для поиска по ФИО сотрудника
    private string? _Filter;
    public string? Filter
    {
        get => _Filter;
        set
        {
            _ = Set(ref _Filter, value);
            if (Departments != null)
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
        return string.IsNullOrEmpty(Filter) || (emp is Departments dep && dep.Name!.ToUpper().Contains(value: Filter.ToUpper()));
    }

    #endregion

    #region Комадны

    private ICommand? _GetToMain;
    public ICommand GetToMain => _GetToMain ??= new LambdaCommand(GetBack);

    private ICommand? _LoadedDepartment;
    public ICommand LoadedDepartment => _LoadedDepartment ??= new LambdaCommand(ApiGetDepartments);

    private ICommand? _AddNewDepartment;
    public ICommand AddNewDepartment => _AddNewDepartment ??= new LambdaCommand(AddDepartmentAsync);

    private ICommand? _DeleteDepartment;
    public ICommand DeleteDepartment => _DeleteDepartment ??= new LambdaCommand(DeleteDepartments, CanUpdateDepartmentCommandExecute);

    private ICommand? _SaveDepartment;
    public ICommand SaveDepartment => _SaveDepartment ??= new LambdaCommand(UpdateDepartment, CanUpdateDepartmentCommandExecute);
    #endregion

    #region Логика

    private bool CanUpdateDepartmentCommandExecute(object p)
    {
        return p is Departments && p is not null;
    }

    private void GetBack(object p)
    {

        navigationStore.CurrentViewModel = new HomeViewModel(_User, navigationStore);
    }

    // Выгрузить данные отделов ввиде списка
    private async void ApiGetDepartments(object p)
    {
        try
        {
            if (_User.Token != null)
            {
                TypeDepartments = await QueryService.JsonDeserializeWithToken<TypeDepartments>(_User!.Token, "/pers/tree/type/get", "GET");

                Departments = await QueryService.JsonDeserializeWithToken<Departments>(_User!.Token, "/pers/tree/all", "GET");
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
    // Создать новую запись
    private async void AddDepartmentAsync(object p)
    {
        try
        {
            Departments dep = new()
            {
                Name = "Новый отдел",
                Short = "Новый отдел",
                Phone = "Не указан",
                TypeName = "Не указано",
                ParentId = 0,
            };
            _Departments!.Insert(0, dep);
            SelectedDepartment = dep;

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
    // Изменить или сохранить новые данные
    private async void UpdateDepartment(object p)
    {
        try
        {
            if (_User.Token == null)
            {
                return;
            }

            if (SelectedDepartment!.Id > 0)
            {
                //Изменить уже текущие данные
                await QueryService.JsonSerializeWithToken(token: _User!.Token, "/pers/tree/rename/" + SelectedDepartment.Id, "POST", SelectedDepartment);
            }
            else
            {
                // Создать новую запись отдела 
                await QueryService.JsonSerializeWithToken(token: _User!.Token, "/pers/tree/add", "POST", SelectedDepartment);
                // Обновить данные
                Departments = await QueryService.JsonDeserializeWithToken<Departments>(_User.Token, "/pers/tree/all", "GET");
            }
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
    // Удалить текущую запись 
    private async void DeleteDepartments(object p)
    {
        try
        {
            if (_User.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await QueryService.JsonSerializeWithToken(_User.Token, "/pers/tree/del/" + SelectedDepartment!.Id, "DELETE", SelectedDepartment);
                //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

                _ = _Departments!.Remove(SelectedDepartment);
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

