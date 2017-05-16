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
    }
}
