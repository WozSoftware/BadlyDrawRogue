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

using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using Woz.Functional.Monads.MaybeMonad;

namespace Woz.Linq.Xml
{
    public static class XAttributeHelpers
    {
        public static XAttribute 
            RequiredAttribute(this XElement element, string name)
        {
            Debug.Assert(element != null);
            Debug.Assert(!string.IsNullOrEmpty(name));

            return element
                .MaybeAttribute(name)
                .OrElseThrow(
                    () => new XmlException(
                        string.Format(
                            "Attribute {0} missing from Element {1}",
                            name, element.Name)));
        }

        public static IMaybe<XAttribute> 
            MaybeAttribute(this XElement element, string name)
        {
            Debug.Assert(element != null);
            Debug.Assert(!string.IsNullOrEmpty(name));

            return element.Attribute(name).ToMaybe();
        }
    }
}