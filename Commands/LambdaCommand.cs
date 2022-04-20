namespace AlphaPersonel.Commands;

internal class LambdaCommand : BaseCommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool>? _canExecute;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    public LambdaCommand(/*Func<Task> Execute*/ Action<object> execute, Func<object, bool>? canExecute = null)
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    protected override bool CanExecute(object p) => _canExecute?.Invoke(p) ?? true;


    protected override void Execute(object p) => _execute(p);
}

