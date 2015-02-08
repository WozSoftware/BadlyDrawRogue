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