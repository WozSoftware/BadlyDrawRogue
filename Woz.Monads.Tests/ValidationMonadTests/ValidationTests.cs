#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Monads.
//
// Woz.Linq is free software: you can redistribute it 
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
using Woz.Monads.ValidationMonad;

namespace Woz.Monads.Tests.ValidationMonadTests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void ToValid()
        {
            var validObject = 1.ToValid();

            Assert.IsTrue(validObject.IsValid);
            Assert.AreEqual(1, validObject.Value);
        }

        [TestMethod]
        public void ToInvalid()
        {
            var errorObject = "bang".ToInvalid<int>();

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreEqual("bang", errorObject.ErrorMessage);
        }

        [TestMethod]
        public void SelectWhenSuccess()
        {
            var result = 1.ToValid().Select(x => 2);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(2, result.Value);
        }

        [TestMethod]
        public void SelectWhenFailed()
        {
            var result = "A".ToInvalid<int>().Select(x => 2);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("A", result.ErrorMessage);
        }

        [TestMethod]
        public void SelectManyWhenSuccess()
        {
            var result =
                from a in 1.ToValid()
                from b in 2.ToValid()
                select a + b;

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(3, result.Value);
        }

        [TestMethod]
        public void SelectManyWhenError()
        {
            var result =
                from a in 1.ToValid()
                from b in "A".ToInvalid<int>()
                select a + b;

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("A", result.ErrorMessage);
        }

        [TestMethod]
        public void WithErrorFromValidValid()
        {
            var parent = "A".ToValid();
            var child = 5.ToValid();

            var result = parent.WithErrorFrom(child);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual("A", result.Value);
        }

        [TestMethod]
        public void WithErrorFromValidInvalid()
        {
            var parent = "A".ToValid();
            var child = "Fail".ToInvalid<int>();

            var result = parent.WithErrorFrom(child);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Fail", result.ErrorMessage);
        }

        [TestMethod]
        public void WithErrorFromInvalidValid()
        {
            var parent = "Fail".ToInvalid<string>();
            var child = 5.ToValid();

            var result = parent.WithErrorFrom(child);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Fail", result.ErrorMessage);
        }

        [TestMethod]
        public void ToTryWhenSuccess()
        {
            var validObject = 1.ToValid();
            var result = validObject.ToTry(x => new Exception());

            Assert.AreEqual(validObject.Value, result.Value);
        }

        [TestMethod]
        public void ToTryWhenInvalid()
        {
            var errorObject = "fail".ToInvalid<int>();
            var tryObject = errorObject.ToTry(x => new Exception());

            Assert.IsFalse(tryObject.IsValid);
        }

        [TestMethod]
        public void OrElseWhenSuccess()
        {
            var validObject = 1.ToValid();
            var result = validObject.OrElse(x => new Exception());

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
