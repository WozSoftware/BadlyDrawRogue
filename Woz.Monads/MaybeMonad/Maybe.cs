﻿#region License
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

using System.Diagnostics;

namespace Woz.Monads.MaybeMonad
{
    public static class Maybe<T>
    {
        public static readonly IMaybe<T> None = new None<T>();
    }

    public static class Maybe
    {
        public static IMaybe<T> ToSome<T>(this T value)
        {
            return new Some<T>(value);
        }

        public static IMaybe<T> ToMaybe<T>(this T? value)
            where T : struct
        {
            return value.HasValue ? new Some<T>(value.Value) : Maybe<T>.None;
        }

        public static IMaybe<T> ToMaybe<T>(this T value)
        {
            return value == null ? Maybe<T>.None : new Some<T>(value);
        }

        public static IMaybe<T> Collapse<T>(this IMaybe<IMaybe<T>> doubleMaybe)
        {
            Debug.Assert(doubleMaybe != null);

            return doubleMaybe.OrElse(Maybe<T>.None);
        }
    }
}