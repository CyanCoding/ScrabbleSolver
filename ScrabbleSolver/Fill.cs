using System;
using System.Collections.Generic;
using System.Threading;

namespace ScrabbleSolver {
    /// <summary>
    /// Works to fill in locations with letters
    /// </summary>
    class Fill {
        /// <summary>
        /// Returns a vector with every anagram for a word
        /// </summary>
        /// <param name="word">The word to search for</param>
        /// <returns>A vector with each anagram.</returns>
        public static List<string> Anagrams(string word) {
            var totalAnagrams = factorial(word.Length);
            var anagramsFound = new List<string>();

            var random = new Random();

            for (var i = 0; i < totalAnagrams; i++) {
                var anagram = "";

                foreach (var t in word) {
                    var foundCharacter = "";
                    while (true) {
                        // TODO: It isn't allowing for duplicate letters
                        var rnd = random.Next(word.Length - 1);
                        foundCharacter = word[rnd] + "";
                        Thread.Sleep(1);

                        if (!anagram.Contains(foundCharacter)) {
                            // The character wasn't in the string, so success
                            break;
                        }
                    }
                    anagram += foundCharacter;
                }

                if (anagramsFound.Contains(anagram)) {
                    i--;
                }
                else {
                    anagramsFound.Add(anagram);
                }

            }

            return anagramsFound;
        }

        private static int factorial(int length) {
            if (length == 0 || length == 1) {
                return 1;
            }
            else {
                return length * factorial(length - 1);
            }
        }
        public static void FindMatches(string[,] boxesArray, string letters) {
            var unused = FindAllPlaces(boxesArray);

            for (var i = 0; i < boxesArray.GetLength(0); i++) {
                for (var j = 0; j < boxesArray.GetLength(1); j++) {
                    if (boxesArray[i, j] != "") {
                        //FindPosition(boxesArray, i, j, letters);

                    }
                }
            }
        }

        private static int FindAllPlaces(string[,] boxesArray) {
            var possibleSpots = 0;

            for (var i = 0; i < boxesArray.GetLength(0); i++) {
                for (var j = 0; j < boxesArray.GetLength(1); j++) {
                    // For each available position, count and then set that
                    // position to a number so we don't count that position again
                    if (boxesArray[i, j] != "" && boxesArray[i, j] != "1") {
                        if (i > 0 && boxesArray[i - 1, j] == "") {
                            possibleSpots++;
                            boxesArray[i - 1, j] = "1";
                        }
                        if (i < 14 && boxesArray[i + 1, j] == "") {
                            possibleSpots++;
                            boxesArray[i + 1, j] = "1";
                        }
                        if (j > 0 && boxesArray[i, j - 1] == "") {
                            possibleSpots++;
                            boxesArray[i, j - 1] = "1";
                        }
                        if (j < 14 && boxesArray[i, j + 1] == "") {
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
                var unused = Board.CalculatePoints(boxesArray,
                    MainWindow.PreviousBoard);
                // Add a match that contains the new letter locations
                //MainWindow.matches.Add()
            }
        }
    }
}
