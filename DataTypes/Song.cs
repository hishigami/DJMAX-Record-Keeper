using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJMAX_Record_Keeper.DataTypes
{
    public class Song
    {
        // Fields
        private string _title;
        private string _artist;
        private string _category;
        private bool _fourNM;
        private bool _fourHD;
        private bool _fourMX;
        private bool _fourSC;
        private bool _fiveNM;
        private bool _fiveHD;
        private bool _fiveMX;
        private bool _fiveSC;
        private bool _sixNM;
        private bool _sixHD;
        private bool _sixMX;
        private bool _sixSC;
        private bool _eightNM;
        private bool _eightHD;
        private bool _eightMX;
        private bool _eightSC;

        // Properties
        public string Title { get => _title; set => _title = value; }
        public string Artist { get => _artist; set => _artist = value; }
        public string Category { get => _category; set => _category = value; }
        public bool FourNM { get => _fourNM; set => _fourNM = value; }
        public bool FourHD { get => _fourHD; set => _fourHD = value; }
        public bool FourMX { get => _fourMX; set => _fourMX = value; }
        public bool FourSC { get => _fourSC; set => _fourSC = value; }
        public bool FiveNM { get => _fiveNM; set => _fiveNM = value; }
        public bool FiveHD { get => _fiveHD; set => _fiveHD = value; }
        public bool FiveMX { get => _fiveMX; set => _fiveMX = value; }
        public bool FiveSC { get => _fiveSC; set => _fiveSC = value; }
        public bool SixNM { get => _sixNM; set => _sixNM = value; }
        public bool SixHD { get => _sixHD; set => _sixHD = value; }
        public bool SixMX { get => _sixMX; set => _sixMX = value; }
        public bool SixSC { get => _sixSC; set => _sixSC = value; }
        public bool EightNM { get => _eightNM; set => _eightNM = value; }
        public bool EightHD { get => _eightHD; set => _eightHD = value; }
        public bool EightMX { get => _eightMX; set => _eightMX = value; }
        public bool EightSC { get => _eightSC; set => _eightSC = value; }

        // Constructor
        public Song(string title, string artist, string category, bool fourNM, bool fourHD, bool fourMX, bool fourSC,
            bool fiveNM, bool fiveHD, bool fiveMX, bool fiveSC, bool sixNM, bool sixHD, bool sixMX, bool sixSC,
            bool eightNM, bool eightHD, bool eightMX, bool eightSC)
        {
            Title = title;
            Artist = artist;
            Category = category;
            FourNM = fourNM;
            FourHD = fourHD;
            FourMX = fourMX;
            FourSC = fourSC;
            FiveNM = fiveNM;
            FiveHD = fiveHD;
            FiveMX = fiveMX;
            FiveSC = fiveSC;
            SixNM = sixNM;
            SixHD = sixHD;
            SixMX = sixMX;
            SixSC = sixSC;
            EightNM = eightNM;
            EightHD = eightHD;
            EightMX = eightMX;
            EightSC = eightSC;
        }
    }
}
