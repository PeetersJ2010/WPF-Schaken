using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Schaken.Model
{
    class Board : ObservableCollection<Cell>
    {
        private int id;
        private Player playerBlack;
        private Player playerWhite;
        private Gamemode gamemode;
        private Color winningColor;
        private ObservableCollection<Player> players;
        private ObservableCollection<Gamemode> gamemodes;
        private ObservableCollection<Color> colors;

        public int ID { get { return id; } set { id = value; } }
        public Player PlayerBlack { get { return playerBlack; } set { playerBlack = value; } }
        public Player PlayerWhite { get { return playerWhite; } set { playerWhite = value; } }
        public Gamemode Gamemode { get { return gamemode; } set { gamemode = value; } }
        public Color WinningColor { get { return winningColor; } set { winningColor = value; } }
        public ObservableCollection<Player> Players { get { return players; } set { players = value; } }
        public ObservableCollection<Gamemode> Gamemodes { get { return gamemodes; } set { gamemodes = value; } }
        public ObservableCollection<Color> Colors { get { return colors; } set { colors = value; } }


        public Board(int id)
        {
            ID = id;
            CreateBoard();
        }

        public Board(int id, int playerBlackID, int playerWhiteID, int gamemodeID, int winningColorID)
        {
            ID = id;

            PlayerDataService playerDS = new PlayerDataService();
            Players = new ObservableCollection<Player>(playerDS.GetPlayers());
            foreach (var player in Players)
            {
                if (player.ID == playerBlackID)
                {
                    PlayerBlack = player;
                }
                if (player.ID == playerWhiteID)
                {
                    PlayerWhite = player;
                }
            }

            GamemodeDataService gamemodeDS = new GamemodeDataService();
            Gamemodes = new ObservableCollection<Gamemode>(gamemodeDS.GetGamemodes());
            foreach (var gamemode in Gamemodes)
            {
                if (gamemode.ID == gamemodeID)
                {
                    Gamemode = gamemode;
                }
            }

            ColorDataService colorDS = new ColorDataService();
            Colors = new ObservableCollection<Color>(colorDS.GetColors());
            foreach (var color in colors)
            {
                if (color.ID == winningColorID)
                {
                    WinningColor = color;
                }
            }
        }

        public void CreateBoard()
        {
            string color_White = "#f2f2f2"; // #f2f2f2 -> White
            string color_Black = "#696969"; // #000000 -> Black
            string colorBG = color_White;
                
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    this.Add(new Cell((int.Parse(i.ToString() + j.ToString())), j, i, (SolidColorBrush)(new BrushConverter().ConvertFrom(colorBG))));
                    colorBG = colorBG == color_White ? color_Black : color_White;
                }
                colorBG = colorBG == color_White ? color_Black : color_White;
            }
        }

        // Method to recreate the state of a previous board
        public void ReCreateBoard(Board board)
        {
            PieceDataService pieceDS = new PieceDataService();
            List<Cell> pieceList = pieceDS.GetBoardPieces(board);

            string color_White = "#f2f2f2"; // #f2f2f2 -> White
            string color_Black = "#696969"; // #000000 -> Black
            string colorBG = color_White;
            bool o;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    o = false;
                    foreach (var piece in pieceList)
                    {
                        if (piece.ColumnNumber == i && piece.RowNumber == j)
                        {
                            this.Add(new Cell(i, j, colorBG, piece));
                            o = true;
                        }
                    }
                    if (!o)
                    {
                        this.Add(new Cell(i, j, colorBG));
                    }
                    colorBG = colorBG == color_White ? color_Black : color_White;
                }
                colorBG = colorBG == color_White ? color_Black : color_White;
            }
        }

        // Resets all cell fill colors
        public void ClearBoard()
        {
            string color_White = "#f2f2f2"; // #f2f2f2 -> White
            string color_Black = "#696969"; // #000000 -> Black
            string colorBG = color_White;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    this.FirstOrDefault(s => s.ColumnNumber == j && s.RowNumber == i).FillColor = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorBG));
                    this.FirstOrDefault(s => s.ColumnNumber == j && s.RowNumber == i).LegalMove = false;
                    colorBG = colorBG == color_White ? color_Black : color_White;
                }
                colorBG = colorBG == color_White ? color_Black : color_White;
            }
        }

        public void Move(Cell currentCell, Cell selectedCell)
        {
            selectedCell.PieceColor = currentCell.PieceColor;
            selectedCell.TextColor = currentCell.TextColor;
            selectedCell.Occupied = true;

            if (currentCell.Piece == "Pawn" && (selectedCell.RowNumber == 7 || selectedCell.RowNumber == 0))
            {
                selectedCell.Piece = "Queen";
                if (selectedCell.RowNumber == 7)
                {
                    selectedCell.PieceImg = "queenw.png";
                }
                else
                {
                    selectedCell.PieceImg = "queenb.png";
                }
            }
            else
            {
                selectedCell.Piece = currentCell.Piece;
                selectedCell.PieceImg = currentCell.PieceImg;
            }
            selectedCell.NotifyPropertyChanged("Content");
            currentCell.Piece = "";
            currentCell.PieceColor = "";
            currentCell.TextColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffffff"));
            currentCell.Occupied = false;
            currentCell.PieceImg = "";
            currentCell.NotifyPropertyChanged("Content");
        }

        private void MarkLegal(Cell cell)
        {
            cell.FillColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00ff00"));
            cell.LegalMove = true;
        }

        // Set fillcolor of all legal moves to green
        public void MarkLegals(Cell currentCell)
        {
            string piece = currentCell.Piece;
            string pieceColor = currentCell.PieceColor;
            int currentCol = currentCell.ColumnNumber;
            int currentRow = currentCell.RowNumber;

            currentCell.FillColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00ff00"));

            switch (piece)
            {
                // Pawn
                case "Pawn":
                    var p = this.FirstOrDefault(s => s.ColumnNumber == currentCol && s.RowNumber == currentRow - 1);
                    if (pieceColor == "Black")
                    {
                        if (p != null)
                        {
                            if (!p.Occupied)
                            {
                                MarkLegal(p);
                            }
                        }
                        if (currentRow == 6)
                        {
                            p = this.FirstOrDefault(s => s.ColumnNumber == currentCol && s.RowNumber == currentRow - 2);
                            if (p != null)
                            {
                                if (!p.Occupied)
                                {
                                    MarkLegal(p);
                                }
                            }
                        }

                        p = this.FirstOrDefault(s => s.ColumnNumber == currentCol - 1 && s.RowNumber == currentRow - 1);
                        if (p != null)
                        {
                            if (p.Occupied && p.PieceColor != pieceColor)
                            {
                                MarkLegal(p);
                            }
                        }

                        p = this.FirstOrDefault(s => s.ColumnNumber == currentCol + 1 && s.RowNumber == currentRow - 1);
                        if (p != null)
                        {
                            if (p.Occupied && p.PieceColor != pieceColor)
                            {
                                MarkLegal(p);
                            }
                        }
                    }
                    else
                    {
                        p = this.FirstOrDefault(s => s.ColumnNumber == currentCol && s.RowNumber == currentRow + 1);
                        if (p != null)
                        {
                            if (!p.Occupied)
                            {
                                MarkLegal(p);
                            }
                        }
                        if (currentRow == 1)
                        {
                            p = this.FirstOrDefault(s => s.ColumnNumber == currentCol && s.RowNumber == currentRow + 2);
                            if (p != null)
                            {
                                if (!p.Occupied)
                                {
                                    MarkLegal(p);
                                }
                            }
                        }

                        p = this.FirstOrDefault(s => s.ColumnNumber == currentCol - 1 && s.RowNumber == currentRow + 1);
                        if (p != null)
                        {
                            if (p.Occupied && p.PieceColor != pieceColor)
                            {
                                MarkLegal(p);
                            }
                        }

                        p = this.FirstOrDefault(s => s.ColumnNumber == currentCol + 1 && s.RowNumber == currentRow + 1);
                        if (p != null)
                        {
                            if (p.Occupied && p.PieceColor != pieceColor)
                            {
                                MarkLegal(p);
                            }
                        }
                    }
                    break;

                // Knight
                case "Knight":
                    // Check if cell is not out of bounds
                    var k = this.FirstOrDefault(s => s.ColumnNumber == currentCol + 1 && s.RowNumber == currentRow + 2);
                    if (k != null)
                    {
                        if ((k.Occupied && k.PieceColor != pieceColor) || (!k.Occupied))
                        {
                            MarkLegal(k);
                        }
                    }

                    k = this.FirstOrDefault(s => s.ColumnNumber == currentCol - 1 && s.RowNumber == currentRow + 2);
                    if (k != null)
                    {
                        if ((k.Occupied && k.PieceColor != pieceColor) || (!k.Occupied))
                        {
                            MarkLegal(k);
                        }
                    }

                    k = this.FirstOrDefault(s => s.ColumnNumber == currentCol - 1 && s.RowNumber == currentRow + 2);
                    if (k != null)
                    {
                        if ((k.Occupied && k.PieceColor != pieceColor) || (!k.Occupied))
                        {
                            MarkLegal(k);
                        }
                    }

                    k = this.FirstOrDefault(s => s.ColumnNumber == currentCol + 1 && s.RowNumber == currentRow - 2);
                    if (k != null)
                    {
                        if ((k.Occupied && k.PieceColor != pieceColor) || (!k.Occupied))
                        {
                            MarkLegal(k);
                        }
                    }

                    k = this.FirstOrDefault(s => s.ColumnNumber == currentCol - 1 && s.RowNumber == currentRow - 2);
                    if (k != null)
                    {
                        if ((k.Occupied && k.PieceColor != pieceColor) || (!k.Occupied))
                        {
                            MarkLegal(k);
                        }
                    }

                    k = this.FirstOrDefault(s => s.ColumnNumber == currentCol + 2 && s.RowNumber == currentRow + 1);
                    if (k != null)
                    {
                        if ((k.Occupied && k.PieceColor != pieceColor) || (!k.Occupied))
                        {
                            MarkLegal(k);
                        }
                    }

                    k = this.FirstOrDefault(s => s.ColumnNumber == currentCol - 2 && s.RowNumber == currentRow + 1);
                    if (k != null)
                    {
                        if ((k.Occupied && k.PieceColor != pieceColor) || (!k.Occupied))
                        {
                            MarkLegal(k);
                        }
                    }

                    k = this.FirstOrDefault(s => s.ColumnNumber == currentCol + 2 && s.RowNumber == currentRow - 1);
                    if (k != null)
                    {
                        if ((k.Occupied && k.PieceColor != pieceColor) || (!k.Occupied))
                        {
                            MarkLegal(k);
                        }
                    }

                    k = this.FirstOrDefault(s => s.ColumnNumber == currentCol - 2 && s.RowNumber == currentRow - 1);
                    if (k != null)
                    {
                        if ((k.Occupied && k.PieceColor != pieceColor) || (!k.Occupied))
                        {
                            MarkLegal(k);
                        }
                    }
                    break;

                // Rook
                case "Rook":
                    // Up
                    var r = this.FirstOrDefault(s => s.RowNumber == currentRow - 1 && s.ColumnNumber == currentCol);
                    while (r != null)
                    {
                        if (r.Occupied)
                        {
                            if (r.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(r);
                            break;
                        }
                        MarkLegal(r);
                        r = this.FirstOrDefault(s => s.RowNumber == r.RowNumber - 1 && s.ColumnNumber == currentCol);
                    }

                    // Right
                    r = this.FirstOrDefault(s => s.RowNumber == currentRow && s.ColumnNumber == currentCol + 1);
                    while (r != null)
                    {
                        if (r.Occupied)
                        {
                            Console.WriteLine("1");
                            if (r.PieceColor == pieceColor)
                            {
                                Console.WriteLine(r.PieceColor);
                                Console.WriteLine(pieceColor);
                                break;
                            }
                            MarkLegal(r);
                            break;
                        }
                        Console.WriteLine("3");
                        MarkLegal(r);
                        r = this.FirstOrDefault(s => s.RowNumber == currentRow && s.ColumnNumber == r.ColumnNumber + 1);
                    }

                    // Down
                    r = this.FirstOrDefault(s => s.RowNumber == currentRow + 1 && s.ColumnNumber == currentCol);
                    while (r != null)
                    {
                        if (r.Occupied)
                        {
                            if (r.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(r);
                            break;
                        }
                        MarkLegal(r);
                        r = this.FirstOrDefault(s => s.RowNumber == r.RowNumber + 1 && s.ColumnNumber == currentCol);
                    }

                    // Left
                    r = this.FirstOrDefault(s => s.RowNumber == currentRow && s.ColumnNumber == currentCol - 1);
                    while (r != null)
                    {
                        Console.WriteLine("Left");
                        if (r.Occupied)
                        {
                            if (r.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(r);
                            break;
                        }
                        MarkLegal(r);
                        r = this.FirstOrDefault(s => s.RowNumber == currentRow && s.ColumnNumber == r.ColumnNumber - 1);
                    }


                    break;

                case "Bishop":
                    // Up-Right
                    var b = this.FirstOrDefault(s => s.RowNumber == currentRow - 1 && s.ColumnNumber == currentCol + 1);
                    while (b != null)
                    {
                        if (b.Occupied)
                        {
                            if (b.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(b);
                            break;
                        }
                        MarkLegal(b);
                        b = this.FirstOrDefault(s => s.RowNumber == b.RowNumber - 1 && s.ColumnNumber == b.ColumnNumber + 1);
                    }

                    // Down-Right
                    b = this.FirstOrDefault(s => s.RowNumber == currentRow + 1 && s.ColumnNumber == currentCol + 1);
                    while (b != null)
                    {
                        if (b.Occupied)
                        {
                            if (b.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(b);
                            break;
                        }
                        MarkLegal(b);
                        b = this.FirstOrDefault(s => s.RowNumber == b.RowNumber + 1 && s.ColumnNumber == b.ColumnNumber + 1);
                    }

                    // Down-Left
                    b = this.FirstOrDefault(s => s.RowNumber == currentRow + 1 && s.ColumnNumber == currentCol - 1);
                    while (b != null)
                    {
                        if (b.Occupied)
                        {
                            if (b.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(b);
                            break;
                        }
                        MarkLegal(b);
                        b = this.FirstOrDefault(s => s.RowNumber == b.RowNumber + 1 && s.ColumnNumber == b.ColumnNumber - 1);
                    }

                    // Up-Left
                    b = this.FirstOrDefault(s => s.RowNumber == currentRow - 1 && s.ColumnNumber == currentCol - 1);
                    while (b != null)
                    {
                        if (b.Occupied)
                        {
                            if (b.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(b);
                            break;
                        }
                        MarkLegal(b);
                        b = this.FirstOrDefault(s => s.RowNumber == b.RowNumber - 1 && s.ColumnNumber == b.ColumnNumber - 1);
                    }

                    break;

                case "King":
                    // Up
                    var ki = this.FirstOrDefault(s => s.RowNumber == currentRow - 1 && s.ColumnNumber == currentCol);
                    if (ki != null)
                    {
                        if (!ki.Occupied || (ki.Occupied && ki.PieceColor != pieceColor))
                        {
                            MarkLegal(ki);
                        }
                    }
                    // Up Right
                    ki = this.FirstOrDefault(s => s.RowNumber == currentRow - 1 && s.ColumnNumber == currentCol + 1);
                    if (ki != null)
                    {
                        if (!ki.Occupied || (ki.Occupied && ki.PieceColor != pieceColor))
                        {
                            MarkLegal(ki);
                        }
                    }
                    // Right
                    ki = this.FirstOrDefault(s => s.RowNumber == currentRow && s.ColumnNumber == currentCol + 1);
                    if (ki != null)
                    {
                        if (!ki.Occupied || (ki.Occupied && ki.PieceColor != pieceColor))
                        {
                            MarkLegal(ki);
                        }
                    }

                    // Down Right
                    ki = this.FirstOrDefault(s => s.RowNumber == currentRow + 1 && s.ColumnNumber == currentCol + 1);
                    if (ki != null)
                    {
                        if (!ki.Occupied || (ki.Occupied && ki.PieceColor != pieceColor))
                        {
                            MarkLegal(ki);
                        }
                    }

                    // Down
                    ki = this.FirstOrDefault(s => s.RowNumber == currentRow + 1 && s.ColumnNumber == currentCol);
                    if (ki != null)
                    {
                        if (!ki.Occupied || (ki.Occupied && ki.PieceColor != pieceColor))
                        {
                            MarkLegal(ki);
                        }
                    }

                    // Down Left
                    ki = this.FirstOrDefault(s => s.RowNumber == currentRow + 1 && s.ColumnNumber == currentCol - 1);
                    if (ki != null)
                    {
                        if (!ki.Occupied || (ki.Occupied && ki.PieceColor != pieceColor))
                        {
                            MarkLegal(ki);
                        }
                    }

                    // Left
                    ki = this.FirstOrDefault(s => s.RowNumber == currentRow && s.ColumnNumber == currentCol - 1);
                    if (ki != null)
                    {
                        if (!ki.Occupied || (ki.Occupied && ki.PieceColor != pieceColor))
                        {
                            MarkLegal(ki);
                        }
                    }

                    // Up Left
                    ki = this.FirstOrDefault(s => s.RowNumber == currentRow - 1 && s.ColumnNumber == currentCol - 1);
                    if (ki != null)
                    {
                        if (!ki.Occupied || (ki.Occupied && ki.PieceColor != pieceColor))
                        {
                            MarkLegal(ki);
                        }
                    }
                    break;
                case "Queen":
                    // Up
                    var q = this.FirstOrDefault(s => s.RowNumber == currentRow - 1 && s.ColumnNumber == currentCol);
                    while (q != null)
                    {
                        if (q.Occupied)
                        {
                            if (q.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(q);
                            break;
                        }
                        MarkLegal(q);
                        q = this.FirstOrDefault(s => s.RowNumber == q.RowNumber - 1 && s.ColumnNumber == currentCol);
                    }

                    // Right
                    q = this.FirstOrDefault(s => s.RowNumber == currentRow && s.ColumnNumber == currentCol + 1);
                    while (q != null)
                    {
                        if (q.Occupied)
                        {
                            Console.WriteLine("1");
                            if (q.PieceColor == pieceColor)
                            {
                                Console.WriteLine(q.PieceColor);
                                Console.WriteLine(pieceColor);
                                break;
                            }
                            MarkLegal(q);
                            break;
                        }
                        Console.WriteLine("3");
                        MarkLegal(q);
                        q = this.FirstOrDefault(s => s.RowNumber == currentRow && s.ColumnNumber == q.ColumnNumber + 1);
                    }

                    // Down
                    q = this.FirstOrDefault(s => s.RowNumber == currentRow + 1 && s.ColumnNumber == currentCol);
                    while (q != null)
                    {
                        if (q.Occupied)
                        {
                            if (q.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(q);
                            break;
                        }
                        MarkLegal(q);
                        q = this.FirstOrDefault(s => s.RowNumber == q.RowNumber + 1 && s.ColumnNumber == currentCol);
                    }

                    // Left
                    q = this.FirstOrDefault(s => s.RowNumber == currentRow && s.ColumnNumber == currentCol - 1);
                    while (q != null)
                    {
                        Console.WriteLine("Left");
                        if (q.Occupied)
                        {
                            if (q.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(q);
                            break;
                        }
                        MarkLegal(q);
                        q = this.FirstOrDefault(s => s.RowNumber == currentRow && s.ColumnNumber == q.ColumnNumber - 1);
                    }

                    // Up-Right
                    q = this.FirstOrDefault(s => s.RowNumber == currentRow - 1 && s.ColumnNumber == currentCol + 1);
                    while (q != null)
                    {
                        if (q.Occupied)
                        {
                            if (q.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(q);
                            break;
                        }
                        MarkLegal(q);
                        q = this.FirstOrDefault(s => s.RowNumber == q.RowNumber - 1 && s.ColumnNumber == q.ColumnNumber + 1);
                    }

                    // Down-Right
                    q = this.FirstOrDefault(s => s.RowNumber == currentRow + 1 && s.ColumnNumber == currentCol + 1);
                    while (q != null)
                    {
                        if (q.Occupied)
                        {
                            if (q.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(q);
                            break;
                        }
                        MarkLegal(q);
                        q = this.FirstOrDefault(s => s.RowNumber == q.RowNumber + 1 && s.ColumnNumber == q.ColumnNumber + 1);
                    }

                    // Down-Left
                    q = this.FirstOrDefault(s => s.RowNumber == currentRow + 1 && s.ColumnNumber == currentCol - 1);
                    while (q != null)
                    {
                        if (q.Occupied)
                        {
                            if (q.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(q);
                            break;
                        }
                        MarkLegal(q);
                        q = this.FirstOrDefault(s => s.RowNumber == q.RowNumber + 1 && s.ColumnNumber == q.ColumnNumber - 1);
                    }

                    // Up-Left
                    q = this.FirstOrDefault(s => s.RowNumber == currentRow - 1 && s.ColumnNumber == currentCol - 1);
                    while (q != null)
                    {
                        if (q.Occupied)
                        {
                            if (q.PieceColor == pieceColor)
                            {
                                break;
                            }
                            MarkLegal(q);
                            break;
                        }
                        MarkLegal(q);
                        q = this.FirstOrDefault(s => s.RowNumber == q.RowNumber - 1 && s.ColumnNumber == q.ColumnNumber - 1);
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
