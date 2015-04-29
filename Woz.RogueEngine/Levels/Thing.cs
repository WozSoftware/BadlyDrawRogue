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

namespace Woz.RogueEngine.Levels
{
    using ICombatStatistics = IImmutableDictionary<DamageTypes, int>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    public sealed class Thing
    {
        public readonly long Id;
        public readonly ThingTypes ThingType;
        public readonly string Name;
        public readonly EquipmentSlots EquipableAs;
        public readonly bool Equiped;
        public readonly ICombatStatistics AttackDetails;
        public readonly ICombatStatistics DefenseDetails;
        public readonly IThingStore Contains;

        public Thing(
            long id, 
            ThingTypes thingType, 
            string name,
            EquipmentSlots equipableAs, 
            bool equiped, 
            ICombatStatistics attackDetails, 
            ICombatStatistics defenseDetails, 
            IThingStore contains)
        {
            Debug.Assert(name != null);
            Debug.Assert(attackDetails != null);
            Debug.Assert(defenseDetails != null);
            Debug.Assert(contains != null);

            Id = id;
            ThingType = thingType;
            Name = name;
            EquipableAs = equipableAs;
            Equiped = equiped;
            AttackDetails = attackDetails;
            DefenseDetails = defenseDetails;
            Contains = contains;
        }

        public static Thing Create(
            long id,
            ThingTypes thingType,
            string name,
            EquipmentSlots equipableAs,
            bool equiped)
        {
            return new Thing(
                id,
                thingType,
                name,
                equipableAs,
                equiped,
                ImmutableDictionary<DamageTypes, int>.Empty,
                ImmutableDictionary<DamageTypes, int>.Empty,
                ImmutableDictionary<long, Thing>.Empty);
        }


        public static Thing Create(
            long id,
            ThingTypes thingType,
            string name,
            EquipmentSlots equipableAs,
            bool equiped,
            ICombatStatistics attackDetails,
            ICombatStatistics defenseDetails,
            IThingStore contains)
        {
            return new Thing(
                id,
                thingType,
                name,
                equipableAs,
                equiped,
                attackDetails,
                defenseDetails,
                contains);
        }

        public Thing With(
            long? id = null,
            ThingTypes? thingType = null,
            string name = null,
            EquipmentSlots? equipableAs = null,
            bool? equiped = null,
            ICombatStatistics attackDetails = null,
            ICombatStatistics defenseDetails = null,
            IThingStore contains = null)
        {
            return
                id.HasValue ||
                thingType.HasValue ||
                name != null ||
                equipableAs.HasValue ||
                equiped.HasValue ||
                attackDetails != null ||
                defenseDetails != null ||
                contains != null
                    ? new Thing(
                        id ?? Id,
                        thingType ?? ThingType,
                        name ?? Name,
                        equipableAs ?? EquipableAs,
                        equiped ?? Equiped,
                        attackDetails ?? AttackDetails,
                        defenseDetails ?? DefenseDetails,
                        contains ?? Contains)
                    : this;
        }
    }
}