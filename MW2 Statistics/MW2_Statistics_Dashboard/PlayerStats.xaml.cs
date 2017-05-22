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
        private List<Player> Players
        {
            set
            {
                lboxPlayers.ItemsSource = value;
                if (lboxPlayers.Items.Count > 0)
                    lboxPlayers.SelectedIndex = 0;
            }
        }
        public PlayerStats()
        {
            InitializeComponent();
        }

        private void lboxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Player p = (Player)e.AddedItems[0];
            induvidualPlayerView.LoadPlayerInfoInControl(p);
        }
    }
}
