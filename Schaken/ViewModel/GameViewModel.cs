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
using System.Windows.Media;

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

        public ObservableCollection<String> CapturedPiecesWhite { get { return capturedPiecesWhite; } set { capturedPiecesWhite = value; NotifyPropertyChanged(); } }
        public ObservableCollection<String> CapturedPiecesBlack { get { return capturedPiecesBlack; } set { capturedPiecesBlack = value; NotifyPropertyChanged(); } }
        public Board Board { get { return board; } set { board = value; NotifyPropertyChanged(); } }
        public string Turn { get { return turn; } set { turn = value; NotifyPropertyChanged(); } }
        public Cell CurrentCell { get { return currentCell; } set { currentCell = value; NotifyPropertyChanged(); } }


        public ICommand MoveCommand { get; set; }

        public GameViewModel()
        {
            CapturedPiecesWhite = new ObservableCollection<String>();
            CapturedPiecesBlack = new ObservableCollection<String>();
            Board = new Board(GetLatestBoardID());
            BindCommands();
        }

        private void BindCommands()
        {
            MoveCommand = new BaseParCommand(Move);
        }

        private void Move(object cellId)
        {
            // Set the selected players and gamemode parameters
            if (gamemodeTemp != null)
            {
                SetParameters();
            }

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
                            if (selectedCell.PieceColor == "Black")
                            {
                                board.WinningPlayer = "White";
                                board.LosingPlayer = "Black";
                            }
                            else
                            {
                                board.WinningPlayer = "Black";
                                board.LosingPlayer = "White";
                            }

                            // Game over, king was captured
                            BoardDataService boardDS = new BoardDataService();
                            boardDS.AddMatch(Board);

                            // Add pieces to database
                            foreach (Cell cell in board)
                            {
                                if (cell.Occupied)
                                {
                                    PieceDataService pieceDS = new PieceDataService();
                                    pieceDS.AddPiece(cell);
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
                            dialogNavigation = new DialogNavigation();
                            dialogNavigation.ShowMainWindow();
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

        private void SetParameters()
        {
            board.PlayerBlack = playerBlackTemp;
            board.PlayerWhite = playerWhiteTemp;
            board.Gamemode = gamemodeTemp;

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
