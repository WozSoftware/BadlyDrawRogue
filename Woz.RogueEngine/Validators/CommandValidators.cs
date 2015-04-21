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

using Woz.Core.Geometry;
using Woz.Monads.ValidationMonad;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Validators
{
    public static class CommandValidators
    {
        public static IValidation<Level> CanSpawnActor(
            this Level level, long actorId, Coordinate location)
        {
            return
                from isNew in level.IsNewActor(actorId)
                from targetTile in level.IsValidLocation(location)
                from validMove in targetTile.IsValidMove()
                select level;
        }

        public static IValidation<Level> CanMoveActor(
            this Level level, long actorId, Coordinate newLocation)
        {
            return
                from actorState in level.ActorStateExists(actorId)
                from actorTile in level.IsValidLocation(actorState.Location)
                from targetTile in level.IsValidLocation(newLocation)
                from withinRange in actorState.IsWithinMoveRange(newLocation)
                from actor in actorTile.HasActor(actorId)
                from validMove in targetTile.IsValidMove()
                select level;
        }
    }
}