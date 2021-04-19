using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schaken.ViewModel
{
    class BaseCommand : ICommand
    {
        Action actie;

        public BaseCommand(Action Actie)
        {
            actie = Actie;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            actie.Invoke();
        }
    }
}
