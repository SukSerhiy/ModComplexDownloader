using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegistryWorker
{
    public class RegFile
    {
        private string regFileName, keyRegStr, currentPath, msWordPath, matlabPath, pathPlaceholder, pathMSWordPlaceholder, pathMatlabPlaceholder;
        public RegFile(string regFileName, string keyRegStr, string currentPath, string msWordPath, string matlabPath, string pathPlaceholder, string pathMSWordPlaceholder, string pathMatlabPlaceholder)
        {
            this.regFileName = regFileName;
            this.keyRegStr = keyRegStr;
            this.currentPath = currentPath;
            this.msWordPath = msWordPath;
            this.matlabPath = matlabPath;
            this.pathPlaceholder = pathPlaceholder;
            this.pathMSWordPlaceholder = pathMSWordPlaceholder;
            this.pathMatlabPlaceholder = pathMatlabPlaceholder;
        }
        private List<string> GetRegPathes(string scriptPath, string keyStr)
        {
            List<string> pathes = new List<string>();
            using (StreamReader sr = new StreamReader(scriptPath))
            {
                string line;
                bool saveNextStr = false;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(keyStr))
                    {
                        saveNextStr = true;
                    }
                    if (saveNextStr && line.Contains("@="))
                    {
                        pathes.Add(line);
                        saveNextStr = false;
                    }
                }
            }
            return pathes;
        }
        
        private List<string> ModifyRegPathes(List<string> pathes, string currentPath, string msWordPath, string matlabPath, string pathPlaceholder, string pathMSWordPlaceholder, string pathMatlabPlaceholder)
        {
            currentPath = currentPath.Replace("\\", "\\\\");
            msWordPath = msWordPath != null ? msWordPath.Replace("\\", "\\\\") : "";
            matlabPath = matlabPath != null ? matlabPath.Replace("\\", "\\\\") : "";
            List<string> modifiedPathes = new List<string>();
            foreach (string path in pathes)
            {
                int pathIdx = path.IndexOf(pathPlaceholder);
                if (pathIdx != -1 && path.Contains(pathPlaceholder))
                {
                    int end = pathIdx + (pathPlaceholder.Length );
                    string newPath = path.Substring(0, pathIdx) + path.Substring(end, path.Length - 1 - end);
                    newPath = newPath.Insert(pathIdx, currentPath);
                    modifiedPathes.Add(newPath);
                }
                int pathMSWordIdx = path.IndexOf(pathMSWordPlaceholder);
                if (pathMSWordIdx != -1 && path.Contains(pathMSWordPlaceholder))
                {
                    int end = pathMSWordIdx + (pathMSWordPlaceholder.Length);
                    string newPath = path.Substring(0, pathMSWordIdx) + path.Substring(end, path.Length - 1 - end);
                    newPath = newPath.Insert(pathMSWordIdx, msWordPath);
                    modifiedPathes.Add(newPath);
                }
                int pathMatlabIdx = path.IndexOf(pathMatlabPlaceholder);
                if (pathMatlabIdx != -1 && path.Contains(pathMatlabPlaceholder))
                {
                    int end = pathMatlabIdx + (pathMatlabPlaceholder.Length);
                    string newPath = path.Substring(0, pathMatlabIdx) + path.Substring(end, path.Length - 1 - end);
                    newPath = newPath.Insert(pathMatlabIdx, matlabPath);
                    modifiedPathes.Add(newPath);
                }
            }
            return modifiedPathes;
        }
        
        private void WriteRegPathesToFile(string regFilePath, string tempFilePath, Queue<string> pathes, string keyStr)
        {
            using (StreamReader sr = new StreamReader(regFilePath))
            {
                string line;
                bool saveNextStr = false;
                File.WriteAllText(tempFilePath, String.Empty);
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(keyStr))
                    {
                        saveNextStr = true;
                    }
                    if (saveNextStr && line.Contains("@="))
                    {
                        using (StreamWriter sw = new StreamWriter(tempFilePath, true, System.Text.Encoding.ASCII))
                        {
                            if (pathes.Count > 0)
                                sw.WriteLine(pathes.Dequeue());
                        }
                        saveNextStr = false;
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(tempFilePath, true, System.Text.Encoding.ASCII))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
            }

            using (StreamReader sr = new StreamReader(tempFilePath))
            {
                string tempFileContent = sr.ReadToEnd();
            }
        }

        private string TempFilePath(string regFilePath)
        {
            int dotIdx = regFilePath.IndexOf(".");
            string fileName = regFilePath.Substring(0, regFilePath.Length - (regFilePath.Length - dotIdx));
            fileName = fileName + "_temp" + ".reg";
            return fileName;
        }

        private string GetTempRegFile()
        {
            //List of [path]
            List<string> pathes = GetRegPathes(regFileName, keyRegStr);
            //List of [mswordpath]
            //List of real pathes
            List<string> realPathes = ModifyRegPathes(pathes, currentPath, msWordPath, matlabPath, pathPlaceholder, pathMSWordPlaceholder, pathMatlabPlaceholder);
            string tempFilePath = TempFilePath(regFileName);
            //write temp file
            WriteRegPathesToFile(regFileName, tempFilePath, new Queue<string>(realPathes), keyRegStr);
            return tempFilePath;
        }

        public void ChangeRegistry()
        {
            string tempFilePath = GetTempRegFile();
            Process process = Process.Start(tempFilePath);
            //File.Delete(tempFilePath);
        }
    }
}
