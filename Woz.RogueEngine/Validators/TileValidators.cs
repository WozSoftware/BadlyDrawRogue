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
using System.Linq;
using Woz.Monads;
using Woz.Monads.ValidationMonad;
using Woz.RogueEngine.State;
using Woz.RogueEngine.Validators.Rules;

namespace Woz.RogueEngine.Validators
{
    public static class TileValidators
    {
        #region Movement
        public static IValidation<Unit> IsValidMoveTileType(this Tile tile)
        {
            return TileTypeRules.BlockMovement.Contains(tile.TileType)
                ? "Can't move there".ToInvalid<Unit>()
                : Unit.Value.ToValid();
        }

        public static IValidation<Unit> IsValidMoveTileThings(this Tile tile)
        {
            return tile.Things.Values.Any(thing =>
                    ThingTypeRules.BlockMovement.Contains(thing.ThingType))
                ? "Can't move there".ToInvalid<Unit>()
                : Unit.Value.ToValid();
        }

        public static IValidation<Unit> IsValidMoveNoActor(this Tile tile)
        {
            return tile.ActorId.HasValue
                ? "Can't move there".ToInvalid<Unit>()
                : Unit.Value.ToValid();
        }
        #endregion

        #region Line Of Sight
        public static IValidation<Unit> BlocksLineOfSightTileType(this Tile tile)
        {
            return TileTypeRules.BlocksLineOfSight.Contains(tile.TileType)
                ? "Can't see through".ToInvalid<Unit>()
                : Unit.Value.ToValid();
        }
        #endregion

        #region Actors
        public static IValidation<long> HasActor(this Tile tile)
        {
            return tile.ActorId.Match(
                some: id => id.ToValid(),
                none: () => "No actor present in the tile".ToInvalid<long>());
        }

        public static IValidation<long> HasActor(
            this Tile tile, long actorId)
        {
            return tile
                .ActorId
                .Where(id => id == actorId)
                .Match(
                    some: id => id.ToValid(),
                    none: () =>
                        string.Format(
                            "Actor {0} not present in the tile",
                            actorId).ToInvalid<long>());
        }
        #endregion
    }
}