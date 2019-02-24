using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Forms
{
    class Factory
    {
        public static Entry Get(string path)
        {

            if (new Forms.File(path.Replace(".archive.zip", "")).GetExtension() != "" && path.EndsWith(".archive.zip"))
            {
                return new ZipFiles(path);
            }
            if (new Forms.File(path.Replace(".archive.zip", "")).GetExtension() != "" && path.EndsWith(".archive.zip\\"))
            {
                return new ZipFiles(path);
            }
            if (new Forms.File(path).GetExtension() != "" && path.Contains(".archive.zip\\") && !path.EndsWith(".archive.zip\\"))
            {
                return new ZipFiles(path);
            }
            if (new Forms.File(path).GetExtension() == "" && path.Contains(".archive.zip\\") && !path.EndsWith(".archive.zip\\"))
            {
                return new ZipFolder(path);
            }
            if (new Folder(path.Replace(".archive.zip", "")).GetExtension() == "" && (path.EndsWith(".archive.zip")))
            {
                return new ZipFolder(path);
            }
            if ((new Forms.File(path).GetExtension() != ""))
            {
                return new Forms.File(path);
            }

            return new Folder(path);
        }
    }
}
