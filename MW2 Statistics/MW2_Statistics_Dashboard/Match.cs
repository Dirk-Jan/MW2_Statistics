using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics_Dashboard
{
    public class Match : Database
    {
        private List<Player> mPlayers = null;
        public int MatchId { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeStop { get; set; }
        public List<Player> Players
        {
            get
            {
                if (mPlayers == null)
                    mPlayers = Player.GetPlayers(this);
                return mPlayers;
            }
        }

        public Match(int matchId, long dateTimeStartTicks, long dateTimeStopTicks)
        {
            MatchId = matchId;
            DateTimeStart = new DateTime(dateTimeStartTicks);
            DateTimeStop = new DateTime(dateTimeStopTicks);
        }

        public override string ToString()
        {
            if (MatchId == -1)
                return "--==   " + DateTimeStart.ToString("dd-MM-yyyy") + "   ==--";            // This string is a date label for the listbox
            return DateTimeStart.ToString("HH:mm") + " -- " + DateTimeStop.ToString("HH:mm");
        }

        public static List<Match> AddDataLabelsToMatchesList(List<Match> matches)
        {
            DateTime lastDate = DateTime.MaxValue;
            for (int i = 0; i < matches.Count; i++)
            {
                if (lastDate.Date != matches[i].DateTimeStart.Date)
                {
                    lastDate = matches[i].DateTimeStart;
                    matches.Insert(i, new Match(-1, lastDate.Ticks, lastDate.Ticks));
                }
            }
            return matches;
        }

        #region SQL Queries
        public static List<Match> GetMatches()
        {
            return GetMatches(false, 0, 0);
        }
        public static List<Match> GetMatches(long rangeStart, long rangeStop)
        {
            return GetMatches(true, rangeStart, rangeStop);
        }
        private static List<Match> GetMatches(bool applyRange, long rangeStart, long rangeStop)
        {
            var list = new List<Match>();
            string query;
            if (!applyRange)
            {
                query = "SELECT * FROM Match ORDER BY id DESC;";
            }
            else
            {
                query = "SELECT * FROM Match WHERE TimeStart BETWEEN @RangeStart AND @RangeStop ORDER BY id DESC;";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                if (applyRange)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@RangeStart", rangeStart);
                    adapter.SelectCommand.Parameters.AddWithValue("@RangeStop", rangeStop);
                }

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        var matchId = Convert.ToInt32(dt.Rows[i]["id"]);
                        var timeStart = Convert.ToInt64(dt.Rows[i]["TimeStart"]);
                        var timeStop = Convert.ToInt64(dt.Rows[i]["TimeStop"]);

                        list.Add(new Match(matchId, timeStart, timeStop));
                    }
                    catch (Exception ex) { }
                }
            }
            return list;
        }
        #endregion
    }
}
