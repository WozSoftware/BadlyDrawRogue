using System.Collections.Immutable;

namespace Woz.RogueEngine.Entities
{
    public class Entity
    {
        private readonly long _id;
        private readonly EntityType _entityType;
        private readonly string _name;
        private readonly IImmutableDictionary<EntityAttributes, int> _attributes;
        private readonly IImmutableDictionary<EntityFlags, bool> _flags;
        private readonly IImmutableList<Entity> _children;

        public Entity(long id, EntityType entityType, string name)
            : this(
                id,
                entityType,
                name, 
                ImmutableDictionary<EntityAttributes, int>.Empty, 
                ImmutableDictionary<EntityFlags, bool>.Empty,
                ImmutableList<Entity>.Empty)
        {
        }

        public Entity(
            long id,
            EntityType entityType,
            string name,
            IImmutableDictionary<EntityAttributes, int> attributes,
            IImmutableDictionary<EntityFlags, bool> flags,
            IImmutableList<Entity> children)
        {
            _id = id;
            _entityType = entityType;
            _name = name;
            _attributes = attributes;
            _flags = flags;
            _children = children;
        }

        public long Id
        {
            get { return _id; }
        }

        public EntityType EntityType
        {
            get { return _entityType; }    
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

        public IImmutableList<Entity> Children
        {
            get { return _children; }
        }

        public Entity Set(
            IImmutableDictionary<EntityAttributes, int> attributes = null,
            IImmutableDictionary<EntityFlags, bool> flags = null,
            IImmutableList<Entity> children = null)
        {
            return attributes == null && flags == null && children == null
                ? this
                : new Entity(
                    _id,
                    _entityType,
                    _name,
                    attributes ?? _attributes,
                    flags ?? _flags,
                    children ?? _children);
        }
    }
}
