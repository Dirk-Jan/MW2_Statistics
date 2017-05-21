using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics
{
    public static class SourceFileReader
    {
        private const string mReadFile = "readfile";
        //private static string mHostFilePath = @"C:\Program Files (x86)\Steam\steamapps\common\Call of Duty Modern Warfare 2\main\games_mp.log";
        //private static string mHostFilePath = @"C:\Users\Dirk-Jan de Beijer\Desktop\logSimulation\games_mp.log";
        private static string mHostFilePath = @"E:\SteamLibrary\steamapps\common\Call of Duty Modern Warfare 2\main\games_mp.log";
        private static string mRootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public static void CollectFeed()
        {
            string newFile = mRootPath + @"\copy";
            string lineCountFile = mRootPath + @"\linesread";
            // Copy file
            if (File.Exists(newFile))
                File.Delete(newFile);
            File.Copy(mHostFilePath, newFile);

            // Get last read line
            UInt32 linesRead = 0;
            if (File.Exists(lineCountFile))
            {
                try
                {
                    linesRead = BitConverter.ToUInt32(File.ReadAllBytes(lineCountFile), 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not read linesread file.");
                }
            }

            StreamReader sr = new StreamReader(newFile);

            // Skip read lines
            SkipLines(sr, linesRead);

            // Read new feed
            string feed = sr.ReadLine();
            while (feed != null)
            {
                linesRead++;
                if (feed == string.Empty)
                    continue;
                // Get the data from the feed and put it in the database
                Console.WriteLine(feed);
                MW2Event mw2Event = new MW2Event(feed);
                MW2EventHandler.HandleMW2Event(mw2Event);
                //Feed.Add(mw2Event.getKillFeed());

                feed = sr.ReadLine();
            }

            // Close the reader
            sr.Close();

            // Save the new value of lines read
            File.WriteAllBytes(lineCountFile, BitConverter.GetBytes(linesRead));
        }

        private static void SkipLines(StreamReader sr, UInt32 lineCount)
        {
            for (int i = 0; i < lineCount; i++)
            {
                sr.ReadLine();
            }
        }
    }
}
