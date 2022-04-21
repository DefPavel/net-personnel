namespace AlphaPersonel.ViewModels;

internal class MasterReportViewModel : BaseViewModel
{
    public MasterReportViewModel(NavigationStore navigationStore, Users user)
    {
        _navigationStore = navigationStore;
        _user = user;

        IsPedagogical = new ObservableCollection<PedagogicalPosition> 
        { 
            new PedagogicalPosition
            {
                IdPed = 1,
                IsPed = "Педагогическая",
                Query = "typ_pos.is_ped = true",
            },
            new PedagogicalPosition
            {
                IdPed = 2,
                IsPed = "Не Педагогическая",
                Query = "typ_pos.is_ped = false",
            },
            new PedagogicalPosition
            {
                IdPed = -1,
                IsPed = "Все",
                Query = "",                
            },
        };
    }

    #region Переменные

    private readonly NavigationStore _navigationStore;
    private readonly Users _user;

    private ObservableCollection<PedagogicalPosition>? _isPedagogical;
    public ObservableCollection<PedagogicalPosition>? IsPedagogical
    {
        get => _isPedagogical;
        set => Set(ref _isPedagogical, value);
    }
    private ObservableCollection<Position>? _positions;
    public ObservableCollection<Position>? Positions
    {
        get => _positions;
        set => Set(ref _positions, value);
    }

    private ObservableCollection<PersonReports>? _persons;
    public ObservableCollection<PersonReports>? Persons
    {
        get => _persons;
        set => Set(ref _persons, value);
    }
    private PedagogicalPosition? _selectedIsPed;
    public PedagogicalPosition? SelectedIsPed
    {
        get => _selectedIsPed;
        set => Set(ref _selectedIsPed, value);
    }

    private ObservableCollection<TypeContract>? _typeContracts;
    public ObservableCollection<TypeContract>? TypeContracts
    {
        get => _typeContracts;
        set => Set(ref _typeContracts, value);
    }
    private TypeContract? _selectedContract;
    public TypeContract? SelectedContract
    {
        get => _selectedContract;
        set => Set(ref _selectedContract, value);
    }

    private Position? _selectedPosition;
    public Position? SelectedPosition
    {
        get => _selectedPosition;
        set => Set(ref _selectedPosition, value);
    }

    private DateTime? _dateStart;
    public DateTime? DateStart
    {
        get => _dateStart;
        set => Set(ref _dateStart, value);
    }

    private DateTime? _dateEnd;
    public DateTime? DateEnd
    {
        get => _dateEnd;
        set => Set(ref _dateEnd, value);
    }


    #endregion

    #region Команды

    private ICommand? _getData;
    public ICommand GetData => _getData ??= new LambdaCommand(LoadedContract);

    private ICommand? _getToMain;
    public ICommand GetToMain => _getToMain ??= new LambdaCommand(GetBack);

    private ICommand? _getPosition;
    public ICommand GetPosition => _getPosition ??= new LambdaCommand(LoadedPositions , _ => SelectedIsPed != null);

    private ICommand? _getReport;
    public ICommand GetReport => _getReport ??= new LambdaCommand(Reports);

    #endregion

    #region Логика

    private void GetBack(object obj)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(_user, _navigationStore);
    }

    private async void Reports(object win)
    {
       
        try
        {

                object person = new
                {
                    is_ped = SelectedIsPed?.IdPed,
                    query_ped = SelectedIsPed?.Query,
                    query_position = SelectedPosition != null ? $"typ_pos.name_position = '{SelectedPosition?.Name}'" : null,
                    query_contract = SelectedContract != null ? $"contract.id = {SelectedContract?.Id}" : null,
                    query_date_start = DateStart != null ? 
                    $"p.date_to_working >= '{DateStart.Value.ToString("yyyy-MM-dd")}' and p.date_to_working < '{DateEnd.Value.ToString("yyyy-MM-dd")}'" : null


                };

            //Persons = await QueryService.JsonDeserializeWithObjectAndParam<Persons>(_user!.Token, "reports/pers/master", "POST", person)
            // Отправить запрос
            //await QueryService.JsonSerializeWithToken(_user!.Token, "reports/pers/master", "POST", person);



#pragma warning disable SYSLIB0014
            var req = (HttpWebRequest)WebRequest.Create("http://localhost:8080/api/reports/pers/master"); 
            #pragma warning restore SYSLIB0014 
            req.Method = "POST";   
            req.Headers.Add("auth-token", _user!.Token);
            req.Accept = "application/json";

            await using (StreamWriter streamWriter = new(req.GetRequestStream()))
            {
                req.ContentType = "application/json";
                var param = JsonSerializer.Serialize(person);
                await streamWriter.WriteAsync(param);
                // Записывает тело
                streamWriter.Close();
            }
            using var response = await req.GetResponseAsync();
            await using var responseStream = response.GetResponseStream();
            using StreamReader reader = new(responseStream, Encoding.UTF8);
            Persons = JsonSerializer.Deserialize<ObservableCollection<PersonReports>>(await reader.ReadToEndAsync());
           


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

    private async void LoadedContract(object p)
    {
        try
        {
           TypeContracts = await QueryService.JsonDeserializeWithToken<TypeContract>(_user.Token, "/pers/position/type/contract/", "GET");

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

    private async void LoadedPositions(object p)
    {
        try
        {
            if (p is PedagogicalPosition ped)
            {
                // Выдать все обещежития по Институту
                Positions = await QueryService.JsonDeserializeWithToken<Position>(_user.Token, "/pers/position/get/ped/" + ped.IdPed, "GET");
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

    #endregion

    public override void Dispose()
    {
        base.Dispose();
    }
}