using System.Windows.Input;

namespace Library.Common.Mvvm;

public class DelegateCommand : ICommand
{
    private readonly Func<bool>? _canExecute;
    private readonly Action _execute;

    /// <summary>
    /// Initializes a new instance of the DelegateCommand class
    /// </summary>
    /// <param name="execute">execute function </param>
    /// <param name="canExecute">can execute function</param>
    public DelegateCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    /// <summary>
    /// can executes event handler
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// implement of icommand can execute method
    /// </summary>
    /// <param name="parameter">parameter by default of icomand interface</param>
    /// <returns>can execute or not</returns>
    public bool CanExecute(object? parameter) => _canExecute is null || _canExecute();

    /// <summary>
    /// implement of icommand interface execute method
    /// </summary>
    /// <param name="parameter">parameter by default of icomand interface</param>

    public void Execute(object? parameter) => _execute();

    /// <summary>
    /// raise can excute changed when property changed
    /// </summary>
    public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}