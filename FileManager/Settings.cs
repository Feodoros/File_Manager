using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileManager
{

    [Serializable]    
    public class Settings
    {
        public static Settings GetSettings()
        {
            Settings settings = null;
            string filename = Globals.SettingsFile;

            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    XmlSerializer serX = new XmlSerializer(typeof(Settings));
                    settings = (Settings) serX.Deserialize(fs);
                    fs.Close();
                }
            }
            else settings = new Settings();
            return settings;
        }
        public void Save_Settings()
        {
            string filename = Globals.SettingsFile;
            if (File.Exists(filename))
                File.Delete(filename);

            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                
                XmlSerializer serX = new XmlSerializer(typeof(Settings));
                serX.Serialize(fs, this);
                fs.Close();
            }
        }

      

        public string Width { get; set; }

        public string Height { get; set; }

        public string Color { get; set; }

        public string User { get; set; }

       

    }

    
    class User_Serial
    {
        public string user;

        BinaryFormatter binformat = new BinaryFormatter();
        Stream stream = new FileStream(@"C:\Users\fzhil\source\repos\File_Manager\FileManager\bin\Debug", FileMode.Open, FileAccess.Write, FileShare.None);
        public void Serialaze()
        {
            binformat.Serialize(stream, user);
            stream.Close();
        }
    }

    
}
