using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics_Dashboard
{
    public class Player : Database
    {
        public long Id { get; set; }
        public DateTime LastSeen { get; set; }
        public List<string> Aliasses { get; set; }

        public Player(long id, long lastSeenTicks, Match match)
        {
            Id = id;
            LastSeen = new DateTime(lastSeenTicks);
            Aliasses = GetAliasses(id, match);
        }

        public override string ToString()
        {
            return Aliasses[0];
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;
            Player p = (Player)obj;
            if (p.Id != Id) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #region SQL Queries
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
                        list.Add(new Player(Convert.ToInt64(dt.Rows[i]["id"]), Convert.ToInt64(dt.Rows[i]["LastSeen"]), match));
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
            if (match == null)
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
                        list.Add(new Player(Convert.ToInt64(dt.Rows[i]["id"]), Convert.ToInt64(dt.Rows[i]["LastSeen"]), match));
                    }
                    catch (Exception ex) { }
                }
            }
            return list;
        }

        private List<string> GetAliasses(long playerId, Match match)
        {
            var list = new List<string>();
            string query;
            if (match == null)
            {
                query = "SELECT DISTINCT PlayerName FROM Alias " +
                "WHERE PlayerId = @PlayerId;";
            }
            else
            {
                query = "SELECT DISTINCT PlayerName FROM Alias " +
                "WHERE PlayerId = @PlayerId AND MatchId = @MatchId;";
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
                    try
                    {
                        list.Add(dt.Rows[i]["PlayerName"].ToString());
                    }
                    catch (Exception ex) { }
                }
            }
            return list;
        }

        public int GetKillCount(Match match)
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

                command.Parameters.AddWithValue("@PlayerId_Attacker", Id);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public int GetDeathCount(Match match)
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

                command.Parameters.AddWithValue("@PlayerId_Victim", Id);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public int GetHeadshotCount(Match match)
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

                command.Parameters.AddWithValue("@PlayerId_Attacker", Id);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public string GetFavouriteWeapon(Match match)
        {
            string name;
            string query;
            if (match == null)
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

                command.Parameters.AddWithValue("@PlayerId_Attacker", Id);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                name = (string)command.ExecuteScalar();
            }
            return name;
        }

        public Player GetMostKilledPlayer(Match match)
        {
            string query;
            if (match == null)
            {
                query = "SELECT * FROM Player p, (SELECT top(1) PlayerId_Victim, COUNT(*) AS KillCount FROM Hit WHERE PlayerId_Attacker = @PlayerId AND FinalBlow = 1 GROUP BY PlayerId_Victim ORDER BY KillCount DESC) sq WHERE sq.PlayerId_Victim = p.id";
            }
            else
            {
                query = "SELECT * FROM Player p, (SELECT top(1) PlayerId_Victim, COUNT(*) AS KillCount FROM Hit WHERE MatchId = @MatchId AND PlayerId_Attacker = @PlayerId AND FinalBlow = 1 GROUP BY PlayerId_Victim ORDER BY KillCount DESC) sq WHERE sq.PlayerId_Victim = p.id";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@PlayerId", Id);
                if (match != null)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    long id = Convert.ToInt64(dt.Rows[0]["id"]);
                    long lastSeen = Convert.ToInt64(dt.Rows[0]["LastSeen"]);
                    return new Player(id, lastSeen, match);
                }
                else
                    return null;
            }
        }

        public Player GetMostKilledByPlayer(Match match)
        {
            string query;
            if (match == null)
            {
                query = "SELECT * FROM Player p, (SELECT top(1) PlayerId_Attacker, COUNT(*) AS KillCount FROM Hit WHERE PlayerId_Victim = @PlayerId AND FinalBlow = 1 GROUP BY PlayerId_Attacker ORDER BY KillCount DESC) sq WHERE sq.PlayerId_Attacker = p.id";
            }
            else
            {
                query = "SELECT * FROM Player p, (SELECT top(1) PlayerId_Attacker, COUNT(*) AS KillCount FROM Hit WHERE MatchId = @MatchId AND PlayerId_Victim = @PlayerId AND FinalBlow = 1 GROUP BY PlayerId_Attacker ORDER BY KillCount DESC) sq WHERE sq.PlayerId_Attacker = p.id";
            }

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@PlayerId", Id);
                if (match != null)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    long id = Convert.ToInt64(dt.Rows[0]["id"]);
                    long lastSeen = Convert.ToInt64(dt.Rows[0]["LastSeen"]);
                    return new Player(id, lastSeen, match);
                }
                else
                    return null;
            }
        }

        public int GetLongestKillingSpree(Match match)
        {
            int maxKillingSpree = 0;
            string query;
            if (match == null)
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

                adapter.SelectCommand.Parameters.AddWithValue("@PlayerId", Id);
                if (match != null)
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

                    if (matchId != currentMatchId)   // New match --> reset killing spree
                    {
                        if (maxKillingSpree < killingSpree)
                            maxKillingSpree = killingSpree;
                        killingSpree = 0;
                        currentMatchId = matchId;
                    }

                    if (victimId == Id)    // Player was killed
                    {
                        killingSpree = 0;
                    }
                    else if (attackerId == Id) // Player got kill, but didn't kill himself (if he kills himself playerId == victimId == attackerId)
                    {
                        killingSpree++;
                    }

                    if (maxKillingSpree < killingSpree)
                        maxKillingSpree = killingSpree;
                }
            }
            return maxKillingSpree;
        }

        #region Weapon specific
        public int GetKillCount(int weaponId, Match match)
        {
            int value;
            string query;
            if (match == null)
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

                command.Parameters.AddWithValue("@PlayerId_Attacker", Id);
                command.Parameters.AddWithValue("@WeaponId", weaponId);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public int GetHeadshotCount(int weaponId, Match match)
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

                command.Parameters.AddWithValue("@PlayerId_Attacker", Id);
                command.Parameters.AddWithValue("@WeaponId", weaponId);
                if (match != null)
                {
                    command.Parameters.AddWithValue("@MatchId", match.MatchId);
                }

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public int GetKilledByCount(int weaponId, Match match)
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

                command.Parameters.AddWithValue("@PlayerId_Victim", Id);
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
