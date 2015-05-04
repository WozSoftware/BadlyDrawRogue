#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Core.
//
// Woz.Core is free software: you can redistribute it 
// and/or modify it under the terms of the GNU General Public 
// License as published by the Free Software Foundation, either 
// version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Geometry;

namespace Woz.Core.Tests.GeometryTests
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void Create()
        {
            var vector = Vector.Create(1, 2);
            Assert.AreEqual(1, vector.X);
            Assert.AreEqual(2, vector.Y);
        }

        [TestMethod]
        public void DistanceFrom()
        {
            Assert.AreEqual(
                1, Vector.Create(1, 1).DistanceFrom(Vector.Create(1, 2)));

            Assert.AreEqual(
                1, Vector.Create(1, 1).DistanceFrom(Vector.Create(2, 1)));

            Assert.AreEqual(
                1, Vector.Create(2, 1).DistanceFrom(Vector.Create(1, 1)));

            Assert.AreEqual(
                1, Vector.Create(1, 2).DistanceFrom(Vector.Create(1, 1)));

            Assert.IsTrue(Math.Abs(
                Vector.Create(2, 2).DistanceFrom(Vector.Create(1, 1)) -
                1.4142135623731d) < 0.0000000000001d);
        }

        [TestMethod]
        public void Abs()
        {
            var vector = Vector.Create(-1, -2).Abs();
            Assert.AreEqual(1, vector.X);
            Assert.AreEqual(2, vector.Y);
        }

        [TestMethod]
        public void ScaleBy()
        {
            var vector = Vector.Create(1, 2).ScaleBy(3);
            Assert.AreEqual(3, vector.X);
            Assert.AreEqual(6, vector.Y);
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(Vector.Create(1, 2).Equals(Vector.Create(1, 2)));
            Assert.IsFalse(Vector.Create(2, 2).Equals(Vector.Create(1, 2)));
            Assert.IsFalse(Vector.Create(1, 1).Equals(Vector.Create(1, 2)));

            // Use object entry point
            Assert.IsFalse(Vector.Create(1, 1).Equals((object)Vector.Create(1, 2)));
        }

        [TestMethod]
        public void OperatorEquals()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsTrue(Vector.Create(1, 2) == Vector.Create(1, 2));
            Assert.IsFalse(Vector.Create(2, 2) == Vector.Create(1, 2));
            Assert.IsFalse(Vector.Create(1, 1) == Vector.Create(1, 2));
        }

        [TestMethod]
        public void OperatorNotEquals()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsFalse(Vector.Create(1, 2) != Vector.Create(1, 2));
            Assert.IsTrue(Vector.Create(2, 2) != Vector.Create(1, 2));
            Assert.IsTrue(Vector.Create(1, 1) != Vector.Create(1, 2));
        }

        [TestMethod]
        public void OperatorAdd()
        {
            var coordinate = Vector.Create(1, 2) + (Directions.SouthEast);
            Assert.AreEqual(2, coordinate.X);
            Assert.AreEqual(1, coordinate.Y);
        }

        [TestMethod]
        public void OperatorSubtract()
        {
            var coordinate = Vector.Create(1, 2) - (Directions.SouthEast);
            Assert.AreEqual(0, coordinate.X);
            Assert.AreEqual(3, coordinate.Y);
        }

        [TestMethod]
        public void GetHashCodeXorValues()
        {
            var vector1 = Vector.Create(10, 11);
            var vector2 = Vector.Create(11, 11);
            
            Assert.AreEqual(vector1.GetHashCode(), vector1.GetHashCode());
            Assert.AreEqual(vector2.GetHashCode(), vector2.GetHashCode());
            Assert.AreNotEqual(vector1.GetHashCode(), vector2.GetHashCode());
        }
    }
}
