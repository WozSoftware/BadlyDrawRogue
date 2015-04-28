﻿#region License
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
            var vector = new Vector(1, 2);
            Assert.AreEqual(1, vector.X);
            Assert.AreEqual(2, vector.Y);
        }

        [TestMethod]
        public void DistanceFrom()
        {
            Assert.AreEqual(
                1, new Vector(1, 1).DistanceFrom(new Vector(1, 2)));

            Assert.AreEqual(
                1, new Vector(1, 1).DistanceFrom(new Vector(2, 1)));

            Assert.AreEqual(
                1, new Vector(2, 1).DistanceFrom(new Vector(1, 1)));

            Assert.AreEqual(
                1, new Vector(1, 2).DistanceFrom(new Vector(1, 1)));

            Assert.IsTrue(Math.Abs(
                new Vector(2, 2).DistanceFrom(new Vector(1, 1)) -
                1.4142135623731d) < 0.0000000000001d);
        }

        [TestMethod]
        public void Add()
        {
            var coordinate = new Vector(1, 2).Add(Vector.SouthEast);
            Assert.AreEqual(2, coordinate.X);
            Assert.AreEqual(1, coordinate.Y);
        }
        [TestMethod]
        public void ScaleBy()
        {
            var vector = new Vector(1, 2).ScaleBy(3);
            Assert.AreEqual(3, vector.X);
            Assert.AreEqual(6, vector.Y);
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(new Vector(1, 2).Equals(new Vector(1, 2)));
            Assert.IsFalse(new Vector(2, 2).Equals(new Vector(1, 2)));
            Assert.IsFalse(new Vector(1, 1).Equals(new Vector(1, 2)));

            // Use object entry point
            Assert.IsFalse(new Vector(1, 1).Equals((object)new Vector(1, 2)));
        }

        [TestMethod]
        public void OperatorEquals()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsTrue(new Vector(1, 2) == new Vector(1, 2));
            Assert.IsFalse(new Vector(2, 2) == new Vector(1, 2));
            Assert.IsFalse(new Vector(1, 1) == new Vector(1, 2));
        }

        [TestMethod]
        public void OperatorNotEquals()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsFalse(new Vector(1, 2) != new Vector(1, 2));
            Assert.IsTrue(new Vector(2, 2) != new Vector(1, 2));
            Assert.IsTrue(new Vector(1, 1) != new Vector(1, 2));
        }

        [TestMethod]
        public void GetHashCodeXorValues()
        {
            Assert.AreEqual(10 ^ 11, new Vector(10, 11).GetHashCode());
        }
    }
}
