using System;
using System.Collections.Generic;
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
    /// Interaction logic for PlayerStats.xaml
    /// </summary>
    public partial class PlayerStats : UserControl
    {
        public List<Player> Players
        {
            set
            {
                lboxPlayers.ItemsSource = value;
                if (lboxPlayers.Items.Count > 0)
                    lboxPlayers.SelectedIndex = 0;
            }
        }
        private Match mMatch;
        public Match Match
        {
            get { return mMatch; }
            set
            {
                mMatch = value;
                //Players = mMatch.Players;
            }
        }
        public PlayerStats()
        {
            InitializeComponent();
            induvidualPlayerView.NameClicked += InduvidualPlayerView_NameClicked;
        }

        private void InduvidualPlayerView_NameClicked(Player player)
        {
            if (lboxPlayers.Items.Contains(player))
            {
                lboxPlayers.SelectedIndex = lboxPlayers.Items.IndexOf(player);
            }
        }

        private void lboxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Player p = (Player)e.AddedItems[0];
                induvidualPlayerView.LoadPlayerInfoInControl(p, Match);
            }
        }

        private void tboxPlayerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Players = Player.GetPlayersWithFilter(Match, tboxPlayerName.Text);
        }
    }
}
