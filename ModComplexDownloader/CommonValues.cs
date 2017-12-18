using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ModComplexDownloader
{
    //public enum Languages {Ukrainian, Russian, English}
    public enum StepOfSetting {DownloadFromGDrive, ChangeRegister};
    public static class CommonValues
    {
        //public static Languages language = Languages.English;

        public static HashSet<StepOfSetting> stepsOfSetting;

        public static List<Page> pagesSeuquence;

        //#region pages

        //public static ChoosePathPage choosePathPage;

        //public static DownloaderPage downloaderPage;

        //public static RegistryChangerPage registryChangerPage;

        //public static GreetingPage greetingPage;

        //public static ChooseWhatToDo chooseWhatToDoPage;

        //#endregion

        #region pathes

        public const string defaultPath = "D:\\";

        public static string pathToMsWord;

        public static string pathToMatlab;

        public static string path;

        public const string regFilePath = @"ModComplexProtocol.reg";

        public const string keyRegStr = @"shell\open\command";

        public const string pathPlaceholder = "[path]";

        public const string pathMSWordPlaceholder = "[mswordpath]";

        public const string pathMatlabPlaceholder = "[matlabpath]";

        public const string rootFolder = "MODEL_COMP_HIACUSTIC";

        #endregion

        static CommonValues()
        {
            //choosePathPage = new ChoosePathPage();

            //downloaderPage = new DownloaderPage();

            //registryChangerPage = new RegistryChangerPage();

            //greetingPage = new GreetingPage();

            //chooseWhatToDoPage = new ChooseWhatToDo();

            //stepsOfSetting = new HashSet<StepOfSetting>();

            path = defaultPath;
            //pagesSeuquence = new List<Page>(new Page[] { choosePathPage, chooseWhatToDoPage });
            Debug.WriteLine(pagesSeuquence);
        }
    }
}
