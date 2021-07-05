using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ConsoleApp1;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            CrtPth crt = new CrtPth();
            crt.kri();
            Assert.AreEqual(crt.max, 29);

        }
    }
}
