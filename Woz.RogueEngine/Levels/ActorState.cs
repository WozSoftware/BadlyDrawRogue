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

namespace Woz.RogueEngine.Levels
{
    public sealed class ActorState 
    {
        public readonly long Id;
        public readonly Coordinate Location;

        private ActorState(long id, Coordinate location)
        {
            Id  = id;
            Location = location;
        }

        public static ActorState Create(long id, Coordinate location)
        {
            return new ActorState(id, location);
        }

        public ActorState With(long? id = null, Coordinate? location = null)
        {
            return id != null || location != null
                ? new ActorState(id ?? Id, location ?? Location)
                : this;
        }
    }
}