using System.Windows.Input;

namespace AlphaPersonel.Commands;
internal abstract class BaseCommand : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

#pragma warning disable CS8769 // Допустимость значений NULL для ссылочных типов в типе параметра не соответствует реализованному элементу (возможно, из-за атрибутов допустимости значений NULL).
    bool ICommand.CanExecute(object parameter)
#pragma warning restore CS8769 // Допустимость значений NULL для ссылочных типов в типе параметра не соответствует реализованному элементу (возможно, из-за атрибутов допустимости значений NULL).
    {
        return CanExecute(parameter);
    }

#pragma warning disable CS8769 // Допустимость значений NULL для ссылочных типов в типе параметра не соответствует реализованному элементу (возможно, из-за атрибутов допустимости значений NULL).
    void ICommand.Execute(object parameter)
#pragma warning restore CS8769 // Допустимость значений NULL для ссылочных типов в типе параметра не соответствует реализованному элементу (возможно, из-за атрибутов допустимости значений NULL).
    {
        if (((ICommand)this).CanExecute(parameter))
        {
            Execute(parameter);
        }
    }
    protected virtual bool CanExecute(object p)
    {
        return true;
    }

    // Если что для асинхронных методов
    protected abstract void /*Task*/ Execute(object p);

}

