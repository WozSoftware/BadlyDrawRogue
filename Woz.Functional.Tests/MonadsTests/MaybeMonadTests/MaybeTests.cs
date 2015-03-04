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
using Woz.Functional.Monads.MaybeMonad;

namespace Woz.Functional.Tests.MonadsTests.MaybeMonadTests
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
        public void SelectWhenSome()
        {
            var maybe = 1.ToMaybe().Select(x => (x + 1));

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(2, maybe.Value);
        }

        [TestMethod]
        public void SelectWhenNone()
        {
            var maybe = Maybe<int>.Nothing.Select(x => (x + 1));

            Assert.IsFalse(maybe.HasValue);
        }

        [TestMethod]
        public void SelectManyWhenSome()
        {
            var maybe = 1.ToMaybe().SelectMany(x => (x + 1).ToMaybe());

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(2, maybe.Value);
        }

        [TestMethod]
        public void SelectManyWhenNone()
        {
            var maybe = Maybe<int>.Nothing.SelectMany(x => (x + 1).ToMaybe());

            Assert.IsFalse(maybe.HasValue);
        }

        [TestMethod]
        public void SelectManyCompose()
        {
            var maybe = 
                from value1 in 1.ToMaybe()
                from value2 in 2.ToMaybe()
                select value1 + value2;

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(3, maybe.Value);
        }

        [TestMethod]
        public void OrElseDefaultWhenSome()
        {
            var maybe = 1.ToMaybe();

            Assert.AreEqual(1, maybe.OrElseDefault());
        }

        [TestMethod]
        public void OrElseDefaultWhenNone()
        {
            Assert.AreEqual(0, Maybe<int>.Nothing.OrElseDefault());
        }

        [TestMethod]
        public void OrElseWhenSome()
        {
            var maybe = 1.ToMaybe();

            Assert.AreEqual(1, maybe.OrElse(5));
        }

        [TestMethod]
        public void OrElseWhenNone()
        {
            Assert.AreEqual(5, Maybe<int>.Nothing.OrElse(5));
        }

        [TestMethod]
        public void OrElseBuilderWhenSome()
        {
            var maybe = 1.ToMaybe();

            Assert.AreEqual(1, maybe.OrElse(() => 5));
        }

        [TestMethod]
        public void OrElseBuilderWhenNone()
        {
            Assert.AreEqual(5, Maybe<int>.Nothing.OrElse(() => 5));
        }

        [TestMethod]
        public void OrElseThrowWhenSome()
        {
            var maybe = 1.ToMaybe();

            Assert.AreEqual(1, maybe.OrElseThrow(() => new Exception()));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void OrElseThrowWhenNone()
        {
            Assert.AreEqual(5, Maybe<int>.Nothing.OrElseThrow(() => new Exception()));
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
