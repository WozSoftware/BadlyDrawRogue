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
using Woz.Functional.Error;

namespace Woz.Functional.Tests.ErrorTests
{
    [TestClass]
    public class ErrorTests
    {
        [TestMethod]
        public void ErrorWhenSuccess()
        {
            var errorObject = 1.ToSuccess();

            Assert.IsTrue(errorObject.IsValid);
            Assert.AreEqual(1, errorObject.Value);
        }

        [TestMethod]
        public void ErrorWhenInvalid()
        {
            var errorObject = "bang".ToError<int>();

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
        public void BindWhenInvalid()
        {
            var errorObject = "bang".ToError<int>().Bind(x => (x + 1).ToSuccess());

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
        public void TryBindWhenInvalid()
        {
            var errorObject = "bang".ToError<int>().TryBind(x => (x + 1).ToSuccess());

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
            var errorObject = "fail".ToError<int>();
            errorObject.ThrowOnError(s => new Exception(s));
        }

        [TestMethod]
        public void ReturnWhenSuccess()
        {
            var errorObject = 1.ToSuccess();
            var result = errorObject.Return(s => new Exception(s));

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void ReturnWhenInvalid()
        {
            var errorObject = "fail".ToError<int>();
            errorObject.Return(s => new Exception(s));
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(1.ToSuccess().Equals(1.ToSuccess()));
            Assert.IsTrue("A".ToError<int>().Equals("A".ToError<int>()));
            Assert.IsFalse(1.ToSuccess().Equals(2.ToSuccess()));
            Assert.IsFalse("A".ToError<int>().Equals("B".ToError<int>()));
            Assert.IsFalse("A".ToError<int>().Equals(null));
        }

        [TestMethod]
        [SuppressMessage("ReSharper", "EqualExpressionComparison")]
        public void OperatorEqual()
        {
            Assert.IsTrue(1.ToSuccess() == 1.ToSuccess());
            Assert.IsTrue("A".ToError<int>() == "A".ToError<int>());
            Assert.IsFalse(1.ToSuccess() == 2.ToSuccess());
            Assert.IsFalse("A".ToError<int>() == "B".ToError<int>());
            Assert.IsFalse("A".ToError<int>() == null);
        }

        [TestMethod]
        [SuppressMessage("ReSharper", "EqualExpressionComparison")]
        public void OperatorNotEqual()
        {
            Assert.IsFalse(1.ToSuccess() != 1.ToSuccess());
            Assert.IsFalse("A".ToError<int>() != "A".ToError<int>());
            Assert.IsTrue(1.ToSuccess() != 2.ToSuccess());
            Assert.IsTrue("A".ToError<int>() != "B".ToError<int>());
            Assert.IsTrue("A".ToError<int>() != null);
        }

        [TestMethod]
        public void OperatorCollapseWhenOuterSuccess()
        {
            var nested = 1.ToSuccess().ToSuccess();
            Error<int> collapsed = nested;

            Assert.IsTrue(collapsed.IsValid);
            Assert.AreEqual(1, collapsed.Value);
        }

        [TestMethod]
        public void OperatorCollapseWhenOuterError()
        {
            var nested = "A".ToError<Error<int>>();
            Error<int> collapsed = nested;

            Assert.IsFalse(collapsed.IsValid);
            Assert.AreEqual("A", collapsed.ErrorMessage);

        }
    }
}
