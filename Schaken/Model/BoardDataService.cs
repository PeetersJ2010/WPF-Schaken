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











        public List<Player> GetPlayersByID()
        {
            //SQL Query
            string sql = "select * from Player order by ID desc";
            //Execute
            return (List<Player>)db.Query<Player>(sql);
        }

        public Player GetPlayer(Player player)
        {
            int id = player.ID;
            string sql = "select * from Player where ID = " + id;
            List<Player> result = (List<Player>)db.Query<Player>(sql);
            if (result.Count != 0)
            {
                return result[0];
            }

            return null;
        }

        public void UpdatePlayer(Player player)
        {
            //SQL Query
            string sql = "update Player set Rating = @rating, Username = @username, RealName = @realname where ID = @id";
            //Execute + Param
            db.Execute(sql, new
            {
                player.ID,
                player.Rating,
                player.RealName,
                player.Username
            });
        }

        public void InsertPlayer(Player player)
        {
            // SQL statement insert
            string sql = "Insert into Player (Rating, Username, RealName) values (@rating, @username, @realname)";

            // Uitvoeren SQL statement en doorgeven parametercollectie
            db.Execute(sql, new
            {
                player.Rating,
                player.RealName,
                player.Username
            });
        }

        public void DeletePlayer(Player player)
        {
            // SQL statement delete 
            string sql = "Delete Player where ID = @id";

            // Uitvoeren SQL statement en doorgeven parametercollectie
            db.Execute(sql, new { player.ID });
        }
    }
}
