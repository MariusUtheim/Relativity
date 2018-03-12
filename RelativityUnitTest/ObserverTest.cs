using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Relativity;

namespace RelativityUnitTest
{
    [TestClass]
    public class ObserverTest
    {
        private static Random _rnd = new Random();

        private static double rand() => 2 * _rnd.NextDouble() - 1;

        [TestMethod]
        public void Observer_is_at_own_origin()
        {
            var observer = Observer.Default.Offset(rand(), rand(), rand());
            Assert.IsTrue(observer.Origin.In(observer) == new Coordinate(0, 0));
        }

    }
}
