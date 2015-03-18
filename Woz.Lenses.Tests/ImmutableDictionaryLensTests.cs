#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Lenses.
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

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Monads.MaybeMonad;

namespace Woz.Lenses.Tests
{
    [TestClass]
    public class ImmutableDictionaryLensTests
    {
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void LensByKeyGetWhenNotPresent()
        {
            var dict = ImmutableDictionary<int, string>.Empty.Add(1, "A");

            var elementLens = ImmutableDictionaryLens.ByKey<int, string>(2);

            dict.Get(elementLens);
        }

        [TestMethod]
        public void ToLensByKeyGetWhenPresent()
        {
            var dict = ImmutableDictionary<int, string>.Empty.Add(1, "A");

            var elementLens = ImmutableDictionaryLens.ByKey<int, string>(1);

            Assert.AreSame("A", dict.Get(elementLens));
        }

        [TestMethod]
        public void ToLensByKeySetAddsEntry()
        {
            var dict = ImmutableDictionary<int, string>.Empty.Add(1, "A");

            var elementLens = ImmutableDictionaryLens.ByKey<int, string>(2);

            var updated = dict.Set(elementLens, "B");

            Assert.AreEqual(2, updated.Count());
            Assert.AreEqual("A", updated[1]);
            Assert.AreEqual("B", updated[2]);
        }

        [TestMethod]
        public void ToLensByKeySetUpdatesEntry()
        {
            var dict = ImmutableDictionary<int, string>.Empty.Add(1, "A");

            var elementLens = ImmutableDictionaryLens.ByKey<int, string>(1);

            var updated = dict.Set(elementLens, "B");

            Assert.AreEqual(1, updated.Count());
            Assert.AreEqual("B", updated[1]);
        }

        [TestMethod]
        public void ToLensLookupGetWhenNotPresent()
        {
            var dict = ImmutableDictionary<int, string>.Empty.Add(1, "A");

            var elementLens = ImmutableDictionaryLens.Lookup<int, string>(2);

            Assert.AreSame(Maybe<string>.None, dict.Get(elementLens));
        }

        [TestMethod]
        public void ToLensLookupGetWhenPresent()
        {
            var dict = ImmutableDictionary<int, string>.Empty.Add(1, "A");

            var elementLens = ImmutableDictionaryLens.Lookup<int, string>(1);

            Assert.AreSame("A", dict.Get(elementLens).Value);
        }

        [TestMethod]
        public void ToLensLookupSetAddsEntry()
        {
            var dict = ImmutableDictionary<int, string>.Empty.Add(1, "A");

            var elementLens = ImmutableDictionaryLens.Lookup<int, string>(2);

            var updated = dict.Set(elementLens, "B".ToMaybe());

            Assert.AreEqual(2, updated.Count());
            Assert.AreEqual("A", updated[1]);
            Assert.AreEqual("B", updated[2]);
        }

        [TestMethod]
        public void ToLensLookupSetUpdatesEntry()
        {
            var dict = ImmutableDictionary<int, string>.Empty.Add(1, "A");

            var elementLens = ImmutableDictionaryLens.Lookup<int, string>(1);

            var updated = dict.Set(elementLens, "B".ToMaybe());

            Assert.AreEqual(1, updated.Count());
            Assert.AreEqual("B", updated[1]);
        }

        [TestMethod]
        public void ToLensLookupSetNothingRemovesEntry()
        {
            var dict = ImmutableDictionary<int, string>.Empty.Add(1, "A");

            var elementLens = ImmutableDictionaryLens.Lookup<int, string>(1);

            var updated = dict.Set(elementLens, Maybe<string>.None);

            Assert.IsFalse(updated.Any());
        }
    }
}