using System.Collections.Immutable;

namespace Woz.RogueEngine.Entities
{
    public interface IEntityFactory
    {
        IImmutableDictionary<EntityType, IImmutableList<IEntity>> Templates { get; }

        IEntity Build(IEntity template);
        IEntity Build(IEntity template, string name);
    }
}