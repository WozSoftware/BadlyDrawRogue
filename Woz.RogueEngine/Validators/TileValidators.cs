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

using System.Linq;
using Woz.Monads.ValidationMonad;
using Woz.RogueEngine.Levels;
using Woz.RogueEngine.Rules;

namespace Woz.RogueEngine.Validators
{
    public static class TileValidators
    {
        public static IValidation<Tile> IsValidMove(this Tile tile)
        {
            return !TypeGroups.BlockMovement.Contains(tile.TileType)
                ? tile.ToValid()
                : "Can't move there".ToInvalid<Tile>();
        }

        public static IValidation<Tile> HasNoActor(this Tile tile)
        {
            return tile.Actor.HasValue
                ? tile.ToValid()
                : string.Format(
                    "Tile already contains {0}", 
                    tile.Actor.Value.Name).ToInvalid<Tile>();
        }

        public static IValidation<Tile> HasActor(this Tile tile)
        {
            return tile.Actor.HasValue
                ? tile.ToValid()
                : "No actor present in the tile".ToInvalid<Tile>();
        }

        public static IValidation<Tile> HasActor(
            this Tile tile, long actorId)
        {
            return tile
                .Actor
                .Select(x => x.Id == actorId)
                .OrElse(false)
                ? tile.ToValid()
                : string.Format(
                    "Actor {0} not present in the tile",
                    actorId).ToInvalid<Tile>();
        }
    }
}