using System.Collections.Generic;
using System.Windows;

namespace ScrabbleSolver {
    /// <summary>
    /// Points value
    /// </summary>
    class Points {
        public readonly Dictionary<string,int> Map = new Dictionary<string, int>();
        public Points() {
            Map.Add("a", 1);
            Map.Add("b", 3);
            Map.Add("c", 3);
            Map.Add("d", 2);
            Map.Add("e", 1);
            Map.Add("f", 4);
            Map.Add("g", 2);
            Map.Add("h", 4);
            Map.Add("i", 1);
            Map.Add("j", 8);
            Map.Add("k", 5);
            Map.Add("l", 1);
            Map.Add("m", 3);
            Map.Add("n", 1);
            Map.Add("o", 1);
            Map.Add("p", 3);
            Map.Add("q", 10);
            Map.Add("r", 1);
            Map.Add("s", 1);
            Map.Add("t", 1);
            Map.Add("u", 1);
            Map.Add("v", 4);
            Map.Add("w", 4);
            Map.Add("x", 8);
            Map.Add("y", 4);
            Map.Add("z", 10);
        }
        
        
        private static bool _doubleValue;
        private static bool _tripleValue;
        
        /// <summary>
        /// Calculates the current points amount based on the new board config
        /// </summary>
        /// <param name="boxesArray">The current board</param>
        /// <param name="previousArray">The previous board</param>
        /// <returns>An int of the points and adjacent letters</returns>
        public static (int, string) CalculatePoints(string[,] boxesArray,
            string[,] previousArray) {
            var lettersAssociated = new string[15, 15];
            Board.LocationsUsed = new List<MainWindow.LocationData>();

            // Sort through the entire board
            for (var i = 0; i < boxesArray.GetLength(0); i++) {
                for (var l = 0; l < boxesArray.GetLength(1); l++) {
                    // Check for differences between current and past board
                    if (boxesArray[i, l] != previousArray[i, l]) {
                        // Different letter than last time!
                        Board.AdjacentLetter(i, l, boxesArray);
                        var text = boxesArray[i, l];
                        lettersAssociated[i, l] = text;
                    }
                }
            }

            var adjacentLetters = "";
            var points = new Points();
            var totalPoints = 0;

            try {
                foreach (var l in Board.LocationsUsed) {
                    var x = l.X;
                    var y = l.Y;

                    adjacentLetters += boxesArray[y, x].ToLower();

                    if (boxesArray[y,x] == "") {
                        continue;
                    }

                    var value = points.Map[boxesArray[y, x].ToLower()];
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
        
        /// <summary>
        /// Fills in Board.SpecialLocationsUsed and used for point calculation
        /// </summary>
        /// <param name="points">The total amount of points</param>
        /// <param name="location">The point location on the board</param>
        /// <returns>A new point score value</returns>
        private static int PointsValue(int points,
            MainWindow.LocationData location) {
            var x = location.X;
            var y = location.Y;

            if (!Board.SpecialLocationsUsed.Contains(location)) {
                // Triple word score
                if (x == 0 && y == 0) {
                    Board.SpecialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 0 && x == 7) {
                    Board.SpecialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 0 && x == 14) {
                    Board.SpecialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 7 && x == 0) {
                    Board.SpecialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 7 && x == 14) {
                    Board.SpecialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 14 && x == 0) {
                    Board.SpecialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 14 && x == 7) {
                    Board.SpecialLocationsUsed.Add(location);
                    _tripleValue = true;
                }
                else if (y == 14 && x == 14) {
                    Board.SpecialLocationsUsed.Add(location);
                    _tripleValue = true;
                }

                // Double word score
                if (y == 1 && x == 1) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 1 && x == 13) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 2 && x == 2) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 2 && x == 12) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 3 && x == 3) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 3 && x == 11) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 4 && x == 4) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 4 && x == 10) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 13 && x == 1) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 13 && x == 13) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 12 && x == 2) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 12 && x == 12) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 11 && x == 3) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 11 && x == 11) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 10 && x == 4) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }
                else if (y == 10 && x == 10) {
                    Board.SpecialLocationsUsed.Add(location);
                    _doubleValue = true;
                }

                if (y == 5 && x == 5) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 5 && x == 9) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 9 && x == 9) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 9 && x == 5) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 5 && x == 1) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 9 && x == 1) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 5 && x == 13) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 9 && x == 13) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 1 && x == 5) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 1 && x == 9) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 13 && x == 5) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }
                else if (y == 13 && x == 9) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 3;
                }

                // Double letter score
                if (y == 6 && x == 6) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 6 && x == 8) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 8 && x == 6) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 8 && x == 8) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 0 && x == 3) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 0 && x == 11) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 3 && x == 7) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 2 && x == 8) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 2 && x == 6) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 3 && x == 0) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 11 && x == 0) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 7 && x == 3) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 6 && x == 2) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 8 && x == 2) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 3 && x == 14) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 11 && x == 14) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 7 && x == 11) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 6 && x == 12) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 8 && x == 12) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 14 && x == 3) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 14 && x == 11) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 11 && x == 7) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 12 && x == 8) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
                else if (y == 12 && x == 6) {
                    Board.SpecialLocationsUsed.Add(location);
                    points *= 2;
                }
            }


            return points;
        }
    }
}