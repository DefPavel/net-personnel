namespace AlphaPersonel.Services;
internal class NavigationStore
{
    public event Action? CurrentViewModelChanged;

    private BaseViewModel? _CurrentViewModel;
    public BaseViewModel? CurrentViewModel
    {
        get => _CurrentViewModel;
        set
        {
            _CurrentViewModel?.Dispose();
            _CurrentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }
    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}

