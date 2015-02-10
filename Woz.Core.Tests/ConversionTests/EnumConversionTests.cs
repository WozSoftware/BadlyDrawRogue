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

using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Conversion;

namespace Woz.Core.Tests.ConversionTests
{
    [TestClass]
    public class EnumConversionTests
    {
        private enum TestEnum
        {
            A = 0,
            B = 1
        };

        [TestMethod]
        public void StringToEnumWhenValid()
        {
            Assert.AreEqual(TestEnum.A, "A".ToEnum<TestEnum>());
            Assert.AreEqual(TestEnum.B, "B".ToEnum<TestEnum>());
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidEnumArgumentException))]
        public void StringToEnumWhenInvalid()
        {
            "C".ToEnum<TestEnum>();
        }

        [TestMethod]
        public void StringToEnumWithDefaultWhenValid()
        {
            Assert.AreEqual(TestEnum.A, "A".ToEnum(TestEnum.B));
            Assert.AreEqual(TestEnum.B, "B".ToEnum(TestEnum.A));
        }

        [TestMethod]
        public void StringToEnumWithDefaultWhenInvalid()
        {
            Assert.AreEqual(TestEnum.B, "C".ToEnum(TestEnum.B));
        }

        [TestMethod]
        public void StringToMaybeEnumWhenValid()
        {
            Assert.AreEqual(TestEnum.A, "A".ToMaybeEnum<TestEnum>().Value);
            Assert.AreEqual(TestEnum.B, "B".ToMaybeEnum<TestEnum>().Value);
        }

        [TestMethod]
        public void StringToMaybeEnumWithInvalid()
        {
            Assert.IsFalse("C".ToMaybeEnum<TestEnum>().HasValue);
        }

        [TestMethod]
        public void ValueToEnumWhenValid()
        {
            Assert.AreEqual(TestEnum.A, 0.ToEnum<int, TestEnum>());
            Assert.AreEqual(TestEnum.B, 1.ToEnum<int, TestEnum>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEnumArgumentException))]
        public void ValueToEnumWhenInvalid()
        {
            3.ToEnum<int, TestEnum>();
        }

        [TestMethod]
        public void ValueToEnumWithDefaultWhenValid()
        {
            Assert.AreEqual(TestEnum.A, 0.ToEnum(TestEnum.B));
            Assert.AreEqual(TestEnum.B, 1.ToEnum(TestEnum.A));
        }

        [TestMethod]
        public void ValueToEnumWithDefaultWhenInvalid()
        {
            Assert.AreEqual(TestEnum.B, 3.ToEnum(TestEnum.B));
        }

        [TestMethod]
        public void ValueToMaybeEnumWhenValid()
        {
            Assert.AreEqual(TestEnum.A, 0.ToMaybeEnum<int, TestEnum>().Value);
            Assert.AreEqual(TestEnum.B, 1.ToMaybeEnum<int, TestEnum>().Value);
        }

        [TestMethod]
        public void ValueToMaybeEnumWithInvalid()
        {
            Assert.IsFalse(3.ToMaybeEnum<int, TestEnum>().HasValue);
        }
    }
}
