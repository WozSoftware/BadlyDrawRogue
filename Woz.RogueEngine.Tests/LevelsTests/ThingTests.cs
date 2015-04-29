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
    using ICombatStatistics = IImmutableDictionary<DamageTypes, int>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    [TestClass]
    public class ThingTests
    {
        public const long Id = 1;
        public const ThingTypes ThingType = ThingTypes.Item;
        public const string Name = "Name";
        public const EquipmentSlots EquipableAs = EquipmentSlots.LeftRing;
        public const bool Equiped = true;

        public static readonly ICombatStatistics AttackDetails =
            new Mock<ICombatStatistics>().Object;

        public static readonly ICombatStatistics DefenseDetails =
            new Mock<ICombatStatistics>().Object;

        public static readonly IThingStore Contains =
            new Mock<IThingStore>().Object;

        public static readonly Thing Thing = Thing.Create(
            Id,
            ThingType,
            Name,
            EquipableAs,
            Equiped,
            AttackDetails,
            DefenseDetails,
            Contains);

        private static void Validate(
            Thing instance,
            long? id = null,
            ThingTypes? thingType = null,
            string name = null,
            EquipmentSlots? equipableAs = null,
            bool? equiped = null,
            ICombatStatistics attackDetails = null,
            ICombatStatistics defenseDetails = null,
            IThingStore contains = null)
        {
            Assert.AreEqual(id ?? Id, instance.Id);
            Assert.AreEqual(thingType ?? ThingType, instance.ThingType);
            Assert.AreEqual(name ?? Name, instance.Name);
            Assert.AreEqual(equipableAs ?? EquipableAs, instance.EquipableAs);
            Assert.AreEqual(equiped ?? Equiped, instance.Equiped);
            Assert.AreSame(attackDetails ?? AttackDetails, instance.AttackDetails);
            Assert.AreSame(defenseDetails ?? DefenseDetails, instance.DefenseDetails);
            Assert.AreSame(contains ?? Contains, instance.Contains);
        }

        [TestMethod]
        public void Create()
        {
            Validate(Thing);
        }

        [TestMethod]
        public void WithNoValues()
        {
            Assert.AreSame(Thing, Thing.With());
        }

        [TestMethod]
        public void WithId()
        {
            Validate(Thing.With(id: 5), id: 5);
        }

        [TestMethod]
        public void WithThingType()
        {
            Validate(
                Thing.With(thingType: ThingTypes.Chest), 
                thingType: ThingTypes.Chest);
        }

        [TestMethod]
        public void WithName()
        {
            Validate(Thing.With(name: "A"), name: "A");
        }

        [TestMethod]
        public void WithEquipableAs()
        {
            Validate(
                Thing.With(equipableAs: EquipmentSlots.Amulet),
                equipableAs: EquipmentSlots.Amulet);
        }

        [TestMethod]
        public void WithEquiped()
        {
            Validate(Thing.With(equiped: false), equiped: false);
        }

        [TestMethod]
        public void WithAttackDetails()
        {
            var details = new Mock<ICombatStatistics>().Object;
            Validate(
                Thing.With(attackDetails: details),
                attackDetails: details);
        }

        [TestMethod]
        public void WithDefenseDetails()
        {
            var details = new Mock<ICombatStatistics>().Object;
            Validate(
                Thing.With(defenseDetails: details),
                defenseDetails: details);
        }

        [TestMethod]
        public void WithContains()
        {
            var store = new Mock<IThingStore>().Object;
            Validate(
                Thing.With(contains: store),
                contains: store);
        }
    }
}