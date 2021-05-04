using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace DJMAX_Record_Keeper
{
    /// <summary>
    /// Interaction logic for FolderWindow.xaml
    /// </summary>
    public partial class FolderWindow : Window
    {
        //Global
        public ObservableCollection<CheckBox> checkCollection = new();

        public FolderWindow()
        {
            InitializeComponent();

            //Populate checkCollection by reference
            checkCollection.Add(CheckRespect);
            checkCollection.Add(CheckPortable1);
            checkCollection.Add(CheckPortable2);

            checkCollection.Add(CheckVExtension);
            checkCollection.Add(CheckEmotionalSense);
            checkCollection.Add(CheckTrilogy);
            checkCollection.Add(CheckClazziquai);
            checkCollection.Add(CheckBlackSquare);
            checkCollection.Add(CheckTechnika1);
            checkCollection.Add(CheckTechnika2);
            checkCollection.Add(CheckTechnika3);
            checkCollection.Add(CheckPortable3);

            checkCollection.Add(CheckGuiltyGear);
            checkCollection.Add(CheckGrooveCoaster);
            checkCollection.Add(CheckDeemo);
            checkCollection.Add(CheckCytus);
            checkCollection.Add(CheckFrontline);
            checkCollection.Add(CheckChunithm);

            //Load user settings from memory
            LoadSettings();
        }

        //Select all checkboxes
        private void SelectClick(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox c in checkCollection)
                c.IsChecked = true;
        }

        //Deselect all checkboxes
        private void DeselectClick(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox c in checkCollection)
                c.IsChecked = false;
        }

        //Update filterSongCollection based on the chosen entries
        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            //Count all checked checkboxes
            int totalChecked = checkCollection.Where(x => x.IsChecked == true).Count();

            //Prevent user from updating if none of the checkboxes are selected
            if (totalChecked == 0)
            {
                MessageBoxResult noChecked = MessageBox.Show("Please select at least one folder to include.",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Purge previous list
            MainWindow.filterSongCollection.Clear();

            //Add songs from master to filter based on selected check boxes
            for(int c = 0; c < checkCollection.Count(); c += 1)
                MainWindow.getSongs(MainWindow.folderList[c], (bool)checkCollection[c].IsChecked);

            //Save all set checkboxes
            SaveSettings();

            MessageBoxResult confirmUpdate = MessageBox.Show("Successfully updated song title filters.", 
                "Update filters", MessageBoxButton.OK, MessageBoxImage.Information);
            
            MainWindow.isRefresh = true;
            Owner.Activate();
            Close();
        }

        //Discard changes
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Apply settings to each checkbox
        private void LoadSettings()
        {
            for (int c = 0; c < checkCollection.Count(); c += 1)
                checkCollection[c].IsChecked = MainWindow.settingList[c];
        }

        //Save checked settings
        private void SaveSettings()
        {
            for (int c = 0; c < checkCollection.Count(); c += 1)
                MainWindow.settingList[c] = (bool)checkCollection[c].IsChecked;

            Folder.Default.Save();
        }
    }
}
