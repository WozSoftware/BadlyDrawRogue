using System;
using System.Collections.Generic;
using Functional.Maybe;
using Woz.Functional.Error;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Queries;

namespace Woz.RogueEngine.Validation
{
    public static class RuleHelpers
    {
        public static Error<IEntity> RuleAttributeHasEnumValue<T>(
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

        public static Error<IEntity> RuleAttributeHasEnumValue<T>(
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
                ? entity.ToSuccees()
                : messageBuilder().ToError<IEntity>();
        }
    }
}