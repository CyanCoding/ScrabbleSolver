using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media.Animation;

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
            List<MainWindow.LocationData> foundPlaces =
                new List<MainWindow.LocationData>();
            foundPlaces.Clear();
            
            // For cloning arrays:
            //string[,] originalBoard = (string[,]) boxesArray.Clone();


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

                            boxesArray[i - 1, j] = "1";

                        }

                        if (i < 14 && boxesArray[i + 1, j] == "") {
                            l.Y = i + 1;
                            l.X = j;
                            foundPlaces.Add(l);

                            boxesArray[i + 1, j] = "1";
                        }

                        if (j > 0 && boxesArray[i, j - 1] == "") {
                            l.Y = i;
                            l.X = j - 1;
                            foundPlaces.Add(l);

                            boxesArray[i, j - 1] = "1";
                        }

                        if (j < 14 && boxesArray[i, j + 1] == "") {
                            l.Y = i;
                            l.X = j + 1;
                            foundPlaces.Add(l);

                            boxesArray[i, j + 1] = "1";
                        }
                    }
                }
            }
            
            // Reset the array after we modified it
            for (int i = 0; i < 14; i++) {
                for (int j = 0; j < 14; j++) {
                    if (boxesArray[i, j] == "1") {
                        boxesArray[i, j] = "";
                    }
                }
            }

            return foundPlaces;
        }
        
        /// <summary>
        /// Gets every letter combination for a location
        /// </summary>
        /// <param name="boxesArray">The board</param>
        /// <param name="index">The index of the location data to check</param>
        /// <param name="letters">The letters to fill</param>
        /// <param name="anagrams">The anagrams list for the letters</param>
        /// <param name="foundPlaces">Every found location</param>
        /// <returns>An array of locations and boards</returns>
        public static List<MainWindow.NewBoardConfig> FillFromPosition(
            string[,] boxesArray,
            int index,
            string letters,
            List<string> anagrams,
            List<MainWindow.LocationData> foundPlaces) {
            
            // The resulting board configs
            List<MainWindow.NewBoardConfig> results =
                new List<MainWindow.NewBoardConfig>();
            
            // The location we are testing
            MainWindow.LocationData testSpot = foundPlaces[index];
            
            // Each of the positions we can go up
            MainWindow.LocationData[] upSpots =
                new MainWindow.LocationData[letters.Length];

            int currentY = testSpot.Y;
            // Finds all the possible up locations and fills them into
            // upLocations
            for (int i = 0; i < letters.Length; i++) {
                if (currentY <= 0) {
                    break; // TODO: Fix this! Doesn't work if too large a length
                }

                if (boxesArray[currentY, testSpot.X] != "") {
                    // There's a letter in this position! Move up
                    i--;
                    continue;
                }

                if (boxesArray[currentY, testSpot.X] == "") {
                    upSpots[i].X = testSpot.X;
                    upSpots[i].Y = currentY;
                }
            }
            
            // The previous boxesArray before it was modified
            // TODO: We have an issue where changing boxesArray in another function is causing this to change
            var defaultBox = boxesArray;
            
            // A list of each position and letter used for a certain combination
            MainWindow.LocationData[] filledLocations =
                new MainWindow.LocationData[letters.Length];
            
            // TODO: We need to go from length 1 to max
            foreach (var anagram in anagrams) {
                for (int j = 0; j < letters.Length; j++) {
                    boxesArray[upSpots[j].Y, upSpots[j].X] =
                        anagram[j] + "";

                    MainWindow.LocationData newLocation;
                    newLocation.X = upSpots[j].X;
                    newLocation.Y = upSpots[j].Y;
                    newLocation.Letter = anagram[j] + "";

                    filledLocations[j] = newLocation;
                    // TODO: Save the current board state with letters filled get letter locations and pass to config add to results.
                }
                
                // Add the new board configuration to results
                MainWindow.NewBoardConfig config;
                config.NewLocations = filledLocations.ToList();
                config.Board = boxesArray;

                results.Add(config);
                
                // Reset the board
                boxesArray = defaultBox;
                
                // Print out results
                for (int j = 0; j < 14; j++) {
                    for (int k = 0; k < 14; k++) {
                        Console.Write(boxesArray[j, k]);
                    }

                    Console.WriteLine();
                }
            }
            
            
            return results;
        }
    }
}
