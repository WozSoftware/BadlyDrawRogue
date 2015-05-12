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
    public class AttributeHelpersTests
    {
        [TestMethod]
        public void RequiredAttributeWhenPresent()
        {
            var element = new XElement("A", new XAttribute("B", "C"));

            var attribute = element.RequiredAttribute("B");

            Assert.AreEqual("B", attribute.Name);
            Assert.AreEqual("C", attribute.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void RequiredAttributeWhenNotPresent()
        {
            new XElement("A").RequiredAttribute("A");
        }

        [TestMethod]
        public void MaybeAttributeWhenPresent()
        {
            var element = new XElement("A", new XAttribute("B", "C"));

            var attribute = element.MaybeAttribute("B");

            Assert.IsTrue(attribute.HasValue);
            Assert.AreEqual("B", attribute.Value.Name);
            Assert.AreEqual("C", attribute.Value.Value);
        }

        [TestMethod]
        public void MaybeAttributeWhenNotPresent()
        {
            var element = new XElement("A");

            var attribute = element.MaybeAttribute("B");

            Assert.IsFalse(attribute.HasValue);
        }
    }
}
