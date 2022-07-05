using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace NIPOM.WPF.Commands
{
    internal class DelegateCommand : ICommand
    {
        private Action<object> action;
        private Func<object, bool> function;

        public DelegateCommand(Action<object> action, Func<object, bool> function = null)
        {
            this.action = action;
            this.function = function;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return function?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            action?.Invoke(parameter);
        }
    }
}
