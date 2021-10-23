using System.Collections.Generic;

namespace ScrabbleSolver {
    public class GameManifest {
        public List<List<string>> Players { get; set; }
        public string Letters { get; set; }
        public int Turn { get; set; }
        public List<List<string>> Board { get; set; }
    }
}