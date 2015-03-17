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
using System.Collections.Immutable;
using System.Diagnostics;

namespace Woz.RogueEngine.Entities
{
    using IAttributeStore = IImmutableDictionary<EntityAttributes, int>;
    using IFlagStore = IImmutableDictionary<EntityFlags, bool>;
    using IChildStore = IImmutableDictionary<long, Entity>;

    public class Entity 
    {
        private readonly long _id;
        private readonly EntityType _entityType;
        private readonly string _name;
        private readonly IAttributeStore _attributes;
        private readonly IFlagStore _flags;
        private readonly IChildStore _children;


        private Entity(
            long id,
            EntityType entityType,
            string name,
            IAttributeStore attributes,
            IFlagStore flags,
            IChildStore children)
        {
            Debug.Assert(Enum.IsDefined(typeof(EntityType), entityType));
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(attributes != null);
            Debug.Assert(flags != null);
            Debug.Assert(children != null);

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

        public IAttributeStore Attributes
        {
            get { return _attributes; }
        }

        public IFlagStore Flags
        {
            get { return _flags; }
        }

        public IChildStore Children
        {
            get { return _children; }
        }

        public static Entity Create(long id, EntityType entityType, string name)
        {
            return Create(
                id,
                entityType,
                name,
                ImmutableDictionary<EntityAttributes, int>.Empty,
                ImmutableDictionary<EntityFlags, bool>.Empty,
                ImmutableDictionary<long, Entity>.Empty);
        }

        public static Entity Create(
            long id,
            EntityType entityType,
            string name,
            IAttributeStore attributes,
            IFlagStore flags,
            IChildStore children)
        {
            return new Entity(id, entityType, name, attributes, flags, children);
        }

        public Entity With(
            long? id = null,
            EntityType? entityType = null,
            string name = null,
            IAttributeStore attributes = null,
            IFlagStore flags = null,
            IChildStore children = null)
        {
            return 
                !id.HasValue &&
                !entityType.HasValue &&
                name == null &
                attributes == null && 
                flags == null && 
                children == null
                ? this // Minimise object churn
                : new Entity(
                    id ?? _id,
                    entityType ?? _entityType,
                    name ?? _name,
                    attributes ?? _attributes,
                    flags ?? _flags,
                    children ?? _children);
        }
    }
}
