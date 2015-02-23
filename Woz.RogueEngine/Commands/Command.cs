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
using System.Drawing;
using Woz.Functional.Monads.ValidationMonad;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Commands
{
    public class Command
    {
        private readonly ILevel _level;
        private readonly IActorState _actorState;
        private readonly Point _targetLocation;
        private readonly IEntity _targetItem;
        private readonly Func<IValidation<ILevel>> _validator;
        private readonly Func<IValidation<ILevel>> _operation;

        public Command(
            ILevel level,
            IActorState actorState,
            Point targetLocation,
            IEntity targetItem,
            Func<IValidation<ILevel>> validator,
            Func<IValidation<ILevel>> operation)
        {
            _level = level;
            _actorState = actorState;
            _targetLocation = targetLocation;
            _targetItem = targetItem;
            _validator = validator;
            _operation = operation;
        }

        public ILevel Level
        {
            get { return _level; }
        }

        public IActorState ActorState
        {
            get { return _actorState; }
        }

        public Point TargetLocation
        {
            get { return _targetLocation; }
        }

        public IEntity TargetItem
        {
            get { return _targetItem; }
        }

        public Func<IValidation<ILevel>> Validator
        {
            get { return _validator; }
        }

        public Func<IValidation<ILevel>> Operation
        {
            get { return _operation; }
        }

        public IValidation<ILevel> Validate()
        {
            return _validator();
        }
    }
}