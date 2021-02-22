using BoxCorp.BusinessLogic;
using System;
using System.IO;

namespace BoxCorp.App {
    class Program {
        static void Main(string[] args) {
            var boxLines = File.ReadAllLines(@"..\..\..\boxes.csv");

            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine($"Loaded {boxLines.Length} lines from the file");

            var selectedBoxes = BoxSelector.SelectBestBoxes(boxLines);
            Console.WriteLine($"Selected only {selectedBoxes.Count} best boxes");
        }
    }
}
