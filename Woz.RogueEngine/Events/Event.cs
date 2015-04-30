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
using System.Diagnostics;
using Woz.Monads.MaybeMonad;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Events
{
    public sealed class Event
    {
        private readonly Level _level;
        private readonly EventTypes _eventType;
        private readonly IMaybe<EventDetails> _details;

        private Event(
            Level level,
            EventTypes eventType, 
            IMaybe<EventDetails> details)
        {
            Debug.Assert(level != null);
            Debug.Assert(details != null);

            _level = level;
            _eventType = eventType;
            _details = details;
        }

        public static Event Create(
            Level level,
            EventTypes eventType)
        {
            return new Event(
                level,
                eventType,
                Maybe<EventDetails>.None);
        }

        public static Event Create(
            Level level,
            EventTypes eventType,
            EventDetails details)
        {
            Debug.Assert(details != null);

            return new Event(
                level, 
                eventType, 
                details.ToSome());
        }

        public Level Level
        {
            get { return _level; }
        }

        public EventTypes EventType
        {
            get { return _eventType; }
        }

        public IMaybe<EventDetails> Details
        {
            get { return _details; }
        }
    }
}