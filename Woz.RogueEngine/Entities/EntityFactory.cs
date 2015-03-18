#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.RoqueEngine.
//
// Woz.RoqueEngine is free software: you can redistribute it 
// and/or modify it under the terms of the GNU General Public 
// License as published by the Free Software Foundation, either 
// version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Woz.Core.Generators;

namespace Woz.RogueEngine.Entities
{
    using ITemplateStore = IImmutableDictionary<EntityType, IImmutableList<Entity>>;

    public class EntityFactory : IEntityFactory
    {
        public static readonly Entity Void;

        private readonly Func<long> _idGenerator;
        private readonly ITemplateStore _templates;

        static EntityFactory()
        {
            Void = CreateVoidEntity();
        }

        private EntityFactory(IEnumerable<Entity> entities)
        {
            Debug.Assert(entities != null);

            _idGenerator = IdGenerator.Create();
            _templates = entities
                .GroupBy(x => x.EntityType)
                .ToImmutableDictionary(
                    x => x.Key, 
                    x => (IImmutableList<Entity>) ImmutableList.ToImmutableList<Entity>(x));
        }

        public ITemplateStore Templates
        {
            get { return _templates; }
        }

        public static IEntityFactory Create(IEnumerable<Entity> entities)
        {
            return new EntityFactory(entities);
        }

        public Entity Create(Entity template)
        {
            return Create(template, template.Name);
        }

        public Entity Create(Entity template, string name)
        {
            return 
                Entity.Create(
                    _idGenerator(),
                    template.EntityType,
                    name,
                    template.Attributes,
                    template.Flags,
                    template.Children);
        }

        private static Entity CreateVoidEntity()
        {
            // Never has an ID. Not expected to be manipulated, just used to
            // mark voids in the map for route finding etc
            return Entity.Create(
                0,
                EntityType.Void,
                "An empty void",
                ImmutableDictionary<EntityAttributes, int>.Empty,
                ImmutableDictionary<EntityFlags, bool>
                    .Empty
                    .SetItem(EntityFlags.BlocksMovement, true)
                    .SetItem(EntityFlags.BlocksLineOfSight, true),
                ImmutableDictionary<long, Entity>.Empty);
        }
    }
}