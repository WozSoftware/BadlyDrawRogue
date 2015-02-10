#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Functional.
//
// Woz.Functional is free software: you can redistribute it 
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
using Woz.Functional.Try;

namespace Woz.Functional.Tests.TryTests
{
    [TestClass]
    public class ErrorLinqTests
    {
        [TestMethod]
        public void SelectWhenSuccess()
        {
            var result = 1.ToSuccess().Select(x => 2);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(2, result.Value);
        }

        [TestMethod]
        public void SelectWhenFailed()
        {
            var result = "A".ToFailed<int>().Select(x => 2);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("A", result.ErrorMessage);
        }

        [TestMethod]
        public void TrySelectWhenSuccess()
        {
            var result = 1.ToSuccess().TrySelect(x => 2);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(2, result.Value);
        }

        [TestMethod]
        public void TrySelectWhenThrows()
        {
            var result = 1
                .ToSuccess()
                .TrySelect<int, int>(
                    x =>
                    {
                        throw new Exception("A");
                    });

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("A", result.ErrorMessage);
        }

        [TestMethod]
        public void TrySelectWhenFailed()
        {
            var result = "A".ToFailed<int>().TrySelect(x => 2);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("A", result.ErrorMessage);
        }

        [TestMethod]
        public void SelectManyWhenSuccess()
        {
            var result =
                from a in 1.ToSuccess()
                from b in 2.ToSuccess()
                select a + b;

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(3, result.Value);
        }

        [TestMethod]
        public void SelectManyWhenError()
        {
            var result =
                from a in 1.ToSuccess()
                from b in "A".ToFailed<int>()
                select a + b;

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("A", result.ErrorMessage);
        }
    }
}
