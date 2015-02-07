using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Functional.Maybe;
using Woz.Core.Conversion;
using Woz.Functional.Maybe;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Queries
{
    public static class EntityQueries
    {
        public static int EffectiveAttributeValue(
            this IEntity entity,
            EntityAttributes attribute)
        {
            return entity
                .Flattern()
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
                .Select(x => x.ToEnum<int, T>());
        }

        public static IEnumerable<IEntity> Flattern(this IEntity root)
        {
            return root.Flattern(x => true);
        }

        public static IEnumerable<IEntity> Flattern(
            this IEntity root, Func<IEntity, bool> predicate)
        {
            if (!predicate(root))
            {
                yield break;
            }

            yield return root;
            foreach (var child in 
                root.Children.SelectMany(x => x.Flattern(predicate)))
            {
                yield return child;
            }
        }

        public static bool TreeHasFlagSet(this IEntity entity, EntityFlags flag)
        {
            return entity
                .Flattern()
                .Select(x => x.HasFlagSet(flag))
                .FirstMaybe(x => x)
                .OrElse(false);
        }

        public static bool HasFlagSet(this IEntity entity, EntityFlags flag)
        {
            return entity.Flags.Lookup(flag).OrElse(false);
        }
    }
}