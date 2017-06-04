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
            get { return new DateTime(mDateTimeLastSeen); }
        }
        public List<string> Aliasses { get; set; }

        public Player(long id, long lastSeen, Match match)
        {
            Id = id;
            mDateTimeLastSeen = lastSeen;
            Aliasses = Database.GetAliasses(id, match);
        }

        public override string ToString()
        {
            return Aliasses[0];
        }
    }
}
