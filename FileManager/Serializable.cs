using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{

    [Serializable] 
    public class UserPrefs
    {

        public string User, Width, Height, Color;

        public UserPrefs(string User, string Width, string Height, string Color)
        {
            User = "Fedor";
            Width = "";
            Height = "";
            Color = "";
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {
            User = "Fedor";
        }

        [OnDeserialized] 
        internal void OnDeserialized(StreamingContext context)
        {
            User = "Fedor";            
        }
    }



    public static void Ser()
    {
        UserPrefs up = new UserPrefs();
        up.User = "";
        up.Width = "";
        up.Height = "";
        up.Color = "";
        BinaryFormatter binformat = new BinaryFormatter();
        Stream stream = new FileStream(@"C:\Users\fzhil\source\repos\File_Manager\FileManager\bin\Debug", FileMode.Open, FileAccess.Write, FileShare.None);
        binformat.Serialize(stream, up);
        stream.Close();
    }

}