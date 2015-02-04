//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Linq;
//using Woz.Functional.Generators;

//namespace Woz.RogueEngine.Entities
//{
//    public static class EntityStore
//    {
//        public static Func<string, Entity> Build(IEnumerable<Entity> entities)
//        {
//            var idGenerator = IdGenerator.Build();
//            var store = entities.ToImmutableDictionary(entity => entity.Name);

//            return name => store[name].Prepare(idGenerator);
//        }

//        private static Entity Prepare(
//            this Entity entity, Func<long> idGenerator)
//        {
//            var children = entity.Children.Any()
//                ? entity
//                    .Children
//                    .Select(x => x.Prepare(idGenerator))
//                    .ToImmutableList()
//                : entity.Children;

//            return new Entity(
//                idGenerator(),
//                entity.Name,
//                entity.Attributes,
//                entity.Flags,
//                children);
//        }
//    }
//}