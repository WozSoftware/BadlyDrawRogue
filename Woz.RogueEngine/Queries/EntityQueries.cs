using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Functional.Maybe;
using Woz.Functional.Maybe;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Queries
{
    public static class EntityQueries
    {
        public static int EffectiveAttributeValue(
            this Entity entity, EntityAttributes attribute)
        {
            return entity
                .Walk()
                .Select(x => x.Attributes.Lookup(attribute))
                .WhereValueExist()
                .Sum();
        }

        public static Maybe<T> LookupAsEnum<T>(
            this IImmutableDictionary<EntityAttributes, int> attributes,
            EntityAttributes attribute)
            where T : struct, IConvertible
        {
            return attributes
                .Lookup(attribute)
                .Select(x => EnumConverter<T>.Convert(x).ToMaybe());
        }

        public static IEnumerable<Entity> Walk(this Entity root)
        {
            yield return root;
            foreach (var child in root.Children.SelectMany(x => x.Walk()))
            {
                yield return child;
            }
        }

        public static bool TreeHasFlagSet(this Entity entity, EntityFlags flag)
        {
            return entity
                .Walk()
                .Select(x => x.HasFlagSet(flag))
                .FirstMaybe(x => x)
                .OrElse(false);
        }

        public static bool HasFlagSet(this Entity entity, EntityFlags flag)
        {
            return entity.Flags.Lookup(flag).OrElse(false);
        }
    }
}