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

using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;
using Woz.Functional.Maybe;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Queries
{
    public static class CombatQueries
    {
        public static IEnumerable<KeyValuePair<DamageTypes, int>>
            GetDamageTypesInflicted(this IEntity entity, EntityAttributes attribute)
        {
            return entity.GetDamageTypes(EntityAttributes.DamageInflicted);
        }

        public static IEnumerable<KeyValuePair<DamageTypes, int>>
            GetDamageTypesAbsorbed(this IEntity entity, EntityAttributes attribute)
        {
            return entity.GetDamageTypes(EntityAttributes.DamageAbsorbed);
        }

        private static IEnumerable<KeyValuePair<DamageTypes, int>>
            GetDamageTypes(this IEntity entity, EntityAttributes attribute)
        {
            Func<IEntity, Maybe<KeyValuePair<DamageTypes, int>>> entityDamage =
                x =>
                {
                    var damageType = x.Attributes.LookupAsEnum<DamageTypes>(EntityAttributes.DamageType);
                    var damage = x.Attributes.Lookup(attribute);

                    return damageType.HasValue && damage.HasValue
                        ? new KeyValuePair<DamageTypes, int>(
                            damageType.Value, damage.Value).ToMaybe()
                        : Maybe<KeyValuePair<DamageTypes, int>>.Nothing;
                };

            return entity
                .Flattern()
                .Select(entityDamage)
                .WhereValueExist()
                .GroupBy(x => x.Key)
                .Select(
                    grouping =>
                        new KeyValuePair<DamageTypes, int>(
                            grouping.Key,
                            grouping.Select(x => x.Value).Sum()));
        }
    }
}