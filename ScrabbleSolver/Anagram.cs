using System;
using System.Collections.Generic;

namespace ScrabbleSolver {
    public class Anagram {
        
        /// <summary>
        /// Creates a dictionary of each the letters
        /// </summary>
        /// <param name="word">The word to create a map from</param>
        /// <returns>The letter dictionary</returns>
        private static IDictionary<char,int> FindDuplicates(string word) {
            IDictionary<char, int> d = new Dictionary<char, int>();

            foreach (char letter in word) {
                try {
                    d.Add(letter, 1);
                }
                catch (ArgumentException) {
                    // Occurs if the letter is already in the dictionary
                    d[letter]++;
                }
                
            }

            return d;
        }
        
        /// <summary>
        /// Returns a vector with every anagram for a word
        /// </summary>
        /// <param name="word">The word to search for</param>
        /// <returns>A vector with each anagram.</returns>
        public static List<string> GetAnagrams(string word) {
            // Gets the factorial of our length of word. This number
            // represents every possibility INCLUDING duplicates
            var lengthFactorial = Factorial(word.Length);
            
            // Find how much we need to divide by in order to exclude
            // duplicates.
            int divisor = 1;
            var dictionary = FindDuplicates(word);
            foreach (var val in dictionary) {
                divisor *= Factorial(val.Value);
            }
            
            // Gets the actual amount of anagrams excluding duplicates
            var totalAnagrams = lengthFactorial / divisor;
            
            var anagramsFound = new List<string>();
            var random = new Random();

            for (var i = 0; i < totalAnagrams; i++) {
                var anagram = "";
                
                // Numbers that represent letter locations from each word
                // Used to fix duplicate letters issue
                var anagramNumbers = "";

                foreach (var t in word) {
                    var foundCharacter = "";
                    while (true) {
                        var rnd = random.Next(word.Length);
                        foundCharacter = word[rnd] + "";

                        if (!anagramNumbers.Contains(rnd + "")) {
                            // The character wasn't in the string, so success
                            anagramNumbers += rnd + "";
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
        
        /// <summary>
        /// Gets the factorial of a number
        /// </summary>
        /// <param name="length">Original number to find factorial of</param>
        /// <returns>Factorial of length</returns>
        private static int Factorial(int length) {
            var factorial = 1;

            for (int i = length; i > 0; i--) {
                factorial *= i;
            }

            return factorial;
        }
    }
}