using System.Collections.Generic;

namespace AlphaPersonel.ViewModels;

internal class ReportVacationViewModel : BaseViewModel
{
    
    private readonly Users _user;
    private readonly string _url;
    private readonly string _title;
    
    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        private set => Set(ref _isLoading, value);
    }
    
    private ObservableCollection<PedagogicalPosition>? _isPedagogical;
    public ObservableCollection<PedagogicalPosition>? IsPedagogical
    {
        get => _isPedagogical;
        private set => Set(ref _isPedagogical, value);
    }
    
    private IEnumerable<PeriodVacation>? _periodVacation;
    public IEnumerable<PeriodVacation>? PeriodVacations
    {
        get => _periodVacation;
        private set => Set(ref _periodVacation, value);
    }
    
    private PedagogicalPosition? _selectedIsPed;
    public PedagogicalPosition? SelectedIsPed
    {
        get => _selectedIsPed;
        set => Set(ref _selectedIsPed, value);
    }
    
    private PeriodVacation? _selectedPeriodVacation;
    public PeriodVacation? SelectedPeriodVacation
    {
        get => _selectedPeriodVacation;
        set => Set(ref _selectedPeriodVacation, value);
    }
    
   
   

    public ReportVacationViewModel(string url, string title, Users user)
    {
        _url = url;
        _title = title;
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
            },
            new()
            {
                IdPed = -1,
                IsPed = "Все",
                Query = "IsAll",
            },
        };
    }

    private ICommand? _getData;
    public ICommand GetData => _getData ??= new LambdaCommand(LoadedApi);
    
    private ICommand? _getReport;
    public ICommand GetReport => _getReport ??= new LambdaCommand(Reports);
    
    
    private async void Reports(object obj)
    {
        if (obj is not Window w) return;
        try
        {
            IsLoading = true;
            if (SelectedIsPed == null)
            {
                _ = MessageBox.Show("Необходимо выбрать тип!");
                return;
            }
            
            if (SelectedPeriodVacation == null)
            {
                _ = MessageBox.Show("Необходимо выбрать период!");
                return;
            }
            
            object person = new
            {
                id_period = SelectedPeriodVacation.Id
            };

            // Отправить запрос
            await ReportService.JsonPostWithToken(person, _user!.Token, _url + SelectedIsPed.Query, "POST", _title);
            IsLoading = false;
            w.DialogResult = true;
            w.Close();
        }
        catch (WebException ex)
        {
            IsLoading = false;
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
    
    private async void LoadedApi(object p)
    {
        try
        {
            // Загрузка периодов
            PeriodVacations = await QueryService.JsonDeserializeWithToken<PeriodVacation>(_user.Token, "/pers/vacation/period/get", "GET");
           
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