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
            return level.With(tiles: level.Tiles.SetTile(location, tile));
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
            // Remove Item from tile
            var itemTile = level.GetTile(itemLocation);
            var item = itemTile.Children[itemId];
            var newTile = itemTile.RemoveChild(itemId);
            var newTiles = level
                .Tiles
                .SetTile(itemLocation, newTile);

            // Add item to actor
            var actorState = level.ActorStates[actorId];
            var newActor = actorState.Actor.AddChild(item);
            var newActorState = actorState.With(actor: newActor);
            var newActorStates = level
                .ActorStates
                .SetActorState(newActorState);

            return level.With(actorStates: newActorStates, tiles: newTiles);
        }

        public static ILevel ActorDropItem(
            this ILevel level, long actorId, long itemId)
        {
            // Remove item from actor
            var actorState = level.ActorStates[actorId];
            var item = actorState.Actor.Children[itemId];
            var newActor = actorState.Actor.RemoveChild(itemId);
            var newActorState = actorState.With(actor: newActor);
            var newActorStates = level
                .ActorStates
                .SetActorState(newActorState);

            // Add item to tile
            var actorLocation = newActorState.Location;
            var itemTile = level.GetTile(actorLocation);
            var newTile = itemTile.AddChild(item);
            var newTiles = level
                .Tiles
                .SetTile(actorLocation, newTile);

            return level.With(actorStates: newActorStates, tiles: newTiles);
        }

        public static ILevel ItemSpawn(
            this ILevel level, Point location, IEntity item)
        {
            var newTile = level.GetTile(location).AddChild(item);

            return level.SetTile(location, newTile);
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

        private static TileStore SetTile(
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