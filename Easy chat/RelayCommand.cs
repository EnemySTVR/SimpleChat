using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Easy_chat
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Predicate<object> canExecute;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public RelayCommand(Action<object> execute)
        : this(execute, null)
        {
        }
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute((object)parameter);
        }
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }

}
