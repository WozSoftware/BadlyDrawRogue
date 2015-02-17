﻿#region License
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
using Woz.Functional.Monads.TryMonad;

namespace Woz.Functional.Monads.IOMonad
{
    public delegate T IO<out T>();

    public static class IO
    {
        public static IO<T> BuildFor<T>(T value)
        {
            return () => value;
        }

        // M<T> -> Func<T, TResult> -> M<TResult>
        public static IO<TResult> Select<T, TResult>(
            this IO<T> io, Func<T, TResult> operation)
        {
            return () => operation(io());
        }

        // M<T> -> Func<T, M<TResult>> -> M<TResult>
        public static IO<TResult> SelectMany<T, TResult>(
            this IO<T> io, Func<T, IO<TResult>> operation)
        {
            return () => operation(io())();
        }

        // M<T1> -> Func<T1, M<T2>> -> Func<T1, T2, TResult> -> M<TResult>
        public static IO<TResult> SelectMany<T1, T2, TResult>(
            this IO<T1> io,
            Func<T1, IO<T2>> transform,
            Func<T1, T2, TResult> composer)
        {
            return io.SelectMany(x =>
                transform(x).SelectMany(y =>
                    new IO<TResult>(() => composer(x, y))));
        }

        public static ITry<T> Run<T>(this IO<T> operation)
        {
            return Try.Catcher(() => operation().ToSuccess());
        }
    }
}