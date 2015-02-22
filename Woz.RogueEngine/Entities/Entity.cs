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

namespace Woz.RogueEngine.Entities
{
    using AttributeStore = IImmutableDictionary<EntityAttributes, int>;
    using FlagStore = IImmutableDictionary<EntityFlags, bool>;
    using ChildStore = IImmutableDictionary<long, IEntity>;

    public class Entity : IEntity
    {
        private readonly long _id;
        private readonly EntityType _entityType;
        private readonly string _name;
        private readonly AttributeStore _attributes;
        private readonly FlagStore _flags;
        private readonly ChildStore _children;

        private Entity(
            long id,
            EntityType entityType,
            string name,
            AttributeStore attributes,
            FlagStore flags,
            ChildStore children)
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

        public AttributeStore Attributes
        {
            get { return _attributes; }
        }

        public FlagStore Flags
        {
            get { return _flags; }
        }

        public ChildStore Children
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
                ImmutableDictionary<long, IEntity>.Empty);
        }

        public static IEntity Build(
            long id,
            EntityType entityType,
            string name,
            AttributeStore attributes,
            FlagStore flags,
            ChildStore children)
        {
            return new Entity(id, entityType, name, attributes, flags, children);
        }

        public IEntity With(
            AttributeStore attributes = null,
            FlagStore flags = null,
            ChildStore children = null)
        {
            return attributes == null && flags == null && children == null
                ? this // Minimise object churn
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
