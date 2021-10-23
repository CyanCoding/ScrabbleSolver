using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Gets every position your letters can be played in
        /// </summary>
        /// <param name="testSpot">The spot to test for</param>
        /// <param name="letters">The letters available</param>
        /// <param name="boxesArray">The board</param>
        /// <returns>A list of lists of positions</returns>
        private static List<List<MainWindow.LocationData>> FindLetterPositions(
            MainWindow.LocationData testSpot,
            string letters,
            string[,] boxesArray) {
            List<List<MainWindow.LocationData>> lists =
                new List<List<MainWindow.LocationData>>();
            
            // Finds the vertical positions kinda randomly
            int currentY = testSpot.Y;
            int up = letters.Length + 1;
            int down = 0;
            for (int i = 0; i < letters.Length; i++) {
                up--;
                down++;
            
                List<MainWindow.LocationData> newList =
                    new List<MainWindow.LocationData>();
            
                int resetUp = up;
                for (int j = 0; j < up; j++) {
                    try {
                        while (boxesArray[currentY + j, testSpot.X] != "") {
                            j++;
                            up++;
                        }
                    
                        MainWindow.LocationData location =
                            new MainWindow.LocationData();
                        location.X = testSpot.X;
                        location.Y = currentY + j;
            
                        if (!newList.Contains(location)) {
                            newList.Add(location);
                        }
                    }
                    catch (IndexOutOfRangeException) {
                        break;
                    }
                }
            
                up = resetUp;
                
                int resetDown = down;
                for (int j = 0; j < down; j++) {
                    try {
                        while (boxesArray[currentY - j, testSpot.X] != "") {
                            j--;
                            down--;
                        }
                    
                        MainWindow.LocationData location =
                            new MainWindow.LocationData();
                        location.X = testSpot.X;
                        location.Y = currentY - j;
            
                        if (!newList.Contains(location)) {
                            newList.Add(location);
                        }
                    }
                    catch (IndexOutOfRangeException) {
                        break;
                    }
                }
            
                down = resetDown;
                lists.Add(newList);
            } 
            
            // Finds the horizontal positions kinda randomly
            int currentX = testSpot.X;
            int left = letters.Length + 1;
            int right = 0;
            for (int i = 0; i < letters.Length; i++) {
                left--;
                right++;
            
                List<MainWindow.LocationData> newList =
                    new List<MainWindow.LocationData>();
            
                int resetLeft = left;
                for (int j = 0; j < left; j++) {
                    try {
                        while (boxesArray[testSpot.Y, currentX + j] != "") {
                            j++;
                            left++;
                        }

                        MainWindow.LocationData location =
                            new MainWindow.LocationData();
                        location.X = currentX + j;
                        location.Y = testSpot.Y;

                        if (!newList.Contains(location)) {
                            newList.Add(location);
                        }
                    }
                    catch (IndexOutOfRangeException) {
                        break;
                    }

                }
            
                left = resetLeft;
                
                int resetRight = right;
                for (int j = 0; j < right; j++) {
                    try {
                        while (boxesArray[testSpot.Y, currentX - j] != "") {
                            j--;
                            right--;
                        }
                    
                        MainWindow.LocationData location =
                            new MainWindow.LocationData();
                        location.X = currentX - j;
                        location.Y = testSpot.Y;
            
                        if (!newList.Contains(location)) {
                            newList.Add(location);
                        }
                    }
                    catch (IndexOutOfRangeException) {
                        break;
                    }
                }
            
                right = resetRight;
                lists.Add(newList);
            }
            
            // Finds vertical letters UP
            for (int i = 0; i < letters.Length; i++) {
                List<MainWindow.LocationData> newList =
                    new List<MainWindow.LocationData>();
                
                for (int j = 0; j < letters.Length; j++) {
                    if (currentY < 0 || currentY > 14) {
                        // The idea is if we hit y = 0 or smaller, we need
                        // to end the array of locations
                        break;
                    }
                    
                    // If there's a letter here
                    if (boxesArray[currentY, testSpot.X] != "") {
                        // There's a letter in this position! Move down
                        j--;
                        currentY--;
                        continue;
                    }
            
                    if (boxesArray[currentY, testSpot.X] == "") {
                        MainWindow.LocationData location =
                            new MainWindow.LocationData();
                        location.X = testSpot.X;
                        location.Y = currentY;
            
                        if (!newList.Contains(location)) {
                            newList.Add(location);
                        }
                    }
            
                    currentY--;
                }
                // Adds our combination list to the main list
                lists.Add(newList);
                currentY++;
                while (boxesArray[currentY, testSpot.X] != "") {
                    currentY++;
                }
            }
            
            // Finds the vertical DOWN positions
            // currentY = testSpot.Y;
            // for (int i = 0; i < letters.Length; i++) {
            //     List<MainWindow.LocationData> newList =
            //         new List<MainWindow.LocationData>();
            //     
            //     for (int j = 0; j < letters.Length; j++) {
            //         if (currentY > 14) {
            //             // The idea is if we hit y = 14 or higher, we need
            //             // to end the array of locations
            //             break;
            //         }
            //         
            //         // If there's a letter here
            //         if (boxesArray[currentY, testSpot.X] != "") {
            //             // There's a letter in this position! Move down
            //             j--;
            //             currentY++;
            //             continue;
            //         }
            //
            //         if (boxesArray[currentY, testSpot.X] == "") {
            //             MainWindow.LocationData location =
            //                 new MainWindow.LocationData();
            //             location.X = testSpot.X;
            //             location.Y = currentY;
            //
            //             if (!newList.Contains(location)) {
            //                 newList.Add(location);
            //             }
            //         }
            //         
            //         currentY++;
            //     }
            //     // Adds our combination list to the main list
            //     lists.Add(newList);
            // }
            //
            // // Find the horizontal left positions
            // for (int i = 0; i < letters.Length; i++) {
            //     List<MainWindow.LocationData> newList =
            //         new List<MainWindow.LocationData>();
            //     
            //     for (int j = 0; j < letters.Length; j++) {
            //         if (currentX < 0) {
            //             // The idea is if we hit x = 0 or smaller, we need
            //             // to end the array of locations
            //             break;
            //         }
            //         
            //         // If there's a letter here
            //         if (boxesArray[testSpot.Y, currentX] != "") {
            //             // There's a letter in this position! Move up
            //             j--;
            //             currentX--;
            //             continue;
            //         }
            //
            //         if (boxesArray[testSpot.Y, currentX] == "") {
            //             MainWindow.LocationData location =
            //                 new MainWindow.LocationData();
            //             location.X = currentX;
            //             location.Y = testSpot.Y;
            //
            //             if (!newList.Contains(location)) {
            //                 newList.Add(location);
            //             }
            //         }
            //
            //         currentX--;
            //     }
            //     // Adds our combination list to the main list
            //     lists.Add(newList);
            // }
            // // Finds the horizontal RIGHT positions
            // currentX = testSpot.X;
            // for (int i = 0; i < letters.Length; i++) {
            //     List<MainWindow.LocationData> newList =
            //         new List<MainWindow.LocationData>();
            //     
            //     for (int j = 0; j < letters.Length; j++) {
            //         if (currentX > 14) {
            //             // The idea is if we hit x = 14 or higher, we need
            //             // to end the array of locations
            //             break;
            //         }
            //         
            //         // If there's a letter here
            //         if (boxesArray[testSpot.Y, currentX] != "") {
            //             // There's a letter in this position! Move up
            //             j--;
            //             currentX++;
            //             continue;
            //         }
            //
            //         if (boxesArray[testSpot.Y, currentX] == "") {
            //             MainWindow.LocationData location =
            //                 new MainWindow.LocationData();
            //             location.X = currentX;
            //             location.Y = testSpot.Y;
            //
            //             if (!newList.Contains(location)) {
            //                 newList.Add(location);
            //             }
            //         }
            //
            //         currentX++;
            //     }
            //     // Adds our combination list to the main list
            //     lists.Add(newList);
            // }

            // Removes duplicates
            //lists = lists.Distinct().ToList();

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
            List<List<string>> anagrams,
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

            for (int i = 0; i < everyPos.Count; i++) {
                var dataList = everyPos[i];
                // There are some instances when dataList count is 0
                if (dataList.Count == 0) {
                    continue;
                }

                for (int j = 0; j < letters.Length; j++) {
                    if (j >= dataList.Count) {
                        break;
                    }
                    foreach (string anagram in anagrams[j]) {
                        for (int k = 0; k <= j; k++) {

                            int y = dataList[k].Y;
                            int x = dataList[k].X;
                            string l = anagram[k] + "";

                            if (y < 0 || y > 14 || x < 0 || x > 14) {
                                continue;
                            }
                            copyBox[y, x] = l;

                            MainWindow.LocationData newLocation;
                            newLocation.Y = y;
                            newLocation.X = x;
                            newLocation.Letter = l;

                            filledLocations[k] = newLocation;
                        }

                        // Add the new board configuration to results
                        MainWindow.NewBoardConfig config;
                        config.NewLocations = filledLocations.ToList();
                        config.Board = copyBox;
                        copyBox = (string[,]) boxesArray.Clone();
                        
                        results.Add(config);
                    }
                }
            }

            for (int i = 0; i < results.Count; i++) {
                bool removeVal = true;
                var board = results[i].Board;
                for (int j = 0; j < 14; j++) {
                    for (int k = 0; k < 14; k++) {
                        if (board[j, k] != boxesArray[j, k]) {
                            removeVal = false;
                        }
                    }
                }

                if (removeVal) {
                    results.RemoveAt(i);
                }
            }

            return results;
        }
    }
}
