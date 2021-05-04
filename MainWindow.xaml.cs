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
        //Globals
        public ObservableCollection<ScoreRecord> scoreCollection = new();
        ICollectionView scoreList;
        ICollectionView filterList;
        const string recordsFile = "Records.json";
        const string songsFile = "SongData.json";
        public static bool isRefresh = false;
        public static List<string> folderList = new() {"RP", "P1", "P2", "VE", "ES", "TR", "CE", "BS", "T1", "T2", "T3", "P3", "GG", "GC", "DM", "CY", "GF", "CHU"};
        public static List<bool> settingList = new()
        {
            Folder.Default.Respect,
            Folder.Default.Portable1,
            Folder.Default.Portable2,
            Folder.Default.VExtension,
            Folder.Default.EmotionalSense,
            Folder.Default.Trilogy,
            Folder.Default.Clazziquai,
            Folder.Default.BlackSquare,
            Folder.Default.Technika1,
            Folder.Default.Technika2,
            Folder.Default.Technika3,
            Folder.Default.Portable3,
            Folder.Default.GuiltyGear,
            Folder.Default.GrooveCoaster,
            Folder.Default.Deemo,
            Folder.Default.Cytus,
            Folder.Default.Frontline,
            Folder.Default.Chunithm
        };
        public static ObservableCollection<Song> masterSongCollection = new();
        public static ObservableCollection<Song> filterSongCollection = new();

        public MainWindow()
        {
            InitializeComponent();

            //Attempt to deserialize Records.json if it exists in base folder directory
            if (File.Exists(recordsFile))
            {
                string json = File.ReadAllText(recordsFile);
                var saved = JsonSerializer.Deserialize<ObservableCollection<ScoreRecord>>(json);
                foreach (var record in saved)
                {
                    scoreCollection.Add(record);
                }
                TextMessage.Text = ("Successfully read saved records from disk.\n");
            }

            //Deserialize SongData.json to masterSongCollection
            if (File.Exists(songsFile))
            {
                string songs = File.ReadAllText(songsFile);
                var songsMaster = JsonSerializer.Deserialize<ObservableCollection<Song>>(songs);
                foreach (var song in songsMaster)
                {
                    masterSongCollection.Add(song);
                }
            }
            else
            {
                MessageBoxResult songDataNotFound = MessageBox.Show("SongData.json was not found. Please do not touch that file.\n" +
                "If you deleted it, redownload it from the github repo.\n" +
                "The program will now terminate.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            //Load folder settings to filterSongCollection
            loadFilters();

            //Set default date
            PickedDate.Value = new DateTime(2019, 12, 18);

            //Set-up collection views
            var scoreSourceList = new CollectionViewSource { Source = scoreCollection };
            scoreList = scoreSourceList.View;
            var songFilterList = new CollectionViewSource { Source = filterSongCollection };
            filterList = songFilterList.View;
            
            //Create Predicate and add to ScoreList before binding it to DataGridRecords
            var patternFilter = new Predicate<object>(pattern => ((ScoreRecord)pattern).PatternName.ToLower().Contains(TextSearch.Text.ToLower()));
            scoreList.Filter = patternFilter;
            DataGridRecords.ItemsSource = scoreList;

            //Set sorting by song title, then bind ComboTitle
            filterList.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            ComboTitle.ItemsSource = filterList;
        }

        //Retrieve songs from the specified series in masterSongCollection if the other condition is satisfied
        public static void getSongs(string series, bool condition)
        {
            foreach (Song s in MainWindow.masterSongCollection) if (s.Series == series && condition)
                    MainWindow.filterSongCollection.Add(s);
        }

        //Add songs from masterSongCollection based on previous settings
        private void loadFilters()
        {
            for (int i = 0; i < folderList.Count; i += 1)
                getSongs(folderList[i], settingList[i]);
        }

        //Enforce non-null values for date by defaulting to EA start date
        private void CheckDate(object sender, RoutedEventArgs e)
        {
            if (PickedDate.Value == null)
            {
                PickedDate.Value = new DateTime(2019, 12, 18);
            }
        }

        //Open folders window
        private void FolderClick(object sender, RoutedEventArgs e)
        {
            FolderWindow folder = new();
            folder.Owner = this;
            folder.ShowDialog();

            //Reset ComboTitle to first item if folders were updated
            if (isRefresh)
            {
                ComboTitle.SelectedIndex = 0;
                isRefresh = false;
            }
        }

        //Add score
        private void AddClick(object sender, RoutedEventArgs e)
        {
            //Null value check
            if (ComboTitle.Text == "" || PickedDate.Value == null)
            {
                TextMessage.Text = ("A blank field was detected.\n" +
                    "Please fill in the blank(s) with a value.");
                return;
            }

            //Variable definitions
            string title = ComboTitle.Text;
            //Here we probe for the radio option selected in both stack groups and ensure they have values
            string selMode = StackMode.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Content.ToString();
            string selDiff = StackDifficulty.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Content.ToString();
            //Generate pattern string
            string pattern = title + " " + selMode + selDiff;

            //filterSongCollection was never sorted, so we need to query the right metadata via LINQ
            string artist = filterSongCollection.FirstOrDefault(s => s.Title == title).Artist;
            string series = filterSongCollection.FirstOrDefault(s => s.Title == title).Series;
            int score = (int)(IntegerScore.Value);
            double rate = (double)(DoubleRate.Value);
            int breaks = (int)(IntegerBreak.Value);
            DateTime scoreDate = (DateTime)(PickedDate.Value);

            //Create ScoreRecord object and add to scoreCollection
            ScoreRecord record = new(title, pattern, artist, series, score, rate, breaks, scoreDate);
            scoreCollection.Add(record);
            TextMessage.Text = ("Record successfully added.");
        }

        //Reset all fields to default values
        private void ResetClick(object sender, RoutedEventArgs e)
        {
            ComboTitle.SelectedIndex = 0;

            Radio4.IsChecked = true;
            RadioNM.IsChecked = true;

            IntegerScore.Value = 0;
            DoubleRate.Value = 0.0;
            IntegerBreak.Value = 0;

            PickedDate.Value = new DateTime(2019, 12, 18);

            TextMessage.Text = ("All fields have been reset to their defaults.");
        }

        //Refresh list for search queries
        private void SearchRecords(object sender, RoutedEventArgs e)
        {
            scoreList.Refresh();
        }

        //Save all records to JSON
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            string json = JsonSerializer.Serialize(scoreCollection, options);
            File.WriteAllText(recordsFile, json);
            TextNotification.Text = "Records have been saved to disk.";
        }

        //Delete record after accepting warning
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmDel = MessageBox.Show("Are you sure you want to delete this record?\n" +
                "This cannot be undone!", "Confirm Record Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(confirmDel == MessageBoxResult.Yes)
            {
                scoreCollection.Remove(DataGridRecords.SelectedItem as ScoreRecord);
            }
            TextNotification.Text = "Record has been deleted.";
        }
        
    }
}
