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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Woz.Core.Collections;
using Woz.Immutable.Collections;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Tests.LevelsTests
{
    using ITileStore = IImmutableGrid<Tile>;
    using IActorStateStore = IImmutableDictionary<long, ActorState>;

    [TestClass]
    public class LevelTests
    {
        public const int Width = 5;
        public const int Height = 5;
        public static readonly Size Size = new Size(Width, Height);

        public static readonly ITileStore Tiles =
            new Mock<ITileStore>().Object;

        public static readonly IActorStateStore ActorStates =
            new Mock<IActorStateStore>().Object;

        public static readonly Level Level = Level.Create(Tiles, ActorStates);

        private static void Validate(
            Level instance,
            ITileStore tiles = null,
            IActorStateStore actorStates = null)
        {
            Assert.AreSame(tiles ?? Tiles, instance.Tiles);
            Assert.AreSame(actorStates ?? ActorStates, instance.ActorStates);
        }

        [TestMethod]
        public void CreateBySize()
        {
            var level = Level.Create(Size);

            var walker =
                from x in Enumerable.Range(0, Size.Width)
                from y in Enumerable.Range(0, Size.Height)
                select new Point(x, y);

            walker.ForEach(point =>
                Assert.AreSame(Tile.Void, level.Tiles[point]));

            Assert.IsFalse(level.ActorStates.Any());
        }

        [TestMethod]
        public void CreateByParams()
        {
            Validate(Level);
        }

        [TestMethod]
        public void WithNoValues()
        {
            Assert.AreSame(Level, Level.With());
        }

        [TestMethod]
        public void WithTiles()
        {
            var newTiles = new Mock<ITileStore>().Object;

            Validate(Level.With(tiles: newTiles), tiles: newTiles);
        }

        [TestMethod]
        public void WithActorStates()
        {
            var newActorStates = new Mock<IActorStateStore>().Object;

            Validate(
                Level.With(actorStates: newActorStates),
                actorStates: newActorStates);
        }
    }
}