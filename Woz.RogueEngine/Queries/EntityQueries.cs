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
using System.Collections.Immutable;
using System.Linq;
using Functional.Maybe;
using Woz.Core.Conversion;
using Woz.Functional.Maybe;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Queries
{
    public static class EntityQueries
    {
        public static int EffectiveAttributeValue(
            this IEntity entity,
            EntityAttributes attribute)
        {
            return entity
                .Flattern()
                .Select(x => x.Attributes.Lookup(attribute))
                .WhereValueExist()
                .Sum();
        }

        public static Maybe<T> LookupAsEnum<T>(
            this IImmutableDictionary<EntityAttributes, int> attributes,
            EntityAttributes attribute)
            where T : struct, IConvertible
        {
            return attributes
                .Lookup(attribute)
                .Select(x => x.ToEnum<int, T>());
        }

        public static IEnumerable<IEntity> Flattern(this IEntity root)
        {
            return root.Flattern(x => true);
        }

        public static IEnumerable<IEntity> Flattern(
            this IEntity root, Func<IEntity, bool> predicate)
        {
            if (!predicate(root))
            {
                yield break;
            }

            yield return root;
            foreach (var child in 
                root.Children.Values.SelectMany(x => x.Flattern(predicate)))
            {
                yield return child;
            }
        }

        public static bool TreeHasFlagSet(this IEntity entity, EntityFlags flag)
        {
            return entity
                .Flattern()
                .Select(x => x.HasFlagSet(flag))
                .FirstMaybe(x => x)
                .OrElse(false);
        }

        public static bool HasFlagSet(this IEntity entity, EntityFlags flag)
        {
            return entity.Flags.Lookup(flag).OrElse(false);
        }
    }
}