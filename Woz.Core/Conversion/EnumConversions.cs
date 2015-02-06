using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Functional.Maybe;

namespace Woz.Core.Conversion
{
    public static class EnumConversions
    {
        public static TEnum ToEnum<TEnum>(this string value)
            where TEnum : struct
        {
            return value
                .ToMaybeEnum<TEnum>()
                .OrElse(
                    () => new InvalidEnumArgumentException(
                        string.Format(
                            "{0} is not part of {1}]", 
                            value ?? "null", 
                            typeof (TEnum).Name)));
        }

        public static TEnum ToEnum<TEnum>(this string value, TEnum defaultValue)
            where TEnum : struct
        {
            return value.ToMaybeEnum<TEnum>().OrElse(defaultValue);
        }

        public static Maybe<TEnum> ToMaybeEnum<TEnum>(this string value)
            where TEnum : struct
        {
            TEnum result;
            return Enum.TryParse(value, out result) 
                ? result.ToMaybe() 
                : Maybe<TEnum>.Nothing;
        }

        public static TEnum ToEnum<TValue, TEnum>(this TValue value)
            where TValue : struct, IConvertible
            where TEnum : struct, IConvertible
        {
            return value
                .ToMaybeEnum<TValue, TEnum>()
                .OrElse(
                    () => new InvalidEnumArgumentException(
                        string.Format(
                            "{0} is not enum in {1}]",
                            value, typeof(TEnum).Name)));
        }

        public static TEnum ToEnum<TValue, TEnum>(this TValue value, TEnum defaultValue)
            where TValue : struct, IConvertible
            where TEnum : struct, IConvertible
        {
            return value.ToMaybeEnum<TValue, TEnum>().OrElse(defaultValue);
        }

        public static Maybe<TEnum> ToMaybeEnum<TValue, TEnum>(this TValue value)
            where TValue : struct, IConvertible
            where TEnum : struct, IConvertible
        {
            var result = EnumConversionHelper<TValue, TEnum>.Convert(value);
            return Enum.IsDefined(typeof(TEnum), result)
                ? result.ToMaybe()
                : Maybe<TEnum>.Nothing;
        }

        private static class EnumConversionHelper<TValue, TEnum>
            where TValue : struct, IConvertible
            where TEnum : struct, IConvertible
        {
            public static readonly Func<TValue, TEnum> Convert = GenerateConversion();

            private static Func<TValue, TEnum> GenerateConversion()
            {
                var parameter = Expression.Parameter(typeof(TValue));

                var dynamicMethod = Expression
                    .Lambda<Func<TValue, TEnum>>(
                        Expression.Convert(parameter, typeof(TEnum)),
                        parameter);

                return dynamicMethod.Compile();
            }
        }
    }

}