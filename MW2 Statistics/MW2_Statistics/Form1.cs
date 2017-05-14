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
            DatabaseFiller df = new DatabaseFiller();
            df.CollectFeed();
            listBox1.DataSource = df.Feed;
            MessageBox.Show(df.Feed[df.Feed.Count-1]);
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
            DataBase.RegisterNewMatch();
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
    }
}
