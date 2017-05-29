using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics
{
    public static class DataBase
    {
        /*private static string mConnectionString = "Data Source=DEFINE_R5\\MSSQLSERVERE;" +
                "Trusted_Connection=Yes;" +
                "Initial Catalog=mw2stats";*/

        private static string mConnectionString = @"Server=localhost\SQLEXPRESS;Database=mw2stats;Trusted_Connection=True;";

        // Should I get the playerId from the db and save it in a var and use it for further queries or use a subquery in each query?
        //
        //
        #region Player
        public static long GetPlayerIdBySteamId(UInt64 steamId)
        {
            long returnVar = -1;

            long convertedSteamId = Convert.ToInt64(steamId);
            string query = "SELECT id FROM Player " +
                "WHERE SteamId = @SteamId;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))                  // Why in using statement?
            {
                connection.Open();
                command.Parameters.AddWithValue("@SteamId", convertedSteamId);

                object obj = command.ExecuteScalar();
                if (obj != null)
                    returnVar = (long)obj;
            }

            return returnVar;
        }

        public static long AddPlayer(UInt64 steamId)
        {
            long convertedSteamId = Convert.ToInt64(steamId);
            string query = "INSERT INTO Player (SteamId, LastSeen)" +
                "VALUES (@SteamId, @LastSeen);";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@SteamId", convertedSteamId);
                command.Parameters.AddWithValue("@LastSeen", DateTime.Now.ToBinary());
                
                command.ExecuteNonQuery();
            }

            return GetPlayerIdBySteamId(steamId);
        }

        public static void UpdatePlayerLastSeen(long playerId)
        {
            string query = "UPDATE Player " +
                "SET LastSeen = @LastSeen " +
                "WHERE id = @PlayerId;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@LastSeen", DateTime.Now.ToBinary());
                command.Parameters.AddWithValue("@PlayerId", playerId);

                command.ExecuteNonQuery();
            }
        }
        #endregion
        #region Alias
        public static bool PlayerAliasExists(string alias, long playerId, int matchId)
        {
            int result = 0;
            string query = "SELECT COUNT(id) FROM Alias " +
                "WHERE MatchId = @MatchId AND PlayerId = @PlayerId AND PlayerName = @PlayerName;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@MatchId", matchId);
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.Parameters.AddWithValue("@PlayerName", alias);

                result = (int)command.ExecuteScalar();
            }

            if (result < 1)
                return false;
            else
                return true;
        }
        /*
         *   2:00 D;011000011670ea41;3;axis;Mr.T-Rekkz;011000010b5366e1;6;allies;CanEHdian;m4_mp;42;MOD_RIFLE_BULLET;left_arm_upper
  2:00 ShutdownGame:
  2:00 ------------------------------------------------------------

        I'm host and getting kicked for inactivity

            What happens when I become host during a match?
        */

        public static void AddPlayerAlias(string alias, long playerId, int matchId)
        {
            /*if (matchId != -1)          // The host quits after the exit level line   so GetCurrentMatchId returns -1                   Do this check in the mw2eventhandler class
                  /*8:51 ExitLevel: executed
                    8:51 Q; 0110000102e85117; 2; Ghost~
                    8:51 ShutdownGame:
                    8:51------------------------------------------------------------*/
            //{
                string query = "INSERT INTO Alias (PlayerId, MatchId, PlayerName) " +
                    "VALUES (@PlayerId, @MatchId, @PlayerName);";

                using (SqlConnection connection = new SqlConnection(mConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@PlayerId", playerId);
                    command.Parameters.AddWithValue("@MatchId", matchId);
                    command.Parameters.AddWithValue("@PlayerName", alias);

                    command.ExecuteNonQuery();
                }
            //}
        }
        #endregion
        #region Match
        public static int RegisterNewMatch()
        {
            int returnVar = -1;
            string query = "INSERT INTO Match (TimeStart)" +
                "VALUES (@TimeStart);" +
                "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@TimeStart", DateTime.Now.ToBinary());

                //command.ExecuteNonQuery();
                object obj = command.ExecuteScalar();
                if (obj != null)
                    returnVar = Convert.ToInt32(obj);
            }
            return returnVar;
        }

        public static void EndMatch(int matchId)
        {
            string query = "UPDATE Match " +
                "SET TimeStop = @TimeStop " +
                "WHERE id = @MatchId;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@TimeStop", DateTime.Now.ToBinary());
                command.Parameters.AddWithValue("@MatchId", matchId);

                command.ExecuteNonQuery();
            }
        }

        public static int GetCurrentMatchId()
        {
            int returnVar = -1;
            string query = "SELECT TOP(1) id FROM Match " +
                "WHERE TimeStop is NULL;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                object obj = command.ExecuteScalar();
                if (obj != null)
                    returnVar = Convert.ToInt32(obj);
            }
            return returnVar;
        }
        #endregion
        #region Weapon
        public static bool WeaponExists(string weaponName)
        {
            int result = 0;
            string query = "SELECT COUNT(id) FROM Weapon " +
                "WHERE Name = @Name;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Name", weaponName);

                result = (int)command.ExecuteScalar();
            }

            if (result < 1)
                return false;
            else
                return true;
        }

        public static int GetWeaponId(string weaponName)
        {
            int result = -1;
            string query = "SELECT id FROM Weapon " +
                "WHERE Name = @Name;";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Name", weaponName);

                object obj = command.ExecuteScalar();
                if(obj != null)
                    result = Convert.ToInt32(obj);
            }

            return result;
        }

        public static void AddWeapon(string weaponName)
        {
            string query = "INSERT INTO Weapon (Name) " +
                "VALUES (@Name);";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Name", weaponName);

                command.ExecuteNonQuery();
            }
        }
        #endregion
        #region Hit
        public static void RegisterHit(long playerIdVictim, long playerIdAttacker, int matchId, int weaponId, int damage, string hitLocation, string meansOfDeath, bool finalBlow)
        {
            string query = "INSERT INTO Hit (PlayerId_Victim, PlayerId_Attacker, MatchId, WeaponId, Damage, HitLocation, MeansOfDeath, FinalBlow) " +
                "VALUES (@PlayerId_Victim, @PlayerId_Attacker, @MatchId, @WeaponId, @Damage, @HitLocation, @MeansOfDeath, @FinalBlow);";

            using (SqlConnection connection = new SqlConnection(mConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PlayerId_Victim", playerIdVictim);
                command.Parameters.AddWithValue("@PlayerId_Attacker", playerIdAttacker);
                command.Parameters.AddWithValue("@MatchId", matchId);
                command.Parameters.AddWithValue("@WeaponId", weaponId);
                command.Parameters.AddWithValue("@Damage", damage);
                command.Parameters.AddWithValue("@HitLocation", hitLocation);
                command.Parameters.AddWithValue("@MeansOfDeath", meansOfDeath);
                command.Parameters.AddWithValue("@FinalBlow", finalBlow);

                command.ExecuteNonQuery();
            }
        }
        #endregion

        public static void EmptyAllTables()
        {
            if (System.Windows.Forms.MessageBox.Show("Are you sure you want to empty all the tables?", "Are you sure?!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                string query = "DELETE FROM Hit;" +
                    "DELETE FROM Weapon;" +
                    "DELETE FROM Alias;" +
                    "DELETE FROM Match;" +
                    "DELETE FROM Player";

                using (SqlConnection connection = new SqlConnection(mConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
