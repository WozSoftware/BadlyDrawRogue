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
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Functional.Try;

namespace Woz.Functional.Tests.TryTests
{
    [TestClass]
    public class ITryTests
    {
        [TestMethod]
        public void ToSuccess()
        {
            var errorObject = 1.ToSuccess();

            Assert.IsTrue(errorObject.IsValid);
            Assert.AreEqual(1, errorObject.Value);
        }

        [TestMethod]
        public void ToFailed()
        {
            var errorObject = "bang".ToFailed<int>();

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreEqual("bang", errorObject.ErrorMessage);
        }

        [TestMethod]
        public void BindWhenSuccess()
        {
            var errorObject = 1.ToSuccess().Bind(x => (x + 1).ToSuccess());

            Assert.IsTrue(errorObject.IsValid);
            Assert.AreEqual(2, errorObject.Value);
        }

        [TestMethod]
        public void BindWhenFailed()
        {
            var errorObject = "bang".ToFailed<int>().Bind(x => (x + 1).ToSuccess());

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreEqual("bang", errorObject.ErrorMessage);
        }

        [TestMethod]
        public void TryBindWhenSuccess()
        {
            var errorObject = 1.ToSuccess().TryBind(x => (x + 1).ToSuccess());

            Assert.IsTrue(errorObject.IsValid);
            Assert.AreEqual(2, errorObject.Value);
        }

        [TestMethod]
        public void TryBindWhenExceptionThrown()
        {
            var errorObject = 1
                .ToSuccess()
                .TryBind<int>(
                    x =>
                    {
                        throw new Exception("thrown");

                    });

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreEqual("thrown", errorObject.ErrorMessage);
        }

        [TestMethod]
        public void TryBindWhenFailed()
        {
            var errorObject = "bang".ToFailed<int>().TryBind(x => (x + 1).ToSuccess());

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreEqual("bang", errorObject.ErrorMessage);
        }

        [TestMethod]
        public void ThrowOnErrorWhenSuccess()
        {
            var errorObject = 1.ToSuccess();
            var result = errorObject.ThrowOnError(s => new Exception(s));

            Assert.AreEqual(errorObject, result);
        }

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void ThrowOnErrorWhenInvalid()
        {
            var errorObject = "fail".ToFailed<int>();
            errorObject.ThrowOnError(s => new Exception(s));
        }

        [TestMethod]
        public void ReturnOrThrowWhenSuccess()
        {
            var errorObject = 1.ToSuccess();
            var result = errorObject.ReturnOrThrow(s => new Exception(s));

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void ReturnOrThrowWhenInvalid()
        {
            var errorObject = "fail".ToFailed<int>();
            errorObject.ReturnOrThrow(s => new Exception(s));
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(1.ToSuccess().Equals(1.ToSuccess()));
            Assert.IsTrue("A".ToFailed<int>().Equals("A".ToFailed<int>()));
            Assert.IsFalse(1.ToSuccess().Equals(2.ToSuccess()));
            Assert.IsFalse("A".ToFailed<int>().Equals("B".ToFailed<int>()));
            Assert.IsFalse("A".ToFailed<int>().Equals(null));
        }
    }
}
