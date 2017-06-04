using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics
{
    public static class SourceFileReader
    {
        private static string mRootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public static void CollectFeed(string hostFilePath)
        {
            if (!File.Exists(hostFilePath))
                return;

            string newFile = mRootPath + @"\copy";
            string lineCountFile = mRootPath + @"\linesread";
            // Copy file
            if (File.Exists(newFile))
                File.Delete(newFile);
            File.Copy(hostFilePath, newFile);

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
                if (feed.Trim() == string.Empty)
                    continue;

                // Get the data from the feed and put it in the database
                Debug.WriteLine("Feed(" + linesRead + "):" + feed);
                MW2Event mw2Event = new MW2Event(feed);
                MW2EventHandler.HandleMW2Event(mw2Event);

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
