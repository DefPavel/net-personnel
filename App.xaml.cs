namespace AlphaPersonel;
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        NavigationStore navigationStore = new();

        // Навигация запускает первой формой Авторизацию
        navigationStore.CurrentViewModel = new LoginViewModel(navigationStore);

        MainWindow = new MainWindow()
        {
            DataContext = new MainViewModel(navigationStore)
        };

        MainWindow.Show();

        base.OnStartup(e);
    }
}

