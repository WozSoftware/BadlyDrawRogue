#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Functional.
//
// Woz.Functional is free software: you can redistribute it 
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

namespace Woz.Functional.Maybe
{
    public static class MaybeEnumerable
    {
        public static IMaybe<T> FirstMaybe<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.FirstOrDefault().ToMaybe();
        }

        public static IMaybe<T> FirstMaybe<T>(
            this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return enumerable.FirstOrDefault(predicate).ToMaybe();
        }

        public static IMaybe<T> LastMaybe<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.LastOrDefault().ToMaybe();
        }

        public static IMaybe<T> LastMaybe<T>(
            this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return enumerable.LastOrDefault(predicate).ToMaybe();
        }

        public static IEnumerable<T> WhereHasValue<T>(
            this IEnumerable<IMaybe<T>> enumerable)
        {
            return enumerable.Where(x => x.HasValue).Select(x => x.Value);
        }

        public static IEnumerable<IMaybe<TResult>> Select<T, TResult>(
            this IEnumerable<IMaybe<T>> enumerable, Func<T, TResult> selector)
        {
            return enumerable.Select(maybe => maybe.Select(selector));
        }
    }
}