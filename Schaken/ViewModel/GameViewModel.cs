using Schaken.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;


namespace Schaken.ViewModel
{
    class GameViewModel : BaseViewModel
    {
        private DialogNavigation dialogNavigation;

        public static Player playerWhiteTemp;
        public static Player playerBlackTemp;
        public static Gamemode gamemodeTemp;

        private Board board;
        private string turn = "White"; // Black = black's turn, White = White's turn
        private Cell currentCell;
        private ObservableCollection<String> capturedPiecesWhite;
        private ObservableCollection<String> capturedPiecesBlack;
        private ObservableCollection<Color> colors;

        public ObservableCollection<Color> Colors { get { return colors; } set { colors = value; NotifyPropertyChanged(); } }
        public ObservableCollection<String> CapturedPiecesWhite { get { return capturedPiecesWhite; } set { capturedPiecesWhite = value; NotifyPropertyChanged(); } }
        public ObservableCollection<String> CapturedPiecesBlack { get { return capturedPiecesBlack; } set { capturedPiecesBlack = value; NotifyPropertyChanged(); } }

        public Board Board { get { return board; } set { board = value; NotifyPropertyChanged(); } }
        public string Turn { get { return turn; } set { turn = value; NotifyPropertyChanged(); } }
        public Cell CurrentCell { get { return currentCell; } set { currentCell = value; NotifyPropertyChanged(); } }


        public ICommand MoveCommand { get; set; }
        public ICommand ResignCommand { get; set; }
        public ICommand ToGameResultCommand { get; set; }

        public GameViewModel()
        {
            dialogNavigation = new DialogNavigation();
            CapturedPiecesWhite = new ObservableCollection<String>();
            CapturedPiecesBlack = new ObservableCollection<String>();
            Board = new Board(GetLatestBoardID());
            BindCommands();
            LoadColors();
        }

        private void BindCommands()
        {
            MoveCommand = new BaseParCommand(Move);
            ResignCommand = new BaseCommand(Resign);
            ToGameResultCommand = new BaseCommand(ToGameResult);
        }
        private void LoadColors()
        {
            ColorDataService colorDS = new ColorDataService();
            Colors = new ObservableCollection<Color>(colorDS.GetColors());
        }

        private void ToGameResult()
        {
            dialogNavigation.ShowGameResultWindow();
        }

        private void Move(object cellId)
        {
            // Get Cell by Id
            int cId = (int)cellId;
            Cell selectedCell = Board.FirstOrDefault(s => s.Id == cId);

            // Check if selection was already made
            if (CurrentCell != null)
            {
                // Cancel selection
                if (selectedCell == CurrentCell)
                {
                    Board.ClearBoard();
                    CurrentCell = null;
                }
                else
                {
                    if (selectedCell.LegalMove)
                    {
                        if(selectedCell.Piece == "King"){
                            // Determine winning colors
                            foreach (var color in Colors)
                            {
                                if (selectedCell.PieceColor == color.ColorName)
                                {
                                    Board.WinningColor = color;
                                }
                                
                            }
                           
                            // End the game
                            EndGame();
                        }
                        else
                        {
                            if (selectedCell.Piece != "")
                            {
                                if (Turn == "White")
                                {
                                    CapturedPiecesWhite.Add(selectedCell.Piece);
                                    NotifyPropertyChanged("ContentCapturedWhite");
                                }
                                else
                                {
                                    CapturedPiecesBlack.Add(selectedCell.Piece);
                                    NotifyPropertyChanged("ContentCapturedBlack");
                                }
                            }
                            Board.Move(CurrentCell, selectedCell);
                            Board.ClearBoard();
                            CurrentCell = null;
                            
                            SwapTurns();
                        }
                    }
                }
            }
            else
            {
                if (IsTurn(selectedCell.PieceColor))
                {
                    CurrentCell = selectedCell;
                    Board.MarkLegals(CurrentCell); 
                }
            }
        }

        private void Resign() {
            // Determine winning player
            foreach (var color in Colors)
            {
                if (Turn != color.ColorName)
                {
                    Board.WinningColor = color;
                }

            }

            EndGame();
        }

