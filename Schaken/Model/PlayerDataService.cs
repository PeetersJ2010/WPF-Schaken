using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Schaken.Model
{
    public class PlayerDataService
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["Azure"].ConnectionString;

        //Connection
        private static IDbConnection db = new SqlConnection(connectionString);
        public List<Player> GetPlayers()
        {
            //SQL Query
            string sql = "select * from Player order by Rating desc";
            //Execute
            return (List<Player>)db.Query<Player>(sql);
        }

        public List<Player> GetPlayersByID()
        {
            //SQL Query
            string sql = "select * from Player order by ID desc";
            //Execute
            return (List<Player>)db.Query<Player>(sql);
        }

        public int GetLastPlayerID()
        {
            string sql = "select * from Player order by ID desc";
            List<String> pl = (List<String>)db.Query<String>(sql);

            if (pl.Count > 0)
            {
                foreach (var item in pl)
                {
                    // Return the first item in the list, parsed to an int
                    return Int32.Parse(item);
                }
            }
            return 0;
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
            db.Execute(sql, new { 
                player.ID, 
                player.Rating,
                player.RealName,
                player.Username
            });
        }

        public void InsertPlayer(Player player)
        {
            // SQL statement insert
            string sql = "SET IDENTITY_INSERT Player ON \n Insert into Player (ID, Rating, Username, RealName) values (@id, @rating, @username, @realname) \n SET IDENTITY_INSERT Player OFF";

            // Uitvoeren SQL statement en doorgeven parametercollectie
            db.Execute(sql, new
            {
                player.ID,
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
