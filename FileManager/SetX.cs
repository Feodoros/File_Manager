using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    [Serializable]
    public class SetX
    {
        
        public Int16 Width { get; set; }
        public Int16 Height { get; set; }
        public String User { get; set; }        
        public String Color { get; set; }


        /*public int Width, Height;
        public string user;
        public string color;
        public SetX(int w, int h, string u, string c)    -----For BinarySerialization-----
        {
            Width = w;
            Height = h;
            user = u;
            color = c;
        }*/
    }
}
