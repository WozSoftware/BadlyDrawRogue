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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    public class ActorManager : IActorManager
    {
        private readonly IImmutableDictionary<long, IActorState> _actorStates;
        private readonly IImmutableDictionary<Point, long> _locationMap;

        private ActorManager(IEnumerable<IActorState> actorStates)
        {
            Debug.Assert(actorStates != null);

            var cached = actorStates.ToArray();

            _actorStates = cached
                .ToImmutableDictionary(x => x.Actor.Id);

            _locationMap = cached
                .ToImmutableDictionary(x => x.Location, x => x.Actor.Id);
        }

        private ActorManager(
            IImmutableDictionary<long, IActorState> actorStates, 
            IImmutableDictionary<Point, long> locationMap)
        {
            Debug.Assert(actorStates != null);
            Debug.Assert(locationMap != null);

            _actorStates = actorStates;
            _locationMap = locationMap;
        }

        public static IActorManager Build()
        {
            return new ActorManager(
                ImmutableDictionary<long, IActorState>.Empty,
                ImmutableDictionary<Point, long>.Empty);
        }

        public static IActorManager Build(IEnumerable<IActorState> actorStates)
        {
            return new ActorManager(actorStates);
        }

        public IActorState GetActorState(long actorId)
        {
            return _actorStates[actorId];
        }

        public IActorState GetActorState(Point location)
        {
            return GetActorState(_locationMap[location]);
        }

        public IActorManager Add(IActorState actorState)
        {
            Debug.Assert(actorState != null);

            return new ActorManager(
                _actorStates.Add(actorState.Actor.Id, actorState),
                _locationMap.Add(actorState.Location, actorState.Actor.Id));
        }

        public IActorManager Remove(long actorId)
        {
            var actorState = GetActorState(actorId);
            return new ActorManager(
                _actorStates.Remove(actorState.Actor.Id),
                _locationMap.Remove(actorState.Location));
        }

        public IActorManager EditActor(long actorId, Func<IEntity, IEntity> actorEditor)
        {
            Debug.Assert(actorEditor != null);

            return EditActorState(
                actorId, x => x.With(actor: actorEditor(x.Actor)));
        }

        public IActorManager EditActorState(
            long actorId,
            Func<IActorState, IActorState> actorStateEditor)
        {
            Debug.Assert(actorStateEditor != null);

            var actorState = actorStateEditor(GetActorState(actorId));

            return new ActorManager(
                _actorStates.SetItem(actorState.Actor.Id, actorState),
                _locationMap.SetItem(actorState.Location, actorState.Actor.Id));
        }

        public IEnumerator<IActorState> GetEnumerator()
        {
            return _actorStates.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}