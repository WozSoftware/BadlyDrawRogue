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
using System.Drawing;
using Woz.RogueEngine.DebugHelpers;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Operations
{
    using ITileStore = IImmutableDictionary<int, IImmutableDictionary<int, IEntity>>;
    using IActorStore = IImmutableDictionary<long, IActorState>;

    public static class LevelOperations
    {
        public static ILevel SetTile(
            this ILevel level, Point location, IEntity tile)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.Bounds.Contains(location));
            Debug.Assert(tile.IsValid(EntityType.Tile));

            return level.With(tiles: level.Tiles.SetTile(location, tile));
        }

        public static ILevel EditTile(
            this ILevel level,
            Point location,
            Func<IEntity, IEntity> tileEditor)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.Bounds.Contains(location));
            Debug.Assert(tileEditor != null);

            return level.With(tiles: level.Tiles.EditTile(location, tileEditor));
        }

        public static ILevel OpenDoor(
            this ILevel level, Point location)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.Bounds.Contains(location));

            return level.UpdateDoor(location, true);
        }

        public static ILevel CloseDoor(
            this ILevel level, Point location)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.Bounds.Contains(location));

            return level.UpdateDoor(location, false);
        }

        private static ILevel UpdateDoor(
            this ILevel level, Point location, bool isOpen)
        {
            return level.EditTile(
                location,
                tile => tile.EditFlags(
                    flags => flags
                        .SetItem(EntityFlags.IsOpen, isOpen)
                        .SetItem(EntityFlags.BlocksMovement, !isOpen)
                        .SetItem(EntityFlags.BlocksLineOfSight, !isOpen)));
        }

        public static ILevel ActorSpawn(
            this ILevel level, Point location, IEntity actor)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.Bounds.Contains(location));
            Debug.Assert(actor.IsValid(EntityType.Actor));

            var actorState = ActorState.Build(actor, location);

            return level.With(
                actorStates: level.ActorStates.SetActorState(actorState));
        }

        public static ILevel ActorMove(
            this ILevel level, long actorId, Point location)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.Bounds.Contains(location));

            return level
                .With(actorStates: 
                    level.ActorStates.SetActorLocation(actorId, location));
        }

        public static ILevel ActorTakeItem(
            this ILevel level, long actorId, Point itemLocation, long itemId)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.Bounds.Contains(itemLocation));

            var item = level.Tiles.GetTile(itemLocation).Children[itemId];

            Debug.Assert(item.IsValid(EntityType.Item));

            // Remove Item from tile
            var newTiles = level
                .Tiles
                .EditTile(itemLocation, tile => tile.RemoveChild(itemId));

            // Add item to actor
            var newActorStates = level
                .ActorStates
                .EditActor(actorId, actor => actor.AddChild(item));

            return level.With(actorStates: newActorStates, tiles: newTiles);
        }

        public static ILevel ActorDropItem(
            this ILevel level, long actorId, Point itemLocation, long itemId)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.Bounds.Contains(itemLocation));

            var item = level.ActorStates[actorId].Actor.Children[itemId];

            Debug.Assert(item.IsValid(EntityType.Item));

            // Remove item from actor
            var newActorStates = level
                .ActorStates
                .EditActor(actorId, actor => actor.RemoveChild(itemId));

            // Add item to tile
            var newTiles = level
                .Tiles
                .EditTile(itemLocation, tile => tile.AddChild(item));

            return level.With(actorStates: newActorStates, tiles: newTiles);
        }

        public static ILevel ItemThing(
            this ILevel level, Point location, IEntity item)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.Bounds.Contains(location));
            Debug.Assert(item.IsNot(EntityType.Void));

            return level.EditTile(location, tile => tile.AddChild(item));
        }
    }
}