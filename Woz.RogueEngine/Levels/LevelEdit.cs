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
using Woz.Immutable.Collections;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    using ITileStore = IImmutableGrid<IEntity>;
    using IActorStateStore = IImmutableDictionary<long, IActorState>;

    public static class LevelEdit
    {
        #region Level State Operations
        public static ILevel SpawnActor(
            this ILevel level, Point location, IEntity actor)
        {
            var actorState = ActorState.Create(actor.Id, location);

            return level.With(
                level.Tiles.AddTileChild(location, actor),
                level.ActorStates.Add(actor.Id, actorState));
        }

        public static ILevel MoveActor(
            this ILevel level, Point newlocation, long actorId)
        {
            var oldLocation = level.ActorStates[actorId].Location;
            
            return level.With(
                level.Tiles.MoveTileChild(oldLocation, newlocation, actorId), 
                level.ActorStates.EditActorStateLocation(actorId, newlocation));
        }

        public static ILevel SpawnTileChild(
            this ILevel level, Point location, IEntity entity)
        {
            return level.With(level.Tiles.AddTileChild(location, entity));
        }

        public static ILevel MoveTileChild(
            this ILevel level, Point oldLocation, Point newLocation, long id)
        {
            return level.With(
                level.Tiles.MoveTileChild(oldLocation, newLocation, id));
        }
        #endregion

        #region ActorStateStore State Manipulation
        private static IActorStateStore EditActorStateLocation(
            this IActorStateStore actorStateStore, long actorId, Point location)
        {
            return actorStateStore.SetItem(
                actorId,
                actorStateStore[actorId].With(location: location));
        }
        #endregion

        #region TileStore State Manipulation
        private static ITileStore AddTileChild(
            this ITileStore tileStore, Point location, IEntity entity)
        {
            return tileStore.EditTile(
                location, tile => tile.AddEntityChild(entity));
        }

        public static ITileStore MoveTileChild(
            this ITileStore tileStore, Point oldLocation, Point newLocation, long id)
        {
            var tileChild = tileStore[oldLocation].Children[id];

            return tileStore
                .RemoveTileChild(oldLocation, id)
                .AddTileChild(newLocation, tileChild);
        }

        private static ITileStore RemoveTileChild(
            this ITileStore tileStore, Point location, long id)
        {
            return tileStore.EditTile(
                location, tile => tile.RemoveEntityChild(id));
        }

        private static ITileStore EditTile(
            this ITileStore tileStore, Point location, Func<IEntity, IEntity> editor)
        {
            return tileStore.Set(location, editor(tileStore[location]));
        }
        #endregion

        #region Entity State Manipulation
        private static IEntity AddEntityChild(
            this IEntity entity, IEntity child)
        {
            return entity.With(children: entity.Children.Add(child.Id, child));
        }

        private static IEntity RemoveEntityChild(
            this IEntity entity, long id)
        {
            return entity.With(children: entity.Children.Remove(id));
        }
        #endregion
    }
}