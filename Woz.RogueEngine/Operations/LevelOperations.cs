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

using System.Collections.Immutable;
using System.Diagnostics;
using Woz.Core.Geometry;
using Woz.Immutable.Collections;
using Woz.Lenses;
using Woz.Monads.MaybeMonad;
using Woz.RogueEngine.State;
using Woz.RogueEngine.State.Lenses;
using Woz.RogueEngine.Validators;

namespace Woz.RogueEngine.Operations
{
    using ITileStore = IImmutableGrid<Tile>;
    using IActorStateStore = IImmutableDictionary<long, ActorState>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    public static class LevelOperations
    {
        #region Level State Operations
        public static Level SpawnActor(
            this Level level, Actor actor, Vector location)
        {
            Debug.Assert(level != null);
            Debug.Assert(actor != null);
            Debug.Assert(level.CanSpawnActor(actor.Id, location).IsValid);

            var actorId = actor.Id;
            var actorState = ActorState.Create(actor, location);

            return level
                .AddActorState(actorState)
                .AddActorIdToTile(location, actorId);
        }

        public static Level MoveActor(
            this Level level, long actorId, Vector newLocation)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.CanMoveActor(actorId, newLocation).IsValid);

            var actorLocationLens = ActorLocation(actorId);
            var oldLocation = level.Get(actorLocationLens);

            return level
                .Set(actorLocationLens, newLocation)
                .RemoveActorIdFromTile(oldLocation)
                .AddActorIdToTile(newLocation, actorId);
        }

        public static Level ActorTakeItem(
            this Level level, long actorId, Vector itemLocation, long itemId)
        {
            Debug.Assert(level != null);

            var item = level.Get(TileThing(itemLocation, itemId));

            return level
                .RemoveThingFromTile(itemLocation, itemId)
                .AddThingToActor(actorId, item);
        }

        //public static ILevel ActorDropItem(
        //    this ILevel level, long actorId, long itemId, Location dropLocation)
        //{
        //    Debug.Assert(level != null);
        //    Debug.Assert(level.ActorStates.ContainsKey(actorId));
        //    Debug.Assert(level.Tiles.IsValidLocation(dropLocation));

        //    var actorLocation = level.ActorStates[actorId].Location;

        //    Debug.Assert(level.Tiles[actorLocation].Children.ContainsKey(actorId));
        //    Debug.Assert(level.Tiles[actorLocation].Children[itemId].Children.ContainsKey(itemId));

        //    var OperationItem = level.Tiles[actorLocation].Children[itemId].Children[itemId];

        //    Func<IEntity, IEntity> removeItemfromActor =
        //        tile => tile.EditEntityChild(actorId, actor => actor.AddEntityChild(OperationItem));

        //    return level.With(
        //        level.Tiles
        //            .EditTile(actorLocation, removeItemfromActor)
        //            .AddTileChild(OperationItem, dropLocation));
        //}
        #endregion

        #region Atomic Operations
        public static Level AddActorState(
            this Level level, ActorState actorState)
        {
            var actorId = actorState.Actor.Id;
            var lens = LevelLens.ActorStates.ByKey(actorId);

            return level.Set(lens, actorState);
        }

        public static Level AddThingToActor(
            this Level level, long actorId, Thing thing)
        {
            return level.Set(ActorThing(actorId, thing.Id), thing);
        }

        public static Level RemoveThingFromActor(
            this Level level, long actorId, long thingId)
        {
            return level.RemoveByKey(ActorThings(actorId), thingId);
        }

        public static Level AddActorIdToTile(
            this Level level, Vector location, long actorId)
        {
            return level.Set(TileActorId(location), actorId.ToSome());
        }

        public static Level RemoveActorIdFromTile(
            this Level level, Vector location)
        {
            return level.Set(TileActorId(location), Maybe<long>.None);
        }

        public static Level AddThingToTile(
            this Level level, Vector location, Thing thing)
        {
            return level.Set(TileThing(location, thing.Id), thing);
        }

        public static Level RemoveThingFromTile(
            this Level level, Vector location, long thingId)
        {
            return level.RemoveByKey(TileThings(location), thingId);
        }
        #endregion

        #region Lenses
        public static Lens<Level, Vector> ActorLocation(long actorId)
        {
            return LevelLens
                .ActorStates.ByKey(actorId)
                .With(ActorStateLens.Location);
        }

        public static Lens<Level, IMaybe<long>> TileActorId(Vector location)
        {
            return LevelLens
                .Tiles.Location(location)
                .With(TileLens.ActorId);
        }

        public static Lens<Level, IThingStore> ActorThings(long actorId)
        {
            return LevelLens
                .ActorStates.ByKey(actorId)
                .With(ActorStateLens.Actor)
                .With(ActorLens.Things);
        }

        public static Lens<Level, Thing> ActorThing(long actorId, long thingId)
        {
            return ActorThings(actorId).ByKey(thingId);
        }

        public static Lens<Level, IThingStore> TileThings(Vector location)
        {
            return LevelLens
                .Tiles.Location(location)
                .With(TileLens.Things);
        }

        public static Lens<Level, Thing> TileThing(
            Vector location, long thingId)
        {
            return TileThings(location).ByKey(thingId);
        }
        #endregion
    }
}