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
    public partial class FolderView : Window
    {
        //Global
        public ObservableCollection<CheckBox> checkCollection = new();

        public FolderView()
        {
            InitializeComponent();

            //Populate checkCollection by reference
            //Base game
            checkCollection.Add(CheckRespect);
            checkCollection.Add(CheckPortable1);
            checkCollection.Add(CheckPortable2);
            //DLCs
            checkCollection.Add(CheckVExtension);
            checkCollection.Add(CheckEmotionalSense);
            checkCollection.Add(CheckTrilogy);
            checkCollection.Add(CheckClazziquai);
            checkCollection.Add(CheckBlackSquare);
            checkCollection.Add(CheckTechnika1);
            checkCollection.Add(CheckTechnika2);
            checkCollection.Add(CheckTechnika3);
            checkCollection.Add(CheckPortable3);
            checkCollection.Add(CheckVExtension2);
            checkCollection.Add(CheckTechnikaTQ);
            //Collabs
            checkCollection.Add(CheckGuiltyGear);
            checkCollection.Add(CheckGrooveCoaster);
            checkCollection.Add(CheckDeemo);
            checkCollection.Add(CheckCytus);
            checkCollection.Add(CheckFrontline);
            checkCollection.Add(CheckChunithm);
            checkCollection.Add(CheckEstimate);
            checkCollection.Add(CheckNexon);
            checkCollection.Add(CheckMuseDash);

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
            MainView.filterSongCollection.Clear();

            //Add songs from master to filter based on selected check boxes
            for(int c = 0; c < checkCollection.Count(); c += 1)
                MainView.GetSongs(MainView.folderList[c], (bool)checkCollection[c].IsChecked);

            //Save all set checkboxes
            SaveSettings();

            MessageBoxResult confirmUpdate = MessageBox.Show("Successfully updated song title filters.", 
                "Update filters", MessageBoxButton.OK, MessageBoxImage.Information);
            
            MainView.isRefresh = true;
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
            //Base game
            checkCollection[0].IsChecked = Folder.Default.Respect;
            checkCollection[1].IsChecked = Folder.Default.Portable1;
            checkCollection[2].IsChecked = Folder.Default.Portable2;
            //DLCs
            checkCollection[3].IsChecked = Folder.Default.VExtension;
            checkCollection[4].IsChecked = Folder.Default.EmotionalSense;
            checkCollection[5].IsChecked = Folder.Default.Trilogy;
            checkCollection[6].IsChecked = Folder.Default.Clazziquai;
            checkCollection[7].IsChecked = Folder.Default.BlackSquare;
            checkCollection[8].IsChecked = Folder.Default.Technika1;
            checkCollection[9].IsChecked = Folder.Default.Technika2;
            checkCollection[10].IsChecked = Folder.Default.Technika3;
            checkCollection[11].IsChecked = Folder.Default.Portable3;
            checkCollection[12].IsChecked = Folder.Default.VExtension2;
            checkCollection[13].IsChecked = Folder.Default.TechnikaTQ;
            //Collabs
            checkCollection[14].IsChecked = Folder.Default.GuiltyGear;
            checkCollection[15].IsChecked = Folder.Default.GrooveCoaster;
            checkCollection[16].IsChecked = Folder.Default.Deemo;
            checkCollection[17].IsChecked = Folder.Default.Cytus;
            checkCollection[18].IsChecked = Folder.Default.Frontline;
            checkCollection[19].IsChecked = Folder.Default.Chunithm;
            checkCollection[20].IsChecked = Folder.Default.Estimate;
            checkCollection[21].IsChecked = Folder.Default.Nexon;
            checkCollection[22].IsChecked = Folder.Default.MuseDash;
        }

        //Save checked settings
        private void SaveSettings()
        {
            //Base game
            Folder.Default.Respect = (bool)checkCollection[0].IsChecked;
            Folder.Default.Portable1 = (bool)checkCollection[1].IsChecked;
            Folder.Default.Portable2 = (bool)checkCollection[2].IsChecked;
            //DLCs
            Folder.Default.VExtension = (bool)checkCollection[3].IsChecked;
            Folder.Default.EmotionalSense = (bool)checkCollection[4].IsChecked;
            Folder.Default.Trilogy = (bool)checkCollection[5].IsChecked;
            Folder.Default.Clazziquai = (bool)checkCollection[6].IsChecked;
            Folder.Default.BlackSquare = (bool)checkCollection[7].IsChecked;
            Folder.Default.Technika1 = (bool)checkCollection[8].IsChecked;
            Folder.Default.Technika2 = (bool)checkCollection[9].IsChecked;
            Folder.Default.Technika3 = (bool)checkCollection[10].IsChecked;
            Folder.Default.Portable3 = (bool)checkCollection[11].IsChecked;
            Folder.Default.VExtension2 = (bool)checkCollection[12].IsChecked;
            Folder.Default.TechnikaTQ = (bool)checkCollection[13].IsChecked;
            //Collabs
            Folder.Default.GuiltyGear = (bool)checkCollection[14].IsChecked;
            Folder.Default.GrooveCoaster = (bool)checkCollection[15].IsChecked;
            Folder.Default.Deemo = (bool)checkCollection[16].IsChecked;
            Folder.Default.Cytus = (bool)checkCollection[17].IsChecked;
            Folder.Default.Frontline = (bool)checkCollection[18].IsChecked;
            Folder.Default.Chunithm = (bool)checkCollection[19].IsChecked;
            Folder.Default.Estimate = (bool)checkCollection[20].IsChecked;
            Folder.Default.Nexon = (bool)checkCollection[21].IsChecked;
            Folder.Default.MuseDash = (bool)checkCollection[22].IsChecked;

            Folder.Default.Save();
        }
    }
}
