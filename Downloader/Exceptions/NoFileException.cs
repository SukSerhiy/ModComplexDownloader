using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Downloader.Exceptions
{
    public class NoFileException: Exception
    {
        public NoFileException(string path) : base()
        {
            this.path = path;
        }

        public string path;
    }
}
