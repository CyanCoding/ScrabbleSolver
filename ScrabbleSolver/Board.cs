using System.Collections.Generic;
using System.Windows;

namespace ScrabbleSolver {
    /// <summary>
    /// Mostly used with board operations, scoring, etc.
    /// </summary>
    static class Board {

        private static List<MainWindow.LocationData> _locationsUsed;
        private static List<MainWindow.LocationData> _specialLocationsUsed =
            new List<MainWindow.LocationData>();

        private static bool CheckLocation(int y, int x) {
            var location = new MainWindow.LocationData {
                X = x,
                Y = y
            };

            if (!_locationsUsed.Contains(location)) {
                _locationsUsed.Add(location);
                return true;
            }
            return false;
        }
        private static void AdjacentLetter(int y, int x, string[,] boxesArray) {
            string thisLetter;

            // Add this location to the data
            var thisLocation = new MainWindow.LocationData
                {
                    X = x,
                    Y = y
                };

            if (!_locationsUsed.Contains(thisLocation)) {
                _locationsUsed.Add(thisLocation);
            }

            // Test for letters above the current letter
            if (y > 0) {
                do {
                    thisLetter = boxesArray[y - 1, x];

                    if (thisLetter != "") {
                        // If it evaluates to FALSE, that character has already
                        // been searched before. If we didn't do this we would
                        // have an infinite loop
                        if (CheckLocation(y - 1, x)) {
                            y--;
                        }
                        else {
                            thisLetter = "";
                        }

                    }
                } while (thisLetter != "" && y > 0);
            }

            y = thisLocation.Y; // Reset y

            // Test for letters below the current letter
            if (y < 14) {
                do {
                    thisLetter = boxesArray[y + 1, x];

                    if (thisLetter != "") {
                        if (CheckLocation(y + 1, x)) {
                            y++;
                        }
                        else {
                            thisLetter = "";
                        }
                        
                    }
                } while (thisLetter != "" && y < 14);
            }

            y = thisLocation.Y; // Reset y

            // Test for letters left of the current letter
            if (x > 0) {
                do {
                    thisLetter = boxesArray[y, x - 1];

                    if (thisLetter != "") {
                        if (CheckLocation(y, x - 1)) {
                            x--;
                        }
                        else {
                            thisLetter = "";
                        }
                        
                    }
                } while (thisLetter != "" && x > 0);
            }

            x = thisLocation.X; // Reset x

            // Test for letters right of the current letter
            if (x < 14) {
                do {
                    thisLetter = boxesArray[y, x + 1];

                    if (thisLetter != "") {
                        if (CheckLocation(y , x + 1)) {
                            x++;
                        }
                        else {
                            thisLetter = "";
                        }
                        
                    }
                } while (thisLetter != "" && x < 14);
            }
        }


        private static bool _doubleValue;
        private static bool _tripleValue;
        public static (int, string) CalculatePoints(string[,] boxesArray, string[,] previousArray) {
            var lettersAssociated = new string[15, 15];
            _locationsUsed = new List<MainWindow.LocationData>();

            // Sort through the entire board
            for (var i = 0; i < boxesArray.GetLength(0); i++) {
                for (var l = 0; l < boxesArray.GetLength(1); l++) {
                    // Check for differences between current and past board
                    if (boxesArray[i, l] != previousArray[i, l]) {
                        // Different letter than last time!
                        AdjacentLetter(i, l, boxesArray);
                        var text = boxesArray[i, l];
                        lettersAssociated[i, l] = text;
                    }
                }
            }

            var adjacentLetters = "";
            var points = new Points();
            var totalPoints = 0;

            try {
                foreach (var l in _locationsUsed) {
                    var x = l.X;
                    var y = l.Y;

                    adjacentLetters += boxesArray[y, x];

                    if (boxesArray[y,x] == "") {
                        continue;
                    }

                    var value = points.Map[boxesArray[y, x]];
                    value = PointsValue(value, l);

                    totalPoints += value;
                }

                if (_tripleValue) {
                    totalPoints *= 3;
                    _tripleValue = false;
                }
                if (_doubleValue) {
                    totalPoints *= 2;
                    _doubleValue = false;
                }
                return (totalPoints, adjacentLetters);
            }
            // This is caused by there being an invalid character
            catch (KeyNotFoundException) {
                var messageBoxText = "An invalid character is present!\nPlease override the board to fix the mistake.";
                var caption = "ScrabbleSolver";
                var button = MessageBoxButton.OK;
                var icon = MessageBoxImage.Error;

                MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                return (0, "");
            }
        }

        private static int PointsValue(int points, MainWindow.LocationData location) {
            var x = location.X;
            var y = location.Y;

            if (!_specialLocationsUsed.Contains(location)) {
                // Triple word score
                if (x == 0 && y == 0) {
                    _specialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 0 && x == 7) {
                    _specialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 0 && x == 14) {
                    _specialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 7 && x == 0) {
                    _specialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 7 && x == 14) {
                    _specialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 14 && x == 0) {
                    _specialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 14 && x == 7) {
                    _specialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 14 && x == 14) {
                    _specialLocationsUsed.Add(location);
                    _tripleValue = true;
                }

                // Double word score
                if (y == 1 && x == 1) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 1 && x == 13) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 2 && x == 2) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 2 && x == 12) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 3 && x == 3) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 3 && x == 11) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 4 && x == 4) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 4 && x == 10) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 13 && x == 1) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 13 && x == 13) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 12 && x == 2) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 12 && x == 12) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 11 && x == 3) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 11 && x == 11) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 10 && x == 4) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 10 && x == 10) {
                    _specialLocationsUsed.Add(location);
                    _doubleValue = true;
                }

                if (y == 5 && x == 5) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 5 && x == 9) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 9 && x == 9) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 9 && x == 5) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 5 && x == 1) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 9 && x == 1) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 5 && x == 13) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 9 && x == 13) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 1 && x == 5) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 1 && x == 9) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 13 && x == 5) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 13 && x == 9) {
                    _specialLocationsUsed.Add(location);
                    points *= 3;
                }

                // Double letter score
                if (y == 6 && x == 6) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 6 && x == 8) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 8 && x == 6) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 8 && x == 8) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 0 && x == 3) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 0 && x == 11) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 3 && x == 7) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 2 && x == 8) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 2 && x == 6) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 3 && x == 0) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 11 && x == 0) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 7 && x == 3) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 6 && x == 2) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 8 && x == 2) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 3 && x == 14) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 11 && x == 14) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 7 && x == 11) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 6 && x == 12) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 8 && x == 12) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 14 && x == 3) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 14 && x == 11) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 11 && x == 7) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 12 && x == 8) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 12 && x == 6) {
                    _specialLocationsUsed.Add(location);
                    points *= 2;
                }
            }


            return points;
        }
    }
}
