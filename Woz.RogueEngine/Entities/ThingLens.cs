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
using Woz.Lenses;

namespace Woz.RogueEngine.Entities
{
    using ICombatStatistics = IImmutableDictionary<DamageTypes, int>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    public static class ThingLens
    {
        public static readonly Lens<Thing, long> Id;
        public static readonly Lens<Thing, ThingTypes> ThingType;
        public static readonly Lens<Thing, string> Name;
        public static readonly Lens<Thing, EquipmentSlots> EquipableAs;
        public static readonly Lens<Thing, bool> Equiped;
        public static readonly Lens<Thing, ICombatStatistics> AttackDetails;
        public static readonly Lens<Thing, ICombatStatistics> DefenseDetails;
        public static readonly Lens<Thing, IThingStore> Contains;

        static ThingLens()
        {
            Id = Lens.Create<Thing, long>(
                thing => thing.Id,
                id => thing => thing.With(id: id));

            ThingType = Lens.Create<Thing, ThingTypes>(
                thing => thing.ThingType,
                thingType => thing => thing.With(thingType: thingType));

            Name = Lens.Create<Thing, string>(
                thing => thing.Name,
                name => thing => thing.With(name: name));

            EquipableAs = Lens.Create<Thing, EquipmentSlots>(
                thing => thing.EquipableAs,
                equipableAs => thing => thing.With(equipableAs: equipableAs));

            Equiped = Lens.Create<Thing, bool>(
                thing => thing.Equiped,
                equiped => thing => thing.With(equiped: equiped));

            AttackDetails = Lens.Create<Thing, ICombatStatistics>(
                thing => thing.AttackDetails,
                attackDetails => thing => thing.With(attackDetails: attackDetails));

            DefenseDetails = Lens.Create<Thing, ICombatStatistics>(
                thing => thing.DefenseDetails,
                defenseDetails => thing => thing.With(defenseDetails: defenseDetails));

            Contains = Lens.Create<Thing, IThingStore>(
                thing => thing.Contains,
                contains => thing => thing.With(contains: contains));
        }
    }
}