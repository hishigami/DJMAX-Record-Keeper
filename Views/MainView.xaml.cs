using DJMAX_Record_Keeper.DataTypes;
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
    public partial class MainView : Window
    {
        //Globals
        public ObservableCollection<ScoreRecord> scoreCollection = new();
        ICollectionView scoreList;
        ICollectionView filterList;
        const string recordsFile = "Data/RecordData.json";
        const string songsFile = "Data/SongData.json";
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
            Folder.Default.VExtension2,
            Folder.Default.GuiltyGear,
            Folder.Default.GrooveCoaster,
            Folder.Default.Deemo,
            Folder.Default.Cytus,
            Folder.Default.Frontline,
            Folder.Default.Chunithm,
            Folder.Default.Estimate,
            Folder.Default.Nexon,
            Folder.Default.MuseDash
        };
        public static List<string> folderList = new() {"RP", "P1", "P2", "VE", "ES", "TR", "CE", "BS", "T1", "T2", "T3", "P3", "VE2", "GG", "GC", "DM", "CY", "GF", "CHU", "ESTI", "NXN", "MD"};
        public static ObservableCollection<Song> masterSongCollection = new();
        public static ObservableCollection<Song> filterSongCollection = new();

        public MainView()
        {
            InitializeComponent();

            //Attempt to deserialize RecordData.json if it exists in base folder directory
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

            //Set date on load to today
            PickedDate.Value = DateTime.Today;

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

        /// <summary>
        /// Retrieve songs from the specified category in masterSongCollection if the appropriate setting is enabled.
        /// </summary>
        /// <param name="category">Song's category</param>
        /// <param name="condition">Category setting</param>
        public static void GetSongs(string category, bool condition)
        {
            foreach (Song s in masterSongCollection) if (s.Category == category && condition)
                    filterSongCollection.Add(s);
        }

        /// <summary>
        /// Removes Respect/Link Disc/V Link songs if their DLC ownership conditions are not satisfied.
        /// </summary>
        private void FilterSpecialSongs()
        {
            //Respect originals
            if (!Folder.Default.Trilogy)
            {
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "Nevermind"));
            }
            if (!Folder.Default.Clazziquai)
            {
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "Rising The Sonic"));
            }
            if (!Folder.Default.BlackSquare)
            {
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "ANALYS"));
            }
            if (!Folder.Default.Technika1)
            {
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "Do you want it"));
            }
            if (!Folder.Default.Technika2)
            {
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "End of Mythology"));
            }
            if (!Folder.Default.Technika3)
            {
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "ALiCE"));
            }
            if (!Folder.Default.Portable3)
            {
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "glory day (Mintorment Remix)"));
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "glory day -JHS Remix-"));
            }

            //Link Disc
            if (!Folder.Default.BlackSquare && !Folder.Default.Technika1)
            {
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "Here in the Moment ~Extended Mix~"));
            }
            if (!Folder.Default.Clazziquai && !Folder.Default.Technika1)
            {
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "Airwave ~Extended Mix~"));
            }
            if (!Folder.Default.BlackSquare && !Folder.Default.Clazziquai)
            {
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "SON OF SUN ~Extended Mix~"));
            }

            //V Link
            if (!Folder.Default.VExtension)
            {
                filterSongCollection.Remove(filterSongCollection.FirstOrDefault(s => s.Title == "Flowering ~Original Ver.~"));
            }
        }

        /// <summary>
        /// Add songs from masterSongCollection based on previous settings, then removes special songs as necessary.
        /// </summary>
        private void LoadFilters()
        {
            for (int i = 0; i < folderList.Count; i += 1)
                GetSongs(folderList[i], settingList[i]);
            FilterSpecialSongs();
        }

        /// <summary>
        /// Defaults the selected difficulty to NM if the previously selected difficulty doesn't actually exist for the current song and mode.
        /// </summary>
        private void DefaultToNM()
        {
            
            var selDiff = StackDifficulty.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            if (selDiff.IsEnabled == false)
            {
                RadioNM.IsChecked = true;
            }
        }

        /// <summary>
        /// Updates selectable difficulties after changing songs.
        /// </summary>
        private void ChangeSong(object sender, EventArgs e)
        {
            CheckDifficulties(StackMode.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value));
            DefaultToNM();
        }

        /// <summary>
        /// Disables non-existent difficulties for the chosen song and mode from being selected.
        /// </summary>
        /// <param name="mode">The current mode to check difficulties against.</param>
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

        /// <summary>
        /// Updates selectable difficulties upon changing modes.
        /// </summary>
        private void ModeClick(object sender, RoutedEventArgs e)
        {
            var mode = sender as RadioButton;
            CheckDifficulties(mode);
            DefaultToNM();
        }

        /// <summary>
        /// Defaults the date field to Respect V's EA start date if it's left blank by the user.
        /// </summary>
        private void CheckDate(object sender, RoutedEventArgs e)
        {
            if (PickedDate.Value == null)
            {
                PickedDate.Value = new DateTime(2019, 12, 18);
            }
        }

        /// <summary>
        /// Opens the folders window.
        /// </summary>
        private void FolderClick(object sender, RoutedEventArgs e)
        {
            FolderView folder = new();
            folder.Owner = this;
            folder.ShowDialog();

            /* Reset ComboTitle to first item if folders were updated after checking on special songs
             * Difficulty also needs to be reset to NM and selectable difficulties are to be verified for the updated list's top item
             */
            if (isRefresh)
            {
                FilterSpecialSongs();
                ComboTitle.SelectedIndex = 0;
                CheckDifficulties(StackMode.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value));
                DefaultToNM();
                isRefresh = false;
            }
        }

        /// <summary>
        /// Adds the score record using all fields' values to the score list.
        /// </summary>
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
            string category = selectedSong.Category;
            int score = (int)(IntegerScore.Value);
            double rate = (double)(DoubleRate.Value);
            int breaks = (int)(IntegerBreak.Value);
            DateTime scoreDate = (DateTime)(PickedDate.Value);

            //Create ScoreRecord object and add to scoreCollection
            ScoreRecord record = new(title, pattern, artist, category, score, rate, breaks, scoreDate);
            scoreCollection.Add(record);
            TextMessage.Text = ("Record successfully added.");
        }

        /// <summary>
        /// Resets all fields to their default values.
        /// </summary>
        private void ResetClick(object sender, RoutedEventArgs e)
        {
            ComboTitle.SelectedIndex = 0;

            Radio4.IsChecked = true;
            RadioNM.IsChecked = true;

            IntegerScore.Value = 0;
            DoubleRate.Value = 0.0;
            IntegerBreak.Value = 0;

            PickedDate.Value = DateTime.Today;

            CheckDifficulties(StackMode.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value));

            TextMessage.Text = ("All fields have been reset to their defaults.");
        }

        /// <summary>
        /// Refreshes the score list for search queries
        /// </summary>
        private void SearchRecords(object sender, RoutedEventArgs e)
        {
            scoreList.Refresh();
        }

        /// <summary>
        /// Saves all records locally to JSON.
        /// </summary>
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

        /// <summary>
        /// Deletes the selected record after accepting warning or if the quick delete option is enabled.
        /// </summary>
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckQuick.IsChecked)
            {
                scoreCollection.Remove(DataGridRecords.SelectedItem as ScoreRecord);
                TextNotification.Text = "Record has been deleted.";
            }
            else
            {
                MessageBoxResult confirmDel = MessageBox.Show("Are you sure you want to delete this record?\n" +
                "This cannot be undone!", "Confirm Record Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (confirmDel == MessageBoxResult.Yes)
                {
                    scoreCollection.Remove(DataGridRecords.SelectedItem as ScoreRecord);
                    TextNotification.Text = "Record has been deleted.";
                }
            }
        }
        
    }
}
