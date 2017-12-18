using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Downloader
{
    public abstract class DataDownloader
    {
        public DataDownloader(string folderName)
        {
            this.folderName = folderName;
        }
        
        protected string folderName { get; private set; }
        public abstract void Download();

        public abstract void Cancel();
    }
}
