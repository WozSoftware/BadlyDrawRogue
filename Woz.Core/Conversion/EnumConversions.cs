#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Core.
//
// Woz.Core is free software: you can redistribute it 
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
using System.ComponentModel;
using System.Linq.Expressions;
using Functional.Maybe;

namespace Woz.Core.Conversion
{
    /// <summary>
    /// Various helpers for the conversion of strings and numerics to Enum in
    /// a safe way that ensures the result is a valid value.
    /// </summary>
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
                            "{0} is not part of {1}", 
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
                            "{0} is not enum in {1}",
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