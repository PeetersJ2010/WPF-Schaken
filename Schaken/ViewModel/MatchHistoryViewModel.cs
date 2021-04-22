using Schaken.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schaken.ViewModel
{
    class MatchHistoryViewModel : BaseViewModel
    {
        private DialogNavigation dialogNavigation;
        private ObservableCollection<Board> matches;
        private ObservableCollection<Player> players;
        private ObservableCollection<Color> colors;
        private Board selectedBoard;
        private string errorMessage;

        public ObservableCollection<Board> Matches { get { return matches; } set { matches = value; NotifyPropertyChanged(); } }  
        public ObservableCollection<Player> Players { get { return players; } set { players = value; NotifyPropertyChanged(); } }  
        public ObservableCollection<Color> Colors { get { return colors; } set { colors = value; NotifyPropertyChanged(); } }  
        public Board SelectedBoard { get { return selectedBoard; } set { selectedBoard = value; NotifyPropertyChanged(); } }
        public String ErrorMessage { get { return errorMessage; } set { errorMessage = value; NotifyPropertyChanged(); } }

        public ICommand ToHomeWindowCommand { get; set; }
        public ICommand AddMatchCommand { get; set; }


        public MatchHistoryViewModel()
        {
            LoadMatches();
            LoadPlayers();
            LoadColors();
            BindCommands();
            dialogNavigation = new DialogNavigation();
        }
        public void LoadMatches()
        {
            BoardDataService boardDS = new BoardDataService();
            Matches = new ObservableCollection<Board>(boardDS.GetMatches());
        }
        public void LoadPlayers()
        {
            PlayerDataService playerDS = new PlayerDataService();
            Players = new ObservableCollection<Player>(playerDS.GetPlayers());
        }

        private void LoadColors()
        {
            ColorDataService colorDS = new ColorDataService();
            Colors = new ObservableCollection<Color>(colorDS.GetColors());
        }

        private void BindCommands()
        {
            ToHomeWindowCommand = new BaseCommand(ToHomeWindow);
            AddMatchCommand = new BaseCommand(AddMatch);
        }

        private void AddMatch()
        {
            BoardDataService boardDS = new BoardDataService();
            ErrorMessage = "";
            int id = boardDS.GetLastMatchID() + 1;

            if (SelectedBoard == null)
            {
                // Check if 2 or more players are available
                LoadPlayers();
                if (Players.Count >= 2)
                {
                    // Create a new board instance and add this to the database 
                    Board newBoard = new Board(id, Players[0].ID, Players[1].ID, 0, 0 );
                    boardDS.AddMatch(newBoard);
                }
                else
                {
                    ErrorMessage = "Atleast 2 players have to exist!";
                }
            }
            else
            {
                if (SelectedBoard.PlayerWhite == SelectedBoard.PlayerBlack)
                {
                    ErrorMessage += "Select 2 different players! \n";
                }
                else
                {
                    SelectedBoard.ID = id;
                    boardDS.AddMatch(SelectedBoard);
                }
            }

            // Refresh
            LoadMatches();
        }

        private void ToHomeWindow()
        {
            dialogNavigation.ShowMainWindow();
        }
    }
}
