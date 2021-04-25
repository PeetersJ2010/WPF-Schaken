using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaken.Model
{
    class Color : BaseModel
    {
        int id;
        string colorName;

        public int ID { get { return id; } set { id = value; } }
        public string ColorName { get { return colorName; } set { colorName = value; } }

        Color(int id, string colorName)
        {
            ID = id;
            ColorName = colorName;
        }
    }
}
