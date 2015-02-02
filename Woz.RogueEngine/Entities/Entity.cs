using Functional.Maybe;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Woz.RogueEngine.Entities
{
    public class Entity
    {
        private readonly long _id;
        private readonly string _name;
        private readonly IImmutableDictionary<string, Field<int>> _attributes;
        private readonly IImmutableDictionary<string, Field<bool>> _flags;
        private readonly IImmutableDictionary<long, Entity> _children;

        public Entity(
            long id,
            string name, 
            IEnumerable<Field<int>> attributes,
            IEnumerable<Field<bool>> flags,
            IEnumerable<Entity> children)
        {
            _id = id;
            _name = name;
            _attributes = BuildDictionary(attributes, x => x.Name);
            _flags = BuildDictionary(flags, x => x.Name);
            _children = BuildDictionary(children, x => x.Id);
        }

        private ImmutableDictionary<TKey, TValue> BuildDictionary<TKey, TValue>(
            IEnumerable<TValue> values, Func<TValue, TKey> keySelector)
        {
            return values
                .ToMaybe()
                .Where(x => x.Any())
                .Select(x => x.ToImmutableDictionary(keySelector))
                .OrElse(ImmutableDictionary<TKey, TValue>.Empty);
        }

        private Entity(
            long id,
            string name,
            IImmutableDictionary<string, Field<int>> attributes,
            IImmutableDictionary<string, Field<bool>> flags,
            IImmutableDictionary<long, Entity> children)
        {
            _id = id;
            _name = name;
            _attributes = attributes;
            _flags = flags;
            _children = children;
        }

        public long Id
        {
            get { return _id; }
        }

        public string Name 
        { 
            get { return _name; } 
        }

        public IImmutableDictionary<string, Field<int>> Attributes
        {
            get { return _attributes; }
        }

        public IImmutableDictionary<string, Field<bool>> Flags
        {
            get { return _flags; }
        }

        public IImmutableDictionary<long, Entity> Children
        {
            get { return _children; }
        }

        public Entity Set(
            IImmutableDictionary<string, Field<int>> attributes = null,
            IImmutableDictionary<string, Field<bool>> flags = null,
            IImmutableDictionary<long, Entity> children = null)
        {
            return attributes == null && flags == null && children == null
                ? this
                : new Entity(
                    Id, _name,
                    attributes ?? _attributes,
                    flags ?? _flags,
                    children ?? _children);
        }
    }
}
