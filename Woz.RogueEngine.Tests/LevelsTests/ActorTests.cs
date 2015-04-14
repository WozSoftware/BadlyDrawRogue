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
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Tests.LevelsTests
{
    using IStatisticsStore = IImmutableDictionary<StatisticTypes, int>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    [TestClass]
    public class ActorTests
    {
        public const int Id = 1;
        public const ActorTypes ActorType = ActorTypes.Monster;
        public const string Name = "Name";
        public static readonly IStatisticsStore Stats = new Mock<IStatisticsStore>().Object;
        public static readonly HitPoints Hp = HitPoints.Create(1, 1);
        public static readonly IThingStore Things = new Mock<IThingStore>().Object;

        public static readonly Actor Actor =
            Actor.Create(Id, ActorType, Name, Stats, Hp, Things);

        [TestMethod]
        public void Create()
        {
            Assert.AreEqual(Id, Actor.Id);
            Assert.AreEqual(ActorType, Actor.ActorType);
            Assert.AreEqual(Name, Actor.Name);
            Assert.AreSame(Stats, Actor.Statistics);
            Assert.AreSame(Hp, Actor.HitPoints);
            Assert.AreSame(Things, Actor.Things);
        }

        [TestMethod]
        public void WithNoValues()
        {
            Assert.AreSame(Actor, Actor.With());
        }

        [TestMethod]
        public void WithId()
        {
            var actor = Actor.With(id: 2);

            Assert.AreEqual(2, actor.Id);
            Assert.AreEqual(ActorType, actor.ActorType);
            Assert.AreEqual(Name, actor.Name);
            Assert.AreSame(Stats, actor.Statistics);
            Assert.AreSame(Hp, actor.HitPoints);
            Assert.AreSame(Things, actor.Things);
        }

        [TestMethod]
        public void WithType()
        {
            var actor = Actor.With(actorType: ActorTypes.Player);

            Assert.AreEqual(Id, actor.Id);
            Assert.AreEqual(ActorTypes.Player, actor.ActorType);
            Assert.AreEqual(Name, actor.Name);
            Assert.AreSame(Stats, actor.Statistics);
            Assert.AreSame(Hp, actor.HitPoints);
            Assert.AreSame(Things, actor.Things);
        }

        [TestMethod]
        public void WithName()
        {
            var actor = Actor.With(name: "A");

            Assert.AreEqual(Id, actor.Id);
            Assert.AreEqual(ActorType, actor.ActorType);
            Assert.AreEqual("A", actor.Name);
            Assert.AreSame(Stats, actor.Statistics);
            Assert.AreSame(Hp, actor.HitPoints);
            Assert.AreSame(Things, actor.Things);
        }

        [TestMethod]
        public void WithStats()
        {
            var newStats = new Mock<IStatisticsStore>().Object;

            var actor = Actor.With(statistics: newStats);

            Assert.AreEqual(Id, actor.Id);
            Assert.AreEqual(ActorType, actor.ActorType);
            Assert.AreEqual(Name, actor.Name);
            Assert.AreSame(newStats, actor.Statistics);
            Assert.AreSame(Hp, actor.HitPoints);
            Assert.AreSame(Things, actor.Things);
        }

        [TestMethod]
        public void WithHp()
        {
            var newHp = HitPoints.Create(2, 2);

            var actor = Actor.With(hitPoints: newHp);

            Assert.AreEqual(Id, actor.Id);
            Assert.AreEqual(ActorType, actor.ActorType);
            Assert.AreEqual(Name, actor.Name);
            Assert.AreSame(Stats, actor.Statistics);
            Assert.AreSame(newHp, actor.HitPoints);
            Assert.AreSame(Things, actor.Things);
        }

        [TestMethod]
        public void WithThings()
        {
            var newThings = new Mock<IThingStore>().Object;

            var actor = Actor.With(things: newThings);

            Assert.AreEqual(Id, actor.Id);
            Assert.AreEqual(ActorType, actor.ActorType);
            Assert.AreEqual(Name, actor.Name);
            Assert.AreSame(Stats, actor.Statistics);
            Assert.AreSame(Hp, actor.HitPoints);
            Assert.AreSame(newThings, actor.Things);
        }
    }
}
