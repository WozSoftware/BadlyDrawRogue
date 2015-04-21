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
            var size = new Size(1, 2);
            Assert.AreEqual(1, size.Width);
            Assert.AreEqual(2, size.Height);
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(new Size(1, 2).Equals(new Size(1, 2)));
            Assert.IsFalse(new Size(2, 2).Equals(new Size(1, 2)));
            Assert.IsFalse(new Size(1, 1).Equals(new Size(1, 2)));
        }

        [TestMethod]
        public void OperatorEquals()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsTrue(new Size(1, 2) == new Size(1, 2));
            Assert.IsFalse(new Size(2, 2) == new Size(1, 2));
            Assert.IsFalse(new Size(1, 1) == new Size(1, 2));
        }

        [TestMethod]
        public void OperatorNotEquals()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsFalse(new Size(1, 2) != new Size(1, 2));
            Assert.IsTrue(new Size(2, 2) != new Size(1, 2));
            Assert.IsTrue(new Size(1, 1) != new Size(1, 2));
        }
    }
}