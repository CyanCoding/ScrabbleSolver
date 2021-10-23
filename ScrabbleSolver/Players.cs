using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ScrabbleSolver {
    /// <summary>
    /// Scoring and players
    /// </summary>
    internal static class Players {
        /// <summary>
        /// Adds a player to the scoring grid
        /// </summary>
        /// <param name="playerName">The name of the player</param>
        /// <param name="playersAdded">The amount of players added</param>
        /// <exception cref="Exception">If more than 6 players are added</exception>
        public static void AddPlayer(string playerName, int playersAdded) {
            if (Application.Current.Windows.Cast<Window>()
                .FirstOrDefault(window => window is MainWindow) is MainWindow
                mainWin) {
                var g = mainWin.ScoringGrid;

                if (playersAdded < 6) {
                    // Add a new row to the grid
                    var row = new RowDefinition {
                        Height = new GridLength(50, GridUnitType.Pixel)
                    };

                    g.RowDefinitions.Add(row);

                    var newPlayer = new TextBlock {
                        FontSize = 18
                    };
                    newPlayer.SetValue(Grid.RowProperty, playersAdded);
                    newPlayer.Text = (playersAdded + 1) + ". " + playerName;

                    var playerScore = new TextBlock {
                        FontSize = 18,
                        TextAlignment = TextAlignment.Right
                    };
                    playerScore.SetValue(Grid.ColumnProperty, 1);
                    playerScore.SetValue(Grid.RowProperty, playersAdded);
                    playerScore.Text = "0";

                    g.Children.Add(newPlayer);
                    g.Children.Add(playerScore);
                }
                else {
                    var messageBoxText = "Too many players! Max is 6.";
                    var caption = "ScrabbleSolver";
                    var button = MessageBoxButton.OK;
                    var icon = MessageBoxImage.Error;

                    MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                }
            }
        }

        public static void UpdateScores(MainWindow.Players[] players) {
            if (Application.Current.Windows.Cast<Window>()
                .FirstOrDefault(window => window is MainWindow) is MainWindow
                mainWin) {
                var g = mainWin.ScoringGrid;

                g.Children.Clear();

                // Sort players from greatest to smallest
                bool sorted;
                do {
                    sorted = false;
                    for (var i = 1; i < players.Length - 1; i++) {
                        if (players[i].Points > players[i - 1].Points) {
                            (players[i], players[i - 1]) =
                                (players[i - 1], players[i]);
                            sorted = true;
                        }
                    }
                } while (sorted);

                for (var i = 0; i < players.Length; i++) {
                    if (players[i].Name == null) {
                        continue;
                    }

                    var newPlayer = new TextBlock {
                        FontSize = 18
                    };
                    newPlayer.SetValue(Grid.RowProperty, i);
                    newPlayer.Text = (i + 1) + ". " + players[i].Name;

                    var playerScore = new TextBlock {
                        FontSize = 18,
                        TextAlignment = TextAlignment.Right
                    };
                    playerScore.SetValue(Grid.ColumnProperty, 1);
                    playerScore.SetValue(Grid.RowProperty, i);
                    playerScore.Text = players[i].Points + "";

                    g.Children.Add(newPlayer);
                    g.Children.Add(playerScore);
                }
            }
        }
    }
}
