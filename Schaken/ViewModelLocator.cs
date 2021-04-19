using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schaken.ViewModel;

namespace Schaken
{
    class ViewModelLocator
    {
        // Main
        private static MainViewModel mainViewModel = new MainViewModel();

        public static MainViewModel MainViewModel
        {
            get
            {
                return mainViewModel;
            }
        }

        // Player
        private static PlayerViewModel playerViewModel = new PlayerViewModel();

        public static PlayerViewModel PlayerViewModel
        {
            get
            {
                return playerViewModel;
            }
        }

        // SelectPlayers
        private static SelectPlayersViewModel selectPlayersViewModel = new SelectPlayersViewModel();

        public static SelectPlayersViewModel SelectPlayersViewModel
        {
            get
            {
                return selectPlayersViewModel;
            }
        }

        // Game
        private static GameViewModel gameViewModel = new GameViewModel();

        public static GameViewModel GameViewModel
        {
            get
            {
                return gameViewModel;
            }
        }

        
    }
}
