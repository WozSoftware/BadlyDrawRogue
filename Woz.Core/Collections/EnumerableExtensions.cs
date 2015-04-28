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
using System.Collections.Generic;

namespace Woz.Core.Collections
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Select<T>(this IEnumerator<T> enumerator)
        {
            return enumerator.Select(x => x);
        }

        public static IEnumerable<TResult> Select<T, TResult>(
            this IEnumerator<T> enumerator, Func<T, TResult> selector)
        {
            while (enumerator.MoveNext())
            {
                yield return selector(enumerator.Current);
            }
        }

        public static void ForEach<T>(
            this IEnumerable<T> self, Action<T> action)
        {
            foreach (var item in self)
            {
                action(item);
            }
        }
    }
}