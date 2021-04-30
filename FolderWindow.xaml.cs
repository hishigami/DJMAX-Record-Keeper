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
        //Folders constants
        public const int completeCheck = 18;

        public FolderWindow()
        {
            InitializeComponent();
        }

        //Select all checkboxes
        private void SelectClick(object sender, RoutedEventArgs e)
        {
            var baseChecks = StackBase.Children.OfType<CheckBox>();
            var dlcChecks = WrapDLC.Children.OfType<CheckBox>();
            var collabChecks = WrapCollabs.Children.OfType<CheckBox>();
            foreach (CheckBox c in baseChecks)
                c.IsChecked = true;
            foreach (CheckBox c in dlcChecks)
                c.IsChecked = true;
            foreach (CheckBox c in collabChecks)
                c.IsChecked = true;
        }

        //Deselect all checkboxes
        private void DeselectClick(object sender, RoutedEventArgs e)
        {
            var baseChecks = StackBase.Children.OfType<CheckBox>();
            var dlcChecks = WrapDLC.Children.OfType<CheckBox>();
            var collabChecks = WrapCollabs.Children.OfType<CheckBox>();
            foreach (CheckBox c in baseChecks)
                c.IsChecked = false;
            foreach (CheckBox c in dlcChecks)
                c.IsChecked = false;
            foreach (CheckBox c in collabChecks)
                c.IsChecked = false;
        }

        //Update filterSongCollection based on the chosen entries
        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            //Count all checked checkboxes
            var baseChecked = StackBase.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
            var dlcChecked = WrapDLC.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
            var collabChecked = WrapCollabs.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);

            //Prevent user from updating if none of the checkboxes are selected
            if (baseChecked.Count() + dlcChecked.Count() + collabChecked.Count() == 0)
            {
                MessageBoxResult noChecked = MessageBox.Show("Please select at least one folder to include.",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Purge previous list
            MainWindow.filterSongCollection.Clear();
            
            //Add songs from master to filter based on selected check boxes
            foreach (Song s in MainWindow.masterSongCollection) if (s.Series == "RP" && (bool)CheckRespect.IsChecked)
                    MainWindow.filterSongCollection.Add(s);

            MessageBoxResult confirmUpdate = MessageBox.Show("Successfully updated song title filters.", 
                "Update filters", MessageBoxButton.OK, MessageBoxImage.Information);

            Owner.Activate();
            Close();
        }

        //Discard changes
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
