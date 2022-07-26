namespace AlphaPersonel.Commands.Base;

internal abstract class AsyncCommands : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    bool ICommand.CanExecute(object? parameter)
    {
        return CanExecute(parameter);
    }
    
    void ICommand.Execute(object? parameter)
    {
        if (((ICommand)this).CanExecute(parameter))
        {
            ExecuteAsync(parameter);
        }
    }
    protected virtual bool CanExecute(object p)
    {
        return true;
    }
    protected abstract Task ExecuteAsync(object p);

}