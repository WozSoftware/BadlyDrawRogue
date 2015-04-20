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

using Woz.Core.Coordinates;
using Woz.Monads.MaybeMonad;

namespace Woz.RogueEngine.Events
{
    public sealed class EventDetails
    {
        public readonly long ActorId;
        public readonly TargetTypes TargetType;
        public readonly Coordinate Location;
        public readonly IMaybe<long> TargetId;

        private EventDetails(
            long actorId, 
            TargetTypes targetType, 
            Coordinate location, 
            IMaybe<long> targetId)
        {
            ActorId = actorId;
            TargetType = targetType;
            Location = location;
            TargetId = targetId;
        }

        public static EventDetails Create(
            long actorId,
            TargetTypes targetType,
            Coordinate location)
        {
            return new EventDetails(
                actorId, targetType, location, Maybe<long>.None);
        }

        public static EventDetails Create(
            long actorId,
            TargetTypes targetType,
            Coordinate location,
            IMaybe<long> targetId)
        {
            return new EventDetails(actorId, targetType, location, targetId);
        }
    }
}