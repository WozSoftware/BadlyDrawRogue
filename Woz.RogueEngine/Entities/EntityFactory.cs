using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Woz.Functional.Generators;

namespace Woz.RogueEngine.Entities
{
    public class EntityFactory : IEntityFactory
    {
        private readonly Func<long> _idGenerator;
        private readonly IImmutableDictionary<EntityType, IImmutableList<IEntity>> _templates;

        private EntityFactory(IEnumerable<IEntity> entities)
        {
            _idGenerator = IdGenerator.Build();
            _templates = entities
                .GroupBy(x => x.EntityType)
                .ToImmutableDictionary(
                    x => x.Key, 
                    x => (IImmutableList<IEntity>) x.ToImmutableList());
        }

        public IImmutableDictionary<EntityType, IImmutableList<IEntity>> Templates
        {
            get { return _templates; }
        }

        public static IEntityFactory Build(IEnumerable<IEntity> entities)
        {
            return new EntityFactory(entities);
        }

        public IEntity Build(IEntity template)
        {
            return Build(template, template.Name);
        }

        public IEntity Build(IEntity template, string name)
        {
            return 
                Entity.Build(
                    _idGenerator(),
                    template.EntityType,
                    name,
                    template.Attributes,
                    template.Flags,
                    template.Children);
        }
    }
}