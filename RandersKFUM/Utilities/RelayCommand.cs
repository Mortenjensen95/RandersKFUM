using System;
using System.Windows.Input;

namespace RandersKFUM.Utilities
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        // Constructor for RelayCommand
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            execute = execute;
            canExecute = canExecute;
        }

        // Event to notify about command state changes
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Determines if the command can be executed
        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        // Executes the command logic
        public void Execute(object? parameter)
        {
            execute(parameter);
        }

    }
}
