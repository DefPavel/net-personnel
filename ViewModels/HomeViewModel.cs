namespace AlphaPersonel.ViewModels;

internal class HomeViewModel : BaseViewModel
{
    private readonly Users account;
    private readonly NavigationStore navigationStore;

    public HomeViewModel(Users account, NavigationStore navigationStore)
    {
        this.account = account;
        this.navigationStore = navigationStore;
    }
}

