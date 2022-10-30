using Employee_Client.Stores;
using System;
using System.Windows.Input;

namespace Employee_Client.Commands
{
    public abstract class CommandBase : ICommand
    {
        private readonly NavigationStore _navigationStore;
        public event EventHandler? CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);

        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
