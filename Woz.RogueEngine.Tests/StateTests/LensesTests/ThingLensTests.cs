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
using Woz.Monads.MaybeMonad;
using Woz.RogueEngine.State;
using Woz.RogueEngine.State.Lenses;

namespace Woz.RogueEngine.Tests.StateTests.LensesTests
{
    using ICombatStatistics = IImmutableDictionary<DamageTypes, int>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    [TestClass]
    public class ThingLensTests : BaseLensTests
    {
        [TestMethod]
        public void Id()
        {
            TestLensWithAreEqual(ThingTests.Thing, ThingLens.Id, 7L);
        }

        [TestMethod]
        public void ThingType()
        {
            TestLensWithAreEqual(
                ThingTests.Thing, ThingLens.ThingType, ThingTypes.Furniture);
        }

        [TestMethod]
        public void Name()
        {
            TestLensWithAreEqual(ThingTests.Thing, ThingLens.Name, "A");
        }

        [TestMethod]
        public void EquipableAs()
        {
            TestLensWithAreEqual(
                ThingTests.Thing, ThingLens.EquipableAs, EquipmentSlots.Belt.ToSome());
        }

        [TestMethod]
        public void AttackDetails()
        {
            TestLensWithAreEqual(
                ThingTests.Thing, ThingLens.AttackDetails, 
                new Mock<ICombatStatistics>().Object);
        }

        [TestMethod]
        public void DefenseDetails()
        {
            TestLensWithAreEqual(
                ThingTests.Thing, ThingLens.DefenseDetails,
                new Mock<ICombatStatistics>().Object);
        }

        [TestMethod]
        public void Contains()
        {
            TestLensWithAreEqual(
                ThingTests.Thing, ThingLens.Contains,
                new Mock<IThingStore>().Object);
        }
    }
}