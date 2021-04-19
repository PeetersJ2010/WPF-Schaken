using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaken.Model
{
    class Gamemode : BaseModel
    {
        private int id;
        private string gamemodeName;
        public string GamemodeName { get { return gamemodeName; } set { gamemodeName = value; NotifyPropertyChanged(); } }
        public int ID { get { return id; } set { id = value; NotifyPropertyChanged(); } }
    }
}
