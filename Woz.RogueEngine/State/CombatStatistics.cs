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
using System.Diagnostics;

namespace Woz.RogueEngine.State
{
    using IDamageTypesStore = IImmutableDictionary<DamageTypes, int>;

    public sealed class CombatStatistics
    {
        private readonly int _range;
        private readonly IDamageTypesStore _attackDetails;
        private readonly IDamageTypesStore _defenseDetails;

        private CombatStatistics(
            int range,
            IDamageTypesStore attackDetails,
            IDamageTypesStore defenseDetails)
        {
            _range = range;
            _attackDetails = attackDetails;
            _defenseDetails = defenseDetails;
        }

        public static CombatStatistics Create(
            int range,
            IDamageTypesStore attackDetails,
            IDamageTypesStore defenseDetails)
        {
            Debug.Assert(attackDetails != null);
            Debug.Assert(defenseDetails != null);

            return new CombatStatistics(range, attackDetails, defenseDetails);
        }

        public int Range
        {
            get { return _range; }
        }

        public IDamageTypesStore AttackDetails
        {
            get { return _attackDetails; }
        }

        public IDamageTypesStore DefenseDetails
        {
            get { return _defenseDetails; }
        }

        public CombatStatistics With(
            int? range = null,
            IDamageTypesStore attackDetails = null,
            IDamageTypesStore defenseDetails = null)
        {
            return
                range.HasValue ||
                attackDetails != null ||
                defenseDetails != null
                    ? new CombatStatistics(
                        range ?? _range,
                        attackDetails ?? _attackDetails,
                        defenseDetails ?? _defenseDetails)
                    : this;
        }
    }
}