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
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Operations
{
    using IActorStore = IImmutableDictionary<long, IActorState>;

    public static class ActorStoreOperations
    {
        public static IActorStore EditActor(
            this IActorStore actorStore,
            long actorId,
            Func<IEntity, IEntity> actorEditor)
        {
            Debug.Assert(actorStore != null);
            Debug.Assert(actorId > 0);
            Debug.Assert(actorEditor != null);

            var actorState = actorStore[actorId];

            return actorStore
                .SetActorState(actorState
                    .With(actor: actorEditor(actorState.Actor)));
        }

        public static IActorStore SetActorState(
            this IActorStore actorStore, IActorState actorState)
        {
            Debug.Assert(actorStore != null);
            Debug.Assert(actorState != null);

            return actorStore
                .SetItem(actorState.Actor.Id, actorState);
        }

        public static IActorStore SetActorLocation(
            this IActorStore actorStore, long actorId, Point location)
        {
            Debug.Assert(actorStore != null);
            Debug.Assert(actorId > 0);
            Debug.Assert(location.X > 0 && location.Y > 0);

            return actorStore.SetActorState(
                actorStore[actorId].With(location: location));
        }

    }
}