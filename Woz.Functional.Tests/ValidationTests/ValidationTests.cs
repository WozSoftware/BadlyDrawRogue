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
using Woz.Functional.Validation;

namespace Woz.Functional.Tests.ValidationTests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void ToValid()
        {
            var errorObject = 1.ToValid();

            Assert.IsTrue(errorObject.IsValid);
            Assert.AreEqual(1, errorObject.Value);
        }

        [TestMethod]
        public void ToInvalid()
        {
            var errorObject = "bang".ToInvalid<int>();

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreEqual("bang", errorObject.ErrorMessage);
        }

        [TestMethod]
        public void BindWhenSuccess()
        {
            var errorObject = 1.ToValid().Bind(x => (x + 1).ToValid());

            Assert.IsTrue(errorObject.IsValid);
            Assert.AreEqual(2, errorObject.Value);
        }

        [TestMethod]
        public void BindWhenFailed()
        {
            var errorObject = "bang".ToInvalid<int>().Bind(x => (x + 1).ToValid());

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreEqual("bang", errorObject.ErrorMessage);
        }

        [TestMethod]
        public void TryBindWhenSuccess()
        {
            var errorObject = 1.ToValid().TryBind(x => (x + 1).ToValid());

            Assert.IsTrue(errorObject.IsValid);
            Assert.AreEqual(2, errorObject.Value);
        }

        [TestMethod]
        public void TryBindWhenExceptionThrown()
        {
            var errorObject = 1
                .ToValid()
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
            var errorObject = "bang".ToInvalid<int>().TryBind(x => (x + 1).ToValid());

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreEqual("bang", errorObject.ErrorMessage);
        }

        [TestMethod]
        public void ThrowOnErrorWhenSuccess()
        {
            var errorObject = 1.ToValid();
            var result = errorObject.ThrowOnError(x => new Exception());

            Assert.AreEqual(errorObject, result);
        }

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void ThrowOnErrorWhenInvalid()
        {
            var errorObject = "fail".ToInvalid<int>();
            errorObject.ThrowOnError(x => new Exception());
        }

        [TestMethod]
        public void OrElseWhenSuccess()
        {
            var errorObject = 1.ToValid();
            var result = errorObject.OrElse(x => new Exception());

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void OrElseWhenInvalid()
        {
            var errorObject = "fail".ToInvalid<int>();
            errorObject.OrElse(x => new Exception());
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(1.ToValid().Equals(1.ToValid()));
            Assert.IsTrue("A".ToInvalid<int>().Equals("A".ToInvalid<int>()));
            Assert.IsFalse(1.ToValid().Equals(2.ToValid()));
            Assert.IsFalse("A".ToInvalid<int>().Equals("B".ToInvalid<int>()));
            Assert.IsFalse("A".ToInvalid<int>().Equals(null));
        }
    }
}
