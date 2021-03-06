﻿#region License
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

using Woz.RogueEngine.Events;
using Woz.RogueEngine.State;

namespace Woz.RogueEngine.Commands
{
    public sealed class CommandResult
    {
        private readonly Level _level;
        private readonly Event _commandEvent;

        private CommandResult(Level level, Event commandEvent)
        {
            _level = level;
            _commandEvent = commandEvent;
        }

        public static CommandResult Create(Level level, Event commandEvent)
        {
            return new CommandResult(level, commandEvent);
        }

        public Level Level
        {
            get { return _level; }
        }

        public Event CommandEvent
        {
            get { return _commandEvent; }
        }
    }
}