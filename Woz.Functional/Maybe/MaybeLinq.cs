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

namespace Woz.Functional.Maybe
{
    public static class MaybeLinq
    {
        public static IMaybe<TResult> Select<T, TResult>(
            this IMaybe<T> maybe, Func<T, TResult> selector)
        {
            return maybe.HasValue
                ? selector(maybe.Value).ToMaybe()
                : Maybe<TResult>.Nothing;
        }

        public static IMaybe<TResult> SelectMany<T, TResult>(
            this IMaybe<T> maybe, Func<T, IMaybe<TResult>> selector)
        {
            return maybe.Bind(selector);
        }

        public static IMaybe<TResult> SelectMany<T1, T2, TResult>(
            this IMaybe<T1> maybe,
            Func<T1, IMaybe<T2>> transform,
            Func<T1, T2, TResult> composer)
        {
            return maybe.Bind(x =>
                transform(x).Bind(y =>
                    composer(x, y).ToMaybe()));
        }

        public static IMaybe<T> Where<T>(IMaybe<T> maybe, Func<T, bool> predicate)
        {
            return !maybe.HasValue || predicate(maybe.Value)
                ? maybe
                : Maybe<T>.Nothing;
        }
    }
}