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
using Woz.Lenses;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Tests.LevelsTests
{
    using IStatisticsStore = IImmutableDictionary<StatisticTypes, int>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    [TestClass]
    public class ActorLensTests
    {
        [TestMethod]
        public void Id()
        {
            var actor = ActorTests.Actor.Set(ActorLens.Id, 2);

            Assert.AreEqual(2, actor.Get(ActorLens.Id));
        }

        [TestMethod]
        public void ActorType()
        {
            var type = ActorTypes.Monster;
            var actor = ActorTests.Actor.Set(ActorLens.ActorType, type);

            Assert.AreEqual(type, actor.Get(ActorLens.ActorType));
        }

        [TestMethod]
        public void Name()
        {
            var actor = ActorTests.Actor.Set(ActorLens.Name, "A");

            Assert.AreEqual("A", actor.Get(ActorLens.Name));
        }

        [TestMethod]
        public void Statistics()
        {
            var stats = new Mock<IStatisticsStore>().Object; 
            var actor = ActorTests.Actor.Set(ActorLens.Statistics, stats);

            Assert.AreEqual(stats, actor.Get(ActorLens.Statistics));
        }

        [TestMethod]
        public void HitPoints()
        {
            var hp = Levels.HitPoints.Create(1, 1);
            var actor = ActorTests.Actor.Set(ActorLens.HitPoints, hp);

            Assert.AreEqual(hp, actor.Get(ActorLens.HitPoints));
        }

        [TestMethod]
        public void Things()
        {
            var things = new Mock<IThingStore>().Object;
            var actor = ActorTests.Actor.Set(ActorLens.Things, things);

            Assert.AreEqual(things, actor.Get(ActorLens.Things));
        }
    }
}
