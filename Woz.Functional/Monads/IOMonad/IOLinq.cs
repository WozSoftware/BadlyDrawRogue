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

namespace Woz.Functional.Monads.IOMonad
{
    public static class IOLinq
    {
        public static IO<TResult> Select<T, TResult>(
            this IO<T> io, Func<T, TResult> selector)
        {
            return io.Map(selector);
        }

        public static IO<TResult> SelectMany<T, TResult>(
            this IO<T> io, Func<T, IO<TResult>> selector)
        {
            return io.FlatMap(selector);
        }

        public static IO<TResult> SelectMany<T1, T2, TResult>(
            this IO<T1> io,
            Func<T1, IO<T2>> transform,
            Func<T1, T2, TResult> composer)
        {
            return io.FlatMap(x =>
                transform(x).FlatMap(y =>
                    IO.ToIO(() => composer(x, y))));
        }
    }
}