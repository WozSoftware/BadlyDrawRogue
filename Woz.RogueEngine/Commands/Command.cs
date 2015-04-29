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
using System;
using Woz.Monads.ValidationMonad;
using Woz.RogueEngine.Events;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Commands
{
    public sealed class Command
    {
        public readonly Func<Level, IValidation<Level>> Validator;
        public readonly Func<Level, IValidation<CommandResult>> Operation;

        private Command(
            Func<Level, IValidation<Level>> validator,
            Func<Level, IValidation<CommandResult>> operation)
        {
            Validator = validator;
            Operation = operation;
        }

        public static Command Create(
            Func<Level, IValidation<Level>> validator,
            Func<Level, Level> operation,
            Func<Level, Event> eventBuilder)
        {
            Func<Level, CommandResult> resultBuilder =
                level => CommandResult.Create(level, eventBuilder(level));

            return new Command(
                validator, 
                level => validator(level)
                    .Select(operation)
                    .Select(resultBuilder));
        }
    }
}