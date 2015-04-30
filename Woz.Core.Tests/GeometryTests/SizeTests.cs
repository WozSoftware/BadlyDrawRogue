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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Geometry;

namespace Woz.Core.Tests.GeometryTests
{
    [TestClass]
    public class SizeTests
    {
        [TestMethod]
        public void Create()
        {
            var size = Size.Create(1, 2);
            Assert.AreEqual(1, size.Width);
            Assert.AreEqual(2, size.Height);
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(Size.Create(1, 2).Equals(Size.Create(1, 2)));
            Assert.IsFalse(Size.Create(2, 2).Equals(Size.Create(1, 2)));
            Assert.IsFalse(Size.Create(1, 1).Equals(Size.Create(1, 2)));

            // Use object entry point
            Assert.IsFalse(Size.Create(1, 1).Equals((object)Size.Create(1, 2)));
        }

        [TestMethod]
        public void OperatorEquals()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsTrue(Size.Create(1, 2) == Size.Create(1, 2));
            Assert.IsFalse(Size.Create(2, 2) == Size.Create(1, 2));
            Assert.IsFalse(Size.Create(1, 1) == Size.Create(1, 2));
        }

        [TestMethod]
        public void OperatorNotEquals()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsFalse(Size.Create(1, 2) != Size.Create(1, 2));
            Assert.IsTrue(Size.Create(2, 2) != Size.Create(1, 2));
            Assert.IsTrue(Size.Create(1, 1) != Size.Create(1, 2));
        }

        [TestMethod]
        public void GetHashCodeXorValues()
        {
            var vector1 = Size.Create(10, 11);
            var vector2 = Size.Create(11, 11);

            Assert.AreEqual(vector1.GetHashCode(), vector1.GetHashCode());
            Assert.AreEqual(vector2.GetHashCode(), vector2.GetHashCode());
            Assert.AreNotEqual(vector1.GetHashCode(), vector2.GetHashCode());
        }
    }
}