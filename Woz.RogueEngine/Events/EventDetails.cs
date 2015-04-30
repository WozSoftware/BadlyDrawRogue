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
using Woz.Core.Geometry;
using Woz.Monads.MaybeMonad;

namespace Woz.RogueEngine.Events
{
    public sealed class EventDetails
    {
        private readonly long _actorId;
        private readonly TargetTypes _targetType;
        private readonly Vector _location;
        private readonly IMaybe<long> _targetId;

        private EventDetails(
            long actorId, 
            TargetTypes targetType,
            Vector location, 
            IMaybe<long> targetId)
        {
            _actorId = actorId;
            _targetType = targetType;
            _location = location;
            _targetId = targetId;
        }

        public static EventDetails Create(
            long actorId,
            TargetTypes targetType,
            Vector location)
        {
            return new EventDetails(
                actorId, targetType, location, Maybe<long>.None);
        }

        public static EventDetails Create(
            long actorId,
            TargetTypes targetType,
            Vector location,
            IMaybe<long> targetId)
        {
            return new EventDetails(actorId, targetType, location, targetId);
        }

        public long ActorId
        {
            get { return _actorId; }
        }

        public TargetTypes TargetType
        {
            get { return _targetType; }
        }

        public Vector Location
        {
            get { return _location; }
        }

        public IMaybe<long> TargetId
        {
            get { return _targetId; }
        }
    }
}