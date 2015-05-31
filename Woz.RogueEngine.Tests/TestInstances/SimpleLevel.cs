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

using System.Collections.Immutable;
using Woz.Core.Geometry;
using Woz.Immutable.Collections;
using Woz.RogueEngine.Operations;
using Woz.RogueEngine.State;

namespace Woz.RogueEngine.Tests.TestInstances
{
    public class SimpleLevel
    {
        public static readonly Actor Player;
        public static readonly Actor Monster;

        public static readonly Tile Floor;
        public static readonly Tile Wall;
        public static readonly Tile OpenDoor;
        public static readonly Tile ClosedDoor;

        public static readonly Level Level;

        static SimpleLevel()
        {
            Player = Actor.Create(
                1, ActorTypes.Player, "Player", HitPoints.Create(10, 10));
            Monster = Actor.Create(
                2, ActorTypes.Monster, "Monster", HitPoints.Create(10, 10));

            Floor = Tile.Create(TileTypes.Floor, "Floor");
            Wall = Tile.Create(TileTypes.Wall, "Wall");
            OpenDoor = Tile.Create(TileTypes.OpenDoor, "OpenDoor");
            ClosedDoor = Tile.Create(TileTypes.ClosedDoor, "ClosedDoor");

            // . = Floor
            // # = Wall
            // C = ClosedDoor
            // O = OpenDoor
            // M = Monster
            // P = Player
            //
            // 3|.#P.
            // 2|##O#
            // 1|.C..
            // 0|.#.M
            //  +----
            //   0123

            var tiles = ImmutableGrid<Tile>
                .CreateBuilder(Size.Create(4, 4))
                .Set(0, 0, Floor)
                .Set(0, 1, Wall)
                .Set(0, 2, Floor)
                .Set(0, 3, Floor)
                .Set(1, 0, Wall)
                .Set(1, 1, ClosedDoor)
                .Set(1, 2, Wall)
                .Set(1, 3, Wall)
                .Set(2, 0, Wall)
                .Set(2, 1, Wall)
                .Set(2, 2, OpenDoor)
                .Set(2, 3, Wall)
                .Set(3, 0, Floor)
                .Set(3, 1, Wall)
                .Set(3, 2, Floor)
                .Set(3, 3, Floor)
                .Build();

            Level = Level
                .Create(tiles, ImmutableDictionary<long, ActorState>.Empty)
                .SpawnActor(Monster, Vector.Create(0, 3))
                .SpawnActor(Player, Vector.Create(3, 2));
        }

        public static Vector ActorLocation(Actor actor)
        {
            return Level.ActorStates[actor.Id].Location;
        }
    }
}