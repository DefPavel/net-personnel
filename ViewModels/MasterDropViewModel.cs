﻿namespace AlphaPersonel.ViewModels;

internal class MasterDropViewModel : BaseViewModel
{
    private readonly Users _user;

    private ObservableCollection<PedagogicalPosition>? _isPedagogical;
    public ObservableCollection<PedagogicalPosition>? IsPedagogical
    {
        get => _isPedagogical;
        private set => Set(ref _isPedagogical, value);
    }
    private PedagogicalPosition? _selectedIsPed;
    public PedagogicalPosition? SelectedIsPed
    {
        get => _selectedIsPed;
        set => Set(ref _selectedIsPed, value);
    }

    private ObservableCollection<string>? _typePersons;
    public ObservableCollection<string>? TypePersons
    {
        get => _typePersons;
        private set => Set(ref _typePersons, value);
    }
    private string? _selectedTypePerson;
    public string? SelectedTypePerson
    {
        get => _selectedTypePerson;
        set => Set(ref _selectedTypePerson, value);
    }


    private ObservableCollection<Persons>? _persons;
    public ObservableCollection<Persons>? Persons
    {
        get => _persons;
        set
        {
            _ = Set(ref _persons, value);
            // Для фильтрации полей 
            if (Persons == null) return;
            CollectionPerson = CollectionViewSource.GetDefaultView(Persons);
            CollectionPerson.Filter = FilterToPerson;
        }
    }
    private ObservableCollection<Persons> _dropPersons = new();
    public ObservableCollection<Persons> DroPersons
    {
        get => _dropPersons;
        set
        {
            _ = Set(ref _dropPersons, value);
        }
    }
    private bool FilterToPerson(object emp)
    {
        return string.IsNullOrEmpty(FilterPerson) || (emp is Persons per && per.FirstName.ToUpper().Contains(value: FilterPerson.ToUpper()));
    }

    // Выбранный сотрудник 
    private Persons? _selectedPerson;
    public Persons? SelectedPerson
    {
        get => _selectedPerson;
        set => Set(ref _selectedPerson, value);
    }
    private Persons? _selectedDropPerson;
    public Persons? SelectedDropPerson
    {
        get => _selectedDropPerson;
        set => Set(ref _selectedDropPerson, value);
    }
    // Поисковая строка для поиска по ФИО сотрудника
    private string? _filterPerson;
    public string? FilterPerson
    {
        get => _filterPerson;
        set
        {
            _ = Set(ref _filterPerson, value);
            if (Persons != null)
            {
                CollectionPerson!.Refresh();
            }
        }
    }
    // Специальная колекция для фильтров
    private ICollectionView? _collectionPerson;
    public ICollectionView? CollectionPerson
    {
        get => _collectionPerson;
        private set => Set(ref _collectionPerson, value);
    }

    // Массив Приказов
    private ObservableCollection<Order>? _orders;
    public ObservableCollection<Order>? Orders
    {
        get => _orders;
        private set => Set(ref _orders, value);
    }
    // Выбранный приказ
    private Order? _selectedOrders;
    public Order? SelectedOrders
    {
        get => _selectedOrders;
        set => Set(ref _selectedOrders, value);
    }
    private DateTime? _deleteWorking;
    public DateTime? DaleteWorking
    {
        get => _deleteWorking;
        set => Set(ref _deleteWorking, value);
    }
    private readonly NavigationStore _navigationStore;

    public MasterDropViewModel(NavigationStore navigationStore, Users user)
    {
        _navigationStore = navigationStore;
        _user = user;

        IsPedagogical = new ObservableCollection<PedagogicalPosition>
        {
            new()
            {
                IdPed = 1,
                IsPed = "Педагогическая",
                Query = "IsPed",
            },
            new()
            {
                IdPed = 2,
                IsPed = "Не Педагогическая",
                Query = "IsNoPed",
            }
        };

        TypePersons = new ObservableCollection<string>
        {
          "Штатные сотрудники",
          "Внутренние совместители",
          "Внешние совместители",
        };
    }

    #region Команды

    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _addPerson;
    public ICommand AddPerson => _addPerson ??= new LambdaCommand(AddDropPerson , _ => SelectedPerson != null);

    private ICommand? _deletePerson;
    public ICommand DeletePerson => _deletePerson ??= new LambdaCommand(DeleteDropPerson, _ => SelectedDropPerson != null);


    private ICommand? _getData;
    public ICommand GetData => _getData ??= new LambdaCommand(LoadedApi);

    private ICommand? _loadedPerson;
    public ICommand LoadedPerson => _loadedPerson ??= new LambdaCommand(LoadedPersons ,_ => SelectedTypePerson != null);
    #endregion

    private async void LoadedPersons(object p)
    {
        try
        {
            if (SelectedTypePerson == "Штатные сотрудники")
            {
                Persons = await QueryService.JsonDeserializeWithToken<Persons>(_user.Token, "/pers/person/get/is_main", "GET");
            }
            if (SelectedTypePerson == "Внутренние совместители")
            {
                Persons = await QueryService.JsonDeserializeWithToken<Persons>(_user.Token, "/pers/person/get/in_pluralist", "GET");
            }
            if (SelectedTypePerson == "Внешние совместители")
            {
                Persons = await QueryService.JsonDeserializeWithToken<Persons>(_user.Token, "/pers/person/get/out_pluralist", "GET");
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

    private void GetBack(object p)
    {

        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }

    private void AddDropPerson(object p)
    {
        try
        {
            if (SelectedPerson != null)
            {
                DroPersons.Add(SelectedPerson!);
                Persons.Remove(SelectedPerson);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
     
    }
    private void DeleteDropPerson(object p)
    {
        try
        {
            if (SelectedDropPerson != null)
            {
                DroPersons.Remove(SelectedDropPerson!);
                Persons.Add(SelectedDropPerson);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
      
    }

    private async void LoadedApi(object p)
    {

        try
        {
            var idTypeOrder = await QueryService.JsonDeserializeWithObjectAndParam(_user.Token, "/pers/order/type/name", "POST", new TypeOrder { Name = "Увольнение" });
            // Загрузка приказов
            Orders = await QueryService.JsonDeserializeWithToken<Order>(_user.Token, "/pers/order/get/" + idTypeOrder.Id, "GET");

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
}

