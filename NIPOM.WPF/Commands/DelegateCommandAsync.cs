using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NIPOM.WPF.Commands
{
    internal class DelegateCommandAsync : ICommand
    {
        protected Func<object, Task> execute;
        protected Func<object, bool> canExecute;

        private bool isExecuting;
        public bool IsExecuting
        {
            get => isExecuting;
            set
            {
                isExecuting = value;
            }
        }

        public DelegateCommandAsync(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? !IsExecuting;
        }

        public async void Execute(object parameter)
        {
            IsExecuting = true;
            await execute?.Invoke(parameter);
            IsExecuting = false;
        }
    }
}
