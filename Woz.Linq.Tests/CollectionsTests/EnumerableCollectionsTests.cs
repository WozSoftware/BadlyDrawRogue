#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Linq.
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
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Linq.Collections;
using Woz.Monads.MaybeMonad;

namespace Woz.Linq.Tests.CollectionsTests
{
    [TestClass]
    public class EnumerableCollectionsTests
    {
        private class Node
        {
            public IMaybe<Node> Parent;
        }

        [TestMethod]
        public void LinkedListToEnumerable()
        {
            var node3 = new Node {Parent = Maybe<Node>.None};
            var node2 = new Node {Parent = node3.ToSome()};
            var node1 = new Node {Parent = node2.ToSome()};

            var expected = new List<Node> {node1, node2, node3};

            var result = node1.LinkedListToEnumerable(x => x.Parent).ToList();

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Select()
        {
            var source = new List<int> { 1, 2, 3 };

            var result = source.GetEnumerator().Select().ToArray();

            CollectionAssert.AreEqual(source, result);
        }

        [TestMethod]
        public void SelectWithSelector()
        {
            var expected = new []{ 1, 2, 3 };
            var source =
                new[]
                {
                    new {A = 1},
                    new {A = 2},
                    new {A = 3},
                }.ToList();

            var result = source.GetEnumerator().Select(x => x.A).ToArray();

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MinBy()
        {
            var source =
                new[]
                {
                    new {A = 1},
                    new {A = 2},
                    new {A = 3},
                };

            Assert.AreSame(source.First(), source.MinBy(x => x.A));
        }

        [TestMethod]
        public void MaxBy()
        {
            var source =
                new[]
                {
                    new {A = 1},
                    new {A = 2},
                    new {A = 3},
                };

            Assert.AreSame(source.Last(), source.MaxBy(x => x.A));
        }

        [TestMethod]
        public void ForEach()
        {
            var source = new[] {1, 2, 3};

            var processed = new List<int>();
            source.ForEach(processed.Add);

            CollectionAssert.AreEqual(source, processed);
        }
    }
}
