using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using RegWorker = RegistryWorker.Registry;
using RegFile = RegistryWorker.RegFile;

namespace ModComplexDownloader
{
    /// <summary>
    /// Логика взаимодействия для RegistryChangerPage.xaml
    /// </summary>
    public partial class RegistryChangerPage : Page
    {
        private const string regFileName = CommonValues.regFilePath;
        private const string keyRegStr = CommonValues.keyRegStr;
        private const string pathPlaceholder = CommonValues.pathPlaceholder;
        private const string pathMSWordPlaceholder = CommonValues.pathMSWordPlaceholder;
        private const string pathMatlabPlaceholder = CommonValues.pathMatlabPlaceholder;
        private string currentPath;
        private string msWordPath;
        private string matlabPath;
        private RegFile regFileWorker;
        public RegistryChangerPage()
        {
            InitializeComponent();
        }

        private void CurrentPathInit()
        {
            currentPath = CommonValues.path.Trim() + CommonValues.rootFolder;
            msWordPath = CommonValues.pathToMsWord;
            matlabPath = CommonValues.pathToMatlab;

            regFileWorker = new RegFile(regFileName, keyRegStr, currentPath, msWordPath, matlabPath, pathPlaceholder, pathMSWordPlaceholder, pathMatlabPlaceholder);
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            CurrentPathInit();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            //try
            //{
                const string message = "Перед внесением изменений в реестр рекомендуется создать резервную копию реестра.\nСоздать резервную копию реестра?";
                var toExportReg = MessageBox.Show($"{message}", "", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

                if (toExportReg == MessageBoxResult.Yes)
                {
                    Task task = Task.Factory.StartNew(() =>
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            progressText.Text += "Зачекайте, створюється резервна копія реєстра ...\n";
                            mainWindow.Closing += CancelWindowClosing;
                        });

                        string regExpFileName = RegExportFileName();
                        if (regExpFileName != null)
                        {
                            RegWorker.exportRegistry(regExpFileName);
                        }

                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            progressText.Text += "Резервна копія створена\n";
                            mainWindow.Closing -= CancelWindowClosing;
                        });
                        progressText.Text += "Внесення змін в реєстр...\n";
                        MakeChangesInRegistry();
                    });
                }
                else
                {
                    progressText.Text += "Внесення змін в реєстр...\n";
                    MakeChangesInRegistry();
                }
                progressText.Text += "Готово!\n";
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"{ex.Message}", "", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void CancelWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void MakeChangesInRegistry()
        {
            regFileWorker.ChangeRegistry();
        }

        private string RegExportFileName()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Файли реєстра (*.reg)|*.reg";
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            else return null;
        }
    }
}
