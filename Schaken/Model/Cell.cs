using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Schaken.Model
{
    class Cell : BaseModel
    {
        private int id, rowNumber, columnNumber;
        private bool occupied, legalMove;
        private string piece, pieceColor, pieceImg;
        private SolidColorBrush fillColor, textColor;

        public Cell(int id, int col, int row, SolidColorBrush colorBG)
        {
            
            Id = id;
            ColumnNumber = col;
            RowNumber = row;
            FillColor = colorBG;

            // Piece Names
            if ((col == 0 || col == 7) && (row == 0 || row == 7))
            {
                Piece = "Rook";
                PieceImg = row == 0 ? "rookw.png" : "rookb.png";  
            }

            else if ((col == 1 || col == 6) && (row == 0 || row == 7))
            {
                Piece = "Knight";
                PieceImg = row == 0 ? "knightw.png" : "knightb.png";
            }

            else if ((col == 2 || col == 5) && (row == 0 || row == 7))
            {
                Piece = "Bishop";
                PieceImg = row == 0 ? "bishopw.png" : "bishopb.png";
            }

            else if ((col == 3) && (row == 0 || row == 7))
            {
                Piece = "Queen";
                PieceImg = row == 0 ? "queenw.png" : "queenb.png";
            }

            else if ((col == 4) && (row == 0 || row == 7))
            {
                Piece = "King";
                PieceImg = row == 0 ? "kingw.png" : "kingb.png";
            }
            else if (row == 1 || row == 6)
            {
                Piece = "Pawn";
                PieceImg = row == 1 ? "pawnw.png" : "pawnb.png";
            }
            else
            {
                Piece = "";
                PieceImg = "";
            }

            // Piece Colors
            if (row == 0 || row == 1)
            {
                TextColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff0000"));
                PieceColor = "White";
                Occupied = true;
            }
            else if (row == 6 || row == 7)
            {
                TextColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#0000ff"));
                PieceColor = "Black";
                Occupied = true;
            }
            else
            {
                TextColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffffff"));
                PieceColor = "";
                Occupied = false;
            }
        }

        public int Id { get { return id; } set { id = value; NotifyPropertyChanged(); } }
        public int RowNumber { get { return rowNumber; } set { rowNumber = value; NotifyPropertyChanged(); } }
        public int ColumnNumber { get { return columnNumber; } set { columnNumber = value; NotifyPropertyChanged(); } }
        public bool LegalMove { get { return legalMove; } set { legalMove = value; NotifyPropertyChanged(); } }
        public bool Occupied { get { return occupied; } set { occupied = value; NotifyPropertyChanged(); } }
        public string Piece { get { return piece; } set { piece = value; NotifyPropertyChanged(); } }
        public string PieceColor { get { return pieceColor; } set { pieceColor = value; NotifyPropertyChanged(); } }
        public string PieceImg { get { return pieceImg; } set { pieceImg = value; NotifyPropertyChanged(); } }
        public SolidColorBrush FillColor { get { return fillColor; } set { fillColor = value; NotifyPropertyChanged(); } }
        public SolidColorBrush TextColor { get { return textColor; } set { textColor = value; NotifyPropertyChanged(); } }

        public string Content
        {
            get
            {
                string content = "/view/resources/images/pieces/" + PieceImg;
                return content;
            }
        }
    }
}
