using System.Collections.Immutable;
using Functional.Maybe;

namespace Woz.Functional.Maybe
{
    public static class MaybeImmutableDictionary
    {
        public static Maybe<T> Lookup<TK, T>(this IImmutableDictionary<TK, T> dictionary, TK key)
        {
            var getter = MaybeFunctionalWrappers.Wrap<TK, T>(dictionary.TryGetValue);
            return getter(key);
        }
    }
}
