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
using Woz.Monads.MaybeMonad;

namespace Woz.Monads.Tests.MaybeMonadTests
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
            var maybe = Maybe<int>.None.Select(x => (x + 1));

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
            var maybe = Maybe<int>.None.SelectMany(x => (x + 1).ToMaybe());

            Assert.IsFalse(maybe.HasValue);
        }

        [TestMethod]
        public void SelectManyComposeAllSome()
        {
            var maybe = 
                from value1 in 1.ToMaybe()
                from value2 in 2.ToMaybe()
                select value1 + value2;

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(3, maybe.Value);
        }

        [TestMethod]
        public void SelectManyComposeFirstNone()
        {
            var maybe =
                from value1 in 1.ToMaybe()
                from value2 in Maybe<int>.None
                select value1 + value2;

            Assert.IsFalse(maybe.HasValue);
        }

        [TestMethod]
        public void SelectManyComposeSecondNone()
        {
            var maybe =
                from value1 in Maybe<int>.None
                from value2 in 1.ToMaybe()
                select value1 + value2;

            Assert.IsFalse(maybe.HasValue);
        }

        [TestMethod]
        public void WhereSomeTrue()
        {
            var maybe = 1.ToMaybe().Where(x => x == 1);

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(1, maybe.Value);
        }

        [TestMethod]
        public void WhereSomeFalse()
        {
            var maybe = 1.ToMaybe().Where(x => x == 2);

            Assert.IsFalse(maybe.HasValue);
        }

        [TestMethod]
        public void WhereNone()
        {
            var maybe = Maybe<int>.None.Where(x => x == 2);

            Assert.IsFalse(maybe.HasValue);
        }

        [TestMethod]
        public void DoSome()
        {
            var called = false;

            1.ToMaybe()
             .Do(
                 x =>
                 {
                     Assert.AreEqual(1, x);
                     called = true;
                 });
            
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void DoNone()
        {
            var called = false;

            Maybe<int>.None
                      .Do(
                          x =>
                          {
                              called = true;
                          });

            Assert.IsFalse(called);
        }

        [TestMethod]
        public void MatchFunctionsWhenSome()
        {
            var maybe = 1.ToMaybe();

            var result = maybe.Match(
                some: value => value + 1,
                none: () => 3);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void MatchFunctionsWhenNone()
        {
            var maybe = Maybe<int>.None;

            var result = maybe.Match(
                some: value => value + 1,
                none: () => 3);

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void MatchActionsWhenSome()
        {
            var maybe = 1.ToMaybe();

            maybe.Match(
                some: value =>
                {
                    Assert.AreEqual(1, value);
                },
                none: Assert.Fail);
        }

        [TestMethod]
        public void MatchActionsWhenNone()
        {
            var maybe = Maybe<int>.None;

            maybe.Match(
                some: value =>
                {
                    Assert.Fail();
                },
                none: () => { });
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
            Assert.AreEqual(0, Maybe<int>.None.OrElseDefault());
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
            Assert.AreEqual(5, Maybe<int>.None.OrElse(5));
        }

        [TestMethod]
        public void OrElseFactoryWhenSome()
        {
            var maybe = 1.ToMaybe();

            Assert.AreEqual(1, maybe.OrElse(() => 5));
        }

        [TestMethod]
        public void OrElseFactoryWhenNone()
        {
            Assert.AreEqual(5, Maybe<int>.None.OrElse(() => 5));
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
            Assert.AreEqual(5, Maybe<int>.None.OrElseThrow(() => new Exception()));
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(1.ToMaybe().Equals(1.ToMaybe()));
            Assert.IsFalse(2.ToMaybe().Equals(1.ToMaybe()));
            Assert.IsFalse(2.ToMaybe().Equals(Maybe<int>.None));
            Assert.IsTrue(Maybe<int>.None.Equals(Maybe<int>.None));
        }
    }
}
