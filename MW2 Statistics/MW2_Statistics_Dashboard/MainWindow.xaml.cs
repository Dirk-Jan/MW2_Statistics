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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            var matches = Database.GetMatches();
            matches.Insert(0, new Match(-1, DateTime.Now.ToBinary(), DateTime.Now.ToBinary()));

            DateTime lastDate = DateTime.Now;
            for (int i = 0; i < matches.Count; i++)
            {
                if(lastDate.Date != matches[i].DateTimeStart.Date)
                {
                    lastDate = matches[i].DateTimeStart;
                    matches.Insert(i, new Match(-1, lastDate.ToBinary(), lastDate.ToBinary()));
                }
            }
            lboxMatches.ItemsSource = matches;

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
    }
}
