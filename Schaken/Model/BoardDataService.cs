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
    class BoardDataService
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["Azure"].ConnectionString;

        //Connection
        private static IDbConnection db = new SqlConnection(connectionString);

        public List<Board> GetMatches()
        {
            //SQL Query
            string sql = "select Match.ID, pb.ID as playerBlackID, pw.ID as playerWhiteID, Gamemode.ID as gamemodeID, Match.winningColorID as winningColorID from Match inner join Player as pb on pb.ID = Match.playerBlackID inner join Player as pw on pw.ID = Match.playerWhiteID inner join Gamemode on Gamemode.ID = Match.gamemodeID";

            // Check if a match exists
            List<String> ml = (List<String>)db.Query<String>("select * from Match");
            if (ml.Count > 0)
            {
                return (List<Board>)db.Query<Board>(sql);
            }
            // Return an empty list
            List<Board> emptyList = new List<Board>();
            return emptyList;
        }

        public void AddMatch(Board board)
        {
            int pbid = board.PlayerBlack.ID;
            int pwid = board.PlayerWhite.ID;
            int gmid = board.Gamemode.ID;
            int wc = board.WinningColor.ID;

            string sql = "Insert into Match (playerBlackID, playerWhiteID, gamemodeID, winningColorID) values (@pbid, @pwid, @gmid, @wc)";

            db.Execute(sql, new
            {
                pbid,
                pwid,
                gmid,
                wc
            });
        }

        public int GetLastMatchID()
        {
            string sql = "select * from Match order by ID desc";
            List<String>bl = (List<String>)db.Query<String>(sql);

            if (bl.Count > 0)
            {
                foreach (var item in bl)
                {
                    return Int32.Parse(item);
                }
            }
            return 0;
        }

        public void UpdateMatch(Board board)
        {
            int id = board.ID;
            int pbid = board.PlayerBlack.ID;
            int pwid = board.PlayerWhite.ID;
            int wcid = board.WinningColor.ID;

            Console.WriteLine(id);
            Console.WriteLine(pwid);
            Console.WriteLine(pbid);
            Console.WriteLine(wcid);

            //SQL Query
            string sql = "update Match set playerBlackID = @pbid, playerWhiteID = @pwid, winningColorID = @wcid where ID = @id";
            //Execute + Param
            db.Execute(sql, new
            {
                pbid,
                pwid,
                wcid,
                id
            });
        }

        public void DeleteMatch(Board board)
        {
            int bid = board.ID; 
            // SQL statement delete 
            string sql = "Delete from Piece where Piece.matchID = @bid";
            // Uitvoeren SQL statement en doorgeven parametercollectie
            db.Execute(sql, new { bid });

            sql = "Delete from Match where Match.ID = @bid";
            db.Execute(sql, new { bid });
        }
    }
}
