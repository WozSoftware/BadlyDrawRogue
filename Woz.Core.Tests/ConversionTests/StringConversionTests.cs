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
using Woz.Core.Conversion;

namespace Woz.Core.Tests.ConversionTests
{
    [TestClass]
    public class StringConversionTests
    {
        [TestMethod]
        public void ParseAsWhenValid()
        {
            Assert.AreEqual(5, "5".ParseAs<int>());
            Assert.AreEqual(5m, "5".ParseAs<decimal>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseAsWhenInvalid()
        {
            "a".ParseAs<int>();
        }

        [TestMethod]
        public void ParseAsWithDefaultWhenValid()
        {
            Assert.AreEqual(5, "5".ParseAs(2));
            Assert.AreEqual(5m, "5".ParseAs(2m));
        }

        [TestMethod]
        public void ParseAsWithDefaultWhenInvalid()
        {
            Assert.AreEqual(5, "a".ParseAs(5));
        }

        [TestMethod]
        public void ParseAsMaybeWhenValid()
        {
            Assert.AreEqual(5, "5".ParseAsMaybe<int>().Value);
            Assert.AreEqual(5m, "5".ParseAsMaybe<decimal>().Value);
        }

        [TestMethod]
        public void ParseAsMaybeWhenInvalid()
        {
            Assert.IsFalse("a".ParseAsMaybe<int>().HasValue);
        }
    }
}