        private void EndGame()
        {
            // Set the selected players and gamemode parameters
            if (gamemodeTemp != null)
            {
                SetParameters();
            }

            // Game over, king was captured
            BoardDataService boardDS = new BoardDataService();
            boardDS.AddMatch(Board);

            // Reload match history
            ViewModelLocator.MatchHistoryViewModel.LoadMatches();

            // Add pieces to database
            foreach (Cell cell in Board)
            {
                if (cell.Occupied)
                {
                    //PieceDataService pieceDS = new PieceDataService();
                    //pieceDS.AddPiece(cell);
                }
            }

            // Pass result messages
            GameResultViewModel grvm = new GameResultViewModel();
            PlayerDataService playerDS = new PlayerDataService();
            if (Board.WinningColor.ColorName == "White")
            {
                grvm.ResultPlayer1 = Board.PlayerWhite.Username + " wins!"; // Winner
                grvm.ScorePlayer1 = Board.PlayerWhite.Rating + " +0";
                grvm.ResultPlayer2 = Board.PlayerBlack.Username + " loses!"; // Loser
                grvm.ScorePlayer2 = Board.PlayerBlack.Rating + " -0";

                if (Board.Gamemode.GamemodeName == "Ranked")
                {
                    grvm.ScorePlayer1 = Board.PlayerWhite.Rating + " +20";
                    grvm.ScorePlayer2 = Board.PlayerBlack.Rating + " -20";
                    // Update rating in DB
                    Board.PlayerWhite.Rating += 20;
                    playerDS.UpdatePlayer(Board.PlayerWhite);
                    Board.PlayerBlack.Rating -= 20;
                    playerDS.UpdatePlayer(Board.PlayerBlack);
                }
            }
            else
            {
                grvm.ResultPlayer1 = Board.PlayerBlack.Username + " wins!"; // Winner
                grvm.ScorePlayer1 = Board.PlayerBlack.Rating + " +0";
                grvm.ResultPlayer2 = Board.PlayerWhite.Username + " loses!"; // Loser
                grvm.ScorePlayer2 = Board.PlayerWhite.Rating + " -0";

                if (board.Gamemode.GamemodeName == "Ranked")
                {
                    grvm.ScorePlayer1 = Board.PlayerBlack.Rating + " +20";
                    grvm.ScorePlayer2 = Board.PlayerWhite.Rating + " -20";
                    // Update rating in DB
                    Board.PlayerBlack.Rating += 20;
                    playerDS.UpdatePlayer(Board.PlayerBlack);
                    Board.PlayerWhite.Rating -= 20;
                    playerDS.UpdatePlayer(Board.PlayerWhite);
                }
            }

            //Reset board
            Board = new Board(GetLatestBoardID());
            CurrentCell = null;
            CapturedPiecesWhite = new ObservableCollection<String>();
            NotifyPropertyChanged("ContentCapturedWhite");
            CapturedPiecesBlack = new ObservableCollection<String>();
            NotifyPropertyChanged("ContentCapturedBlack");
            Turn = "White";


            // Navigate to home
            ToGameResult();
        }

        private void SetParameters()
        {
            Board.PlayerBlack = playerBlackTemp;
            Board.PlayerWhite = playerWhiteTemp;
            Board.Gamemode = gamemodeTemp;

            playerBlackTemp = null;
            playerWhiteTemp = null;
            gamemodeTemp = null;
        }

  
        private bool IsTurn(string currentTurn)
        {
            if (currentTurn == Turn)
            {
                return true;
            }
            return false;
        }

        private void SwapTurns()
        {
            if (Turn == "White")
            {
                Turn = "Black";
            }
            else
            {
                Turn = "White";
            }
        }

        private int GetLatestBoardID()
        {
            BoardDataService boardDS = new BoardDataService();
            return boardDS.GetLastMatchID();
        }

        public string ContentCapturedWhite
        {
            get
            {
                string content = "";
                CapturedPiecesWhite = new ObservableCollection<string>(CapturedPiecesWhite.OrderBy(i => i));
                foreach (var item in CapturedPiecesWhite)
                {
                    content += item + "\n";
                }
                return content;
            }
        }

        public string ContentCapturedBlack
        {
            get
            {
                string content = "";
                CapturedPiecesBlack = new ObservableCollection<string>(CapturedPiecesBlack.OrderBy(i => i));
                foreach (var item in CapturedPiecesBlack)
                {
                    content += item + "\n";
                }
                return content;
            }
        }
    }
}
