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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Woz.Lenses.Tests;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Tests.LevelsTests
{
    using IStatisticsStore = IImmutableDictionary<StatisticTypes, int>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    [TestClass]
    public class ActorLensTests : BaseLensTests
    {
        [TestMethod]
        public void Id()
        {
            TestLensWithAreEqual(ActorTests.Actor, ActorLens.Id, 2L);
        }

        [TestMethod]
        public void ActorType()
        {
            TestLensWithAreEqual(
                ActorTests.Actor, ActorLens.ActorType, ActorTypes.Monster);
        }

        [TestMethod]
        public void Name()
        {
            TestLensWithAreEqual(ActorTests.Actor, ActorLens.Name, "A");
        }

        [TestMethod]
        public void Statistics()
        {
            TestLensWithAreSame(
                ActorTests.Actor, ActorLens.Statistics, 
                new Mock<IStatisticsStore>().Object);
        }

        [TestMethod]
        public void HitPoints()
        {
            TestLensWithAreSame(
                ActorTests.Actor, ActorLens.HitPoints,
                Levels.HitPoints.Create(1, 1));
        }

        [TestMethod]
        public void Things()
        {
            TestLensWithAreSame(
                ActorTests.Actor, ActorLens.Things,
                new Mock<IThingStore>().Object);
        }
    }
}
