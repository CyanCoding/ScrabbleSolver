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

            return foundPlaces;
        }
        
        /// <summary>
        /// Gets every letter combination for a location
        /// </summary>
        /// <param name="boxesArray">The board</param>
        /// <param name="index">The index of the location data to check</param>
        /// <param name="letters">The letters to fill</param>
        /// <param name="anagrams">The anagrams list for the letters</param>
        /// <param name="FoundPlaces">Every found location</param>
        /// <returns>An array of locations and boards</returns>
        public static List<MainWindow.NewBoardConfig> FillFromPosition(
            string[,] boxesArray,
            int index,
            string letters,
            List<string> anagrams,
            List<MainWindow.LocationData> FoundPlaces) {
            List<MainWindow.NewBoardConfig> results =
                new List<MainWindow.NewBoardConfig>();

            MainWindow.LocationData l = FoundPlaces[index];

            MainWindow.LocationData[] upLocations =
                new MainWindow.LocationData[letters.Length];

            int currentY = l.Y;
            // Finds all the possible up locations and fills them into
            // upLocations
            for (int i = 0; i < letters.Length; i++) {
                if (currentY <= 0) {
                    break; // TODO: Fix this! Doesn't work if too large a length
                }

                if (boxesArray[currentY, l.X] != "") {
                    // There's a letter in this position! Move up
                    i--;
                    continue;
                }

                if (boxesArray[currentY, l.X] == "") {
                    upLocations[i].X = l.X;
                    upLocations[i].Y = currentY;
                }
            }
            
            // TODO: We need to go from length 1 to max
            for (int i = 0; i < anagrams.Count; i++) {
                for (int j = 0; j < letters.Length; j++) {
                    boxesArray[upLocations[j].Y, upLocations[j].X] =
                        anagrams[i][j] + "";

                    MainWindow.LocationData newLocation;
                    newLocation.X = upLocations[j].X;
                    newLocation.Y = upLocations[j].Y;
                    newLocation.Letter = anagrams[i][j] + "";

                    // TODO: Save the current board state with letters filled get letter locations and pass to config add to results.
                    MainWindow.NewBoardConfig config;

                    //results.Add(newLocation);
                    //results[i].NewLocations[j] = newLocation;
                }
                
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
