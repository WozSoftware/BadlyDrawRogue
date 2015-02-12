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
    public class TryTests
    {
        [TestMethod]
        public void ToSuccess()
        {
            var errorObject = 1.ToSuccess();

            Assert.IsTrue(errorObject.IsValid);
            Assert.AreEqual(1, errorObject.Value);
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
        public void BindWhenSuccess()
        {
            var errorObject = 1.ToSuccess().Bind(x => (x + 1).ToSuccess());

            Assert.IsTrue(errorObject.IsValid);
            Assert.AreEqual(2, errorObject.Value);
        }

        [TestMethod]
        public void BindWhenException()
        {
            var exception = new Exception();

            var errorObject = exception
                .ToException<int>()
                .Bind(x => (x + 1).ToSuccess());

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreSame(exception, errorObject.Error);
        }

        [TestMethod]
        public void BindWhenExceptionThrown()
        {
            var exception = new Exception();

            var errorObject = 1
                .ToSuccess()
                .Bind<int>(
                    x =>
                    {
                        throw exception;

                    });

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreSame(exception, errorObject.Error);
        }

        [TestMethod]
        public void ThrowOnErrorWithBuilderWhenSuccess()
        {
            var errorObject = 1.ToSuccess();
            var result = errorObject
                .ThrowOnError(x => new InvalidOperationException());

            Assert.AreEqual(errorObject, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowOnErrorWithBuilderWhenInvalid()
        {
            var exception = new Exception();
            var errorObject = exception.ToException<int>();

            errorObject.ThrowOnError(x => new InvalidOperationException());
        }

        [TestMethod]
        public void ThrowOnErrorWhenSuccess()
        {
            var errorObject = 1.ToSuccess();
            var result = errorObject.ThrowOnError();

            Assert.AreEqual(errorObject, result);
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
        public void OrElseWithBuilderWhenSuccess()
        {
            var errorObject = 1.ToSuccess();
            var result = errorObject
                .OrElse(s => new InvalidOperationException());

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void OrElseWithBuilderWhenInvalid()
        {
            var exception = new Exception();
            var errorObject = exception.ToException<int>();

            errorObject.OrElse(x => new InvalidOperationException());
        }

        [TestMethod]
        public void OrElseWhenSuccess()
        {
            var errorObject = 1.ToSuccess();
            var result = errorObject.OrElseException();

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
