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
using Woz.Functional.Monads.TryMonad;

namespace Woz.Functional.Tests.MonadsTests.TryMonadTests
{
    [TestClass]
    public class TryTests
    {
        [TestMethod]
        public void ToSuccess()
        {
            var successObject = 1.ToSuccess();

            Assert.IsTrue(successObject.IsValid);
            Assert.AreEqual(1, successObject.Value);
        }

        [TestMethod]
        public void ToException()
        {
            var exception = new Exception();
            var errorObject = exception.ToException<int>();

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreSame(exception, errorObject.Error);
        }

        [TestMethod]
        public void SelectWhenSuccess()
        {
            var result = 1.ToSuccess().Select(x => 2);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(2, result.Value);
        }

        [TestMethod]
        public void SelectWhenThrows()
        {
            var exception = new Exception();

            var result = 1
                .ToSuccess()
                .Select<int>(
                    x =>
                    {
                        throw exception;
                    });

            Assert.IsFalse(result.IsValid);
            Assert.AreSame(exception, result.Error);
        }

        [TestMethod]
        public void SelectWhenFailed()
        {
            var exception = new Exception();

            var result = exception.ToException<int>().Select(x => 2);

            Assert.IsFalse(result.IsValid);
            Assert.AreSame(exception, result.Error);
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
            var exception = new Exception();

            var result =
                from a in 1.ToSuccess()
                from b in exception.ToException<int>()
                select a + b;

            Assert.IsFalse(result.IsValid);
            Assert.AreSame(exception, result.Error);
        }

        [TestMethod]
        public void ThrowOnErrorWithFactoryWhenSuccess()
        {
            var successObject = 1.ToSuccess();
            var result = successObject
                .ThrowOnError(x => new InvalidOperationException());

            Assert.AreEqual(successObject, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowOnErrorWithFactoryWhenInvalid()
        {
            var exception = new Exception();
            var errorObject = exception.ToException<int>();

            errorObject.ThrowOnError(x => new InvalidOperationException());
        }

        [TestMethod]
        public void ThrowOnErrorWhenSuccess()
        {
            var successObject = 1.ToSuccess();
            var result = successObject.ThrowOnError();

            Assert.AreEqual(successObject, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThrowOnErrorWhenInvalid()
        {
            var exception = new Exception();
            var errorObject = exception.ToException<int>();

            errorObject.ThrowOnError();
        }

        [TestMethod]
        public void OrElseWithFactoryWhenSuccess()
        {
            var successObject = 1.ToSuccess();
            var result = successObject
                .OrElse(s => new InvalidOperationException());

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void OrElseWithFactoryWhenInvalid()
        {
            var exception = new Exception();
            var errorObject = exception.ToException<int>();

            errorObject.OrElse(x => new InvalidOperationException());
        }

        [TestMethod]
        public void OrElseWhenSuccess()
        {
            var successObject = 1.ToSuccess();
            var result = successObject.OrElseException();

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void OrElseWhenInvalid()
        {
            var exception = new Exception();
            var errorObject = exception.ToException<int>();

            errorObject.OrElseException();
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(1.ToSuccess().Equals(1.ToSuccess()));

            var exception = new Exception();
            Assert.IsTrue(exception.ToException<int>().Equals(exception.ToException<int>()));

            Assert.IsFalse(new Exception().ToException<int>().Equals(new Exception().ToException<int>()));

            Assert.IsFalse(1.ToSuccess().Equals(2.ToSuccess()));

            Assert.IsFalse(new Exception("A").ToException<int>().Equals(new Exception("B").ToException<int>()));

            Assert.IsFalse(new Exception().ToException<int>().Equals(null));
        }
    }
}
