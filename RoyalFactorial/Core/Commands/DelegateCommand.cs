using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RoyalFactorial.Core.Commands
{
    public class DelegateCommand(Action execute, Func<bool>? canExecute = null) : ICommand
    {
        private readonly Action _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Func<bool>? _canExecute = canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();

        public void Execute(object? parameter) => _execute();
    }

    public class DelegateCommand<T>(Action<T> execute, Func<T, bool>? canExecute = null) : ICommand
    {
        private readonly Action<T> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Func<T, bool>? _canExecute = canExecute;

    public event EventHandler? CanExecuteChanged
    {
        add
        {
            if (_canExecute != null)
                CommandManager.RequerySuggested += value;
        }
        remove
        {
            if (_canExecute != null)
                CommandManager.RequerySuggested -= value;
        }
    }

    public bool CanExecute(object? parameter)
    {
        if (_canExecute == null) return true;
        if (parameter == null && typeof(T).IsValueType) return _canExecute(default!);

        return parameter == null || parameter is T t && _canExecute(t);
    }

    public void Execute(object? parameter)
    {
        if (parameter == null)
        {
            if (typeof(T).IsValueType)
            {
                _execute(default!);
            }
            else
            {
                throw new ArgumentException("Parameter is required.", nameof(parameter));
            }
        }
        else
        {
            _execute((T)parameter);
        }
    }
}
}
