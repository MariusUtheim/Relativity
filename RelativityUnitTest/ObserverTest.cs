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
        private const double delta = 1.0e-10;
        private Observer randomObserver() => Observer.Default.Offset(rand(), rand(), rand());

        [TestMethod]
        public void Observer_is_at_own_origin()
        {
            var observer = randomObserver();
            Assert.AreEqual(new Coordinate(0, 0), observer.Origin.In(observer));
        }

        [TestMethod]
        public void Observer_is_at_correct_offset()
        {
            double t1 = rand(), t2 = rand(), x1 = rand(), x2 = rand();

            var observer1 = Observer.Default.Offset(t1, x1, 0);
            Assert.AreEqual(new Coordinate(t1, x1), observer1.Origin.In(Observer.Default));

            var observer2 = observer1.Offset(t2, x2, 0);
            Assert.AreEqual(new Coordinate(t2, x2), observer2.Origin.In(observer1));
        }

        [TestMethod]
        public void Observer_has_correct_velocity()
        {
            double v1 = rand(), v2 = rand();

            var observer1 = Observer.Default.Offset(rand(), rand(), v1);
            Assert.AreEqual(v1, observer1.Velocity(Observer.Default), delta);

            var observer2 = observer1.Offset(rand(), rand(), v2);
            Assert.AreEqual(v2, observer2.Velocity(observer1), delta);
        }

        [TestMethod]
        public void Observer_offsets_add()
        {
            double t1 = rand(), t2 = rand(), x1 = rand(), x2 = rand();
            var observer = Observer.Default.Offset(t1, x1, 0).Offset(t2, x2, 0);
            Assert.AreEqual(new Coordinate(t1 + t2, x1 + x2), observer.Origin.In(Observer.Default));
        }

        [TestMethod]
        public void Observer_gives_correct_LorentzFactor()
        {
            var observer0 = randomObserver();
            var observer1 = observer0.Offset(rand(), rand(), 4.0 / 5.0);

            Assert.AreEqual(5.0 / 3.0, observer1.LorentzFactor(observer0), delta);
        }

        [TestMethod]
        public void Observer_point_is_at_correct_coordinates()
        {
            var observer = randomObserver();
            double t = rand(), x = rand();
            var point = observer.Point(t, x);

            Assert.AreEqual(new Coordinate(t, x), point.In(observer));
        }

        [TestMethod]
        public void Observer_transformation_inverts_correctly()
        {
            var observer = randomObserver();
            double t = rand(), x = rand(), v = rand();
            var observer1 = observer.Offset(t, x, v).Offset(-t, -x, -v);

            double pt = rand(), px = rand();

            Assert.AreEqual(observer.Point(pt, px), observer1.Point(pt, px));
        }
    }
}
