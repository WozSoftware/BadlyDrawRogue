#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.RoqueEngine.
//
// Woz.RoqueEngine is free software: you can redistribute it 
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
using System.Xml.Linq;
using Woz.Core.Conversion;
using Woz.Functional.Maybe;
using Woz.Linq.Xml;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Definitions
{
    public static class EntityParser
    {
        public static IEnumerable<IEntity> ReadEntities(
            this XElement entitiesElement)
        {
            return entitiesElement
                .Elements(XmlElements.Entity)
                .Select(ReadEntity);
        }

        public static IEntity ReadEntity(this XElement entityElement)
        {
            var id = entityElement
                .MaybeAttribute(XmlAttributes.Id)
                .Select(x => x.Value.ParseAs<long>())
                .OrElse(0);

            return
                Entity.Build(
                    id,
                    entityElement.RequiredAttribute(XmlAttributes.Type).Value.ToEnum<EntityType>(),
                    entityElement.RequiredAttribute(XmlAttributes.Name).Value,
                    ReadAttributes(entityElement.ElementOrDefault(XmlElements.Attributes)),
                    ReadFlags(entityElement.ElementOrDefault(XmlElements.Flags)),
                    ReadEntities(entityElement.ElementOrDefault(XmlElements.Entities))
                        .ToImmutableDictionary(entity => entity.Id));
        }

        public static IImmutableDictionary<EntityAttributes, int>
            ReadAttributes(this XElement attributesElement)
        {
            return attributesElement
                .Elements(XmlElements.Attribute)
                .ToImmutableDictionary(
                    x => x.RequiredAttribute(XmlAttributes.Type).Value.ToEnum<EntityAttributes>(),
                    x => x.RequiredAttribute(XmlAttributes.Value).Value.ParseAs<int>());
        }

        public static IImmutableDictionary<EntityFlags, bool>
            ReadFlags(this XElement flagsElement)
        {
            return flagsElement
                .Elements(XmlElements.Flag)
                .ToImmutableDictionary(
                    x => x.RequiredAttribute(XmlAttributes.Type).Value.ToEnum<EntityFlags>(), 
                    x => x.RequiredAttribute(XmlAttributes.Value).Value.ParseAs<bool>());
        }
    }
}