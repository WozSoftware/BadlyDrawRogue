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
        private readonly ImmutableDictionary<string, Field<int>> _attributes;
        private readonly ImmutableDictionary<string, Field<bool>> _flags;
        private readonly ImmutableDictionary<long, Entity> _children;

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
                .ToMaybe<IEnumerable<TValue>>()
                .Where(x => x.Any())
                .Select(x => x.ToImmutableDictionary(keySelector))
                .OrElse(ImmutableDictionary<TKey, TValue>.Empty);
        }

        private Entity(
            long id,
            string name,
            ImmutableDictionary<string, Field<int>> attributes,
            ImmutableDictionary<string, Field<bool>> flags,
            ImmutableDictionary<long, Entity> children)
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

        public ImmutableDictionary<string, Field<int>> Attributes
        {
            get { return _attributes; }
        }

        public ImmutableDictionary<string, Field<bool>> Flags
        {
            get { return _flags; }
        }

        public ImmutableDictionary<long, Entity> Children
        {
            get { return _children; }
        }

        public Entity Set(
            ImmutableDictionary<string, Field<int>> attributes,
            ImmutableDictionary<string, Field<bool>> flags,
            ImmutableDictionary<long, Entity> children)
        {
            return new Entity(
                Id, _name, 
                attributes ?? _attributes, 
                flags ?? _flags,
                children ?? _children);
        }
    }
}
