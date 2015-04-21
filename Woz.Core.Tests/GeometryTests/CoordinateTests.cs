using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Geometry;

namespace Woz.Core.Tests.GeometryTests
{
    [TestClass]
    public class CoordinateTests
    {
        [TestMethod]
        public void Create()
        {
            var coordinate = new Coordinate(1, 2);
            Assert.AreEqual(1, coordinate.X);
            Assert.AreEqual(2, coordinate.Y);
        }

        [TestMethod]
        public void DistanceFrom()
        {
            Assert.AreEqual(
                1, new Coordinate(1, 1).DistanceFrom(new Coordinate(1, 2)));
            
            Assert.AreEqual(
                1, new Coordinate(1, 1).DistanceFrom(new Coordinate(2, 1)));
            
            Assert.AreEqual(
                1, new Coordinate(2, 1).DistanceFrom(new Coordinate(1, 1)));
            
            Assert.AreEqual(
                1, new Coordinate(1, 2).DistanceFrom(new Coordinate(1, 1)));

            Assert.IsTrue(Math.Abs(
                new Coordinate(2, 2).DistanceFrom(new Coordinate(1, 1)) - 
                1.4142135623731d) < 0.0000000000001d);
        }

        [TestMethod]
        public void Add()
        {
            var coordinate = new Coordinate(1, 2).Add(Vector.SouthEast);
            Assert.AreEqual(2, coordinate.X);
            Assert.AreEqual(1, coordinate.Y);
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(new Coordinate(1, 2).Equals(new Coordinate(1, 2)));
            Assert.IsFalse(new Coordinate(2, 2).Equals(new Coordinate(1, 2)));
            Assert.IsFalse(new Coordinate(1, 1).Equals(new Coordinate(1, 2)));
        }

        [TestMethod]
        public void OperatorEquals()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsTrue(new Coordinate(1, 2) == new Coordinate(1, 2));
            Assert.IsFalse(new Coordinate(2, 2) == new Coordinate(1, 2));
            Assert.IsFalse(new Coordinate(1, 1) == new Coordinate(1, 2));
        }

        [TestMethod]
        public void OperatorNotEquals()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsFalse(new Coordinate(1, 2) != new Coordinate(1, 2));
            Assert.IsTrue(new Coordinate(2, 2) != new Coordinate(1, 2));
            Assert.IsTrue(new Coordinate(1, 1) != new Coordinate(1, 2));
        }
    }
}