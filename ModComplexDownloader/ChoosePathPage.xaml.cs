using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ModComplexDownloader
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class ChoosePathPage : Page
    {
        private static string path;
        public static string GetPath
        {
            get
            {
                return path;
            }
        }
        public ChoosePathPage()
        {
            InitializeComponent();
            path = CommonValues.defaultPath;
            pathTextBox.Text = path;
        }

        private void pathChooseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    path = dialog.SelectedPath;
                    CommonValues.path = path;
                    pathTextBox.Text = path;
                }
            }
        }

        private void pathWordChooseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "exe-файлы|*.exe";
                dialog.CheckFileExists = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = dialog.FileName;
                    pathWordTextBox.Text = fileName;
                    CommonValues.pathToMsWord = fileName;
                }
            }
        }

        private void pathMatlabChooseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "exe-файлы|*.exe";
                dialog.CheckFileExists = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = dialog.FileName;
                    pathMatlabTextBox.Text = fileName;
                    CommonValues.pathToMatlab = fileName;
                }
            }
        }
    }
}
