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
        /// Returns a list of lists of anagrams.
        /// Each list is i + 1 anagrams long, so List[0] is length 1 anagrams
        ///
        /// We need multiple length anagrams to save resources during board
        /// filling.
        /// </summary>
        /// <param name="word">The word to search for</param>
        /// <returns>A vector with each anagram.</returns>
        public static List<List<string>> GetAnagrams(string word) {
            List<List<string>> anagramsFound = new List<List<string>>();
            
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
            
            var a = new List<string>();
            var random = new Random();

            for (var j = 0; j < totalAnagrams; j++) {
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

                if (a.Contains(anagram)) {
                    j--;
                }
                else {
                    a.Add(anagram);
                }
            }

            // Copy the array for the length amount
            for (int i = 0; i < word.Length; i++) {
                // Copies the List
                var newList = new List<string>(a);
                var blankList = new List<string>();
                
                // For each item in the list we make a substring of the length.
                // We have anagrams of length, say, 7. We want 1 - 7. So for
                // length 3 we get a substring of 0 - 3, see if we already have
                // it in a new list, and add it.
                for (int j = 0; j < newList.Count; j++) {
                    newList[j] = newList[j].Substring(0, i + 1);
                    
                    if (!blankList.Contains(newList[j])) {
                        blankList.Add(newList[j]);
                    }
                }
                
                anagramsFound.Add(blankList);
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