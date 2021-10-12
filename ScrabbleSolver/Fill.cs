using System;
using System.Collections.Generic;
using System.Threading;

namespace ScrabbleSolver {
    /// <summary>
    /// Works to fill in locations with letters
    /// </summary>
    class Fill {
        
        /// <summary>
        /// Gets every possible location for playing on the board
        /// </summary>
        /// <param name="boxesArray">The board</param>
        /// <returns>Location data for each position</returns>
        public static List<MainWindow.LocationData> FindAllPlaces(string[,] boxesArray) {
            List<MainWindow.LocationData> foundPlaces = new List<MainWindow.LocationData>();
            foundPlaces.Clear();
            
            var possibleSpots = 0;

            for (var i = 0; i < boxesArray.GetLength(0); i++) {
                for (var j = 0; j < boxesArray.GetLength(1); j++) {
                    MainWindow.LocationData l =
                        new MainWindow.LocationData();

                    // For each available position, count and then set that
                    // position to a number so we don't count that position again
                    if (boxesArray[i, j] != "" && boxesArray[i, j] != "1") {
                        if (i > 0 && boxesArray[i - 1, j] == "") {
                            l.Y = i - 1;
                            l.X = j;
                            foundPlaces.Add(l);

                            possibleSpots++;
                            boxesArray[i - 1, j] = "1";

                        }

                        if (i < 14 && boxesArray[i + 1, j] == "") {
                            l.Y = i + 1;
                            l.X = j;
                            foundPlaces.Add(l);

                            possibleSpots++;
                            boxesArray[i + 1, j] = "1";
                        }

                        if (j > 0 && boxesArray[i, j - 1] == "") {
                            l.Y = i;
                            l.X = j - 1;
                            foundPlaces.Add(l);

                            possibleSpots++;
                            boxesArray[i, j - 1] = "1";
                        }

                        if (j < 14 && boxesArray[i, j + 1] == "") {
                            l.Y = i;
                            l.X = j + 1;
                            foundPlaces.Add(l);

                            possibleSpots++;
                            boxesArray[i, j + 1] = "1";
                        }
                    }
                }
            }
            return possibleSpots;
        }

        private static void FindPosition(string[,] boxesArray,
            int x,
            int y,
            string letters) {
            var r = new Random();

            if (boxesArray[y,x] == "") {
                var value = r.Next(0, letters.Length);

                var character = letters[value];
                letters = letters.Remove(value);
                boxesArray[y, x] = character + "";
            }

            if (boxesArray[y - 1,x] == "" && letters.Length != 0) {
                FindPosition(boxesArray, x, y - 1, letters);
            }
            if (boxesArray[y + 1, x] == "" && letters.Length != 0) {
                FindPosition(boxesArray, x, y + 1, letters);
            }

            // Whole word has been used
            if (letters.Length == 0) {
                var unused = Points.CalculatePoints(boxesArray,
                    MainWindow.PreviousBoard);
                // Add a match that contains the new letter locations
                //MainWindow.matches.Add()
            }
        }
    }
}
