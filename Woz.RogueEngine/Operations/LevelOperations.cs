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
using Woz.RogueEngine.Levels;
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
            this Level level, Actor actor, Point location)
        {
            Debug.Assert(level != null);
            Debug.Assert(actor != null);
            Debug.Assert(level.CanSpawnActor(actor.Id, location).IsValid);

            var actorState = ActorState.Create(actor.Id, location);

            return level
                .Set(ActorLookupAt(location), actor.ToSome())
                .Set(ActorStateLookup(actor.Id), actorState.ToSome());
        }

        public static Level MoveActor(
            this Level level, long actorId, Point newLocation)
        {
            Debug.Assert(level != null);
            Debug.Assert(level.CanMoveActor(actorId, newLocation).IsValid);

            var actorLocationLens = ActorLocationByKey(actorId);
            var oldLocation = level.Get(actorLocationLens);

            var actorAtOldLocationLens = ActorLookupAt(oldLocation);
            var actor = level.Get(actorAtOldLocationLens);

            return level
                .Set(actorAtOldLocationLens, Maybe<Actor>.None)
                .Set(ActorLookupAt(newLocation), actor)
                .Set(actorLocationLens, newLocation);
        }

        public static Level ActorTakeItem(
            this Level level, long actorId, long itemId, Point itemLocation)
        {
            Debug.Assert(level != null);

            var actorLocation = level.Get(ActorLocationByKey(actorId));

            var itemTileThingsLens = TileThingsAt(itemLocation);
            var item = level.Get(itemTileThingsLens.ByKey(itemId));

            return level
                .RemoveByKey(itemTileThingsLens, itemId)
                .Set(ActorThingLookup(actorLocation, itemId), item.ToSome());
        }

        //public static ILevel ActorDropItem(
        //    this ILevel level, long actorId, long itemId, Point dropLocation)
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

        #region Lenses
        public static Lens<Level, Tile> TileAt(Point location)
        {
            return LevelLens.Tiles.Location(location);
        }

        public static Lens<Level, IMaybe<Actor>> ActorLookupAt(Point location)
        {
            return TileAt(location).With(TileLens.Actor);
        }

        public static Lens<Level, Actor> ActorAt(Point location)
        {
            return TileAt(location)
                .With(TileLens.Actor)
                .With(MaybeLens<Actor>.ExpectSome);
        }

        public static Lens<Level, IThingStore> ActorThingsAt(Point location)
        {
            return ActorAt(location).With(ActorLens.Things);
        }

        public static Lens<Level, IMaybe<Thing>> ActorThingLookup(Point location, long thingId)
        {
            return ActorThingsAt(location).Lookup(thingId);
        }

        public static Lens<Level, Thing> ActorThingByKey(Point location, long thingId)
        {
            return ActorThingsAt(location).ByKey(thingId);
        }

        public static Lens<Level, IMaybe<ActorState>> ActorStateLookup(long actorId)
        {
            return LevelLens.ActorStates.Lookup(actorId);
        }

        public static Lens<Level, ActorState> ActorStateByKey(long actorId)
        {
            return LevelLens.ActorStates.ByKey(actorId);
        }

        public static Lens<Level, Point> ActorLocationByKey(long actorId)
        {
            return ActorStateByKey(actorId).With(ActorStateLens.Location);
        }

        public static Lens<Level, IThingStore> TileThingsAt(Point location)
        {
            return TileAt(location).With(TileLens.Things);
        }

        public static Lens<Level, IMaybe<Thing>> TileThingLookup(Point location, long thingId)
        {
            return TileThingsAt(location).Lookup(thingId);
        }

        public static Lens<Level, Thing> TileThingByKey(Point location, long thingId)
        {
            return TileThingsAt(location).ByKey(thingId);
        }
        #endregion
    }
}