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
using System.Drawing;
using Woz.Functional.Monads.MaybeMonad;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Operations
{
    using TileStore = IImmutableDictionary<int, IImmutableDictionary<int, IEntity>>;
    using ActorStore = IImmutableDictionary<long, IActorState>;

    public static class LevelOperations
    {
        #region Operations
        public static IEntity GetTile(
            this ILevel level, Point location)
        {
            return level
                .Tiles
                .Lookup(location.X)
                .SelectMany(x => x.Lookup(location.Y))
                .OrElse(EntityFactory.Void);
        }

        public static ILevel SetTile(
            this ILevel level, Point location, IEntity tile)
        {
            return level.With(tiles: level.Tiles.UpdateTile(location, tile));
        }

        public static ILevel OpenDoor(
            this ILevel level, Point location)
        {
            return level.UpdateDoor(location, true);
        }

        public static ILevel CloseDoor(
            this ILevel level, Point location)
        {
            return level.UpdateDoor(location, false);
        }

        public static ILevel ActorSpawn(
            this ILevel level, Point location, IEntity actor)
        {
            var actorState = ActorState.Build(actor, location);

            return level.SetActorState(actorState);
        }

        public static ILevel ActorMove(
            this ILevel level, long actorId, Point newLocation)
        {
            var newActorState = level
                .ActorStates[actorId]
                .With(location: newLocation);

            return level.SetActorState(newActorState);
        }

        public static ILevel ActorTakeItem(
            this ILevel level, long actorId, Point itemLocation, long itemId)
        {
            var itemTile = level.GetTile(itemLocation);
            var item = itemTile.Children[itemId];

            // Remove Item from tile
            var newTiles = level
                .Tiles
                .UpdateTile(itemLocation, itemTile.RemoveChild(itemId));

            // Add item to actor
            var newActorStates = level
                .ActorStates
                .UpdateActor(actorId, actor => actor.AddChild(item));

            return level.With(actorStates: newActorStates, tiles: newTiles);
        }

        public static ILevel ActorDropItem(
            this ILevel level, long actorId, Point itemLocation, long itemId)
        {
            var item = level.ActorStates[actorId].Actor.Children[itemId];

            // Remove item from actor
            var newActorStates = level
                .ActorStates
                .UpdateActor(actorId, actor => actor.RemoveChild(itemId));

            // Add item to tile
            var itemTile = level.GetTile(itemLocation);
            var newTiles = level
                .Tiles
                .UpdateTile(itemLocation, itemTile.AddChild(item));

            return level.With(actorStates: newActorStates, tiles: newTiles);
        }

        public static ILevel ItemSpawn(
            this ILevel level, Point location, IEntity item)
        {
            return level.SetTile(
                location,
                level.GetTile(location).AddChild(item));
        }
        #endregion

        #region Helpers
        public static ILevel SetActorState(
            this ILevel level, IActorState actorState)
        {
            var newActorStates = level.ActorStates.SetActorState(actorState);
            return level.With(actorStates: newActorStates);
        }

        private static ActorStore SetActorState(
            this ActorStore actorStates, IActorState actorState)
        {
            return actorStates
                .SetItem(actorState.Actor.Id, actorState);
        }

        private static ActorStore UpdateActor(
            this ActorStore actorStates,
            long actorId,
            Func<IEntity, IEntity> actorEditor)
        {
            var actorState = actorStates[actorId];
            var newActor = actorEditor(actorState.Actor);
            var newActorState = actorState.With(actor: newActor);
            return actorStates.SetActorState(newActorState);
        }

        private static TileStore UpdateTile(
            this TileStore tiles, Point location, IEntity tile)
        {
            return tiles
                .SetItem(
                    location.X,
                    tiles[location.X].SetItem(location.Y, tile));
        }

        public static ILevel UpdateDoor(
            this ILevel level, Point location, bool isOpen)
        {
            var doorTile = level.GetTile(location);

            var newFlags = doorTile
                .Flags
                .SetItem(EntityFlags.IsOpen, isOpen)
                .SetItem(EntityFlags.BlocksMovement, !isOpen)
                .SetItem(EntityFlags.BlocksLineOfSight, !isOpen);

            var newDoorTile = doorTile.With(flags: newFlags);

            return level.SetTile(location, newDoorTile);
        }
        #endregion
    }
}