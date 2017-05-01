using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics
{
    public class DatabaseFiller
    {
        private const string mReadFile = "readfile";
        private string mHostFilePath = @"C:\Program Files (x86)\Steam\steamapps\common\Call of Duty Modern Warfare 2\main\games_mp.log";
        private string mRootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        // Copy file
        // var for lines read
        
        private void CollectFeed(StreamReader sr)
        {
            // Get last read line
            UInt32 linesRead = 0;
            if(File.Exists(mRootPath + @"\linesread"))
            {
                try
                {
                    linesRead = BitConverter.ToUInt32(File.ReadAllBytes(mRootPath + @"\linesread"), 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not read linesread file.");
                }
            }

            // Skip read lines
            SkipLines(sr, linesRead);

            while (true)
            {
                // Read new feed
                string feed = sr.ReadLine();
                if (!String.IsNullOrEmpty(feed))
                {

                }
            }
        }

        private void SkipLines(StreamReader sr, UInt32 lineCount)
        {
            for (int i = 0; i < lineCount; i++)
            {
                sr.ReadLine();
            }
        }
    }
}
