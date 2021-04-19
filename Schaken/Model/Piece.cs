using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaken.Model
{
    class Piece : BaseModel
    {
        private string name, color;
        private bool isCaptured;
        private Cell currentCell;

        public Piece(string name, string color)
        {
            Name = name;
            Color = color;
            IsCaptured = false;
            if (color == "White")
            {
                switch (name)
                {
                    case "Rook":
                        
                        break;
                }
            }
            else
            {
                switch (name)
                {
                    case "Rook":

                        break;
                }
            }
        }

        public string Name { get; set; }
        public string Color { get; set; }
        public bool IsCaptured { get { return isCaptured; } set { isCaptured = value; NotifyPropertyChanged(); } }
        public Cell CurrentCell { get { return currentCell; } set { CurrentCell = value; NotifyPropertyChanged(); } }

    }
}
