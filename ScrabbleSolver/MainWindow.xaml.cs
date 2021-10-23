using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace ScrabbleSolver {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    #region Structs
    public partial class MainWindow {
        public struct PlayerData {
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
        #endregion

        public static List<LocationData> Matches;

        public static int PlayersAdded = 0; // Stores amount of players
        public static PlayerData[] GamePlayers = new PlayerData[6];
        public static string[] Order = new string[6];
        private List<List<string>> _anagrams;

        private static string[,] _debugBoard;

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
                    Dispatcher.Invoke(() => {
                        _anagrams = Anagram.GetAnagrams(text);
                    });
                });
                thread.Start();

                // Send it to uppercase
                YourLettersBox.Text = YourLettersBox.Text.ToUpper();
            }
        }
        
        private string[,] FillBoard() {
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

            return boxesArray;
        }

        private void FillButton_Click(object sender, RoutedEventArgs e) {
            if (PlayersAdded == 0) {
                AddPlayerDialog();

                return;
            }
            // TODO: Figure out the best first move
            
            string letters = YourLettersBox.Text;
            var boxesArray = FillBoard();
            // Do something
            var thread = new Thread(() => {
                var positions = Fill.FindAllPlaces(boxesArray);
            
                // Check if Anagrams has been filled or not
                if (_anagrams == null) {
                    _anagrams = Anagram.GetAnagrams(letters);
                }

                Fill.FillFromPosition(boxesArray, 0, letters, _anagrams,
                    positions);

            });
            thread.Start();
            //Fill.FindMatches(boxesArray, YourLettersBox.Text);

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
                    box.Text = box.Text.ToUpper();
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

            Players.UpdateScores(GamePlayers);

            PreviousBoard = (string[,]) boxesArray.Clone();
        }
        private void DebugModeItem_OnClick(object sender, RoutedEventArgs e) {
            if (DebugModeItem.IsChecked) {
                FirstDebugButton.Visibility = Visibility.Visible;
                NextDebugButton.Visibility = Visibility.Visible;
                FirstDebugLabel.Visibility = Visibility.Visible;
            }
            else {
                FirstDebugButton.Visibility = Visibility.Hidden;
                NextDebugButton.Visibility = Visibility.Hidden;
                FirstDebugLabel.Visibility = Visibility.Hidden;
            }
        }

        private void SetBoard(string[,] boxesArray) {
            foreach (var b in BoardGrid.Children) {
                if (b is Border border) {
                    Object t = border.Child;
                    if (!(t is TextBox box)) continue;
                    
                    var y = (int)border.GetValue(Grid.RowProperty);
                    var x = (int)border.GetValue(Grid.ColumnProperty);
                    box.Text = boxesArray[y, x];
                }
            }
        }

        private int _viewing;
        private List<NewBoardConfig> _results;
        private void NextDebugButtonSelected(object sender, RoutedEventArgs e) {
            _viewing = 0;
            foreach (var b in BoardGrid.Children) {
                if (b is Border border) {
                    Object t = border.Child;
                    
                    if (!(t is TextBox box)) continue;
                    
                    box.IsReadOnly = _isReadOnly;
                    var y = (int)border.GetValue(Grid.RowProperty);
                    var x = (int)border.GetValue(Grid.ColumnProperty);
                    box.Text = _debugBoard[y, x];
                }
            }
            
            _results = new List<NewBoardConfig>();
            string letters = YourLettersBox.Text.ToUpper();
            var thread = new Thread(() => {
                Dispatcher.Invoke(() => {
                    
                    var boxesArray = FillBoard();

                    var positions = Fill.FindAllPlaces(boxesArray);

                    // Check if Anagrams has been filled or not
                    if (_anagrams == null) {
                        _anagrams = Anagram.GetAnagrams(letters);
                    }

                    // for (int i = -1; i < positions.Count; ++i) {
                    //     var positionThreads = new Thread(() => {
                    //         var newResults = Fill.FillFromPosition(boxesArray, i, letters,
                    //             _anagrams, positions);
                    //
                    //         lock (_results) {
                    //             foreach (var result in newResults) {
                    //                 _results.Add(result);
                    //             }
                    //         }
                    //     });
                    //     positionThreads.Start();
                    // }
                    
                    // TODO: There's a million duplicates and it's putting letters in invalid places

                    for (int i = 0; i < positions.Count; i++) {
                        var newResults = Fill.FillFromPosition(boxesArray, i, letters,
                            _anagrams, positions);
                        foreach (var result in newResults) {
                            _results.Add(result);
                        }
                    }

                    // var newResults = Fill.FillFromPosition(boxesArray, 0, letters,
                    //     _anagrams, positions);

                    // foreach (var result in newResults) {
                    //     _results.Add(result);
                    // }
                    FirstDebugLabel.Text = "Viewing " + _viewing + "/" + _results.Count;
                        
                    _viewing++;
                });
            });
            thread.Start();
        }

        private void FirstDebugButton_OnClick(object sender, RoutedEventArgs e) {
            NewBoardConfig config = _results[_viewing - 1];
            
            SetBoard(config.Board);
                
            FirstDebugLabel.Text = "Viewing " + _viewing + "/" + _results.Count;
            _viewing += 1;
        }

        private void OverrideBoardClick(object sender, RoutedEventArgs e) {
            bool isEditing = false;
            // We allow the user to edit the board without changing scoring
            // An array of TextBox objects from our board
            var boxesArray = new string[15, 15];

            foreach (var b in BoardGrid.Children) {
                if (b is Border border) {
                    Object t = border.Child;
                    
                    if (!(t is TextBox box)) continue;
                    
                    var y = (int)border.GetValue(Grid.RowProperty);
                    var x = (int)border.GetValue(Grid.ColumnProperty);
                    boxesArray[y, x] = box.Text.ToUpper();
                    
                    box.IsReadOnly = !box.IsReadOnly;
                    box.Text = box.Text.ToUpper();
                    if (box.IsReadOnly == true) {
                        isEditing = true;
                    }
                    
                }
            }

            if (isEditing) {
                var messageBoxText = "The board has been saved!";
                var caption = "ScrabbleSolver";
                var button = MessageBoxButton.OK;
                var icon = MessageBoxImage.Information;

                MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
            _debugBoard = (string[,]) boxesArray.Clone();
        }

        /// <summary>
        /// Saves the game board, players, scores, turn, and letters
        /// </summary>
        private void SaveGameClick(object sender, RoutedEventArgs e) {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Board"; // Default file name
            dlg.DefaultExt = ".ss"; // Default file extension
            dlg.Filter = "ScrabbleSolver Boards (.ss)|*.ss"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            var path = "";
            if (result == true) {
                // Save file path
                path = dlg.FileName;
            }
            
            var boxesArray = new string[15, 15];
            foreach (var b in BoardGrid.Children) {
                if (b is Border border) {
                    Object t = border.Child;
                    
                    if (!(t is TextBox box)) continue;
                    
                    box.Text = box.Text.ToUpper();
                    var y = (int)border.GetValue(Grid.RowProperty);
                    var x = (int)border.GetValue(Grid.ColumnProperty);
                    boxesArray[y, x] = box.Text;
                }
            }
            
            // Here we convert the array to a list
            var boardList = new List<List<String>>();
            for (int i = 0; i < 15; i++) {
                var miniList = new List<String>();
                for (int j = 0; j < 15; j++) {
                    miniList.Add(boxesArray[i, j]);
                }

                boardList.Add(miniList);
            }

            var players = new List<List<String>>();

            foreach (var player in GamePlayers) {
                // The format is "name", "points"
                var l = new List<String>();
                l.Add(player.Name);
                l.Add(player.Points + "");

                players.Add(l);
            }

            List<GameManifest> save = new List<GameManifest>();
            save.Add(new GameManifest() {
                Board = boardList,
                Letters = YourLettersBox.Text.ToUpper(),
                Turn = _turnNumber,
                Players = players
            });

            string json = JsonConvert.SerializeObject(save.ToArray());
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Restores the game board, players, scores, turn, and letters
        /// </summary>
        private void RestoreGameClick(object sender, RoutedEventArgs e) {
            PlayersAdded = 0;
            
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".ss";
            dlg.Filter = "ScrabbleSolver Data (*.ss)|*.ss";
            
            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            var path = "";
            if (result == true) {
                // Open document 
                path = dlg.FileName;
            }
            
            var json = File.ReadAllText(path);
            
            List<GameManifest> readData =
                JsonConvert.DeserializeObject<List<GameManifest>>(json);

            GameManifest save = readData[0];
            
            // Convert list back to board
            string[,] board = new string[15, 15];
            for (int i = 0; i < 15; i++) {
                var subBoard = save.Board[i];
                for (int j = 0; j < 15; j++) {
                    board[i, j] = subBoard[j];
                }
            }

            SetBoard(board);
            // Set letters
            YourLettersBox.Text = save.Letters.ToUpper();
            
            // Adds a new player
            for (int i = 0; i < 6; i++) {
                var player = new PlayerData();

                var subList = save.Players[i];
                player.Name = subList[0];
                player.Points = Int32.Parse(subList[1]);
                
                // Skip if nonexistent
                if (player.Name == null) {
                    continue;
                }
                
                GamePlayers[PlayersAdded] = player;
                Order[PlayersAdded] = player.Name;
                PlayersAdded++;
                
                Players.AddPlayer(player.Name, PlayersAdded - 1);
            }
            // Sets the scores
            Players.UpdateScores(GamePlayers);
            
            // Set the player turn
            _turnNumber = save.Turn;
            TurnTextBox.Text = "Turn: " + Order[_turnNumber];
        }
        
        /// <summary>
        /// Clears the game board
        /// </summary>
        private void ClearBoardClick(object sender, RoutedEventArgs e) {
            // Iterates over the board and for each text box in the baord,
            // set text to ""
            foreach (var b in BoardGrid.Children) {
                if (b is Border border) {
                    Object t = border.Child;
                    
                    if (!(t is TextBox box)) continue;

                    box.Text = "";
                    var y = (int)border.GetValue(Grid.RowProperty);
                    var x = (int)border.GetValue(Grid.ColumnProperty);
                }
            }
        }
    }
}
