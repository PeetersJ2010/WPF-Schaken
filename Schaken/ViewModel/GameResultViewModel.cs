using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schaken.ViewModel
{
    class GameResultViewModel : BaseViewModel
    {
        private DialogNavigation dialogNavigation;

        private static string resultPlayer1;
        private static string resultPlayer2;
        private static string scorePlayer1;
        private static string scorePlayer2;

        public String ResultPlayer1 { get { return resultPlayer1; } set { resultPlayer1 = value; NotifyPropertyChanged(); } }
        public String ResultPlayer2 { get { return resultPlayer2; } set { resultPlayer2 = value; NotifyPropertyChanged(); } }
        public String ScorePlayer1 { get { return scorePlayer1; } set { scorePlayer1 = value; NotifyPropertyChanged(); } }
        public String ScorePlayer2 { get { return scorePlayer2; } set { scorePlayer2 = value; NotifyPropertyChanged(); } }

        public ICommand ToHomeWindowCommand { get; set; }

        public GameResultViewModel()
        {
            BindCommands();
            dialogNavigation = new DialogNavigation();
        }

        private void BindCommands()
        {
            ToHomeWindowCommand = new BaseCommand(ToHomeWindow);
        }

        private void ToHomeWindow()
        {
            dialogNavigation.ShowMainWindow();
        }
    }
}
