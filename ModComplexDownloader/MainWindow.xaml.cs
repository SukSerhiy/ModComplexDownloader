using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModComplexDownloader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Page> pagesSeuquence;
        static int currentPageIndex;
        public MainWindow()
        {
            InitializeComponent();
            currentPageIndex = 0;
            pagesSeuquence = new List<Page>(new Page[] { new ChoosePathPage(), /*new DownloaderPage(),*/ new RegistryChangerPage(), new GreetingPage() });
            Main.Content = pagesSeuquence[0];
            MovePrevBtn.IsEnabled = false;
        }

        private void MoveNextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex < pagesSeuquence.Count - 1)
            {
                if (!MovePrevBtn.IsEnabled)
                    MovePrevBtn.IsEnabled = true;
                Main.Content = pagesSeuquence[++currentPageIndex];
                if (currentPageIndex == pagesSeuquence.Count - 1)
                {
                    MoveNextBtn.IsEnabled = false;
                }
            }
            else if (currentPageIndex == pagesSeuquence.Count - 1)
            {
                if (MoveNextBtn.IsEnabled)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void MovePrevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex > 0)
            {
                if (!MoveNextBtn.IsEnabled)
                    MoveNextBtn.IsEnabled = true;
                Main.Content = pagesSeuquence[--currentPageIndex];
                if (currentPageIndex == 0)
                    MovePrevBtn.IsEnabled = false;
            }
        }
    }
}
