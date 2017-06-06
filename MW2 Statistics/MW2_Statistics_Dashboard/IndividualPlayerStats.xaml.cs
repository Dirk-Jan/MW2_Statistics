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
        private static readonly string mImagePath = "pack://application:,,,/Resources/";

        public delegate void NameClickedEventHandler(Player player);
        public event NameClickedEventHandler NameClicked;

        private Match mMatch;
        private Player mPlayer, mMostKilled, mMostKilledBy;

        public IndividualPlayerStats()
        {
            InitializeComponent();
        }

        public void LoadPlayerInfoInControl(Player p, Match match)
        {
            //mPlayerId = p.Id;
            mPlayer = p;
            mMatch = match;

            tblkPlayerName.Text = p.Aliasses[0];

            tblkLastSeen.Text = "Last seen: " + p.LastSeen.ToString("dd-MM-yyyy HH:mm");

            coboxAliases.ItemsSource = p.Aliasses;                  // Load aliases in combobox
            coboxAliases.SelectedIndex = 0;

            int kills = p.GetKillCount(mMatch);
            int deaths = p.GetDeathCount(mMatch);
            tblkKills.Text = kills.ToString();
            tblkDeaths.Text = deaths.ToString();
            if (deaths != 0)
            {
                double kdr = Math.Round((double)kills / (double)deaths, 2);
                tblkKDR.Text = kdr.ToString();
            }
            else
                tblkKDR.Text = "∞";

            tblkHeadshots.Text = p.GetHeadshotCount(mMatch).ToString();

            string favouriteWepon = p.GetFavouriteWeapon(mMatch);
            tblkFavWepName.Text = "";
            imgFavouriteWeapon.Source = null;                               // Empty image, otherwise when favouriteWeapon is null or no image was found in the for loop the image won't change
            imgFavWepAttachment1.Source = null;
            imgFavWepAttachment2.Source = null;
            if (!String.IsNullOrEmpty(favouriteWepon))
            {
                Weapon wep = new Weapon(-1, favouriteWepon);
                tblkFavWepName.Text = wep.CleanName;

                if (wep.WeaponImage != ".png")
                    imgFavouriteWeapon.Source = new BitmapImage(new Uri(mImagePath + wep.WeaponImage));
                
                if (wep.AttachmentImage1 != ".png")
                    imgFavWepAttachment1.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage1));
                
                if (wep.AttachmentImage2 != ".png")
                    imgFavWepAttachment2.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage2));
            }

            mMostKilled = p.GetMostKilledPlayer(mMatch);
            mMostKilledBy = p.GetMostKilledByPlayer(mMatch);
            tblkMostKilled.Text = mMostKilled == null ? "No-one" : mMostKilled.Aliasses[0];
            tblkMostKilledBy.Text = mMostKilledBy == null ? "No-one" : mMostKilledBy.Aliasses[0];

            tblkLongestKillingSpree.Text = p.GetLongestKillingSpree(mMatch).ToString();

            // Weapons tab
            lboxWeapons.ItemsSource = Weapon.GetWeapons(p.Id, mMatch);
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
                
                if (wep.AttachmentImage1 != ".png")
                    imgAttachment1.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage1));
                
                if (wep.AttachmentImage2 != ".png")
                    imgAttachment2.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage2));

                tblkWeaponKills.Text = mPlayer.GetKillCount(wep.Id, mMatch).ToString();
                tblkWeaponHeadShots.Text = mPlayer.GetHeadshotCount(wep.Id, mMatch).ToString();
                tblkWeaponKilledBy.Text = mPlayer.GetKilledByCount(wep.Id, mMatch).ToString();
            }

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

        protected virtual void OnNameClicked(Player player)
        {
            NameClicked?.Invoke(player);
        }
    }
}
