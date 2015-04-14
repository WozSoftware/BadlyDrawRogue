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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Woz.Immutable.Collections;
using Woz.Lenses;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Tests.LevelsTests
{
    using ITileStore = IImmutableGrid<Tile>;
    using IActorStateStore = IImmutableDictionary<long, ActorState>;

    [TestClass]
    public class LevelLensTests
    {
        [TestMethod]
        public void Tiles()
        {
            var newTiles = new Mock<ITileStore>().Object;
            var level = LevelTests.Level.Set(LevelLens.Tiles, newTiles);

            Assert.AreSame(newTiles, level.Get(LevelLens.Tiles));
        }

        [TestMethod]
        public void ActorStates()
        {
            var states = new Mock<IActorStateStore>().Object;
            var level = LevelTests.Level.Set(LevelLens.ActorStates, states);

            Assert.AreSame(states, level.Get(LevelLens.ActorStates));
        }
    }
}