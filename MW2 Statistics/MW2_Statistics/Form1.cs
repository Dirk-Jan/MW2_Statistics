using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MW2_Statistics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataBase.AddPlayer(666);
            
        }

        private void btnTimeToLong_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            long now = dt.ToBinary();
            MessageBox.Show(now.ToString());
        }

        private void btnRegMatch_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataBase.RegisterNewMatch().ToString());
        }

        private void btnEndMatch_Click(object sender, EventArgs e)
        {
            DataBase.EndMatch(1);
        }

        private void btnGetPlayerId_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataBase.GetPlayerIdBySteamId(9999).ToString());
        }

        private void btnAddHit_Click(object sender, EventArgs e)
        {
            DataBase.RegisterHit(1, 1, 1, 1, 78, "upper_arm", "MOD_riflebullet", false);
        }

        private void btnAddWeapon_Click(object sender, EventArgs e)
        {
            DataBase.AddWeapon("ump45_mp");
        }

        private void btnWepExists_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataBase.WeaponExists("ump45_mp").ToString());
        }

        private void btnAddAlias_Click(object sender, EventArgs e)
        {
            DataBase.AddPlayerAlias("koekert", 1, 1);
        }

        private void btnAliasExists_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataBase.PlayerAliasExists("koekert", 1, 1).ToString());
        }

        private void btnUpdatePlayerLastSeen_Click(object sender, EventArgs e)
        {
            DataBase.UpdatePlayerLastSeen(1);
        }

        private void btnEmptyAllTables_Click(object sender, EventArgs e)
        {
            DataBase.EmptyAllTables();
        }

        private void btnReadLogFile_Click(object sender, EventArgs e)
        {
            SourceFileReader.CollectFeed();
        }

        private void btnSteamIdConvert_Click(object sender, EventArgs e)
        {
            string s = "0110000102e85117";
            byte[] temp = StringToByteArray(s);
            temp = temp.Reverse().ToArray();

            string output = string.Empty;
            for (int i = 0; i < temp.Length; i++)
            {
                output += temp[i].ToString("X2") + " ";
            }
            MessageBox.Show( output + Environment.NewLine + BitConverter.ToUInt64(temp, 0).ToString() );
        }

        public byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
