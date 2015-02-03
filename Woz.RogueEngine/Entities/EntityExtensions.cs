using System.Collections.Immutable;
using Functional.Maybe;
using Woz.Functional.Maybe;

namespace Woz.RogueEngine.Entities
{
    public static class EntityExtensions
    {
        public static IImmutableDictionary<long, Entity> SetItem(
            this IImmutableDictionary<long, Entity> collection, Entity item)
        {
            return collection.SetItem(item.Id, item);
        }

        public static bool TestFlag(
            this IImmutableDictionary<string, bool> flags, string name)
        {
            return flags.Lookup(name).OrElse(false);
        }
    }
}