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

using System.Drawing;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    public class ActorState : IActorState
    {
        private readonly IEntity _actor;
        private readonly Point _location;

        private ActorState(IEntity actor, Point location)
        {
            _actor = actor;
            _location = location;
        }

        public IEntity Actor
        {
            get { return _actor; }
        }

        public Point Location
        {
            get { return _location; }
        }

        public static IActorState Create(IEntity actor, Point location)
        {
            return new ActorState(actor, location);
        }

        public IActorState With(IEntity actor = null, Point? location = null)
        {
            return actor == null && location == null
                ? this
                : new ActorState(actor ?? _actor, location ?? _location);
        }
    }
}