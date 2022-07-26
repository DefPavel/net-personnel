using AlphaPersonel.Models;

namespace AlphaPersonel.ViewModels;

internal class LoginViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    public LoginViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;

        if (File.Exists(Path.Combine(AppData, "settings.json"))) return;
        var settings = new Settings { Login = "" };
        using var createStream = File.Create(Path.Combine(AppData, "settings.json"));
        JsonSerializer.Serialize(createStream, settings);
    }

    #region Свойства

    private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        private set => Set(ref _isLoading, value);
    }
    private string? _userName = JsonSerializer.Deserialize<Settings>(File.ReadAllText(Path.Combine(AppData, "settings.json")))?.Login;
    public string? UserName
    {
        get => _userName;
        set => Set(ref _userName, value);
    }
    // Введённый Пароль
    private string? _password;
    public string? Password
    {
        get => _password;
        set => Set(ref _password, value);
    }
    // Сообщение ошибки
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            _ = Set(ref _errorMessage, value);
            OnPropertyChanged(nameof(IsErrorVisible));
        }
    }

    private bool _isRememberMe;
    public bool IsRememberMe
    {
        get => _isRememberMe;
        set
        {
            _ = Set(ref _isRememberMe, value);
            OnPropertyChanged(nameof(IsRememberMe));
        }
    }

    // Свойство отображения уведомления при не валидных данных 
    public bool IsErrorVisible => !string.IsNullOrEmpty(ErrorMessage);
    // Доступность
    private bool CanSignIn(object parameter)
    {
        return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
    }
    #endregion

    #region Команды
    // Команда Авторизации
    private ICommand? _auth;
    // Лямбда Команда 
    public ICommand Auth => _auth ??= new LambdaAsyncCommand(OnSignInAsync, CanSignIn);
    #endregion

    #region Логика

    private async Task OnSignInAsync(object p)
    {
        Users account;
        try
        {
            if(IsRememberMe == true)
            {
                var settings = new Settings { Login = UserName!, Checked = true };
                var settingsText = JsonSerializer.Serialize(settings);
                
                await File.WriteAllTextAsync(Path.Combine(AppData, "settings.json"), settingsText);

            }

            IsLoading = true;
            account = await SignIn.Authentication(
            username: UserName!,
            password: Password!);
            IsLoading = false;

            _navigationStore.CurrentViewModel = new HomeViewModel(account: account,
                                                              navigationStore: _navigationStore);

               
        }
        catch (WebException ex)
        {
            IsLoading = false;
            //TitleButton = "Войти";
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());
                    account = JsonSerializer.Deserialize<Users>(await reader.ReadToEndAsync()) ?? throw new InvalidOperationException();
                    ErrorMessage = account.Error;
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

