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
using System.Diagnostics;
using System.Drawing;
using Woz.Immutable.Collections;
using Woz.Lenses;
using Woz.Monads.MaybeMonad;
using Woz.RogueEngine.DebugHelpers;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Operations
{
    using ITileStore = IImmutableGrid<Entity>;
    using IActorStateStore = IImmutableDictionary<long, ActorState>;

    public static class LevelOperations
    {
        #region Level State Operations
        public static Level SpawnActor(
            this Level level, Entity actor, Point location)
        {
            Debug.Assert(level != null);
            Debug.Assert(actor != null);
            Debug.Assert(actor.IsValid(EntityType.Actor));
            Debug.Assert(!level.ActorStates.ContainsKey(actor.Id));
            Debug.Assert(level.Tiles.IsValidLocation(location));

            var actorState = ActorState.Create(actor.Id, location);

            return level
                .Set(ActorLookup(location, actor.Id), actor.ToMaybe())
                .Set(ActorStateLookup(actor.Id), actorState);
        }

        public static Level MoveActor(
            this Level level, long actorId, Point newLocation)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.ActorStates.ContainsKey(actorId));
            Debug.Assert(level.Tiles.IsValidLocation(newLocation));

            var actorLocation = ActorLocationBy(actorId);
            var oldLocation = level.Get(actorLocation);

            var oldTile = ActorLookup(oldLocation, actorId);
            var newTile = ActorLookup(newLocation, actorId);

            var actor = level.Get(oldTile);

            return level
                .Set(oldTile, Maybe<Entity>.None)
                .Set(newTile, actor)
                .Set(actorLocation, newLocation);
        }

        //public static Level ActorTakeItem(
        //    this Level level, long actorId, long itemId, Point itemLocation)
        //{
        //    Debug.Assert(level != null);
        //    Debug.Assert(level.ActorStates.ContainsKey(actorId));
        //    Debug.Assert(level.Tiles.IsValidLocation(itemLocation));
        //    Debug.Assert(level.Tiles[itemLocation].Children.ContainsKey(itemId));

        //    var actorLocation = level.ActorStates[actorId].Location;
        //    var item = level.Tiles[itemLocation].Children[itemId];

        //    Func<IEntity, IEntity> addItemToActor =
        //        tile => tile.EditEntityChild(actorId, actor => actor.AddEntityChild(item));

        //    return level.With(
        //        level.Tiles
        //            .RemoveTileChild(itemId, itemLocation)
        //            .EditTile(actorLocation, addItemToActor));
        //}

        //public static ILevel ActorDropItem(
        //    this ILevel level, long actorId, long itemId, Point dropLocation)
        //{
        //    Debug.Assert(level != null);
        //    Debug.Assert(level.ActorStates.ContainsKey(actorId));
        //    Debug.Assert(level.Tiles.IsValidLocation(dropLocation));

        //    var actorLocation = level.ActorStates[actorId].Location;

        //    Debug.Assert(level.Tiles[actorLocation].Children.ContainsKey(actorId));
        //    Debug.Assert(level.Tiles[actorLocation].Children[itemId].Children.ContainsKey(itemId));

        //    var item = level.Tiles[actorLocation].Children[itemId].Children[itemId];

        //    Func<IEntity, IEntity> removeItemfromActor =
        //        tile => tile.EditEntityChild(actorId, actor => actor.AddEntityChild(item));

        //    return level.With(
        //        level.Tiles
        //            .EditTile(actorLocation, removeItemfromActor)
        //            .AddTileChild(item, dropLocation));
        //}

        //public static ILevel SpawnTileChild(
        //    this ILevel level, IEntity entity, Point location)
        //{
        //    Debug.Assert(level != null);
        //    Debug.Assert(entity != null);
        //    Debug.Assert(level.Tiles.IsValidLocation(location));

        //    return level.With(level.Tiles.AddTileChild(entity, location));
        //}

        //public static ILevel MoveTileChild(
        //    this ILevel level, long id, Point oldLocation, Point newLocation)
        //{
        //    Debug.Assert(level != null);
        //    Debug.Assert(level.Tiles.IsValidLocation(oldLocation));
        //    Debug.Assert(level.Tiles.IsValidLocation(newLocation));

        //    return level.With(
        //        level.Tiles.MoveTileChild(id, oldLocation, newLocation));
        //}

        //public static ILevel RemoveTileChild(
        //    this ILevel level, long id, Point location)
        //{
        //    Debug.Assert(level != null);
        //    Debug.Assert(level.Tiles.IsValidLocation(location));

        //    return level.With(level.Tiles.RemoveTileChild(id, location));
        //}
        #endregion

        #region Lenses
        public static Lens<Level, IMaybe<Entity>> ActorLookup(Point location, long actorId)
        {
            return LevelLens.Tiles.Location(location).With(EntityLens.Children.Lookup(actorId));
        }

        public static Lens<Level, ActorState> ActorStateLookup(long actorId)
        {
            return LevelLens.ActorStates.ByKey(actorId);
        }

        public static Lens<Level, Point> ActorLocationBy(long actorId)
        {
            return ActorStateLookup(actorId).With(ActorStateLens.Location);
        }
        #endregion
    }
}