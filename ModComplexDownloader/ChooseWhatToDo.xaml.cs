using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModComplexDownloader
{
    /// <summary>
    /// Логика взаимодействия для ChooseWhatToDo.xaml
    /// </summary>
    public partial class ChooseWhatToDo : Page
    {
        private HashSet<StepOfSetting> stepsOfSettings = CommonValues.stepsOfSetting;
        private List<Page> pagesSeuquence = CommonValues.pagesSeuquence;
        public ChooseWhatToDo()
        {
            InitializeComponent();
        }

        private void chooseAllCheckbox_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine(chooseAllCheckbox.IsChecked);
            //if (chooseAllCheckbox.IsChecked == true)
            //{
                
            //    chooseDownloadCheckbox.IsChecked = true;
            //    chooseRegCheckbox.IsChecked = true;
            //    chooseAllCheckbox.IsChecked = null;
            //    if (!(stepsOfSettings.Contains(StepOfSetting.DownloadFromGDrive) && stepsOfSettings.Contains(StepOfSetting.ChangeRegister)))
            //    {
            //        CommonValues.pagesSeuquence.Add(CommonValues.downloaderPage);
            //        CommonValues.pagesSeuquence.Add(CommonValues.registryChangerPage);
            //    }
            //}
            //else
            //{
            //    chooseDownloadCheckbox.IsChecked = false;
            //    chooseRegCheckbox.IsChecked = false;
            //    chooseAllCheckbox.IsChecked = false;
            //    CommonValues.pagesSeuquence.Remove(CommonValues.downloaderPage);
            //    CommonValues.pagesSeuquence.Remove(CommonValues.registryChangerPage);
            //}
        }

        private void chooseDownloadCheckbox_Click(object sender, RoutedEventArgs e)
        {
            //var cb = e.Source as System.Windows.Controls.CheckBox;
            //if (!cb.IsChecked.HasValue)
            //{
            //    if (stepsOfSettings.Contains(StepOfSetting.DownloadFromGDrive))
            //        stepsOfSettings.Remove(StepOfSetting.DownloadFromGDrive);
            //    if (CommonValues.pagesSeuquence.Contains(CommonValues.downloaderPage))
            //    {
            //        CommonValues.pagesSeuquence.Remove(CommonValues.downloaderPage);
            //    }
            //    cb.IsChecked = false;
            //}
            //else
            //{
            //    if (!stepsOfSettings.Contains(StepOfSetting.DownloadFromGDrive))
            //        stepsOfSettings.Add(StepOfSetting.DownloadFromGDrive);
            //    if (!CommonValues.pagesSeuquence.Contains(CommonValues.downloaderPage))
            //    {
            //        CommonValues.pagesSeuquence.Add(CommonValues.downloaderPage);
            //    }
            //}
        }

        private void chooseRegCheckbox_Click(object sender, RoutedEventArgs e)
        {
            //var cb = e.Source as System.Windows.Controls.CheckBox;
            //if (!cb.IsChecked.HasValue)
            //{
            //    if (stepsOfSettings.Contains(StepOfSetting.ChangeRegister))
            //        stepsOfSettings.Remove(StepOfSetting.ChangeRegister);
            //    if (CommonValues.pagesSeuquence.Contains(CommonValues.registryChangerPage))
            //    {
            //        CommonValues.pagesSeuquence.Remove(CommonValues.registryChangerPage);
            //    }
            //    cb.IsChecked = false;
            //}
            //else
            //{
            //    if (!stepsOfSettings.Contains(StepOfSetting.ChangeRegister))
            //        stepsOfSettings.Add(StepOfSetting.ChangeRegister);
            //    if (!CommonValues.pagesSeuquence.Contains(CommonValues.registryChangerPage))
            //    {
            //        CommonValues.pagesSeuquence.Add(CommonValues.registryChangerPage);
            //    }
            //}
        }
    }
}
