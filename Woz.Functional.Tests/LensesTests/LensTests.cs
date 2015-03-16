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
using Woz.Functional.Lenses;

namespace Woz.Functional.Tests.LensesTests
{
    [TestClass]
    public class LensTests
    {
        // Lenses do not make sense with mutable state but this eases the
        // scaffolding required for immutable objects and is still a valid 
        // set of tests

        private class Root
        {
            public int Value { get; set; }
            public Child Child { get; set; }
        }

        public class Child
        {
            public int Value { get; set; }
        }

        [TestMethod]
        public void Get()
        {
            var instance = new Root {Value = 5};

            Assert.AreEqual(5, instance.Get(RootValue));
        }

        [TestMethod]
        public void Set()
        {
            var instance = new Root().Set(RootValue, 5);

            Assert.AreEqual(5, instance.Value);
        }

        [TestMethod]
        public void WithGetComposes()
        {
            var instance = new Root {Child = new Child {Value = 5}};

            Assert.AreEqual(5, instance.Get(RootChildValue));
        }

        [TestMethod]
        public void WithSetComposes()
        {
            var instance = new Root { Child = new Child()};

            instance = instance.Set(RootChildValue, 5);

            Assert.AreEqual(5, instance.Child.Value);
        }

        private static Lens<Root, int> RootValue
        {
            get
            {
                return Lens.Create<Root, int>(
                    root => root.Value,
                    value => root =>
                    {
                        root.Value = value;
                        return root;
                    });
            }
        }

        private static Lens<Root, int> RootChildValue
        {
            get
            {
                var rootChild =
                    Lens.Create<Root, Child>(
                        root => root.Child,
                        child => root =>
                        {
                            root.Child = child;
                            return root;
                        });

                var childValue =
                    Lens.Create<Child, int>(
                        child => child.Value,
                        value => child =>
                        {
                            child.Value = value;
                            return child;
                        });

                return rootChild.With(childValue);
            }
        }
    }
}
