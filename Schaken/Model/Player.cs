using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaken.Model
{
    public class Player : BaseModel
    {
        private int id, rating;
        private string username, realName;

        public Player(int id, int rating, string username, string realname)
        {
            ID = id;
            Rating = rating;
            Username = username;
            RealName = realname;
        }

        public int ID { get { return id; } set { id = value; } }
        public int Rating { get { return rating; } set { rating = value; } }
        public string Username { get { return username; } set { username = value; } }
        public string RealName { get { return realName; } set { realName = value; } }
    }
}
