using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GinghamSharpTest
{
    public class SampleTest
    {
        [TestCase(3, 5, 5, 3)]
        [TestCase(-1, 0, 0, -1)]
        public void SwapTest(int expectedA, int expectedB, int a, int b)
        {
            Sample.Swap(ref a, ref b);

            Assert.AreEqual(expectedA, a);
            Assert.AreEqual(expectedB, b);
        }
    }
}
