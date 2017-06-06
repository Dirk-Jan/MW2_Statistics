using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MW2_Statistics_Dashboard
{
    /// <summary>
    /// Interaction logic for IndividualPlayerStats.xaml
    /// </summary>
    public partial class IndividualPlayerStats : UserControl
    {
        //private static readonly string mImagePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"/images/";
        private string mImagePath = "pack://application:,,,/Resources/";

        private long mPlayerId;
        private Match mMatch;
        private Player mMostKilled, mMostKilledBy;

        public IndividualPlayerStats()
        {
            InitializeComponent();
        }

        public void LoadPlayerInfoInControl(Player p, Match match)
        {
            mPlayerId = p.Id;
            mMatch = match;

            tblkPlayerName.Text = p.Aliasses[0];

            tblkLastSeen.Text = "Last seen: " + p.LastSeen.ToString("dd-MM-yyyy HH:mm");

            coboxAliases.ItemsSource = p.Aliasses;                  // Load aliases in combobox
            coboxAliases.SelectedIndex = 0;

            int kills = Database.GetKillCount(p.Id, mMatch);
            int deaths = Database.GetDeathCount(p.Id, mMatch);
            tblkKills.Text = kills.ToString();
            tblkDeaths.Text = deaths.ToString();
            if (deaths != 0)
            {
                double kdr = Math.Round((double)kills / (double)deaths, 2);
                tblkKDR.Text = kdr.ToString();
            }
            else
                tblkKDR.Text = "∞";

            tblkHeadshots.Text = Database.GetHeadshotCount(p.Id, mMatch).ToString();

            string favouriteWepon = Database.GetFavouriteWeapon(p.Id, mMatch);
            tblkFavWepName.Text = "";
            imgFavouriteWeapon.Source = null;                               // Empty image, otherwise when favouriteWeapon is null or no image was found in the for loop the image won't change
            imgFavWepAttachment1.Source = null;
            imgFavWepAttachment2.Source = null;
            if (!String.IsNullOrEmpty(favouriteWepon))
            {
                Weapon wep = new Weapon(-1, favouriteWepon);
                tblkFavWepName.Text = wep.CleanName;
                //if(File.Exists(mImagePath + wep.WeaponImage))
                if (wep.WeaponImage != ".png")
                    imgFavouriteWeapon.Source = new BitmapImage(new Uri(mImagePath + wep.WeaponImage));

                //if (wep.AttachmentImage1 != null && File.Exists(mImagePath + wep.AttachmentImage1))
                if (wep.AttachmentImage1 != ".png")
                    imgFavWepAttachment1.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage1));

                //if (wep.AttachmentImage2 != null && File.Exists(mImagePath + wep.AttachmentImage2))
                if (wep.AttachmentImage2 != ".png")
                    imgFavWepAttachment2.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage2));
            }

            mMostKilled = Database.GetMostKilledPlayer(p.Id, mMatch);
            mMostKilledBy = Database.GetMostKilledByPlayer(p.Id, mMatch);
            tblkMostKilled.Text = mMostKilled == null ? "No-one" : mMostKilled.Aliasses[0];
            tblkMostKilledBy.Text = mMostKilledBy == null ? "No-one" : mMostKilledBy.Aliasses[0];

            //tblkMostKilled.Text = Database.GetMostKilledPlayerName(p.Id, mMatch);
            //tblkMostKilledBy.Text = Database.GetMostKilledByPlayerName(p.Id, mMatch);

            tblkLongestKillingSpree.Text = Database.GetLongestKillingSpree(p.Id, mMatch).ToString();

            // Weapons tab
            lboxWeapons.ItemsSource = Database.GetWeapons(p.Id, mMatch);
            if (lboxWeapons.Items.Count > 0)
                lboxWeapons.SelectedIndex = 0;
        }

        private void lboxWeapons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            imgWeapon.Source = null;
            imgAttachment1.Source = null;
            imgAttachment2.Source = null;
            if (e.AddedItems.Count < 1)
            {
                tblkWeaponKills.Text = "-";
                tblkWeaponHeadShots.Text = "-";
                tblkWeaponKilledBy.Text = "-";
            }
            else
            {
                Weapon wep = (Weapon)e.AddedItems[0];

                if (wep.WeaponImage != ".png")
                    imgWeapon.Source = new BitmapImage(new Uri(mImagePath + wep.WeaponImage));

                //if(wep.AttachmentImage1 != null && File.Exists(mImagePath + wep.AttachmentImage1))
                if (wep.AttachmentImage1 != ".png")
                    imgAttachment1.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage1));

                //if (wep.AttachmentImage2 != null && File.Exists(mImagePath + wep.AttachmentImage2))
                if (wep.AttachmentImage2 != ".png")
                    imgAttachment2.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage2));

                tblkWeaponKills.Text = Database.GetKillCount(mPlayerId, wep.Id, mMatch).ToString();
                tblkWeaponHeadShots.Text = Database.GetHeadshotCount(mPlayerId, wep.Id, mMatch).ToString();
                tblkWeaponKilledBy.Text = Database.GetKilledByCount(mPlayerId, wep.Id, mMatch).ToString();
            }

        }

        public delegate void NameClickedEventHandler(Player player);
        public event NameClickedEventHandler NameClicked;
        protected virtual void OnNameClicked(Player player)
        {
            NameClicked?.Invoke(player);
        }

        private void tblkName_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(((TextBlock)sender).Name == "tblkMostKilled")
            {
                OnNameClicked(mMostKilled);
            }
            else if (((TextBlock)sender).Name == "tblkMostKilledBy")
            {
                OnNameClicked(mMostKilledBy);
            }
        }
    }
}
