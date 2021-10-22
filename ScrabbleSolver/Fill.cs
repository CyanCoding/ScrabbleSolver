using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Documents;
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
        
        
        public static List<List<MainWindow.LocationData>> FindLetterPositions(
            MainWindow.LocationData testSpot,
            string letters,
            string[,] boxesArray) {
            List<List<MainWindow.LocationData>> lists =
                new List<List<MainWindow.LocationData>>();
            
            // Finds the vertical positions
            for (int i = 0; i < /*letters.Length*/ 1; i++) {
                List<MainWindow.LocationData> newList =
                    new List<MainWindow.LocationData>();
                
                int currentY = testSpot.Y;
                for (int j = 0; j < letters.Length; j++) {
                    if (currentY < 0) {
                        // The idea is if we hit y = 0 or smaller, we need
                        // to end the array of locations
                        break;
                    }
                    
                    // If there's a letter here or we're too high
                    if (boxesArray[currentY, testSpot.X] != "" || currentY > 14) {
                        // There's a letter in this position! Move up
                        j--;
                        currentY--;
                        continue;
                    }

                    if (boxesArray[currentY, testSpot.X] == "") {
                        MainWindow.LocationData location =
                            new MainWindow.LocationData();
                        location.X = testSpot.X;
                        location.Y = currentY;
                        
                        newList.Add(location);
                    }

                    currentY--;
                }
                // Adds our combination list to the main list
                lists.Add(newList);
                
                currentY = testSpot.Y + 1;
            }

            return lists;
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

            // Clones the array
            string[,] copyBox = (string[,]) boxesArray.Clone();
            
            // A list of each position and letter used for a certain combination
            MainWindow.LocationData[] filledLocations =
                new MainWindow.LocationData[letters.Length];

            var everyPos = FindLetterPositions(testSpot, letters, boxesArray);
            
            // This contains each anagram, so we can check if one has already
            // be found
            List<string> anagramsFound = new List<string>();

            foreach (List<MainWindow.LocationData> dataList in everyPos) {
                for (int i = 0; i < letters.Length; i++) {
                    // For each anagram at each length
                    foreach (string anagram in anagrams) {
                        // We add letters to this as we progress
                        // For each letter of each anagram
                        for (int j = 0; j <= i; j++) {
                            // Avoid argument out of bounds exceptions
                            if (i >= dataList.Count) {
                                break;
                            }
                            
                            int y = dataList[j].Y;
                            int x = dataList[j].X;
                            string l = anagram[j] + "";

                            copyBox[y, x] = l;
                        
                            MainWindow.LocationData newLocation;
                            newLocation.Y = y;
                            newLocation.X = x;
                            newLocation.Letter = l;

                            filledLocations[j] = newLocation;
                        }
                        // Add the new board configuration to results
                        MainWindow.NewBoardConfig config;
                        config.NewLocations = filledLocations.ToList();
                        config.Board = copyBox;
                        copyBox = (string[,]) boxesArray.Clone();

                        bool addedTo = false;

                        foreach (var result in results) {
                            var locations = result.NewLocations;
                            if (locations.SequenceEqual(config.NewLocations)) {
                                addedTo = true;
                            }
                        }

                        if (!addedTo) {
                            results.Add(config);
                        }
                    }
                }   
            }

            return results;
        }
    }
}
