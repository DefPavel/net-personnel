using System.IO;
using System.Net;
using System.Windows.Input;

namespace AlphaPersonel.ViewModels;

internal class LoginViewModel : BaseViewModel
{
    private readonly NavigationStore navigationStore;

    public LoginViewModel(NavigationStore navigationStore)
    {
        this.navigationStore = navigationStore;
    }

    #region Свойства
    private string? _UserName;
    public string? UserName
    {
        get => _UserName;
        set => Set(ref _UserName, value);
    }
    // Введённый Пароль
    private string? _Password;
    public string? Password
    {
        get => _Password;
        set => Set(ref _Password, value);
    }
    // Сообщение ошибки
    private string? _ErrorMessage;
    public string? ErrorMessage
    {
        get => _ErrorMessage;
        private set
        {
            _ = Set(ref _ErrorMessage, value);
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
    private ICommand? _Auth;
    // Лямбда Команда 
    public ICommand Auth => _Auth ??= new LambdaCommand(OnSignInAsync, CanSignIn);
    #endregion

    #region Логика

    private async void OnSignInAsync(object p)
    {
        Users account;
        try
        {
            account = await SignIn.Authentication(username: UserName!, password: Password!);

            if (account.StatusCode == HttpStatusCode.OK)
            {
                navigationStore.CurrentViewModel = new HomeViewModel(account, navigationStore);
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
                        account = JsonSerializer.Deserialize<Users>(await reader.ReadToEndAsync())
                            ?? throw new NullReferenceException();

                        ErrorMessage = account!.Error!.Ru;
                    }
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

