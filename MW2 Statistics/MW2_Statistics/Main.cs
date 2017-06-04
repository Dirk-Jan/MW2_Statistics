using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MW2_Statistics
{
    public partial class Main : Form
    {
        private string mHostFilePath = string.Empty;
        private static readonly string mSettingsPath = Application.StartupPath + @"/hostfilelocation";

        private Thread mCollectorThread = null;
        private bool mKeepReading = true;
        private bool mExitedThread = true;

        public Main()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName))
            {
                mHostFilePath = ofd.FileName;
                tboxHostFilePath.Text = mHostFilePath;
                SaveHostFilePathToFile();
            }
        }

        private void btnCollect_Click(object sender, EventArgs e)
        {
            if (!mHostFilePath.Contains("games_mp.log"))
            {
                MessageBox.Show("Please locate the \"games_mp.log\" file on your computer. It's found in <MW2 root>/main/.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                btnBrowse.Enabled = false;
                btnCollect.Enabled = false;
                btnCollect.Text = "Collecting data ...";

                mExitedThread = false;
                mCollectorThread = new Thread(new ThreadStart(CollectFeed));
                mCollectorThread.Start();
            }
        }

        private void CollectFeed()
        {
            while(mKeepReading)
            {
                SourceFileReader.CollectFeed(mHostFilePath);
                Thread.Sleep(100);
            }
            mExitedThread = true;
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            LoadHostFilePathFromFile();
            tboxHostFilePath.Text = mHostFilePath;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            mKeepReading = false;
            this.Text = "Waiting for the collector to stop...";
            while (!mExitedThread)
                Thread.Sleep(100);
        }

        private void LoadHostFilePathFromFile()
        {
            try
            {
                if(File.Exists(mSettingsPath))
                {
                    using (var sr = new StreamReader(mSettingsPath))
                    {
                        mHostFilePath = sr.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error loading the settings.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveHostFilePathToFile()
        {
            try
            {
                using (var sw = new StreamWriter(mSettingsPath, false))
                {
                    sw.WriteLine(mHostFilePath);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("There was an error saving the settings.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
