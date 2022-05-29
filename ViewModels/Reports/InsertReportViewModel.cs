namespace AlphaPersonel.ViewModels;

internal class InsertReportViewModel : BaseViewModel
{
    private readonly Users _user;

    private readonly string _titleReport;

    private readonly string _url;

    private bool _isLoading = false;
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
    private PedagogicalPosition? _selectedIsPed;
    public PedagogicalPosition? SelectedIsPed
    {
        get => _selectedIsPed;
        set => Set(ref _selectedIsPed, value);
    }
    private DateTime? _dateBegin = DateTime.Now;
    public DateTime? DateBegin
    {
        get => _dateBegin;
        set => Set(ref _dateBegin, value);
    }
    private DateTime? _dateEnd = DateTime.Now.AddDays(1);
    public DateTime? DateEnd
    {
        get => _dateEnd;
        set => Set(ref _dateEnd, value);
    }
   

    public InsertReportViewModel(string url, string titleReport, Users user)
    {
        _url = url;
        _titleReport = titleReport;
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

    private ICommand? _getReport;
    public ICommand GetReport => _getReport ??= new LambdaCommand(Reports , _ => IsLoading != true);

    private async void Reports(object obj)
    {
        if (obj is not Window w) return;
        try
        {
            IsLoading = true;
            if (SelectedIsPed == null)
            {
                _ = MessageBox.Show("Необходимо выбрать элемент!");
                return;
            }
            if(DateBegin == null || DateEnd == null)
            {
                _ = MessageBox.Show("Необходимо выбрать дату!");
                return;
            }
            object person = new
            {
                dateStart = DateBegin!.Value.Date.ToString("yyyy-MM-dd"),
                dateEnd = DateEnd!.Value.Date.ToString("yyyy-MM-dd"),
            };

            // Отправить запрос
            await ReportService.JsonPostWithToken(person, _user!.Token, _url + SelectedIsPed.Query, "POST", _titleReport);
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

    public override void Dispose()
    {
        base.Dispose();
    }

}

