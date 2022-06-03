﻿using AlphaPersonel.Models;

namespace AlphaPersonel.ViewModels;

internal class LoginViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    public LoginViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;

        if (!File.Exists(Path.Combine(appData, "settings.json")))
        {
            var settings = new Settings { Login = "" };
            using FileStream createStream = File.Create(Path.Combine(appData, "settings.json"));
            JsonSerializer.Serialize(createStream, settings);
        }
       _userName = JsonSerializer.Deserialize<Settings>(File.ReadAllText(Path.Combine(appData, "settings.json")))?.Login;
    }

    #region Свойства

    private static readonly string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        private set => Set(ref _isLoading, value);
    }
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
    public ICommand Auth => _auth ??= new LambdaCommand(OnSignInAsync, CanSignIn);
    #endregion

    #region Логика

    private async void OnSignInAsync(object p)
    {
        Users account;
        try
        {
            if(IsRememberMe == true)
            {
                var settings = new Settings { Login = UserName!, Checked = true };
                string settingsText = JsonSerializer.Serialize(settings);
                
                File.WriteAllText(Path.Combine(appData, "settings.json"), settingsText);

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

