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
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Operations
{
    using ChildStore = IImmutableDictionary<long, IEntity>;

    public static class EntityOperations
    {
        public static IEntity AddChild(this IEntity entity, IEntity thing)
        {
            return entity.EditChild(x => x.Add(thing.Id, thing));
        }

        public static IEntity UpdateChild(this IEntity entity, IEntity thing)
        {
            return entity.EditChild(x => x.SetItem(thing.Id, thing));
            
        }

        public static IEntity RemoveChild(this IEntity entity, long thingId)
        {
            return entity.EditChild(x => x.Remove(thingId));
        }

        public static IEntity EditChild(
            this IEntity entity, Func<ChildStore, ChildStore> childEditor)
        {
            return entity.With(children: childEditor(entity.Children));
        }
    }
}