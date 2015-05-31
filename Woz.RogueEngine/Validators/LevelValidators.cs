#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.RogueEngine.
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
using Woz.Monads;
using Woz.Monads.ValidationMonad;
using Woz.RogueEngine.State;

namespace Woz.RogueEngine.Validators
{
    public static class LevelValidators
    {
        public static IValidation<Unit> 
            IsValidMove(this Level level, Vector location)
        {
            return
                from tile in level.IsValidLocation(location)
                from tileType in tile.IsValidMoveTileType()
                from thingType in tile.IsValidMoveTileThings()
                from noActor in tile.IsValidMoveNoActor()
                select Unit.Value;
        }

        public static IValidation<Unit>
            BlocksLineOfSight(this Level level, Vector location)
        {
            return
                from tile in level.IsValidLocation(location)
                from validMove in tile.BlocksLineOfSightTileType()
                select Unit.Value;
        }

        public static IValidation<Unit> IsNewActor(
            this Level level, long actorId)
        {
            return level.ActorStates.ContainsKey(actorId)
                ? string.Format(
                    "Actor Id={0} already exists",
                    actorId).ToInvalid<Unit>()
                : Unit.Value.ToValid();
        }

        public static IValidation<ActorState> ActorStateExists(
            this Level level, long actorId)
        {
            return level.ActorStates.ContainsKey(actorId)
                ? level.ActorStates[actorId].ToValid()
                : string.Format(
                    "Unknow actor Id={0}",
                    actorId).ToInvalid<ActorState>();

        }

        public static IValidation<Tile> IsValidLocation(
            this Level level, Vector location)
        {
            return level.Tiles.IsValidLocation(location)
                ? level.Tiles[location].ToValid()
                : string.Format(
                    "Location {0} is outside the map",
                    location).ToInvalid<Tile>();
        }
    }
}