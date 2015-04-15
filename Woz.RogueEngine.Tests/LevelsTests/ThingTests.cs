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

        [TestMethod]
        public void Create()
        {
            Assert.AreEqual(Id, Thing.Id);
            Assert.AreEqual(ThingType, Thing.ThingType);
            Assert.AreEqual(Name, Thing.Name);
            Assert.AreEqual(EquipableAs, Thing.EquipableAs);
            Assert.AreEqual(Equiped, Thing.Equiped);
            Assert.AreSame(AttackDetails, Thing.AttackDetails);
            Assert.AreSame(DefenseDetails, Thing.DefenseDetails);
            Assert.AreSame(Contains, Thing.Contains);
        }

        [TestMethod]
        public void WithNoValues()
        {
            Assert.AreSame(Thing, Thing.With());
        }
    }
}