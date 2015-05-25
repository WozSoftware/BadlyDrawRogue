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

using System.Collections.Generic;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Validators.Rules
{
    public static class TileTypeRules
    {
        public static readonly IEnumerable<TileTypes> DoorTypes = 
            new[] {TileTypes.OpenDoor, TileTypes.ClosedDoor};

        public static readonly IEnumerable<TileTypes> BlockMovement = 
            new[] {TileTypes.Void, TileTypes.Wall, TileTypes.ClosedDoor};

        public static readonly IEnumerable<TileTypes> BlocksLineOfSight =
            new[] {TileTypes.Void, TileTypes.Wall, TileTypes.ClosedDoor};
    }

}