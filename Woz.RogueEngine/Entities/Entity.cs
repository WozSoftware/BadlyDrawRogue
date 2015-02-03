using System.Collections.Immutable;

namespace Woz.RogueEngine.Entities
{
    public class Entity 
    {
        private readonly long _id;
        private readonly string _name;
        private readonly IImmutableDictionary<EntityAttributes, int> _attributes;
        private readonly IImmutableDictionary<EntityFlags, bool> _flags;
        private readonly IImmutableDictionary<long, Entity> _children;

        public Entity(long id, string name)
            : this(
                id, name, 
                ImmutableDictionary<EntityAttributes, int>.Empty, 
                ImmutableDictionary<EntityFlags, bool>.Empty,
                ImmutableDictionary<long, Entity>.Empty)
        {
        }

        public Entity(
            long id,
            string name,
            IImmutableDictionary<EntityAttributes, int> attributes,
            IImmutableDictionary<EntityFlags, bool> flags,
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

        public IImmutableDictionary<EntityAttributes, int> Attributes
        {
            get { return _attributes; }
        }

        public IImmutableDictionary<EntityFlags, bool> Flags
        {
            get { return _flags; }
        }

        public IImmutableDictionary<long, Entity> Children
        {
            get { return _children; }
        }

        public Entity Set(
            IImmutableDictionary<EntityAttributes, int> attributes = null,
            IImmutableDictionary<EntityFlags, bool> flags = null,
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
