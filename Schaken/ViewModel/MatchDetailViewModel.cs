using Schaken.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schaken.ViewModel
{
    class MatchDetailViewModel : BaseViewModel
    {
        private DialogNavigation dialogNavigation;
        private Board board;

        public Board Board { get { return board; } set { board = value; NotifyPropertyChanged(); } }
        public ICommand ToMatchHistoryWindowCommand { get; set; }


        public MatchDetailViewModel()
        {
            BindCommands();
            dialogNavigation = new DialogNavigation();
        }

        private void BindCommands()
        {
            ToMatchHistoryWindowCommand = new BaseCommand(ToMatchHistoryWindow);
        }

        private void ToMatchHistoryWindow()
        {
            dialogNavigation.ShowMatchHistoryWindow();
        }
    }
}
