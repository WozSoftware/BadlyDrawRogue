#region License
// Copyright � Woz.Software 2015
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
    [TestClass]
    public class ActorTests
    {
        private const int Id = 1;
        private const ActorTypes ActorType = ActorTypes.Monster;
        private const string Name = "Name";
        private readonly IImmutableDictionary<StatisticTypes, int> _stats = 
            new Mock<IImmutableDictionary<StatisticTypes, int>>().Object;
        private readonly HitPoints _hp = HitPoints.Create(1, 1);
        private readonly IImmutableDictionary<long, Thing> _things = 
            new Mock<IImmutableDictionary<long, Thing>>().Object;

        [TestMethod]
        public void Create()
        {
            var actor = Actor.Create(Id, ActorType, Name, _stats, _hp, _things);

            Assert.AreEqual(Id, actor.Id);
            Assert.AreEqual(ActorType, actor.ActorType);
            Assert.AreEqual(Name, actor.Name);
            Assert.AreSame(_stats, actor.Statistics);
            Assert.AreSame(_hp, actor.HitPoints);
            Assert.AreSame(_things, actor.Things);
        }

        [TestMethod]
        public void WithNoValues()
        {
            var actor = Actor.Create(Id, ActorType, Name, _stats, _hp, _things);
                
            Assert.AreSame(actor, actor.With());
        }

        [TestMethod]
        public void WithId()
        {
            var actor = Actor
                .Create(Id, ActorType, Name, _stats, _hp, _things)
                .With(id: 2);

            Assert.AreEqual(2, actor.Id);
            Assert.AreEqual(ActorType, actor.ActorType);
            Assert.AreEqual(Name, actor.Name);
            Assert.AreSame(_stats, actor.Statistics);
            Assert.AreSame(_hp, actor.HitPoints);
            Assert.AreSame(_things, actor.Things);
        }

        [TestMethod]
        public void WithType()
        {
            var actor = Actor
                .Create(Id, ActorType, Name, _stats, _hp, _things)
                .With(actorType: ActorTypes.Player);

            Assert.AreEqual(Id, actor.Id);
            Assert.AreEqual(ActorTypes.Player, actor.ActorType);
            Assert.AreEqual(Name, actor.Name);
            Assert.AreSame(_stats, actor.Statistics);
            Assert.AreSame(_hp, actor.HitPoints);
            Assert.AreSame(_things, actor.Things);
        }

        [TestMethod]
        public void WithName()
        {
            var actor = Actor
                .Create(Id, ActorType, Name, _stats, _hp, _things)
                .With(name: "A");

            Assert.AreEqual(Id, actor.Id);
            Assert.AreEqual(ActorType, actor.ActorType);
            Assert.AreEqual("A", actor.Name);
            Assert.AreSame(_stats, actor.Statistics);
            Assert.AreSame(_hp, actor.HitPoints);
            Assert.AreSame(_things, actor.Things);
        }

        [TestMethod]
        public void WithStats()
        {
            var newStats = 
                new Mock<IImmutableDictionary<StatisticTypes, int>>().Object;

            var actor = Actor
                .Create(Id, ActorType, Name, _stats, _hp, _things)
                .With(statistics: newStats);

            Assert.AreEqual(Id, actor.Id);
            Assert.AreEqual(ActorType, actor.ActorType);
            Assert.AreEqual(Name, actor.Name);
            Assert.AreSame(newStats, actor.Statistics);
            Assert.AreSame(_hp, actor.HitPoints);
            Assert.AreSame(_things, actor.Things);
        }

        [TestMethod]
        public void WithHp()
        {
            var newHp = HitPoints.Create(2, 2);

            var actor = Actor
                .Create(Id, ActorType, Name, _stats, _hp, _things)
                .With(hitPoints: newHp);

            Assert.AreEqual(Id, actor.Id);
            Assert.AreEqual(ActorType, actor.ActorType);
            Assert.AreEqual(Name, actor.Name);
            Assert.AreSame(_stats, actor.Statistics);
            Assert.AreSame(newHp, actor.HitPoints);
            Assert.AreSame(_things, actor.Things);
        }

        [TestMethod]
        public void WithThings()
        {
            var newThings =
                new Mock<IImmutableDictionary<long, Thing>>().Object;

            var actor = Actor
                .Create(Id, ActorType, Name, _stats, _hp, _things)
                .With(things: newThings);

            Assert.AreEqual(Id, actor.Id);
            Assert.AreEqual(ActorType, actor.ActorType);
            Assert.AreEqual(Name, actor.Name);
            Assert.AreSame(_stats, actor.Statistics);
            Assert.AreSame(_hp, actor.HitPoints);
            Assert.AreSame(newThings, actor.Things);
        }
    }
}
