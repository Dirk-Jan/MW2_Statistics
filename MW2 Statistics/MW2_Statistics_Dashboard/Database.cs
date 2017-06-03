using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MW2_Statistics_Dashboard
{
    public static class Database
    {
        private static string mConnectionString = "Data Source=DEFINE_R5\\MSSQLSERVERE;" +
                "Trusted_Connection=Yes;" +
                "Initial Catalog=mw2stats";

        //private static string mConnectionString = @"Server=localhost\SQLEXPRESS;Database=mw2stats;Trusted_Connection=True;";

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
            if(!applyRange)
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
        #region Overal Player Stats
        public static List<Player> GetPlayersWithFilter(Match match, string filterValue)
        {
            var list = new List<Player>();
            string query;
            if (match == null)
                query = "SELECT * FROM Player p WHERE id IN (SELECT PlayerId FROM Alias WHERE PlayerName LIKE '%'+@FilterValue+'%');";
            else
                query = "SELECT * FROM Player p WHERE id IN (SELECT PlayerId FROM Alias WHERE MatchId = @MatchId AND PlayerName LIKE '%'+@FilterValue+'%');";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@FilterValue", filterValue);
                if (match != null)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        list.Add(new Player(Convert.ToInt64(dt.Rows[i]["id"]), Convert.ToInt64(dt.Rows[i]["LastSeen"])));
                    }
                    catch (Exception ex) { }
                }
            }
            return list;
        }

        public static List<Player> GetPlayers(Match match)
        {
            var list = new List<Player>();
            string query;
            if(match == null)
                query = "SELECT * FROM Player;";
            else
                query = "SELECT * FROM Player WHERE id IN (SELECT PlayerId FROM Alias WHERE MatchId = @MatchId);";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();
                if (match != null)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        list.Add(new Player(Convert.ToInt64(dt.Rows[i]["id"]), Convert.ToInt64(dt.Rows[i]["LastSeen"])));
                    }
                    catch (Exception ex) { }
                }
            }
            return list;
        }

        public static List<string> GetAliasses(long playerId)
        {
            var list = new List<string>();
            string query = "SELECT DISTINCT PlayerName FROM Alias " +
                "WHERE PlayerId = @PlayerId;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();
                adapter.SelectCommand.Parameters.AddWithValue("@PlayerId", playerId);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        list.Add(dt.Rows[i]["PlayerName"].ToString());
                    }
                    catch (Exception ex) { }
                }
            }
            return list;
        }

        public static int GetKillCount(long playerId, Match match)
        {
            int value;
            string query;
            if (match == null)
            {
                query = "SELECT COUNT(*) FROM Hit " +
                    "WHERE PlayerId_Attacker = @PlayerId_Attacker " +
                    "AND FinalBlow = 1;";
            }
            else
            {
                query = "SELECT COUNT(*) FROM Hit " +
                    "WHERE PlayerId_Attacker = @PlayerId_Attacker " +
                    "AND FinalBlow = 1 " +
                    "AND MatchId = @MatchId;";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                
                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public static int GetDeathCount(long playerId, Match match)
        {
            int value;
            string query;
            if (match == null)
            {
                query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Victim = @PlayerId_Victim " +
                "AND FinalBlow = 1;";
            }
            else
            {
                query = "SELECT COUNT(*) FROM Hit " +
                    "WHERE PlayerId_Victim = @PlayerId_Victim " +
                    "AND FinalBlow = 1 " +
                    "AND MatchId = @MatchId;";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@PlayerId_Victim", playerId);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public static int GetHeadshotCount(long playerId, Match match)
        {
            int value;
            string query;
            if (match == null)
            {
                query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Attacker = @PlayerId_Attacker " +
                "AND FinalBlow = 1 " +
                "AND HitLocation = 'head';";
            }
            else
            {
                query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Attacker = @PlayerId_Attacker " +
                "AND FinalBlow = 1 " +
                "AND HitLocation = 'head' " +
                "AND MatchId = @MatchId;";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public static string GetFavouriteWeapon(long playerId, Match match)
        {
            string name;
            string query;
            if(match == null)
            {
                query = "SELECT TOP(1) w.Name FROM(SELECT WeaponId, COUNT(WeaponId) AS KillCount FROM Hit WHERE PlayerId_Attacker = @PlayerId_Attacker AND FinalBlow = 1 GROUP BY WeaponId) t, Weapon w WHERE W.id = t.WeaponId ORDER BY t.KillCount DESC";
            }
            else
            {
                query = "SELECT TOP(1) w.Name FROM(SELECT WeaponId, COUNT(WeaponId) AS KillCount FROM Hit WHERE PlayerId_Attacker = @PlayerId_Attacker AND FinalBlow = 1 AND MatchId = @MatchId GROUP BY WeaponId) t, Weapon w WHERE W.id = t.WeaponId ORDER BY t.KillCount DESC";
            }
            
            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                name = (string)command.ExecuteScalar();
            }
            return name;
        }

        public static string GetMostKilledPlayerName(long playerId, Match match)
        {
            string name;
            string query;
            if(match == null)
            {
                query = "SELECT TOP(1) a.PlayerName FROM(SELECT PlayerID_Victim, COUNT(PlayerId_Victim) AS TimesKilled FROM Hit WHERE FinalBlow = 1 AND PlayerId_Attacker = @PlayerId_Attacker GROUP BY PlayerId_Victim) t1, Alias a WHERE t1.PlayerId_Victim = a.PlayerId ORDER BY t1.TimesKilled DESC, a.id DESC";
            }
            else
            {
                query = "SELECT TOP(1) a.PlayerName FROM(SELECT PlayerID_Victim, COUNT(PlayerId_Victim) AS TimesKilled FROM Hit WHERE FinalBlow = 1 AND PlayerId_Attacker = @PlayerId_Attacker AND MatchId = @MatchId GROUP BY PlayerId_Victim) t1, Alias a WHERE t1.PlayerId_Victim = a.PlayerId AND a.MatchId = @MatchId ORDER BY t1.TimesKilled DESC, a.id DESC";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                name = (string)command.ExecuteScalar();
            }
            if (String.IsNullOrEmpty(name))
                name = "No-one";
            return name;
        }

        public static string GetMostKilledByPlayerName(long playerId, Match match)
        {
            string name;
            string query;
            if(match == null)
            {
                query = "SELECT TOP(1) a.PlayerName FROM(SELECT PlayerID_Attacker, COUNT(PlayerId_Attacker) AS TimesKilled FROM Hit WHERE FinalBlow = 1 AND PlayerId_Victim = @PlayerId_Victim GROUP BY PlayerId_Attacker) t1, Alias a WHERE t1.PlayerId_Attacker = a.PlayerId ORDER BY t1.TimesKilled DESC, a.id DESC";
            }
            else
            {
                query = "SELECT TOP(1) a.PlayerName FROM(SELECT PlayerID_Attacker, COUNT(PlayerId_Attacker) AS TimesKilled FROM Hit WHERE FinalBlow = 1 AND PlayerId_Victim = @PlayerId_Victim AND MatchId = @MatchId GROUP BY PlayerId_Attacker) t1, Alias a WHERE t1.PlayerId_Attacker = a.PlayerId AND a.MatchId = @MatchId ORDER BY t1.TimesKilled DESC, a.id DESC";
            }
            
            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@PlayerId_Victim", playerId);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                name = (string)command.ExecuteScalar();
            }
            if (String.IsNullOrEmpty(name))
                name = "No-one";
            return name;
        }

        public static int GetLongestKillingSpree(long playerId, Match match)
        {
            int maxKillingSpree = 0;
            string query;
            if(match == null)
            {
                query = "SELECT MatchId, PlayerId_Victim, PlayerId_Attacker FROM Hit WHERE FinalBlow = 1 AND (PlayerId_Attacker = @PlayerId OR PlayerId_Victim = @PlayerId);";
            }
            else
            {
                query = "SELECT MatchId, PlayerId_Victim, PlayerId_Attacker FROM Hit WHERE MatchId = @MatchId AND FinalBlow = 1 AND (PlayerId_Attacker = @PlayerId OR PlayerId_Victim = @PlayerId);";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@PlayerId", playerId);
                if(match != null)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                int killingSpree = 0;
                int currentMatchId = -1;
                //foreach (var item in dt.Rows)
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int matchId = Convert.ToInt32(dt.Rows[i]["MatchId"]);
                    long victimId = Convert.ToInt64(dt.Rows[i]["PlayerId_Victim"]);
                    long attackerId = Convert.ToInt64(dt.Rows[i]["PlayerId_Attacker"]);

                    if(matchId != currentMatchId)   // New match --> reset killing spree
                    {
                        if (maxKillingSpree < killingSpree)
                            maxKillingSpree = killingSpree;
                        killingSpree = 0;
                        currentMatchId = matchId;
                    }

                    if(victimId == playerId)    // Player was killed
                    {
                        killingSpree = 0;
                    }
                    else if(attackerId == playerId) // Player got kill, but didn't kill himself (if he kills himself playerId == victimId == attackerId)
                    {
                        killingSpree++;
                    }

                    if (maxKillingSpree < killingSpree)
                        maxKillingSpree = killingSpree;
                }
            }
            return maxKillingSpree;
        }
        #region Weapon Tab
        public static List<Weapon> GetWeapons(long playerId, Match match)
        {
            var list = new List<Weapon>();
            string query;
            if(match == null)
            {
                query = "SELECT t.WeaponId, w.Name FROM(SELECT DISTINCT WeaponId FROM Hit WHERE PlayerId_Attacker = @PlayerId AND FinalBlow = 1) t, Weapon w WHERE t.WeaponId = w.id";
            }
            else
            {
                query = "SELECT t.WeaponId, w.Name FROM(SELECT DISTINCT WeaponId FROM Hit WHERE MatchId = @MatchId AND PlayerId_Attacker = @PlayerId AND FinalBlow = 1) t, Weapon w WHERE t.WeaponId = w.id";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@PlayerId", playerId);
                if (match != null)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int id = Convert.ToInt32(dt.Rows[i]["WeaponId"]);
                    string name = (string)dt.Rows[i]["Name"];

                    list.Add(new Weapon(id, name));
                }
            }
            return list;
        }

        public static int GetKillCount(long playerId, int weaponId, Match match)
        {
            int value;
            string query;
            if(match == null)
            {
                query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Attacker = @PlayerId_Attacker " +
                "AND WeaponId = @WeaponId " +
                "AND FinalBlow = 1;";
            }
            else
            {
                query = "SELECT COUNT(*) FROM Hit " +
                "WHERE MatchId = @MatchId " +
                "AND PlayerId_Attacker = @PlayerId_Attacker " +
                "AND WeaponId = @WeaponId " +
                "AND FinalBlow = 1;";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);
                command.Parameters.AddWithValue("@WeaponId", weaponId);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public static int GetHeadshotCount(long playerId, int weaponId, Match match)
        {
            int value;
            string query;
            if (match == null)
            {
                query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Attacker = @PlayerId_Attacker " +
                "AND WeaponId = @WeaponId " +
                "AND FinalBlow = 1 " +
                "AND HitLocation = 'head';";
            }
            else
            {
                query = "SELECT COUNT(*) FROM Hit " +
                "WHERE MatchId = @MatchId " +
                "AND PlayerId_Attacker = @PlayerId_Attacker " +
                "AND WeaponId = @WeaponId " +
                "AND FinalBlow = 1 " +
                "AND HitLocation = 'head';";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);
                command.Parameters.AddWithValue("@WeaponId", weaponId);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public static int GetKilledByCount(long playerId, int weaponId, Match match)
        {
            int value;
            string query;
            if (match == null)
            {
                query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Victim = @PlayerId_Victim " +
                "AND WeaponId = @WeaponId " +
                "AND FinalBlow = 1;";
            }
            else
            {
                query = "SELECT COUNT(*) FROM Hit " +
                "WHERE MatchId = @MatchId " +
                "AND PlayerId_Victim = @PlayerId_Victim " +
                "AND WeaponId = @WeaponId " +
                "AND FinalBlow = 1;";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@PlayerId_Victim", playerId);
                command.Parameters.AddWithValue("@WeaponId", weaponId);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                value = (int)command.ExecuteScalar();
            }
            return value;
        }
        #endregion
        #endregion
    }
}
