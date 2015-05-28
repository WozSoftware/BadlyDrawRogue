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
using Woz.Core.Geometry;

namespace Woz.RogueEngine.State
{
    public sealed class ActorState 
    {
        private readonly Actor _actor;
        private readonly Vector _location;

        private ActorState(Actor actor, Vector location)
        {
            Debug.Assert(actor != null);

            _actor  = actor;
            _location = location;
        }

        public static ActorState Create(Actor actor, Vector location)
        {
            return new ActorState(actor, location);
        }

        public Actor Actor
        {
            get { return _actor; }
        }

        public Vector Location
        {
            get { return _location; }
        }

        public ActorState With(Actor actor = null, Vector? location = null)
        {
            return actor != null || location != null
                ? new ActorState(actor ?? _actor, location ?? _location)
                : this;
        }
    }
}