using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaken.Model
{
    class PieceType : BaseModel
    {
        private int id;
        private string pieceTypeName;
        
        public int ID { get { return id; } set { id = value; NotifyPropertyChanged(); } }
        public string PieceTypeName { get { return pieceTypeName; } set { pieceTypeName = value; NotifyPropertyChanged(); } }

        public PieceType()
        {

        }
        

        public PieceType(int id, string typeName)
        {
            ID = id;
            PieceTypeName = typeName;
        }
    }
}
