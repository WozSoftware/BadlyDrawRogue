﻿#region License
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
using Woz.Functional.Error;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Queries;

namespace Woz.RogueEngine.Validation
{
    public static class TileRules
    {
        public static Error<IEntity> RuleIsTile(this IEntity entity)
        {
            return entity.EntityType == EntityType.Tile
                ? entity.ToSuccees()
                : "Entity is not a tile".ToError<IEntity>();
        }

        public static Error<IEntity> RuleIsTileType(this IEntity entity, TileTypes tileType)
        {
            return entity.RuleAttributeHasEnumValue(
                EntityAttributes.TileType,
                tileType,
                () => string.Format("Entity is not a {0}", tileType));
        }

        public static Error<IEntity> RuleCanOpenDoor(this IEntity entity)
        {
            return entity.HasFlagSet(EntityFlags.IsOpen)
                ? entity.ToSuccees()
                : "The door is already open".ToError<IEntity>();
        }

        public static Error<IEntity> RuleCanCloseDoor(this IEntity entity)
        {
            return !entity.HasFlagSet(EntityFlags.IsOpen)
                ? entity.ToSuccees()
                : "The door is already closed".ToError<IEntity>();
        }
    }
}