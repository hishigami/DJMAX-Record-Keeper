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
        private Song selectedSong;
        public List<bool> settingList = new()
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
        public static List<string> folderList = new() {"RP", "P1", "P2", "VE", "ES", "TR", "CE", "BS", "T1", "T2", "T3", "P3", "GG", "GC", "DM", "CY", "GF", "CHU"};
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
            LoadFilters();

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

            //Set difficulties for the initial song
            CheckDifficulties(StackMode.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value));
        }

        //Retrieve songs from the specified series in masterSongCollection if the other condition is satisfied
        public static void GetSongs(string series, bool condition)
        {
            foreach (Song s in MainWindow.masterSongCollection) if (s.Series == series && condition)
                    MainWindow.filterSongCollection.Add(s);
        }

        //Add songs from masterSongCollection based on previous settings
        private void LoadFilters()
        {
            for (int i = 0; i < folderList.Count; i += 1)
                GetSongs(folderList[i], settingList[i]);
        }

        //Reset selected difficulty to NM after changing songs and update selectable difficulties
        private void ChangeSong(object sender, EventArgs e)
        {
            RadioNM.IsChecked = true;
            CheckDifficulties(StackMode.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value));
        }

        //Prevent non-existent difficulties from being selected by the user
        private void CheckDifficulties(RadioButton mode)
        {
            //Query selected song via LINQ
            selectedSong = filterSongCollection.FirstOrDefault(s => s.Title == ComboTitle.Text);
            //Do not run function on init
            if (selectedSong != null)
            {
                //Disable HD, MX, and SC options
                RadioHD.IsEnabled = false;
                RadioMX.IsEnabled = false;
                RadioSC.IsEnabled = false;

                /* Main switch case body
                 * Check the selected song's difficulty availability from its properties based on the selected mode
                 * Note that all songs have a NM for each mode, so the existence of a NM never needs to be verified
                 */
                switch (mode.Name)
                {
                    case "Radio4":
                        if (selectedSong.FourHD)
                            RadioHD.IsEnabled = true;
                        if (selectedSong.FourMX)
                            RadioMX.IsEnabled = true;
                        if (selectedSong.FourSC)
                            RadioSC.IsEnabled = true;
                        break;
                    case "Radio5":
                        if (selectedSong.FiveHD)
                            RadioHD.IsEnabled = true;
                        if (selectedSong.FiveMX)
                            RadioMX.IsEnabled = true;
                        if (selectedSong.FiveSC)
                            RadioSC.IsEnabled = true;
                        break;
                    case "Radio6":
                        if (selectedSong.SixHD)
                            RadioHD.IsEnabled = true;
                        if (selectedSong.SixMX)
                            RadioMX.IsEnabled = true;
                        if (selectedSong.SixSC)
                            RadioSC.IsEnabled = true;
                        break;
                    case "Radio8":
                        if (selectedSong.EightHD)
                            RadioHD.IsEnabled = true;
                        if (selectedSong.EightMX)
                            RadioMX.IsEnabled = true;
                        if (selectedSong.EightSC)
                            RadioSC.IsEnabled = true;
                        break;
                    default:
                        TextMessage.Text = "Something went wrong.";
                        break;
                }
            }
        }

        //Update selectable difficulties upon changing modes
        private void ModeClick(object sender, RoutedEventArgs e)
        {
            var mode = sender as RadioButton;
            CheckDifficulties(mode);
            
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

            /* Reset ComboTitle to first item if folders were updated
             * Difficulty also needs to be reset to NM and selectable difficulties are to be verified for the updated list's top item
             */
            if (isRefresh)
            {
                ComboTitle.SelectedIndex = 0;
                RadioNM.IsChecked = true;
                CheckDifficulties(StackMode.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value));
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

            //Query the matching song object via LINQ
            selectedSong = filterSongCollection.FirstOrDefault(s => s.Title == ComboTitle.Text);

            //Variable definitions
            string title = selectedSong.Title;
            //Here we probe for the radio option selected in both stack groups and ensure they have values
            string selMode = StackMode.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Content.ToString();
            string selDiff = StackDifficulty.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Content.ToString();
            //Generate pattern string
            string pattern = title + " " + selMode + selDiff;
            string artist = selectedSong.Artist;
            string series = selectedSong.Series;
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

            CheckDifficulties(StackMode.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value));

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
