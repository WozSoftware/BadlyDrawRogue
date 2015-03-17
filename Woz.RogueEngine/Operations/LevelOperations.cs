//#region License
//// Copyright (C) Woz.Software 2015
//// [https://github.com/WozSoftware/BadlyDrawRogue]
////
//// This file is part of Woz.RoqueEngine.
////
//// Woz.RoqueEngine is free software: you can redistribute it 
//// and/or modify it under the terms of the GNU General Public 
//// License as published by the Free Software Foundation, either 
//// version 3 of the License, or (at your option) any later version.
////
//// This program is distributed in the hope that it will be useful,
//// but WITHOUT ANY WARRANTY; without even the implied warranty of
//// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//// GNU General Public License for more details.
////
//// You should have received a copy of the GNU General Public License
//// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//#endregion

//using System;
//using System.Collections.Immutable;
//using System.Diagnostics;
//using System.Drawing;
//using Woz.Lenses;
//using Woz.Immutable.Collections;
//using Woz.RogueEngine.DebugHelpers;
//using Woz.RogueEngine.Entities;
//using Woz.RogueEngine.Levels;

//namespace Woz.RogueEngine.Operations
//{
//    using ITileStore = IImmutableGrid<IEntity>;
//    using IActorStateStore = IImmutableDictionary<long, IActorState>;

//    public static class LevelOperations
//    {
//        #region Level State Operations
//        public static ILevel SpawnActor(
//            this ILevel level, IEntity actor, Point location)
//        {
//            Debug.Assert(level != null);
//            Debug.Assert(actor != null);
//            Debug.Assert(actor.IsValid(EntityType.Actor));
//            Debug.Assert(!level.ActorStates.ContainsKey(actor.Id));
//            Debug.Assert(level.Tiles.IsValidLocation(location));

//            var actorState = ActorState.Create(actor.Id, location);

//            return level.With(
//                level.Tiles.AddTileChild(actor, location),
//                level.ActorStates.Add(actor.Id, actorState));
//        }

//        public static ILevel MoveActor(
//            this ILevel level, long actorId, Point newLocation)
//        {
//            Debug.Assert(level != null);
//            Debug.Assert(level.ActorStates.ContainsKey(actorId));
//            Debug.Assert(level.Tiles.IsValidLocation(newLocation));

//            var oldLocation = level.ActorStates[actorId].Location;

//            var oldLocationLens = level
//                .Tiles
//                .ToLensLocation(oldLocation)
//                .With(EntityLenses.Children)
//                .With(ImmutableDictionaryLens.ToLensByKey(actorId));

//            var newLocation = level
//            return level.With(
//                level.Tiles.MoveTileChild(actorId, oldLocation, newLocation), 
//                level.ActorStates.EditActorStateLocation(actorId, newLocation));
//        }

//        public static ILevel ActorTakeItem(
//            this ILevel level, long actorId, long itemId, Point itemLocation)
//        {
//            Debug.Assert(level != null);
//            Debug.Assert(level.ActorStates.ContainsKey(actorId));
//            Debug.Assert(level.Tiles.IsValidLocation(itemLocation));
//            Debug.Assert(level.Tiles[itemLocation].Children.ContainsKey(itemId));

//            var actorLocation = level.ActorStates[actorId].Location;
//            var item = level.Tiles[itemLocation].Children[itemId];

//            Func<IEntity, IEntity> addItemToActor =
//                tile => tile.EditEntityChild(actorId, actor => actor.AddEntityChild(item));

//            return level.With(
//                level.Tiles
//                    .RemoveTileChild(itemId, itemLocation)
//                    .EditTile(actorLocation, addItemToActor));
//        }

//        public static ILevel ActorDropItem(
//            this ILevel level, long actorId, long itemId, Point dropLocation)
//        {
//            Debug.Assert(level != null);
//            Debug.Assert(level.ActorStates.ContainsKey(actorId));
//            Debug.Assert(level.Tiles.IsValidLocation(dropLocation));

//            var actorLocation = level.ActorStates[actorId].Location;

