using System.Windows;

namespace ScrabbleSolver {
    /// <summary>
    /// Interaction logic for AddPlayerWindow.xaml
    /// </summary>
    public partial class AddPlayerWindow {
        public AddPlayerWindow() {
            InitializeComponent();
        }


        /// <summary>
        /// Add button clicked
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e) {
            Players.AddPlayer(PlayerNameBox.Text, MainWindow.PlayersAdded);

            var newPlayer = new MainWindow.Players {
                Name = PlayerNameBox.Text,
                Points = 0
            };

            MainWindow.GamePlayers[MainWindow.PlayersAdded] = newPlayer;
            MainWindow.Order[MainWindow.PlayersAdded] = PlayerNameBox.Text;

            MainWindow.PlayersAdded++;
            Close();
        }

        /// <summary>
        /// Cancel button clicked
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}