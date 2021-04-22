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
    class PlayerViewModel : BaseViewModel
    {
        private DialogNavigation dialogNavigation;
        private int newId = 0, baseRating = 500;
        private string errorMessage;

        private ObservableCollection<Player> players;
        public ObservableCollection<Player> Players { get { return players; } set { players = value; NotifyPropertyChanged(); }}
        public String ErrorMessage { get { return errorMessage; } set { errorMessage = value; NotifyPropertyChanged(); }}

        public ICommand AddPlayerCommand { get; set; }
        public ICommand ChangePlayerCommand { get; set; }
        public ICommand DeletePlayerCommand { get; set; }
        public ICommand ToHomeWindowCommand { get; set; }

        public PlayerViewModel()
        {
            LoadPlayers();
            BindCommands();
            dialogNavigation = new DialogNavigation();
        }
        private void LoadPlayers()
        {
            PlayerDataService playerDS = new PlayerDataService();
            Players = new ObservableCollection<Player>(playerDS.GetPlayers());
        }
       

        private void BindCommands()
        {
            AddPlayerCommand = new BaseCommand(AddPlayer);
            ChangePlayerCommand = new BaseCommand(ChangePlayer);
            DeletePlayerCommand = new BaseCommand(DeletePlayer);
            // nav
            ToHomeWindowCommand = new BaseCommand(ToHomeWindow);
        }
        private void AddPlayer()
        {
            PlayerDataService playerDS = new PlayerDataService();
            int id;
            ErrorMessage = "";
            bool copyUName = false;
            List<Player> players;

            if (SelectedPlayer == null)
            {
                // Get id of most recent player and add 1
                id = playerDS.GetLastPlayerID() + 1;
                string name = "Player " + id.ToString();

                // Check if username already exists, if so, add 1 to the name to prevent duplicate usernames
                do
                {
                    players = playerDS.GetPlayers();
                    copyUName = false;
                    foreach (var player in players)
                    {
                        if (player.Username == name)
                        {
                            copyUName = true;
                            name = "Player " + (id++).ToString();
                        }
                    }
                } while (copyUName == true);


                // Create a new player instance and add this to the database 
                Player newPlayer = new Player(id, baseRating, name, "Unknown");
                playerDS.InsertPlayer(newPlayer);
            }
            else
            {
                // Check if username already exists
                players = playerDS.GetPlayers();
                foreach (var player in players)
                {
                    if (player.Username == SelectedPlayer.Username)
                    {
                        copyUName = true;
                    }
                }
                if (copyUName)
                {
                    ErrorMessage += "Username already exists. \n";
                }
                else
                {
                    if (SelectedPlayer.RealName == "" || SelectedPlayer.Username == "")
                    {
                        ErrorMessage += "Fill all fields. \n";
                    }
                    else
                    {
                        SelectedPlayer.ID = playerDS.GetLastPlayerID() + 1;
                        playerDS.InsertPlayer(SelectedPlayer);
                    }
                }
            }

            // Refresh
            LoadPlayers();
        }
        private void ChangePlayer()
        {
            if (SelectedPlayer != null)
            {
                PlayerDataService playerDS = new PlayerDataService();
                ErrorMessage = "";
                bool copyUName = false;
                string uName = "";
                
                // Get the current username of the selected item.
                if (playerDS.GetPlayer(SelectedPlayer) != null)
                {
                    uName = playerDS.GetPlayer(SelectedPlayer).Username;
                }

                // Loop through all usernames to check for duplicates, except for when the username equals the current username (uName).
                List<Player> players = playerDS.GetPlayers();
                foreach (var player in players)
                {
                    if (player.Username == SelectedPlayer.Username && SelectedPlayer.Username != uName)
                    {
                        copyUName = true;
                    }
                }

                // If a duplicate was found, print an errormessage.
                if (copyUName)
                {
                    ErrorMessage += "Username already exists. \n";
                }
                else
                {
                    // Check if both fields are not empty else, print an errormessage.
                    if (SelectedPlayer.RealName == "" || SelectedPlayer.Username == "")
                    {
                        ErrorMessage += "Fill all fields. \n";
                    }
                    else
                    {
                        playerDS.UpdatePlayer(SelectedPlayer);

                        // Refresh
                        LoadPlayers();
                    }
                }
            }
        }

        private void DeletePlayer()
        {
            if (SelectedPlayer != null)
            {
                PlayerDataService playerDS = new PlayerDataService();
                playerDS.DeletePlayer(SelectedPlayer);

                //Refresh
                LoadPlayers();
            }
        }

        private Player selectedPlayer;
        public Player SelectedPlayer { get { return selectedPlayer; } set { selectedPlayer = value; NotifyPropertyChanged(); } }

        private void ToHomeWindow()
        {
            dialogNavigation.ShowMainWindow();
        }
    }
}
