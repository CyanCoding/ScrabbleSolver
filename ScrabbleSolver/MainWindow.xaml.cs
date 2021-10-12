using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ScrabbleSolver {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public struct Players {
            public string Name;
            public int Points;
        }
        public struct LocationData {
            public int X;
            public int Y;
            public string Letter;
        }

        public struct NewBoardConfig {
            public string[,] Board;
            public List<LocationData> NewLocations;
        }

        public static List<LocationData> Matches;

        public static int PlayersAdded = 0; // Stores amount of players
        public static Players[] GamePlayers = new Players[6];
        public static string[] Order = new string[6];
        

        public MainWindow() {
            InitializeComponent();
        }

        private void AddPlayerMenuItemClick(object sender, RoutedEventArgs e) {
            var window = new AddPlayerWindow();
            window.Show();
        }

        private void SetButton_Click(object sender, RoutedEventArgs e) {
            // They're setting the characters
            if (SetButton.Content.Equals("Set")) {
                SetButton.Content = "Solidify";
                YourLettersBox.IsReadOnly = false;
            }
            // They're done setting characters
            else if (SetButton.Content.Equals("Solidify")) {
                SetButton.Content = "Set";
                YourLettersBox.IsReadOnly = true;
                
                // We create a thread to get the anagrams
                var text = YourLettersBox.Text;
            
                var thread = new Thread(() => {
                    var anagrams = Anagram.GetAnagrams(text);
                });
                thread.Start();

                // Send it to uppercase
                YourLettersBox.Text = YourLettersBox.Text.ToUpper();
            }
        }

        private void FillButton_Click(object sender, RoutedEventArgs e) {
            if (PlayersAdded == 0) {
                AddPlayerDialog();

                return;
            }
            // TODO: Figure out the best first move

            // An array of TextBox objects from our board
            var boxesArray = new string[15, 15];

            // I'm proud of myself for making this
            // Gets children of grid and children of the borders
            // Then sets those children to be readonly or not
            foreach (var b in BoardGrid.Children) {
                if (b is Border border) {
                    Object t = border.Child;
                    
                    if (!(t is TextBox box)) continue;
                    
                    var y = (int)border.GetValue(Grid.RowProperty);
                    var x = (int)border.GetValue(Grid.ColumnProperty);
                    boxesArray[y, x] = box.Text;
                }
            }

            Fill.FindMatches(boxesArray, YourLettersBox.Text);

        }
        private void ShuffleButton_Click(object sender, RoutedEventArgs e) {
            var text = YourLettersBox.Text;

            var rand = new Random();
            // nobody actually knows how this works; it just does
            YourLettersBox.Text = new string(text.ToCharArray().OrderBy(x => (rand.Next(2) % 2) == 0).ToArray());
        }

        /// <summary>
        /// When a dialog prompting the user to add a player needs to be shown
        /// </summary>
        private void AddPlayerDialog() {
            var messageBoxText = "You must add a player first!";
            var caption = "ScrabbleSolver";
            var button = MessageBoxButton.OK;
            var icon = MessageBoxImage.Error;

            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

        int _turnNumber = -1;
        /// <summary>
        /// When the user clicks on the Next Turn button.
        /// </summary>
        private void nextTurn_Click(object sender, RoutedEventArgs e) {
            if (PlayersAdded == 0) {
                AddPlayerDialog();

                return;
            }

            if (_turnNumber + 1 >= PlayersAdded) {
                _turnNumber = -1;
            }

            _turnNumber++;
            TurnTextBox.Text = "Turn: " + Order[_turnNumber];
            
            // Set turn number to 0 if we've finished all player turns

        }

        private bool _isReadOnly = true;
        public static string[,] PreviousBoard = new string[15,15];
        private void editBoard_Click(object sender, RoutedEventArgs e) {
            if (PlayersAdded == 0) {
                AddPlayerDialog();

                return;
            }
            if (TurnTextBox.Text == "Turn: ") {
                var messageBoxText = "Please select a player first.";
                var caption = "ScrabbleSolver";
                var button = MessageBoxButton.OK;
                var icon = MessageBoxImage.Error;

                MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                return;
            }

            _isReadOnly = !_isReadOnly;

            // An array of TextBox objects from our board
            var boxesArray = new string[15, 15];

            // I'm proud of myself for making this
            // Gets children of grid and children of the borders
            // Then sets those children to be readonly or not
            foreach (var b in BoardGrid.Children) {
                if (b is Border border) {
                    Object t = border.Child;
                    
                    if (!(t is TextBox box)) continue;
                    
                    box.IsReadOnly = _isReadOnly;
                    var y = (int)border.GetValue(Grid.RowProperty);
                    var x = (int)border.GetValue(Grid.ColumnProperty);
                    boxesArray[y, x] = box.Text;
                }
            }

            var value = Points.CalculatePoints(boxesArray, PreviousBoard);

            for (var i = 0; i < GamePlayers.Length; i++) {
                if (Order[_turnNumber] == GamePlayers[i].Name) {
                    GamePlayers[i].Points += value.Item1;
                }
            }

            ScrabbleSolver.Players.UpdateScores(GamePlayers);

            PreviousBoard = boxesArray;
        }
    }
}
