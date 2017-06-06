using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MW2_Statistics_Dashboard
{
    public abstract class Database
    {
        protected static readonly string mRootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        // SQL Connection strings
        /*private static readonly string mConnectionString = "Data Source=DEFINE_R5\\MSSQLSERVERE;" +
                "Trusted_Connection=Yes;" +
                "Initial Catalog=mw2stats";*/

        //private static readonly string mConnectionString = @"Server=localhost\SQLEXPRESS;Database=mw2stats;Trusted_Connection=True;";
        protected static readonly string mConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + mRootPath + @"\mw2stats.mdf;Integrated Security=True";
    }
}
