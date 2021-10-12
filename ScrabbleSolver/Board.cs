using System.Collections.Generic;
using System.Windows;

namespace ScrabbleSolver {
    /// <summary>
    /// Mostly used with board operations, scoring, etc.
    /// </summary>
    static class Board {

        public static List<MainWindow.LocationData> LocationsUsed;
        public static List<MainWindow.LocationData> SpecialLocationsUsed =
            new List<MainWindow.LocationData>();

        private static bool CheckLocation(int y, int x) {
            var location = new MainWindow.LocationData {
                X = x,
                Y = y
            };

            if (!LocationsUsed.Contains(location)) {
                LocationsUsed.Add(location);
                return true;
            }
            return false;
        }
        public static void AdjacentLetter(int y, int x, string[,] boxesArray) {
            string thisLetter;

            // Add this location to the data
            var thisLocation = new MainWindow.LocationData
                {
                    X = x,
                    Y = y
                };

            if (!LocationsUsed.Contains(thisLocation)) {
                LocationsUsed.Add(thisLocation);
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
    }
}
