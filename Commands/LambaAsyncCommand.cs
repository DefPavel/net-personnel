using AlphaPersonel.Commands.Base;

namespace AlphaPersonel.Commands;

internal class LambdaAsyncCommand : AsyncCommands
{
    private readonly Func<object, Task> _execute;
    private readonly Func<object, bool>? _canExecute;

    public LambdaAsyncCommand(Func<object, Task> execute, Func<object, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }
    protected override bool CanExecute(object p) => _canExecute?.Invoke(p) ?? true;
    protected override async Task ExecuteAsync(object p)
    {
        await _execute(p);
    }
}