namespace AlphaPersonel.ViewModels;
internal class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _NavigationStore;
    public BaseViewModel CurrentViewModel => _NavigationStore!.CurrentViewModel!;

    public MainViewModel(NavigationStore navigationStore)
    {
        _NavigationStore = navigationStore;

        _NavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}

