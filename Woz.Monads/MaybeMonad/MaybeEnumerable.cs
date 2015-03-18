#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Monads.
//
// Woz.Linq is free software: you can redistribute it 
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
using System.Diagnostics;
using System.Linq;

namespace Woz.Monads.MaybeMonad
{
    public static class MaybeEnumerable
    {
        public static IMaybe<T> FirstMaybe<T>(this IEnumerable<T> self)
        {
            Debug.Assert(self != null);

            return self.FirstOrDefault().ToMaybe();
        }

        public static IMaybe<T> FirstMaybe<T>(
            this IEnumerable<T> self, Func<T, bool> predicate)
        {
            Debug.Assert(self != null);
            Debug.Assert(predicate != null);

            return self.FirstOrDefault(predicate).ToMaybe();
        }

        public static IMaybe<T> LastMaybe<T>(this IEnumerable<T> self)
        {
            Debug.Assert(self != null);

            return self.LastOrDefault().ToMaybe();
        }

        public static IMaybe<T> LastMaybe<T>(
            this IEnumerable<T> self, Func<T, bool> predicate)
        {
            Debug.Assert(self != null);
            Debug.Assert(predicate != null);

            return self.LastOrDefault(predicate).ToMaybe();
        }

        public static IEnumerable<T> WhereHasValue<T>(
            this IEnumerable<IMaybe<T>> self)
        {
            Debug.Assert(self != null);

            return self.Where(x => x.HasValue).Select(x => x.Value);
        }

        public static IEnumerable<IMaybe<TResult>> Select<T, TResult>(
            this IEnumerable<IMaybe<T>> self, Func<T, TResult> selector)
        {
            Debug.Assert(self != null);
            Debug.Assert(selector != null);

            return self.Select(maybe => maybe.Select(selector));
        }
    }
}