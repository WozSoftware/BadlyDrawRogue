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
using Woz.Functional;
using Woz.Functional.Monads.StateMonad;
using Woz.Immutable.Collections;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    using ITileStore = IImmutableGrid<IEntity>;
    using IActorStateStore = IImmutableDictionary<long, IActorState>;

    public static class LevelStateEdit
    {
        #region Level State Operations
        public static State<ILevel, Unit> CreateSpawnActorOperation(
            Point location, IEntity actor)
        {
            var actorState = ActorState.Create(actor.Id, location);
            return State.Modify<ILevel>(level =>
                level.With(
                    AddTileChild(location, actor).Exec(level.Tiles),
                    AddActorState(actorState).Exec(level.ActorStates)));
        }

        //public static State<ILevel, Unit> MoveActor(
        //    Point location, long actorId)
        //{
        //    return
        //        level =>
        //        {
        //            var oldLocation = level.ActorStates[actorId].Location;
        //            return StateResult.Create(
        //                level.With(
        //                    TileStore(location, actor)
        //                        .Exec(level.Tiles),
        //                    EditActorStateLocation(actor.Id, location)
        //                        .Exec(level.ActorStates)));
        //        };
        //}

        public static State<ILevel, Unit> CreateSpawnTileChildOperation(
            Point location, IEntity entity)
        {
            return State.Modify<ILevel>(level =>
                level.With(AddTileChild(location, entity).Exec(level.Tiles)));
        }

        public static State<ILevel, Unit> CreateMoveTileChildOperation(
            Point oldLocation, Point newLocation, long id)
        {
            var operation =
                from entity in RemoveTileChild(oldLocation, id)
                from _ in AddTileChild(newLocation, entity)
                select Unit.Value;

            return State.Modify<ILevel>(level =>
                level.With(operation.Exec(level.Tiles)));
        }
        #endregion

        #region ActorStateStore State Manipulation
        private static State<IActorStateStore, Unit>
            AddActorState(IActorState actorState)
        {
            return State.Modify<IActorStateStore>(actorStateStore => 
                actorStateStore.Add(actorState.ActorId, actorState));
        }

        private static State<IActorStateStore, Unit>
            EditActorStateLocation(long actorId, Point location)
        {
            return State.Modify<IActorStateStore>(actorStateStore =>
                actorStateStore.SetItem(
                    actorId,
                    actorStateStore[actorId].With(location: location)));
        }

        private static State<IActorStateStore, IActorState>
            RemoveActorState(long actorId)
        {
            return actorStateStore =>
            {
                var actorState = actorStateStore[actorId];
                return StateResult.Create(
                    actorStateStore.Remove(actorId),
                    actorState);
            };
        }
        #endregion

        #region TileStore State Manipulation
        private static State<ITileStore, Unit> 
            AddTileChild(Point location, IEntity entity)
        {
            return EditTile(location, AddEntityChild(entity));
        }

        private static State<ITileStore, Unit>
            MoveTileChild(Point oldLocation, Point newLocation, long id)
        {
            return
                from entity in RemoveTileChild(oldLocation, id)
                from _ in AddTileChild(newLocation, entity)
                select Unit.Value;
        }

        private static State<ITileStore, IEntity>
            RemoveTileChild(Point location, long id)
        {
            return EditTile(location, RemoveEntityChild(id));
        }

        private static State<ITileStore, T> 
            EditTile<T>(Point location, State<IEntity, T> editor)
        {
            return tileStore =>
            {
                var tileResult = editor(tileStore[location]);
                return StateResult.Create(
                    tileStore.Set(location, tileResult.State),
                    tileResult.Value);
            };
        }
        #endregion

        #region Entity State Manipulation
        private static State<IEntity, Unit> AddEntityChild(IEntity child)
        {
            return State.Modify<IEntity>(entity => 
                entity.With(children: entity.Children.Add(child.Id, child)));
        }

        private static State<IEntity, IEntity> RemoveEntityChild(long id)
        {
            return entity =>
            {
                var child = entity.Children[id];
                return StateResult.Create(
                    entity.With(children: entity.Children.Remove(id)),
                    child);
            };
        }
        #endregion
    }
}