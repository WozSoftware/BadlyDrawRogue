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
            GetDamageTypesInflicted(this Entity entity, EntityAttributes attribute)
        {
            return entity.GetDamageTypes(EntityAttributes.DamageInflicted);
        }

        public static IEnumerable<KeyValuePair<DamageTypes, int>>
            GetDamageTypesAbsorbed(this Entity entity, EntityAttributes attribute)
        {
            return entity.GetDamageTypes(EntityAttributes.DamageAbsorbed);
        }

        private static IEnumerable<KeyValuePair<DamageTypes, int>>
            GetDamageTypes(this Entity entity, EntityAttributes attribute)
        {
            Func<Entity, Maybe<KeyValuePair<DamageTypes, int>>> entityDamage =
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