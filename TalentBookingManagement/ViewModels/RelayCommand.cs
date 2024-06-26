using System;
using System.Windows.Input;

namespace TalentBookingManagement.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            var canExecute = _canExecute == null || _canExecute();
            Console.WriteLine($"CanExecute: {canExecute}");
            return canExecute;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            Logger.Log("RaiseCanExecuteChanged called");
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
