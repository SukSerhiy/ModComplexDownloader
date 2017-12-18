using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModComplexDownloader
{
    /// <summary>
    /// Логика взаимодействия для GreetingPage.xaml
    /// </summary>
    public partial class GreetingPage : Page
    {
        public GreetingPage()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MoveNextBtn.IsEnabled = true;
            mainWindow.MoveNextBtn.Content = "Готово";
        }
    }
}   
