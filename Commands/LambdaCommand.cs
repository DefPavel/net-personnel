namespace AlphaPersonel.Commands;

internal class LambdaCommand : BaseCommand
{
    private readonly Action<object> _Execute;
    private readonly Func<object, bool> _CanExecute;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    public LambdaCommand(/*Func<Task> Execute*/ Action<object> execute, Func<object, bool>? canExecute = null)
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    {
        _Execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _CanExecute = canExecute;
    }

    protected override bool CanExecute(object p) => _CanExecute?.Invoke(p) ?? true;


    protected override void Execute(object p) => _Execute(p);
}

