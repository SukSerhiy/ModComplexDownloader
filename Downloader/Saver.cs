using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Downloader.Exceptions;

namespace Downloader
{
    public class Saver
    {
        public event Action<string> onPassChange;

        private string rootPath;
        private string path;

        public Saver()
        {
            rootPath = "D:";
            path = rootPath;
        }

        public Saver(string rootPath)
        {
            this.rootPath = rootPath;
            this.path = rootPath;
        }

        public void StepToDepth(string directoryName)
        {

            CreateDirectory(path, directoryName);
            path += "\\" + directoryName;
            Console.WriteLine(path);
        }

        public void StepUp()
        {
            path = path.Substring(0, path.LastIndexOf("\\"));
            Console.WriteLine(path);
        }

        private void CreateDirectory(string path, string subpath)
        {
            onPassChange($"{path} ...");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            dirInfo.CreateSubdirectory(subpath);
        }

        public void SaveFile(byte[] arrBytes, string fileName)
        {
            string filePath = path + "\\" + fileName;
            if (arrBytes != null)
            {
                onPassChange($"{filePath} ...");
                File.WriteAllBytes(filePath, arrBytes);
            }
        }
    }
}
