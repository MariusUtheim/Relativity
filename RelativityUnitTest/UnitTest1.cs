using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Relativity;

namespace RelativityUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private Random _rnd = new Random();

        [TestMethod]
        public void Observer_is_at_own_origin()
        {
            var observer = Observer.Default.Offset(_rnd.NextDouble(), _rnd.NextDouble(), _rnd.NextDouble());
            Assert.IsTrue(observer.Origin.In(observer) == new Coordinate(0, 0));
        }
    }
}
