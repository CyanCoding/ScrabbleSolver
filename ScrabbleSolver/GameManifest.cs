using System.Collections.Generic;

namespace ScrabbleSolver {
    public class GameManifest {
        public List<List<object>> players { get; set; }
        public string letters { get; set; }
        public List<List<string>> board { get; set; }
    }
}