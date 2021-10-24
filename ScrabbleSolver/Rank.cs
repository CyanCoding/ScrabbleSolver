using System.Collections.Generic;
using System.Windows;

namespace ScrabbleSolver {
    /// <summary>
    /// Ranks results found and validates words
    /// </summary>
    public class Rank {
        public static List<string> wordsFound;

        private static bool DetectFloating(MainWindow.LocationData location,
            string[,] board) {

            int x = location.X;
            int y = location.Y;

            int checks = 0;
            if (y == 0) {
                checks++;
            }
            else if (y > 0 && board[y - 1, x] == "") {
                checks++;
            }

            if (y == 14) {
                checks++;
            }
            else if (y < 14 && board[y + 1, x] == "") {
                checks++;
            }
            
            if (x == 0) {
                checks++;
            }
            else if (x > 0 && board[y, x - 1] == "") {
                checks++;
            }
            
            if (x == 14) {
                checks++;
            }
            else if (x < 14 && board[y, x + 1] == "") {
                checks++;
            }
            
            // If checks == 4, this means there is a blank spot on every
            // side of a letter
            if (checks == 4) {
                return true;
            }
            
            return false;
        }
        
        public static bool RankData(
            MainWindow.NewBoardConfig config,
            List<string> dictionary) {

            // For each result, find each item that isn't a blank value
            // and find adjacent ones to see if it's a valid word
            var board = config.Board;

            List<MainWindow.LocationData> placesWithLetters =
                new List<MainWindow.LocationData>();

            // i - 15, j - 15
            for (int i = 0; i < 15; i++) {
                for (int j = 0; j < 15; j++) {
                    // For each letter found, add it to a list
                    if (board[i, j] != "") {
                        MainWindow.LocationData l =
                            new MainWindow.LocationData() {
                                Y = i,
                                X = j,
                                Letter = board[i, j]
                            };

                        placesWithLetters.Add(l);
                    }
                }
            }

            var words = new List<string>();

            // Now operate on each letter we found
            foreach (var place in placesWithLetters) {
                if (DetectFloating(place, board)) {
                    return false;
                }
                
                string horizontalWord = "";
                string verticalWord = "";

                // We have to go from left to right since that's how
                // words are spelled
                var currentX = place.X;
                var currentY = place.Y;
                if (currentX > 0 && board[currentY, currentX - 1] != "") {
                    currentX = place.X - 1;
                }
                
                
                // Navigate all the way to the left
                if (currentX < 0) {
                    currentX = 0;
                }
                while (board[currentY, currentX] != "") {
                    currentX--;

                    if (currentX < 0) {
                        // We've reached the left side of the board
                        currentX = 0;
                        break;
                    }
                }
                // We just moved to a blank spot so move back
                currentX++;

                // Go from left to right and fill in horizontalWord
                while (board[currentY, currentX] != "") {
                    horizontalWord += board[currentY, currentX];
                    currentX++;

                    if (currentX > 14) {
                        // We've reached the right side of the board
                        break;
                    }
                }

                // Reset X so we can use it in calculating verticalWord
                currentX = place.X;
                currentY = place.Y;
                if (currentY > 0 && board[currentY - 1, currentX] != "") {
                    currentY = place.Y - 1;
                }

                // Navigate all the way to the top
                if (currentY < 0) {
                    currentY = 0;
                }
                while (board[currentY, currentX] != "") {
                    currentY--;

                    if (currentY < 0) {
                        // We've reached the top of the board
                        currentY = 0;
                        break;
                    }
                }
                
                // We just moved from a blank spot so move up
                currentY++;

                // Go from top to bottom and fill verticalWord
                while (board[currentY, currentX] != "") {
                    verticalWord += board[currentY, currentX];
                    currentY++;

                    if (currentY > 14) {
                        // We've reached the bottom of the board
                        break;
                    }
                }

                // Do something with finished words
                if (horizontalWord.Length == 1 && verticalWord.Length == 1) {
                    continue;
                }
                
                if (horizontalWord.Length != 1) {
                    words.Add(horizontalWord);
                }

                if (verticalWord.Length != 1) {
                    words.Add(verticalWord);
                }
            }
            
            foreach (var word in words) {
                if (!dictionary.Contains(word.ToLower())) {
                    if (word == "") {
                        continue;
                    }

                    return false;
                }
            }

            if (wordsFound == null) {
                wordsFound = new List<string>();
            }

            bool foundBefore = true;
            foreach (var validWords in words) {
                if (!wordsFound.Contains(validWords)) {
                    wordsFound.Add(validWords);
                    foundBefore = false;
                    break;
                }
            }
            
            // The word didn't already exist.
            if (!foundBefore) {
                return true;
            }

            return false;
        }
    }
}