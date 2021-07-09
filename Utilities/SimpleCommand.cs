using System;
using System.Windows.Input;

namespace net_speed_indicator.Utilities
{
    public class SimpleCommand : ICommand
    {
        private readonly Action _execute;

        public SimpleCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}
