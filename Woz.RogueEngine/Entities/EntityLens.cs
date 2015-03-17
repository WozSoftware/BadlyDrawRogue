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

using System.Collections.Immutable;
using Woz.Lenses;

namespace Woz.RogueEngine.Entities
{
    using IAttributeStore = IImmutableDictionary<EntityAttributes, int>;
    using IFlagStore = IImmutableDictionary<EntityFlags, bool>;
    using IChildStore = IImmutableDictionary<long, Entity>;

    public static class EntityLens
    {
        public static readonly Lens<Entity, long> Id;
        public static readonly Lens<Entity, EntityType> EntityType;
        public static readonly Lens<Entity, string> Name;
        public static readonly Lens<Entity, IAttributeStore> Attributes;
        public static readonly Lens<Entity, IFlagStore> Flags;
        public static readonly Lens<Entity, IChildStore> Children;

        static EntityLens()
        {
            Id = Lens.Create<Entity, long>(
                entity => entity.Id,
                id => entity => entity.With(id: id));

            EntityType = Lens.Create<Entity, EntityType>(
                entity => entity.EntityType,
                entityType => entity => entity.With(entityType: entityType));

            Name = Lens.Create<Entity, string>(
                entity => entity.Name,
                name => entity => entity.With(name: name));

            Attributes = Lens.Create<Entity, IAttributeStore>(
                entity => entity.Attributes,
                attributes => entity => entity.With(attributes: attributes));

            Flags = Lens.Create<Entity, IFlagStore>(
                entity => entity.Flags,
                flags => entity => entity.With(flags: flags));

            Children = Lens.Create<Entity, IChildStore>(
                entity => entity.Children,
                children => entity => entity.With(children: children));
        }
    }
}