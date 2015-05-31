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
using Woz.RogueEngine.State;

namespace Woz.RogueEngine.Tests.StateTests
{
    using IDamageTypesStore = IImmutableDictionary<DamageTypes, int>;

    [TestClass]
    public class CombatStatisticsTests
    {
        public const int Range = 1;

        public static readonly IDamageTypesStore AttackDetails =
            ImmutableDictionary<DamageTypes, int>.Empty;

        public static readonly IDamageTypesStore DefenseDetails =
            ImmutableDictionary<DamageTypes, int>.Empty;

        public static readonly CombatStatistics Statistics =
            CombatStatistics.Create(Range, AttackDetails, DefenseDetails);

        private static void Validate(
            CombatStatistics statistics,
            int? range = null,
            IDamageTypesStore attackDetails = null,
            IDamageTypesStore defenseDetails = null)
        {
            Assert.AreEqual(range ?? Range, statistics.Range);
            Assert.AreSame(attackDetails ?? AttackDetails, statistics.AttackDetails);
            Assert.AreSame(defenseDetails ?? DefenseDetails, statistics.DefenseDetails);
        }

        [TestMethod]
        public void Create()
        {
            Validate(Statistics);
        }

        [TestMethod]
        public void WithRange()
        {
            Validate(Statistics.With(range: 2), range: 2);
        }

        [TestMethod]
        public void WithAttackDetails()
        {
            var attackDetails = AttackDetails.SetItem(DamageTypes.BluntImpact, 3);

            Validate(
                Statistics.With(attackDetails: attackDetails), 
                attackDetails: attackDetails);
        }

        [TestMethod]
        public void WithDefenseDetails()
        {
            var defenseDetails = DefenseDetails.SetItem(DamageTypes.BluntImpact, 3);

            Validate(
                Statistics.With(defenseDetails: defenseDetails),
                defenseDetails: defenseDetails);
        }
    }
}