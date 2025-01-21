using System;
using System.Windows.Input;

namespace RunTracker.Command
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        // Constructor to accept the action and an optional condition for CanExecute
        public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = canExecute;
        }

        // Raises CanExecuteChanged so the UI can refresh whether the command can execute
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        // CanExecute method which checks if the command is executable
        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        // Execute method that triggers the action passed to the constructor
        public void Execute(object? parameter) => _execute(parameter);
    }
}
