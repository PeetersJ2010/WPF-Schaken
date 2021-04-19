using Schaken.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schaken.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private DialogNavigation dialogNavigation;

        public ICommand ToPlayersWindowCommand { get; set; }
        public ICommand ToSelectPlayersWindowCommand { get; set; }
        
        public MainViewModel()
        {
            BindCommands();
            dialogNavigation = new DialogNavigation();
        }

        private void BindCommands()
        {
            ToPlayersWindowCommand = new BaseCommand(ToPlayersWindow);
            ToSelectPlayersWindowCommand = new BaseCommand(ToSelectPlayersWindow);
        }

        private void ToPlayersWindow()
        {
            dialogNavigation.ShowPlayersWindow();
        }

        private void ToSelectPlayersWindow()
        {
            dialogNavigation.ShowSelectPlayersWindow();
        }
    }
}
