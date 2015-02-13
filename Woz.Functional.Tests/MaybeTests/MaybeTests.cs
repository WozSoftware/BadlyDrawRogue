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
using Woz.Functional.Maybe;

namespace Woz.Functional.Tests.MaybeTests
{
    [TestClass]
    public class MaybeTests
    {
        [TestMethod]
        public void ToMaybeObjectNull()
        {
            var maybe = ((object)null).ToMaybe();

            Assert.IsFalse(maybe.HasValue);
        }

        [TestMethod]
        public void ToMaybeObjectSet()
        {
            var item = new object();
            var maybe = item.ToMaybe();

            Assert.IsTrue(maybe.HasValue);
            Assert.AreSame(item, maybe.Value);
        }

        [TestMethod]
        public void ToMaybeNullableNull()
        {
            var maybe = ((int?)null).ToMaybe();

            Assert.IsFalse(maybe.HasValue);
        }

        [TestMethod]
        public void ToMaybeNullableSet()
        {
            var maybe = ((int?)1).ToMaybe();

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(1, maybe.Value);
        }

        [TestMethod]
        public void ToMaybeValue()
        {
            var maybe = 1.ToMaybe();

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(1, maybe.Value);
        }

        [TestMethod]
        public void ToSumNull()
        {
            var maybe = ((object)null).ToSome();

            Assert.IsTrue(maybe.HasValue);
            Assert.IsNull(maybe.Value);
        }

        [TestMethod]
        public void ToSumValue()
        {
            var maybe = 1.ToSome();

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(1, maybe.Value);
        }

        [TestMethod]
        public void BindWhenValue()
        {
            var maybe = 1.ToMaybe().Bind(x => (x + 1).ToMaybe());

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(2, maybe.Value);
        }

        [TestMethod]
        public void BindWhenNoValue()
        {
            var maybe = Maybe<int>.Nothing.Bind(x => (x + 1).ToMaybe());

            Assert.IsFalse(maybe.HasValue);
        }

        [TestMethod]
        public void OrElseDefaultWhenValue()
        {
            var maybe = 1.ToMaybe();

            Assert.AreEqual(1, maybe.OrElseDefault());
        }

        [TestMethod]
        public void OrElseDefaultWhenNoValue()
        {
            Assert.AreEqual(0, Maybe<int>.Nothing.OrElseDefault());
        }

        [TestMethod]
        public void OrElseWhenValue()
        {
            var maybe = 1.ToMaybe();

            Assert.AreEqual(1, maybe.OrElse(5));
        }

        [TestMethod]
        public void OrElseWhenNoValue()
        {
            Assert.AreEqual(5, Maybe<int>.Nothing.OrElse(5));
        }

        [TestMethod]
        public void OrElseBuilderWhenValue()
        {
            var maybe = 1.ToMaybe();

            Assert.AreEqual(1, maybe.OrElse(() => 5));
        }

        [TestMethod]
        public void OrElseBuilderWhenNoValue()
        {
            Assert.AreEqual(5, Maybe<int>.Nothing.OrElse(() => 5));
        }

        [TestMethod]
        public void OrElseExceptionWhenValue()
        {
            var maybe = 1.ToMaybe();

            Assert.AreEqual(1, maybe.OrElse(() => new Exception()));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void OrElseExceptionWhenNoValue()
        {
            Assert.AreEqual(5, Maybe<int>.Nothing.OrElse(() => new Exception()));
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(1.ToMaybe().Equals(1.ToMaybe()));
            Assert.IsFalse(2.ToMaybe().Equals(1.ToMaybe()));
            Assert.IsFalse(2.ToMaybe().Equals(Maybe<int>.Nothing));
            Assert.IsTrue(Maybe<int>.Nothing.Equals(Maybe<int>.Nothing));
        }
    }
}