//            Debug.Assert(level.Tiles[actorLocation].Children.ContainsKey(actorId));
//            Debug.Assert(level.Tiles[actorLocation].Children[itemId].Children.ContainsKey(itemId));

//            var item = level.Tiles[actorLocation].Children[itemId].Children[itemId];

//            Func<IEntity, IEntity> removeItemfromActor =
//                tile => tile.EditEntityChild(actorId, actor => actor.AddEntityChild(item));

//            return level.With(
//                level.Tiles
//                    .EditTile(actorLocation, removeItemfromActor)
//                    .AddTileChild(item, dropLocation));
//        }

//        public static ILevel SpawnTileChild(
//            this ILevel level, IEntity entity, Point location)
//        {
//            Debug.Assert(level != null);
//            Debug.Assert(entity != null);
//            Debug.Assert(level.Tiles.IsValidLocation(location));

//            return level.With(level.Tiles.AddTileChild(entity, location));
//        }

//        public static ILevel MoveTileChild(
//            this ILevel level, long id, Point oldLocation, Point newLocation)
//        {
//            Debug.Assert(level != null);
//            Debug.Assert(level.Tiles.IsValidLocation(oldLocation));
//            Debug.Assert(level.Tiles.IsValidLocation(newLocation));

//            return level.With(
//                level.Tiles.MoveTileChild(id, oldLocation, newLocation));
//        }

//        public static ILevel RemoveTileChild(
//            this ILevel level, long id, Point location)
//        {
//            Debug.Assert(level != null);
//            Debug.Assert(level.Tiles.IsValidLocation(location));

//            return level.With(level.Tiles.RemoveTileChild(id, location));
//        }
//        #endregion

//        #region ActorStateStore State Manipulation
//        private static IActorStateStore EditActorStateLocation(
//            this IActorStateStore actorStateStore, long actorId, Point location)
//        {
//            Debug.Assert(actorStateStore.ContainsKey(actorId));

//            return actorStateStore.SetItem(
//                actorId,
//                actorStateStore[actorId].With(location: location));
//        }
//        #endregion

//        #region TileStore State Manipulation
//        private static ITileStore AddTileChild(
//            this ITileStore tileStore, IEntity entity, Point location)
//        {
//            return tileStore.EditTile(
//                location, tile => tile.AddEntityChild(entity));
//        }

//        public static ITileStore MoveTileChild(
//            this ITileStore tileStore, long id, Point oldLocation, Point newLocation)
//        {
//            Debug.Assert(tileStore[oldLocation].Children.ContainsKey(id));

//            var tileChild = tileStore[oldLocation].Children[id];

//            return tileStore
//                .RemoveTileChild(id, oldLocation)
//                .AddTileChild(tileChild, newLocation);
//        }

//        private static ITileStore RemoveTileChild(
//            this ITileStore tileStore, long id, Point location)
//        {
//            return tileStore.EditTile(
//                location, tile => tile.RemoveEntityChild(id));
//        }

//        private static ITileStore EditTile(
//            this ITileStore tileStore, Point location, Func<IEntity, IEntity> editor)
//        {
//            return tileStore.Set(location, editor(tileStore[location]));
//        }
//        #endregion

//        #region Entity State Manipulation
//        private static IEntity AddEntityChild(
//            this IEntity entity, IEntity child)
//        {
//            Debug.Assert(entity != null);
//            Debug.Assert(entity.Children.ContainsKey(child.Id));

//            return entity.With(children: entity.Children.Add(child.Id, child));
//        }

//        private static IEntity RemoveEntityChild(
//            this IEntity entity, long id)
//        {
//            Debug.Assert(entity != null);
//            Debug.Assert(entity.Children.ContainsKey(id));

//            return entity.With(children: entity.Children.Remove(id));
//        }

//        private static IEntity EditEntityChild(
//            this IEntity entity,
//            long id,
//            Func<IEntity, IEntity> editor)
//        {
//            return entity.With(children: entity.Children.SetItem(id, editor(entity.Children[id])));
//        }
//        #endregion
//    }
//}