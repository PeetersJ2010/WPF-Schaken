using Schaken.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Schaken
{
    class DialogNavigation
    {
        private Window mainWindow = null;
        private Window playersWindow = null;
        private Window gameWindow = null;
        private Window selectPlayersWindow = null;
        private Window gameResultWindow = null;
        private Window matchHistoryWindow = null;

        public DialogNavigation()
        {

        }

        public void ShowMainWindow()
        {
            mainWindow = new MainWindow();
            Application.Current.Windows[0].Close();
            mainWindow.ShowDialog();
        }

        public void ShowMatchHistoryWindow()
        {
            matchHistoryWindow = new MatchHistoryWindow();
            Application.Current.Windows[0].Close();
            matchHistoryWindow.ShowDialog();
        }

        public void ShowPlayersWindow()
        {
            playersWindow = new PlayersWindow();
            Application.Current.Windows[0].Close();
            playersWindow.ShowDialog();
        }

        public void ShowSelectPlayersWindow()
        {
            selectPlayersWindow = new SelectPlayersWindow();
            Application.Current.Windows[0].Close();
            selectPlayersWindow.ShowDialog();
        }

        public void ShowGameWindow()
        {
            gameWindow = new PlayWindow();
            Application.Current.Windows[0].Close();
            gameWindow.ShowDialog();
        }

        public void ShowGameResultWindow()
        {
            gameResultWindow = new GameResultWindow();
            Application.Current.Windows[0].Close();
            gameResultWindow.ShowDialog();
        }
    }
}
