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

using System.Collections.Immutable;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Immutable.Collections;
using Woz.RogueEngine.Levels;
using Woz.RogueEngine.Operations;
using Woz.RogueEngine.Validators;

namespace Woz.RogueEngine.Tests.ValidatorsTests
{
    [TestClass]
    public class LevelValidatorsTests
    {
        public static readonly Actor Actor;
        public static readonly Level Level;

        static LevelValidatorsTests()
        {
            Actor = Actor.Create(
                1, ActorTypes.Player, "Player", HitPoints.Create(10, 10));

            var floor = Tile.Create(TileTypes.Floor, "Floor");
            
            var tiles = ImmutableGrid<Tile>
                .CreateBuilder(new Size(2, 2))
                .Set(0, 0, floor)
                .Set(0, 1, floor)
                .Set(1, 0, floor)
                .Set(1, 1, floor)
                .Build();

            Level = Level
                .Create(tiles, ImmutableDictionary<long, ActorState>.Empty)
                .SpawnActor(Actor, new Point(1, 1));
        }

        [TestMethod]
        public void IsNewActor()
        {
            Assert.IsFalse(Level.IsNewActor(Actor.Id).IsValid);
            Assert.IsTrue(Level.IsNewActor(Actor.Id + 1).IsValid);
        }

        [TestMethod]
        public void ActorStateExists()
        {
            Assert.IsTrue(Level.ActorStateExists(Actor.Id).IsValid);
            Assert.IsFalse(Level.ActorStateExists(Actor.Id + 1).IsValid);
        }

        [TestMethod]
        public void IsValidLocation()
        {
            Assert.IsTrue(Level.IsValidLocation(new Point(1, 1)).IsValid);
            Assert.IsFalse(Level.IsValidLocation(new Point(2, 2)).IsValid);
        }
    }
}