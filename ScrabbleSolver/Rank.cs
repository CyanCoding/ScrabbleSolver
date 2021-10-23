using System.Collections.Generic;

namespace ScrabbleSolver {
    /// <summary>
    /// Ranks results found and validates words
    /// </summary>
    public class Rank {
        public static List<MainWindow.NewBoardConfig> RankData(
            List<MainWindow.NewBoardConfig> beginData,
            List<string> dictionary) {

            // We store ranked data in here
            var results = new List<MainWindow.NewBoardConfig>();
            
            // For each result, find each item that isn't a blank value
            // and find adjacent ones to see if it's a valid word
            foreach (var individual in beginData) {
                var board = individual.Board;

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
                
                // Now operate on each letter we found
                foreach (var place in placesWithLetters) {
                    string horizontalWord = "";
                    string verticalWord = "";
                    
                    // We have to go from left to right since that's how
                    // words are spelled
                    var currentX = place.X - 1;
                    var currentY = place.Y;
                    // Navigate all the way to the left
                    while (board[currentY, currentX] != "") {
                        currentX--;
                        
                        if (currentX < 0) {
                            // We've reached the left side of the board
                            currentX = 0;
                            break;
                        }
                    }
                    
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
                    currentY = place.Y - 1;
                    
                    // Navigate all the way to the top
                    while (board[currentY, currentX] != "") {
                        currentY--;

                        if (currentY < 0) {
                            // We've reached the top of the board
                            currentY = 0;
                            break;
                        }
                    }
                    
                    // Go from top to bottom and fill verticalWord
                    while (board[currentY, currentX] != "") {
                        horizontalWord += board[currentY, currentX];
                        currentY++;

                        if (currentY > 14) {
                            // We've reached the bottom of the board
                            break;
                        }
                    }
                    
                    // Do something with finished words
                }
            }
            

            return results;
        }
    }
}