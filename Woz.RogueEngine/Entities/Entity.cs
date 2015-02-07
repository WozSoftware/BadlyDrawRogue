using System.Collections.Immutable;

namespace Woz.RogueEngine.Entities
{
    public class Entity : IEntity
    {
        private readonly long _id;
        private readonly EntityType _entityType;
        private readonly string _name;
        private readonly IImmutableDictionary<EntityAttributes, int> _attributes;
        private readonly IImmutableDictionary<EntityFlags, bool> _flags;
        private readonly IImmutableList<IEntity> _children;

        private Entity(
            long id,
            EntityType entityType,
            string name,
            IImmutableDictionary<EntityAttributes, int> attributes,
            IImmutableDictionary<EntityFlags, bool> flags,
            IImmutableList<IEntity> children)
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

        public IImmutableList<IEntity> Children
        {
            get { return _children; }
        }

        public static IEntity Build(long id, EntityType entityType, string name)
        {
            return Build(
                id,
                entityType,
                name,
                ImmutableDictionary<EntityAttributes, int>.Empty,
                ImmutableDictionary<EntityFlags, bool>.Empty,
                ImmutableList<IEntity>.Empty);
        }

        public static IEntity Build(
            long id,
            EntityType entityType,
            string name,
            IImmutableDictionary<EntityAttributes, int> attributes,
            IImmutableDictionary<EntityFlags, bool> flags,
            IImmutableList<IEntity> children)
        {
            return new Entity(id, entityType, name, attributes, flags, children);
        }

        public IEntity Set(
            IImmutableDictionary<EntityAttributes, int> attributes = null,
            IImmutableDictionary<EntityFlags, bool> flags = null,
            IImmutableList<IEntity> children = null)
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
