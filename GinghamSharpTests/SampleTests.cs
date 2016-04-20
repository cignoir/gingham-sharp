using Microsoft.VisualStudio.TestTools.UnitTesting;
using GinghamSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GinghamSharp.Tests
{
    [TestClass()]
    public class SampleTests
    {
        [TestMethod()]
        public void SwapTest()
        {
            var a = 1;
            var b = 2;
            Sample.Swap(ref a, ref b);
            Assert.AreEqual(a, 2);
            Assert.AreEqual(b, 1);
        }
    }
}