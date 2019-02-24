using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Forms
{
    abstract class Entry
    {
        public string Path;


        public Entry(string path)
        {
            this.Path = path;
        }

       

        public string GetExtension()
        {
            string ext = System.IO.Path.GetExtension(Path);
            return ext;
        }


        public string Combine(string Path1, string Path2)
        {
            string  Final_Path = System.IO.Path.Combine(Path1, Path2);
            return Final_Path;
        }


        public FileStream FileStreamCreate()
        {
            FileStream fs = new FileStream(Path, FileMode.OpenOrCreate);
            return fs;
        }


        public List<string> GetDriveName()
        {
            List<string> dr = new List<string>();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                dr.Add(drive.Name);
            }

            return dr;
        }
        public abstract bool Existing();
        public abstract string GetFullName(string name);
        public abstract string GetName();
    }


    class PersonalException : Exception
    {
        public PersonalException(string message)
            : base(message)
        { }
    }
}

