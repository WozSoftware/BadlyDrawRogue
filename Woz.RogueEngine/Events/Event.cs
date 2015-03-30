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

using System.Diagnostics;
using Woz.Monads.MaybeMonad;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Events
{
    public sealed class Event
    {
        public readonly Level Level;
        public readonly EventTypes EventType;
        public readonly IMaybe<long> ActorId; 
        public readonly IMaybe<EventTarget> Target;

        private Event(
            Level level,
            EventTypes eventType,
            IMaybe<long> actorId, 
            IMaybe<EventTarget> target)
        {
            Level = level;
            EventType = eventType;
            ActorId = actorId;
            Target = target;
        }

        public static Event Create(
            Level level,
            EventTypes eventType)
        {
            return Create(
                level, 
                eventType, 
                Maybe<long>.None, 
                Maybe<EventTarget>.None);
        }

        public static Event Create(
            Level level,
            EventTypes eventType,
            long actorId,
            EventTarget target)
        {
            Debug.Assert(target != null);

            return new Event(level, eventType, actorId.ToSome(), target.ToSome());
        }

        public static Event Create(
            Level level,
            EventTypes eventType,
            IMaybe<long> actorId,
            IMaybe<EventTarget> target)
        {
            return new Event(level, eventType, actorId, target);
        }
    }
}