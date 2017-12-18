using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Downloader;
using System.Windows.Threading;
using System.Threading;
using System.Net.Http;
using Downloader.Exceptions;

namespace ModComplexDownloader
{
    /// <summary>
    /// Логика взаимодействия для DownloaderPage.xaml
    /// </summary>
    public partial class DownloaderPage : Page
    {
        public event Action onCancelled;
        private BackgroundWorker backgroundWorker;
        private Button movePrevBtn;
        private Button moveNextBtn;
        private string path;
        private bool doneSuccess;
        private const string rootFolder = CommonValues.rootFolder;
        public DownloaderPage()
        {
            InitializeComponent();
            backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));
            var mainWindow = Application.Current.MainWindow as MainWindow;
            movePrevBtn = mainWindow.MovePrevBtn;
            moveNextBtn = mainWindow.MoveNextBtn;
            doneSuccess = false;
        }
        
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            path = ChoosePathPage.GetPath;
            DataDownloader dataDownloader = new GoogleDriveDataDownloader(path, rootFolder, backgroundWorker_ProgressChanged, ProgressTextChanded);
            onCancelled = dataDownloader.Cancel;
            progressText.Text = "Підготовка до завантаження...\n";
            var wnd = Window.GetWindow(this);
            movePrevBtn.IsEnabled = false;
            moveNextBtn.IsEnabled = false;
            backgroundWorker.RunWorkerAsync(dataDownloader);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DataDownloader dataDownloader = (DataDownloader)e.Argument;
            try
            {
                dataDownloader.Download();
                doneSuccess = true;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"{ex.Message}\n{ex.InnerException.Message}\nВозможно отсутствует подключение к Интернету", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"У вас немає прав для доступу шляху {CommonValues.path}\n Спробуйте запустити програму від імені адміністратора", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(NoFileException ex)
            {
                MessageBox.Show($"Не вдалося завантажити файл {ex.path}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (doneSuccess)
            {
                progressBar.Value = 100;
                moveNextBtn.IsEnabled = true;
            }
        }

        private void ProgressTextChanded(string currentPath)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () {
                progressText.Text += $"{currentPath}\n";
            });
        }

        private void backgroundWorker_ProgressChanged(int step)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () {
                progressBar.Value = step;
            });
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate() {
                progressBar.Value = e.ProgressPercentage;
            });
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult result = MessageBox.Show("Ви дійсно хочете перервати завантаження і завершити роботу програми?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    backgroundWorker.CancelAsync();
                    onCancelled();
                    ProgressTextChanded("Завантаження перервано");
                    Application.Current.MainWindow.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
