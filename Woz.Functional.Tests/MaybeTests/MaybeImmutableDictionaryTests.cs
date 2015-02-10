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

using System.Collections.Immutable;
using Functional.Maybe;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Woz.Functional.Tests.MaybeTests
{
    [TestClass]
    public class MaybeImmutableDictionaryTests
    {
        [TestMethod]
        public void LookupWhenPresent()
        {
            var dictionary = ImmutableDictionary<int, string>.Empty.Add(1, "A");

            var result = dictionary.Lookup(1);

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual("A", result.Value);
        }

        [TestMethod]
        public void LookupWhenNotPresent()
        {
            var result = ImmutableDictionary<int, string>.Empty.Lookup(1);

            Assert.IsFalse(result.HasValue);
        }
    }
}
