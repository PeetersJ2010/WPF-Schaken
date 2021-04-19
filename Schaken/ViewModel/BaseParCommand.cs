using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schaken.ViewModel
{
    class BaseParCommand : ICommand
    {
        Action<object> actie;

        public BaseParCommand(Action<object> Actie)
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
            actie.Invoke(parameter);
        }
    }
}
