using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics_Dashboard
{
    public class Player
    {
        private long mDateTimeLastSeen;
        public long Id { get; set; }
        public DateTime LastSeen
        {
            get { return DateTime.FromBinary(mDateTimeLastSeen); }
        }
        public List<string> Aliasses { get; set; }

        public Player(long id, long lastSeen)
        {
            Id = id;
            mDateTimeLastSeen = lastSeen;
            Aliasses = Database.GetAliasses(id);
        }

        public override string ToString()
        {
            return Aliasses[Aliasses.Count - 1] + " (" + LastSeen.ToString("HH:mm") + ")";
        }
    }
}
