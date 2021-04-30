using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

namespace DJMAX_Record_Keeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ScoreRecord> scoreCollection = new ObservableCollection<ScoreRecord>();
        ICollectionView ScoreList;
        const string recordsFile = "records.json";

        public MainWindow()
        {
            InitializeComponent();
            //Attempt to deserialize json if it exists in base folder directory
            if (File.Exists(recordsFile))
            {
                string json = File.ReadAllText(recordsFile);
                var saved = JsonSerializer.Deserialize<ObservableCollection<ScoreRecord>>(json);
                foreach (var record in saved)
                {
                    scoreCollection.Add(record);
                }
                txtMsg.Text = ("Successfully read saved records from disk.");
            }

            //Set defaults to song name and date
            txtName.Text = "Ask to Wind";
            dpDate.Value = new DateTime(2019, 12, 18);

            //Set-up collection views
            var scoreSourceList = new CollectionViewSource { Source = scoreCollection };
            ScoreList = scoreSourceList.View;
            //Create Predicate and add to ScoreList before binding it to DataGrid
            var patternFilter = new Predicate<object>(pattern => ((ScoreRecord)pattern).PatternName.ToLower().Contains(txtSearch.Text.ToLower()));
            ScoreList.Filter = patternFilter;
            dgRecords.ItemsSource = ScoreList;
        }

        //Enforce non-null values for date by defaulting to EA start date
        private void check_Date(object sender, RoutedEventArgs e)
        {
            if (dpDate.Value == null)
            {
                dpDate.Value = new DateTime(2019, 12, 18);
            }
        }

        //Open folders window
        private void folder_Click(object sender, RoutedEventArgs e)
        {
            FolderWindow folder = new FolderWindow();
            folder.Owner = this;
            folder.Show();
        }

        //Add score
        private void add_Click(object sender, RoutedEventArgs e)
        {
            //Null value check
            if (txtName.Text == "" || dpDate.Value == null)
            {
                txtMsg.Text = ("A blank field was detected.\n" +
                    "Please fill in the blank(s) with a value.");
                return;
            }

            //Variable definitions
            string song = txtName.Text;
            //Here we probe for the radio option selected in both stack groups and ensure they have values
            string selMode = stkMode.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Content.ToString();
            string selDiff = stkDiff.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Content.ToString();
            int score = (int)(intScore.Value);
            double rate = (double)(dblRate.Value);
            int breaks = (int)(intBreak.Value);
            DateTime scoreDate = (DateTime)(dpDate.Value);

            //Create ScoreRecord object and add to scoreCollection
            ScoreRecord record = new ScoreRecord(song, selMode, selDiff, score, rate, breaks, scoreDate);
            scoreCollection.Add(record);
            txtMsg.Text = ("Record successfully added.");
        }

        //Reset all fields to default values
        private void reset_Click(object sender, RoutedEventArgs e)
        {
            txtName.Text = "Ask to Wind";

            rdo4.IsChecked = true;
            rdoNm.IsChecked = true;

            intScore.Value = 0;
            dblRate.Value = 0.0;
            intBreak.Value = 0;

            dpDate.Value = new DateTime(2019, 12, 18);

            txtMsg.Text = ("All fields have been reset to their defaults.");
        }

        //Refresh list for search queries
        private void searchRecords(object sender, RoutedEventArgs e)
        {
            ScoreList.Refresh();
        }

        //Save all records to JSON
        private void save_Click(object sender, RoutedEventArgs e)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            string json = JsonSerializer.Serialize(scoreCollection, options);
            File.WriteAllText(recordsFile, json);
            txtNotif.Text = "Records have been saved to disk.";
        }

        //Delete record after accepting warning
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmDel = MessageBox.Show("Are you sure you want to delete this record?\n" +
                "This cannot be undone!", "Confirm Record Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(confirmDel == MessageBoxResult.Yes)
            {
                scoreCollection.Remove(dgRecords.SelectedItem as ScoreRecord);
            }
            txtNotif.Text = "Record has been deleted.";
        }
        
    }
}
