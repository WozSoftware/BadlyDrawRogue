using System.Collections.Immutable;

namespace Woz.RogueEngine.Entities
{
    public interface IEntity
    {
        long Id { get; }
        EntityType EntityType { get; }
        string Name { get; }
        IImmutableDictionary<EntityAttributes, int> Attributes { get; }
        IImmutableDictionary<EntityFlags, bool> Flags { get; }
        IImmutableList<IEntity> Children { get; }

        IEntity Set(
            IImmutableDictionary<EntityAttributes, int> attributes = null,
            IImmutableDictionary<EntityFlags, bool> flags = null,
            IImmutableList<IEntity> children = null);
    }
}