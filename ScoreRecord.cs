using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJMAX_Record_Keeper
{
    //Currently, the program only covers the game's standard modes and difficulties
    public class ScoreRecord
    {
        //Rank cutlines
        private const double bMin = 80.0;
        private const double aMin = 90.0;
        private const double sMin = 97.0;

        //Fields
        private string _songName;
        private string _mode;
        private string _difficulty;
        private string _patternName;
        private int _score;
        private double _rate;
        private string _rank;
        private int _breaks;
        private DateTime _date;

        //Properties
        public string SongName { get => _songName; set => _songName = value; }
        public string Mode { get => _mode; set => _mode = value; }
        public string Difficulty { get => _difficulty; set => _difficulty = value; }
        public string PatternName { get => _patternName; set => _patternName = value; }
        public int Score { get => _score; set => _score = value; }
        public double Rate { get => _rate; set => _rate = value; }
        public string Rank { get => _rank; set => _rank = value; }
        public int Breaks { get => _breaks; set => _breaks = value; }
        public DateTime Date { get => _date; set => _date = value; }

        //Constructor
        public ScoreRecord(string songName, string mode, string difficulty, int score, double rate, int breaks, DateTime date)
        {
            SongName = songName;
            Mode = mode;
            Difficulty = difficulty;
            PatternName = SongName + " " + Mode + Difficulty;
            Score = score;
            Rate = rate;
            Rank = measureRank(rate, breaks);
            Breaks = breaks;
            Date = date;
        }

        //Determine rank from rate and breaks
        private string measureRank(double rate, int breaks)
        {
            //PP check
            if (rate == 100.0 && breaks == 0)
                return "PP";

            //Not a PP. Begin constructing output rank
            string output;
            
            if (rate >= sMin)
                output = "S";
            else if (rate >= aMin)
                output = "A";
            else if (rate >= bMin)
                output = "B";
            else
                output = "C";

            //MC check
            if (breaks == 0)
                output += " MC";
            
            return output;
        }
    }
}
