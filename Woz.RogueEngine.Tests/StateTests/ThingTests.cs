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
using Woz.Immutable.Collections;
using Woz.Linq.Collections;
using Woz.Monads.MaybeMonad;
using Woz.RogueEngine.State;

namespace Woz.RogueEngine.Tests.StateTests
{
    using ISlotList = IImmutableArray<EquipmentSlots>;
    using IDamageTypesStore = IImmutableDictionary<DamageTypes, int>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    [TestClass]
    public class ThingTests
    {
        public const long Id = 1;
        public const ThingTypes ThingType = ThingTypes.Item;
        public const string Name = "Name";

        public static readonly ISlotList ValidSlots = SlotLists.NotEquipable;

        public static readonly IMaybe<EquipmentSlots> EquipedAs = 
            EquipmentSlots.LeftRing.ToSome();

        public static readonly IMaybe<CombatStatistics> CombatStats =
            Maybe<CombatStatistics>.None;

        public static readonly IThingStore Contains =
            new Mock<IThingStore>().Object;

        public static readonly Thing Thing = Thing.Create(
            Id,
            ThingType,
            Name,
            ValidSlots,
            EquipedAs,
            CombatStats,
            Contains);

        private static void Validate(
            Thing instance,
            long? id = null,
            ThingTypes? thingType = null,
            string name = null,
            ISlotList validSlots = null,
            IMaybe<EquipmentSlots> equipedAs = null,
            IMaybe<CombatStatistics> combatStatistics = null,
            IThingStore contains = null)
        {
            Assert.AreEqual(id ?? Id, instance.Id);
            Assert.AreEqual(thingType ?? ThingType, instance.ThingType);
            Assert.AreEqual(name ?? Name, instance.Name);
            Assert.AreSame(validSlots ?? ValidSlots, instance.ValidSlots);
            Assert.AreSame(equipedAs ?? EquipedAs, instance.EquipedAs);
            Assert.AreSame(combatStatistics ?? CombatStats, instance.CombatStatistics);
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
        public void WithValidSlots()
        {
            var validSlots = ImmutableArray
                .Create(EquipmentSlots.Amulet.ToEnumerable());

            Validate(
                Thing.With(validSlots: validSlots),
                validSlots: validSlots);
        }

        [TestMethod]
        public void WithEquipedAs()
        {
            var equipedAs = Maybe<EquipmentSlots>.None;
            Validate(
                Thing.With(equipedAs: equipedAs),
                equipedAs: equipedAs);
        }

        [TestMethod]
        public void WithCombatStatistics()
        {
            var attack = ImmutableDictionary<DamageTypes, int>
                .Empty
                .SetItem(DamageTypes.Acid, 1);

            var defense = ImmutableDictionary<DamageTypes, int>
                .Empty
                .SetItem(DamageTypes.BluntImpact, 1);

            var statistics = CombatStatistics
                .Create(1, attack, defense)
                .ToSome();

            Validate(
                Thing.With(combatStatistics: statistics),
                combatStatistics: statistics);
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