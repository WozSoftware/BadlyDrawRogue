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

namespace Woz.RogueEngine.State
{
    public sealed class ActorState 
    {
        private readonly long _id;
        private readonly Vector _location;

        private ActorState(long id, Vector location)
        {
            _id  = id;
            _location = location;
        }

        public static ActorState Create(long id, Vector location)
        {
            return new ActorState(id, location);
        }

        public long Id
        {
            get { return _id; }
        }

        public Vector Location
        {
            get { return _location; }
        }

        public ActorState With(long? id = null, Vector? location = null)
        {
            return id != null || location != null
                ? new ActorState(id ?? _id, location ?? _location)
                : this;
        }
    }
}