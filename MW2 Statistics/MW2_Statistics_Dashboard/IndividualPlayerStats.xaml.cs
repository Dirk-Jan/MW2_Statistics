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
        private long mPlayerId;
        private Match mMatch;

        private string mImagePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"/images/";
        public IndividualPlayerStats()
        {
            InitializeComponent();
        }

        public void LoadPlayerInfoInControl(Player p, Match match)
        {
            mPlayerId = p.Id;
            mMatch = match;

            tblkPlayerName.Text = p.Aliasses[p.Aliasses.Count - 1];

            tblkLastSeen.Text = "Last seen: " + p.LastSeen.ToString("dd-MM-yyyy HH:mm");

            coboxAliases.ItemsSource = p.Aliasses;                  // Load aliases in combobox
            coboxAliases.SelectedIndex = p.Aliasses.Count > 0 ? p.Aliasses.Count - 1 : 0;      // Set selected index to last

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
            /*if (!String.IsNullOrEmpty(favouriteWepon))
            {
                Uri val = GetWeaponImageUri(favouriteWepon);
                if(val != null)
                    imgFavouriteWeapon.Source = new BitmapImage(val);
            }*/
            imgFavWepAttachment1.Source = null;
            imgFavWepAttachment2.Source = null;
            if (!String.IsNullOrEmpty(favouriteWepon))
            {
                Weapon wep = new Weapon(-1, favouriteWepon);
                tblkFavWepName.Text = wep.CleanName;
                if(File.Exists(mImagePath + wep.WeaponImage))
                    imgFavouriteWeapon.Source = new BitmapImage(new Uri(mImagePath + wep.WeaponImage));
                
                if (wep.AttachmentImage1 != null && File.Exists(mImagePath + wep.AttachmentImage1))
                    imgFavWepAttachment1.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage1));
                
                if (wep.AttachmentImage2 != null && File.Exists(mImagePath + wep.AttachmentImage2))
                    imgFavWepAttachment2.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage2));
            }

            tblkMostKilled.Text = Database.GetMostKilledPlayerName(p.Id, mMatch);
            tblkMostKilledBy.Text = Database.GetMostKilledByPlayerName(p.Id, mMatch);

            tblkLongestKillingSpree.Text = Database.GetLongestKillingSpree(p.Id, mMatch).ToString();

            //Weapon tab
            lboxWeapons.ItemsSource = Database.GetWeapons(p.Id, mMatch);
            if (lboxWeapons.Items.Count > 0)
                lboxWeapons.SelectedIndex = 0;
        }

        private Uri GetWeaponImageUri(string weaponName)
        {
            Uri value = null;
            string imagePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"/images/";
            string[] images = Directory.GetFiles(imagePath);
            foreach (var item in images)
            {
                if (weaponName.Contains(System.IO.Path.GetFileNameWithoutExtension(item)))
                {
                    value = new Uri(item);
                    break;
                }
            }
            return value;
        }

        private void lboxWeapons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
            {
                imgWeapon.Source = null;
                tblkWeaponKills.Text = "-";
                tblkWeaponHeadShots.Text = "-";
                tblkWeaponKilledBy.Text = "-";
            }
            else
            {
                Weapon wep = (Weapon)e.AddedItems[0];

                imgWeapon.Source = null;
                /*Uri val = GetWeaponImageUri(wep.Name);
                if (val != null)
                    imgWeapon.Source = new BitmapImage(val);*/
                if (File.Exists(mImagePath + wep.WeaponImage))
                    imgWeapon.Source = new BitmapImage(new Uri(mImagePath + wep.WeaponImage));
                imgAttachment1.Source = null;
                if(wep.AttachmentImage1 != null && File.Exists(mImagePath + wep.AttachmentImage1))
                    imgAttachment1.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage1));
                imgAttachment2.Source = null;
                if (wep.AttachmentImage2 != null && File.Exists(mImagePath + wep.AttachmentImage2))
                    imgAttachment2.Source = new BitmapImage(new Uri(mImagePath + wep.AttachmentImage2));

                tblkWeaponKills.Text = Database.GetKillCount(mPlayerId, wep.Id, mMatch).ToString();
                tblkWeaponHeadShots.Text = Database.GetHeadshotCount(mPlayerId, wep.Id, mMatch).ToString();
                tblkWeaponKilledBy.Text = Database.GetKilledByCount(mPlayerId, wep.Id, mMatch).ToString();
            }

        }
    }
}
