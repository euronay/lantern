using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lantern.Core.Colors;
using System.Drawing;

namespace Lantern.Core.Tests.Colors
{
    [TestClass]
    public class HslTests
    {
        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0, 0)] //Black
        [DataRow(0, 0, 1, 255, 255, 255)] //White
        [DataRow(0, 1, 0.5, 255, 0, 0)] //Red
        [DataRow(360, 1, 0.5, 255, 0, 0)] //Red
        [DataRow(120, 1, 0.5, 0, 255, 0)] //Green
        [DataRow(240, 1, 0.5, 0, 0, 255)] //Blue
        [DataRow(60, 1, 0.5, 255, 0, 255)] //Yellow
        [DataRow(180, 1, 0.5, 0, 255, 255)] //Cyan
        [DataRow(300, 1, 0.5, 255, 255, 0)] //Magenta
        [DataRow(330, 0.333, 0.376, 128, 64, 96)] //Odd wine color
        public void TestHslToRGB(double h, double s, double l, int r, int g, int b)
        {
            // Arrange & Act
            var hsl = new Hsl(h,s,l);

            Color color = hsl.Rgb;

            // Assert
            Assert.AreEqual(r, color.R);
            Assert.AreEqual(g, color.B);
            Assert.AreEqual(b, color.G);
        }
    }
}
