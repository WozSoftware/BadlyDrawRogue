using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;
using Functional.Maybe;
using Woz.Core.Conversion;
using Woz.Linq.Xml;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Definitions
{
    public static class EntityParser
    {
        public static IEnumerable<Entity> ReadEntities(
            this XElement entitiesElement)
        {
            return entitiesElement
                .Elements(XmlElements.Entity)
                .Select(ReadEntity);
        }

        public static Entity ReadEntity(this XElement entityElement)
        {
            var id = entityElement
                .MaybeAttribute(XmlAttributes.Id)
                .Select(x => x.Value.ParseAs<long>())
                .OrElse(0);

            return 
                new Entity(
                    id,
                    entityElement.RequiredAttribute(XmlAttributes.Type).Value.ToEnum<EntityType>(),
                    entityElement.RequiredAttribute(XmlAttributes.Name).Value,
                    ReadAttributes(entityElement.ElementOrDefault(XmlElements.Attributes)),
                    ReadFlags(entityElement.ElementOrDefault(XmlElements.Flags)),
                    ReadEntities(entityElement.ElementOrDefault(XmlElements.Entities)).ToImmutableList());
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