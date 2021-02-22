using BoxCorp.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace BoxCorp.App.Tests {
    [TestClass]
    public class BoxTests {
        [TestMethod]
        public void TestMethod1()
        {
            var boxLines = File.ReadAllLines(@"..\..\..\boxes.csv");

            Assert.AreEqual(5, boxLines.Length);

            var selectedBoxes = BoxSelector.SelectBestBoxes(boxLines);

            Assert.IsNotNull(selectedBoxes, "Returned list of selected boxes was null");
            Assert.AreEqual(2, selectedBoxes.Count);

            Assert.AreEqual(0.8, selectedBoxes[0].Rank, "Selected wrong box");
            Assert.AreEqual(0.9, selectedBoxes[1].Rank, "Selected wrong box");
        }
    }
}
