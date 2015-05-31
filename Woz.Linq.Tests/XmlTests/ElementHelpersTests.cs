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

using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Linq.Xml;

namespace Woz.Linq.Tests.XmlTests
{
    [TestClass]
    public class ElementHelpersTests
    {
        [TestMethod]
        public void RequiredElementWhenPresent()
        {
            var element = new XElement("A", new XElement("B", "C"));

            var child = element.RequiredElement("B");

            Assert.AreEqual("B", child.Name);
            Assert.AreEqual("C", child.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void RequiredElementWhenNotPresent()
        {
            new XElement("A").RequiredElement("A");
        }

        [TestMethod]
        public void MaybeElementWhenPresent()
        {
            var element = new XElement("A", new XElement("B", "C"));

            var child = element.MaybeElement("B");

            Assert.IsTrue(child.HasValue);
            Assert.AreEqual("B", child.Value.Name);
            Assert.AreEqual("C", child.Value.Value);
        }

        [TestMethod]
        public void MaybeElementWhenNotPresent()
        {
            var element = new XElement("A");

            var child = element.MaybeElement("B");

            Assert.IsFalse(child.HasValue);
        }

        [TestMethod]
        public void ElementOrDefaultWhenPresent()
        {
            var element = new XElement("A", new XElement("B", "C"));

            var child = element.ElementOrDefault("B");

            Assert.AreEqual("B", child.Name);
            Assert.AreEqual("C", child.Value);
        }

        [TestMethod]
        public void ElementOrDefaultWhenNotPresent()
        {
            var element = new XElement("A");

            var child = element.ElementOrDefault("B");

            Assert.AreEqual("B", child.Name);
            Assert.AreEqual(string.Empty, child.Value);
        }
    }
}
