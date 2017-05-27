using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics_Dashboard
{
    public class Match
    {
        private long mDateTimeStart;
        private long mDateTimeStop;
        public int MatchId { get; set; }
        public DateTime DateTimeStart
        {
            get { return DateTime.FromBinary(mDateTimeStart); }
        }

        public DateTime DateTimeStop
        {
            get { return DateTime.FromBinary(mDateTimeStop); }
        }

        public Match(int matchId, long dateTimeStart, long dateTimeStop)
        {
            MatchId = matchId;
            mDateTimeStart = dateTimeStart;
            mDateTimeStop = dateTimeStop;
        }

        public override string ToString()
        {
            if (MatchId == -1)   // This instance is a date label for the listbox
                return "--==   " + DateTimeStart.ToString("dd-MM-yyyy") + "   ==--";
            return DateTimeStart.ToString("HH:mm") + " -- " + DateTimeStop.ToString("HH:mm");
        }
    }
}
