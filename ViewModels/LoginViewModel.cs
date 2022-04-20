namespace AlphaPersonel.ViewModels;

internal class LoginViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    public LoginViewModel(NavigationStore navigationStore)
    {
        this._navigationStore = navigationStore;
    }

    #region Свойства
    private string? _userName;
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
    public ICommand Auth => _auth ??= new LambdaCommand(OnSignInAsync, CanSignIn);
    #endregion

    #region Логика

    private async void OnSignInAsync(object p)
    {
        try
        {
            var account = await SignIn.Authentication(username: UserName!, password: Password!);

            _navigationStore.CurrentViewModel = new HomeViewModel(account, _navigationStore);
        }
        catch (WebException ex)
        {

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());
                    ErrorMessage = await reader.ReadToEndAsync();
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
    #endregion


}

