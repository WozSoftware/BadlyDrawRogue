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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Woz.Lenses.Tests
{
    [TestClass]
    public class LensTests
    {
        public class Root
        {
            private readonly int _value;
            private readonly Child _child;

            public Root(int value, Child child)
            {
                _value = value;
                _child = child;
            }

            public int Value
            {
                get { return _value; }
            }

            public Child Child
            {
                get { return _child; }
            }

            public Root With(int? value = null, Child child = null)
            {
                return new Root(value ?? _value, child ?? _child);
            }
        }

        public class Child
        {
            private readonly int _value;

            public Child(int value)
            {
                _value = value;
            }

            public int Value
            {
                get { return _value; }
            }

            public Child With(int? value = null)
            {
                return new Child(value ?? _value);
            }
        }

        [TestMethod]
        public void Get()
        {
            var instance = new Root(5, null);

            Assert.AreEqual(5, instance.Get(RootValue));
        }

        [TestMethod]
        public void Set()
        {
            var instance = new Root(0, null).Set(RootValue, 5);

            Assert.AreEqual(5, instance.Value);
        }

        [TestMethod]
        public void WithGetComposes()
        {
            var instance = new Root(0, new Child(5));

            Assert.AreEqual(5, instance.Get(RootChildValue));
        }

        [TestMethod]
        public void WithSetComposes()
        {
            var instance = new Root(0, new Child(0));

            instance = instance.Set(RootChildValue, 5);

            Assert.AreEqual(5, instance.Child.Value);
        }

        private static Lens<Root, int> RootValue
        {
            get
            {
                return Lens.Create<Root, int>(
                    root => root.Value,
                    value => root => root.With(value: value));
            }
        }

        private static Lens<Root, int> RootChildValue
        {
            get
            {
                var rootChild =
                    Lens.Create<Root, Child>(
                        root => root.Child,
                        child => root => root.With(child: child));

                var childValue =
                    Lens.Create<Child, int>(
                        child => child.Value,
                        value => child => child.With(value: value));

                return rootChild.With(childValue);
            }
        }
    }
}
