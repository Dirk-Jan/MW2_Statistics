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
        /*private static string mConnectionString = "Data Source=DEFINE_R5\\MSSQLSERVERE;" +
                "Trusted_Connection=Yes;" +
                "Initial Catalog=mw2stats";*/

        private static string mConnectionString = @"Server=localhost\SQLEXPRESS;Database=mw2stats;Trusted_Connection=True;";

        public static List<Match> GetMatches()
        {
            var list = new List<Match>();
            string query = "SELECT TimeStart, TimeStop FROM Match;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();
                
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        var timeStart = Convert.ToInt64(dt.Rows[i]["TimeStart"]);
                        var timeStop = Convert.ToInt64(dt.Rows[i]["TimeStop"]);

                        list.Add(new Match(timeStart, timeStop));
                    }
                    catch (Exception ex) { }
                }
            }
            return list;
        }

        public static List<Player> GetPlayers()
        {
            var list = new List<Player>();
            string query = "SELECT * FROM Player;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

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
            string query = "SELECT PlayerName FROM Alias " +
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

        #region Overal Player Stats
        public static int GetKillCount(long playerId)
        {
            int value;
            string query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Attacker = @PlayerId_Attacker " +
                "AND FinalBlow = 1;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public static int GetDeathCount(long playerId)
        {
            int value;
            string query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Victim = @PlayerId_Victim " +
                "AND FinalBlow = 1;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PlayerId_Victim", playerId);

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public static int GetHeadshotCount(long playerId)
        {
            int value;
            string query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Attacker = @PlayerId_Attacker " +
                "AND FinalBlow = 1 " +
                "AND HitLocation = 'head';";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public static string GetFavouriteWeapon(long playerId)
        {
            string name;
            string query = "SELECT TOP(1) w.Name FROM(SELECT WeaponId, COUNT(WeaponId) AS KillCount FROM Hit WHERE PlayerId_Attacker = @PlayerId_Attacker AND FinalBlow = 1 GROUP BY WeaponId) t, Weapon w WHERE W.id = t.WeaponId ORDER BY t.KillCount DESC";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);

                name = (string)command.ExecuteScalar();
            }
            return name;
        }

        public static string GetMostKilledPlayerName(long playerId)
        {
            string name;
            string query = "SELECT TOP(1) a.PlayerName FROM(SELECT PlayerID_Victim, COUNT(PlayerId_Victim) AS TimesKilled FROM Hit WHERE FinalBlow = 1 AND PlayerId_Attacker = @PlayerId_Attacker GROUP BY PlayerId_Victim) t1, Alias a WHERE t1.PlayerId_Victim = a.PlayerId ORDER BY t1.TimesKilled DESC, a.id DESC";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);

                name = (string)command.ExecuteScalar();
            }
            if (String.IsNullOrEmpty(name))
                name = "No-one";
            return name;
        }

        public static string GetMostKilledByPlayerName(long playerId)
        {
            string name;
            string query = "SELECT TOP(1) a.PlayerName FROM(SELECT PlayerID_Attacker, COUNT(PlayerId_Attacker) AS TimesKilled FROM Hit  WHERE FinalBlow = 1 AND PlayerId_Victim = @PlayerId_Victim GROUP BY PlayerId_Attacker) t1, Alias a WHERE t1.PlayerId_Attacker = a.PlayerId ORDER BY t1.TimesKilled DESC, a.id DESC";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PlayerId_Victim", playerId);

                name = (string)command.ExecuteScalar();
            }
            if (String.IsNullOrEmpty(name))
                name = "No-one";
            return name;
        }

        public static int GetLongestKillingSpree(long playerId)
        {
            int maxKillingSpree = 0;
            string query = "SELECT PlayerId_Victim, PlayerId_Attacker FROM Hit WHERE FinalBlow = 1 AND (PlayerId_Attacker = @PlayerId OR PlayerId_Victim = @PlayerId);";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();
                adapter.SelectCommand.Parameters.AddWithValue("@PlayerId", playerId);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                int killingSpree = 0;
                //foreach (var item in dt.Rows)
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    long victimId = Convert.ToInt64(dt.Rows[i]["PlayerId_Victim"]);
                    long attackerId = Convert.ToInt64(dt.Rows[i]["PlayerId_Attacker"]);

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
        public static List<Weapon> GetWeapons(long playerId)
        {
            var list = new List<Weapon>();
            string query = "SELECT t.WeaponId, w.Name FROM(SELECT DISTINCT WeaponId FROM Hit WHERE PlayerId_Attacker = @PlayerId AND FinalBlow = 1) t, Weapon w WHERE t.WeaponId = w.id";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();
                adapter.SelectCommand.Parameters.AddWithValue("@PlayerId", playerId);

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

        public static int GetKillCount(long playerId, int weaponId)
        {
            int value;
            string query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Attacker = @PlayerId_Attacker " +
                "AND WeaponId = @WeaponId " +
                "AND FinalBlow = 1;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);
                command.Parameters.AddWithValue("@WeaponId", weaponId);

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public static int GetHeadshotCount(long playerId, int weaponId)
        {
            int value;
            string query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Attacker = @PlayerId_Attacker " +
                "AND WeaponId = @WeaponId " +
                "AND FinalBlow = 1 " +
                "AND HitLocation = 'head';";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PlayerId_Attacker", playerId);
                command.Parameters.AddWithValue("@WeaponId", weaponId);

                value = (int)command.ExecuteScalar();
            }
            return value;
        }

        public static int GetKilledByCount(long playerId, int weaponId)
        {
            int value;
            string query = "SELECT COUNT(*) FROM Hit " +
                "WHERE PlayerId_Victim = @PlayerId_Victim " +
                "AND WeaponId = @WeaponId " +
                "AND FinalBlow = 1;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PlayerId_Victim", playerId);
                command.Parameters.AddWithValue("@WeaponId", weaponId);

                value = (int)command.ExecuteScalar();
            }
            return value;
        }
        #endregion
        #endregion
    }
}
