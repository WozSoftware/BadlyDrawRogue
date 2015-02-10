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
using Functional.Maybe;

namespace Woz.Core.Conversion
{
    /// <summary>
    /// Various helpers to ease parsing strings into value types in a 
    /// safe way.
    /// </summary>
    public static class StringConversion
    {
        public static T ParseAs<T>(this string value)
            where T : struct
        {
            return value
                .ParseAsMaybe<T>()
                .OrElse(
                    () => new ArgumentException(
                        string.Format(
                            "{0} does not convert to {1}", 
                            value ?? "null", 
                            typeof (T).Name)));
        }

        public static T ParseAs<T>(this string value, T defaultValue)
            where T : struct
        {
            return value.ParseAsMaybe<T>().OrElse(defaultValue);
        }

        public static Maybe<T> ParseAsMaybe<T>(this string value)
            where T : struct
        {
            T result;
            return TryParseHelper<T>.TryParse(value, out result)
                ? result.ToMaybe()
                : Maybe<T>.Nothing;
        }
    
        private static class TryParseHelper<T>
            where T : struct
        {
            public delegate bool TryParseFunc(string str, out T result);

            public static readonly TryParseFunc TryParse = CreateTryParse();

            private static TryParseFunc CreateTryParse()
            {
                return (TryParseFunc)Delegate.CreateDelegate(
                    typeof(TryParseFunc), typeof(T), "TryParse");
            }
        }
    }
}