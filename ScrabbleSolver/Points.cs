using System.Collections.Generic;

namespace ScrabbleSolver {
    /// <summary>
    /// Points value
    /// </summary>
    class Points {
        // TODO: Move point scoring here?
        public readonly Dictionary<string,int> Map = new Dictionary<string, int>();
        public Points() {
            Map.Add("a", 1);
            Map.Add("b", 3);
            Map.Add("c", 3);
            Map.Add("d", 2);
            Map.Add("e", 1);
            Map.Add("f", 4);
            Map.Add("g", 2);
            Map.Add("h", 4);
            Map.Add("i", 1);
            Map.Add("j", 8);
            Map.Add("k", 5);
            Map.Add("l", 1);
            Map.Add("m", 3);
            Map.Add("n", 1);
            Map.Add("o", 1);
            Map.Add("p", 3);
            Map.Add("q", 10);
            Map.Add("r", 1);
            Map.Add("s", 1);
            Map.Add("t", 1);
            Map.Add("u", 1);
            Map.Add("v", 4);
            Map.Add("w", 4);
            Map.Add("x", 8);
            Map.Add("y", 4);
            Map.Add("z", 10);
        }
    }
}