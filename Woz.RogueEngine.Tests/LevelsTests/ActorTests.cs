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

        private static void Validate(
            Actor instance,
            long? id = null,
            ActorTypes? actorType = null,
            string name = null,
            IStatisticsStore stats = null,
            HitPoints hp = null,
            IThingStore things = null)
        {
            Assert.AreEqual(id ?? Id, instance.Id);
            Assert.AreEqual(actorType ?? ActorType, instance.ActorType);
            Assert.AreEqual(name ?? Name, instance.Name);
            Assert.AreSame(stats ?? Stats, instance.Statistics);
            Assert.AreSame(hp ?? Hp, instance.HitPoints);
            Assert.AreSame(things ?? Things, instance.Things);
        }

        [TestMethod]
        public void Create()
        {
            Validate(Actor);
        }

        [TestMethod]
        public void WithNoValues()
        {
            Assert.AreSame(Actor, Actor.With());
        }

        [TestMethod]
        public void WithId()
        {
            Validate(Actor.With(id: 2), id: 2);
        }

        [TestMethod]
        public void WithType()
        {
            Validate(
                Actor.With(actorType: ActorTypes.Player), 
                actorType: ActorTypes.Player);
        }

        [TestMethod]
        public void WithName()
        {
            Validate(Actor.With(name: "A"), name: "A");
        }

        [TestMethod]
        public void WithStats()
        {
            var newStats = new Mock<IStatisticsStore>().Object;

            Validate(
                Actor.With(statistics: newStats),
                stats: newStats);
        }

        [TestMethod]
        public void WithHp()
        {
            var newHp = HitPoints.Create(2, 2);

            Validate(Actor.With(hitPoints: newHp), hp: newHp);
        }

        [TestMethod]
        public void WithThings()
        {
            var newThings = new Mock<IThingStore>().Object;

            Validate(Actor.With(things: newThings), things: newThings);
        }
    }
}
