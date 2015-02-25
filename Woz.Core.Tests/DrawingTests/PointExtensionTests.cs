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

using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Drawing;

namespace Woz.Core.Tests.DrawingTests
{
    [TestClass]
    public class PointExtensionTests
    {
        [TestMethod]
        public void GreaterThan()
        {
            Assert.IsTrue(new Point(1, 1).GreaterThan(new Point(0, 0)));
            Assert.IsFalse(new Point(0, 1).GreaterThan(new Point(0, 0)));
            Assert.IsFalse(new Point(1, 0).GreaterThan(new Point(0, 0)));
        }

        [TestMethod]
        public void GreaterOrEqualTo()
        {
            Assert.IsTrue(new Point(2, 2).GreaterOrEqualTo(new Point(1, 1)));
            Assert.IsTrue(new Point(1, 1).GreaterOrEqualTo(new Point(1, 1)));
            Assert.IsFalse(new Point(0, 1).GreaterOrEqualTo(new Point(1, 1)));
            Assert.IsFalse(new Point(1, 0).GreaterOrEqualTo(new Point(1, 1)));
        }

        [TestMethod]
        public void LessThan()
        {
            Assert.IsTrue(new Point(0, 0).LessThan(new Point(1, 1)));
            Assert.IsFalse(new Point(0, 1).LessThan(new Point(1, 1)));
            Assert.IsFalse(new Point(1, 0).LessThan(new Point(1, 1)));
        }

        [TestMethod]
        public void LessOrEqualTo()
        {
            Assert.IsTrue(new Point(1, 1).LessOrEqualTo(new Point(1, 1)));
            Assert.IsTrue(new Point(0, 0).LessOrEqualTo(new Point(1, 1)));
            Assert.IsFalse(new Point(2, 1).LessOrEqualTo(new Point(1, 1)));
            Assert.IsFalse(new Point(1, 2).LessOrEqualTo(new Point(1, 1)));
        }

        [TestMethod]
        public void Within()
        {
            Assert.IsTrue(new Point(1, 1).Within(new Point(1, 1), new Point(3, 3)));
            Assert.IsTrue(new Point(1, 3).Within(new Point(1, 1), new Point(3, 3)));
            Assert.IsTrue(new Point(3, 1).Within(new Point(1, 1), new Point(3, 3)));
            Assert.IsTrue(new Point(3, 3).Within(new Point(1, 1), new Point(3, 3)));
            Assert.IsTrue(new Point(2, 2).Within(new Point(1, 1), new Point(3, 3)));
            Assert.IsFalse(new Point(0, 2).Within(new Point(1, 1), new Point(3, 3)));
            Assert.IsFalse(new Point(2, 0).Within(new Point(1, 1), new Point(3, 3)));
            Assert.IsFalse(new Point(0, 4).Within(new Point(1, 1), new Point(3, 3)));
            Assert.IsFalse(new Point(4, 0).Within(new Point(1, 1), new Point(3, 3)));
        }
    }
}
