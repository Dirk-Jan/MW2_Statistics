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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random mRandom = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            var matches = Database.GetMatches();
            Database.AddDataLabelsToMatchesList(matches);
            lboxMatches.ItemsSource = matches;
            if (matches.Count > 1)
                lboxMatches.SelectedIndex = 1;

            psOverall.Match = null;                             // For the database methods
            psOverall.Players = Database.GetPlayers(null);
        }

        private void lboxMatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Match m = (Match)e.AddedItems[0];
                psMainWindow.Match = m;                         // For the database methods
                psMainWindow.Players = Database.GetPlayers(m);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (dpRangeStart.SelectedDate.HasValue && dpRangeStop.SelectedDate.HasValue)
            {
                lboxMatches.ItemsSource = Database.AddDataLabelsToMatchesList(Database.GetMatches(dpRangeStart.SelectedDate.Value.Ticks, dpRangeStop.SelectedDate.Value.Ticks));
            }
            else
                lboxMatches.ItemsSource = Database.AddDataLabelsToMatchesList(Database.GetMatches());

            if (lboxMatches.Items.Count > 1)
                lboxMatches.SelectedIndex = 1;
        }

        private void btnRandomNumber_Click(object sender, RoutedEventArgs e)
        {
            int r = mRandom.Next();
            StringBuilder sb = new StringBuilder("This is your useless random number: ");
            sb.Append(r.ToString());
            sb.Append(". Have fun with this useless random number!");
            MessageBox.Show(sb.ToString());
        }
    }
}
