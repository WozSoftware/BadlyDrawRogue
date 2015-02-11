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
using Woz.Functional.Maybe;
using Woz.Functional.Try;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Queries;

namespace Woz.RogueEngine.Validation
{
    public static class RuleHelpers
    {
        public static ITry<IEntity> RuleAttributeHasEnumValue<T>(
            this IEntity entity,
            EntityAttributes attribute,
            T requiredType,
            string message)
            where T : struct, IConvertible
        {
            return entity
                .RuleAttributeHasEnumValue(
                    attribute,
                    requiredType,
                    () => message);
        }

        public static ITry<IEntity> RuleAttributeHasEnumValue<T>(
            this IEntity entity,
            EntityAttributes attribute,
            T requiredType,
            Func<string> messageBuilder)
            where T : struct, IConvertible
        {
            return entity
                .Attributes
                .LookupAsEnum<T>(attribute)
                .Select(x => EqualityComparer<T>.Default.Equals(x, requiredType))
                .OrElse(false)
                ? entity.ToSuccess()
                : messageBuilder().ToFailed<IEntity>();
        }
    }
}