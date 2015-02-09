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
    public interface IEntity
    {
        long Id { get; }
        EntityType EntityType { get; }
        string Name { get; }
        IImmutableDictionary<EntityAttributes, int> Attributes { get; }
        IImmutableDictionary<EntityFlags, bool> Flags { get; }
        IImmutableDictionary<long, IEntity> Children { get; }

        IEntity Set(
            IImmutableDictionary<EntityAttributes, int> attributes = null,
            IImmutableDictionary<EntityFlags, bool> flags = null,
            IImmutableDictionary<long, IEntity> children = null);
    }
}