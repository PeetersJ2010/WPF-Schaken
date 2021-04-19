using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaken.Model
{
    class PieceDataService
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["Azure"].ConnectionString;

        //Connection
        private static IDbConnection db = new SqlConnection(connectionString);

        public void AddPiece(Cell cell)
        {
            string pieceType = cell.Piece;
            string pieceColor = cell.PieceColor;
            int pieceTypeID;
            int pieceColorID;
            int matchID = 0;
            int positionX = cell.ColumnNumber;
            int positionY = cell.RowNumber;

            // Get PieceTypeID
            switch (pieceType)
            {
                case "Pawn":
                    pieceTypeID = 0;
                    break;
                case "Rook":
                    pieceTypeID = 1;
                    break;
                case "Bishop":
                    pieceTypeID = 2;
                    break;
                case "Knight":
                    pieceTypeID = 3;
                    break;
                case "King":
                    pieceTypeID = 4;
                    break;
                case "Queen":
                    pieceTypeID = 5;
                    break;
                default:
                    pieceTypeID = -1;
                    break;
            }

            // Get PieceColorID
            switch (pieceColor)
            {
                case "Black":
                    pieceColorID = 0;
                    break;
                case "White":
                    pieceColorID = 1;
                    break;
               
                default:
                    pieceColorID = -1;
                    break;
            }

            //Get MatchID
            string sql = "select * from Match order by ID desc";
            List<String> bl = (List<String>)db.Query<String>(sql);

            if (bl.Count > 0)
            {
                foreach (var item in bl)
                {
                    matchID = Int32.Parse(item);
                }
            }

            // Insert
            sql = "Insert into Piece (pieceTypeID, colorID, matchID, positionX, positionY) values (@pieceTypeID, @pieceColorID, @matchID, @positionX, @positionY)";

            db.Execute(sql, new
            {
                pieceTypeID,
                pieceColorID,
                matchID,
                positionX,
                positionY
            });
        }
    }
}
