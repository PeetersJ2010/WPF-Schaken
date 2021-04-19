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
    class SelectPlayersViewModel : BaseViewModel
    {
        private DialogNavigation dialogNavigation;
        private ObservableCollection<Player> players;
        private ObservableCollection<Gamemode> gamemodes;

        private Player selectedPlayerWhite, selectedPlayerBlack;
        private Gamemode selectedGamemode;

        private string errorMessage;

        public ObservableCollection<Player> Players { get { return players; } set { players = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Gamemode> Gamemodes { get { return gamemodes; } set { gamemodes = value; NotifyPropertyChanged(); } }

        public Player SelectedPlayerWhite { get { return selectedPlayerWhite; } set { selectedPlayerWhite = value; NotifyPropertyChanged(); } }
        public Player SelectedPlayerBlack { get { return selectedPlayerBlack; } set { selectedPlayerBlack = value; NotifyPropertyChanged(); } }
        public Gamemode SelectedGamemode { get { return selectedGamemode; } set { selectedGamemode = value; NotifyPropertyChanged(); } }

        public string ErrorMessage { get { return errorMessage; } set { errorMessage = value; NotifyPropertyChanged(); } }

        public ICommand ToGameWindowCommand { get; set; }

        public SelectPlayersViewModel()
        {
            dialogNavigation = new DialogNavigation();
            ToGameWindowCommand = new BaseCommand(ToGameWindow);
            LoadPlayers();
            LoadGamemodes();
        }
        private void ToGameWindow()
        {
            ErrorMessage = "";
            if (SelectedPlayerWhite == null || SelectedPlayerBlack == null || SelectedGamemode == null)
            {
                ErrorMessage += "Zorg dat elk veld ingevuld is! \n";
            }
            else {
                if (SelectedPlayerWhite.Username == SelectedPlayerBlack.Username)
                {
                    ErrorMessage += "Selecteer twee verschillende spelers! \n";
                }
                else
                {
                    GameViewModel.playerWhiteTemp = SelectedPlayerWhite;
                    GameViewModel.playerBlackTemp = SelectedPlayerBlack;
                    GameViewModel.gamemodeTemp = SelectedGamemode;
                    dialogNavigation.ShowGameWindow();
                }
            }
        }
        private void LoadPlayers()
        {
            PlayerDataService playerDS = new PlayerDataService();
            Players = new ObservableCollection<Player>(playerDS.GetPlayers());
        }
        private void LoadGamemodes()
        {
            GamemodeDataService gamemodeDS = new GamemodeDataService();
            Gamemodes = new ObservableCollection<Gamemode>(gamemodeDS.GetGamemodes());
        }
    }
}
