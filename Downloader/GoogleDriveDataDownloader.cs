using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Apis.Services;
using GoogleDriveV2 = Google.Apis.Drive.v2;
using System.Diagnostics;

namespace Downloader
{
    public class GoogleDriveDataDownloader : DataDownloader
    {
        private const string APP_NAME = "MKHIDRO_GD";
        private const string CLIENT_FILE_NAME = "client_secret.json";

        private string[] scopes = { DriveService.Scope.Drive };
        private string applicationName;
        private Saver saver;
        private ProgressConfiger progress;
        private bool isCancelled;
        public GoogleDriveDataDownloader(string folderName) : base(folderName)
        {
            this.applicationName = APP_NAME;
            this.saver = new Saver();
            this.progress = new ProgressConfiger();
            isCancelled = false;
        }

        public GoogleDriveDataDownloader(string folderName, Action<int> progressStateChanger) : base(folderName)
        {
            this.applicationName = APP_NAME;
            this.saver = new Saver();
            this.progress = new ProgressConfiger();
            progress.onProgressStateChange += progressStateChanger;
            isCancelled = false;
        }

        public GoogleDriveDataDownloader(string pathToSave, string folderName, Action<int> progressStateChanger, Action<string> progressTextChanger) : base(folderName)
        {
            this.applicationName = APP_NAME;
            this.saver = new Saver(pathToSave);
            this.progress = new ProgressConfiger();
            progress.onProgressStateChange += progressStateChanger;
            saver.onPassChange += progressTextChanger;
            isCancelled = false;
        }

        public GoogleDriveDataDownloader(string pathToSave, string folderName, Action<int> progressStateChanger, Action<string> progressTextChanger, Action cancelMethod) : base(folderName)
        {
            this.applicationName = APP_NAME;
            this.saver = new Saver(pathToSave);
            this.progress = new ProgressConfiger();
            progress.onProgressStateChange += progressStateChanger;
            saver.onPassChange += progressTextChanger;
            isCancelled = false;
        }

        public override void Cancel()
        {
            isCancelled = true;
        }

        public override void Download()
        {
            UserCredential credentials = GetUserCredantionals();
            DriveService services = GetDriveServices(credentials);
            var Res = ResFromFolder(services, "root").Where(x => x.Title == folderName).FirstOrDefault();
            getHierarchy(Res, services);
        }
        
        private UserCredential GetUserCredantionals()
        {
            using (var stream = new FileStream(CLIENT_FILE_NAME, FileMode.Open, FileAccess.Read))
            {
                //Получаем путь к папке "Мои документы"
                string creadPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                //Указываем, что будем писать туда json-файл с данными об аутентификации
                creadPath = Path.Combine(creadPath, "driveApiCredentials", "drive-credetials.json");
                //осуществляем саму авторизацию и возвращаем крэды
                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "User",
                    CancellationToken.None,
                    new FileDataStore(creadPath, true)).Result;
            }
        }

        // Create Drive API service.
        private DriveService GetDriveServices(UserCredential credentials)
        {
            return new DriveService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = applicationName
                }
                );
        }

        private void getHierarchy(Google.Apis.Drive.v2.Data.File Res, DriveService driveService)
        {
            if (isCancelled)
            {
                return;
            }
            //Отримуємо childrenCount
            var children = driveService.Children.List(Res.Id).Execute();
            var childrenCount = children.Items.Count;

            Debug.WriteLine(Res.Title);
            Debug.WriteLine("Children count " + childrenCount);

            progress.HandleCurrElem(childrenCount);

            //Якщо Res - папка
            if (Res.MimeType == "application/vnd.google-apps.folder")
            {
                saver.StepToDepth(Res.Title);
                //Перебираємо файли в цій папці
                foreach (var res in ResFromFolder(driveService, Res.Id).ToList())
                    getHierarchy(res, driveService);
                saver.StepUp();
            }
            else
            {
                saver.SaveFile(getByteArray(driveService, Res), Res.Title);
            }
        }

        private List<Google.Apis.Drive.v2.Data.File> ResFromFolder(DriveService service, string folderId)
        {
            //Get list of folder's childern
            var request = service.Children.List(folderId);
            request.MaxResults = 1000;

            List<Google.Apis.Drive.v2.Data.File> TList = new List<Google.Apis.Drive.v2.Data.File>();
            do
            {
                //A list of children of the folder
                var children = request.Execute();
                foreach (var child in children.Items)
                {
                    TList.Add(service.Files.Get(child.Id).Execute());
                }
                request.PageToken = children.NextPageToken;
            } while (!String.IsNullOrEmpty(request.PageToken));

            return TList;
        }

        private byte[] getByteArray(DriveService _service, GoogleDriveV2.Data.File _fileResource)
        {
            if (!string.IsNullOrEmpty(_fileResource.DownloadUrl))
            {
                try
                {
                    var x = _service.HttpClient.GetByteArrayAsync(_fileResource.DownloadUrl);
                    return x.Result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else return null;
        }
    }
}
