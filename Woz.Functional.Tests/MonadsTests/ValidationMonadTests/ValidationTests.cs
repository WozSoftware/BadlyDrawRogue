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
using Woz.Functional.Monads.ValidationMonad;

namespace Woz.Functional.Tests.MonadsTests.ValidationMonadTests
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
        public void MapWhenSuccess()
        {
            var errorObject = 1.ToValid().Map(x => x + 1);

            Assert.IsTrue(errorObject.IsValid);
            Assert.AreEqual(2, errorObject.Value);
        }

        [TestMethod]
        public void MapWhenFailed()
        {
            var errorObject = "bang".ToInvalid<int>().Map(x => x + 1);

            Assert.IsFalse(errorObject.IsValid);
            Assert.AreEqual("bang", errorObject.ErrorMessage);
        }

        [TestMethod]
        public void FlatMapWhenSuccess()
        {
            var errorObject = 1.ToValid().FlatMap(x => (x + 1).ToValid());

            Assert.IsTrue(errorObject.IsValid);
            Assert.AreEqual(2, errorObject.Value);
        }

        [TestMethod]
        public void FlatMapWhenFailed()
        {
            var errorObject = "bang".ToInvalid<int>().FlatMap(x => (x + 1).ToValid());

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
