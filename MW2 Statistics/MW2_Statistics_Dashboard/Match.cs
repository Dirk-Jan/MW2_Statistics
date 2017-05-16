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

        public Match(long dateTimeStart, long dateTimeStop)
        {
            mDateTimeStart = dateTimeStart;
            mDateTimeStop = dateTimeStop;
        }

        public override string ToString()
        {
            return DateTimeStart.ToString("HH:mm") + " - " + DateTimeStop.ToString("HH:mm");
        }
    }
}
