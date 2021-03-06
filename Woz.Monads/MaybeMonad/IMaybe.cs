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

using System;

namespace Woz.Monads.MaybeMonad
{
    public interface IMaybe<T> : IEquatable<IMaybe<T>>
    {
        bool HasValue { get; }

        T Value { get; }

        // M<T> -> Func<T, TResult> -> M<TResult>
        IMaybe<TResult> Select<TResult>(Func<T, TResult> operation);

        // M<T> -> Func<T, M<TResult>> -> M<TResult>
        IMaybe<TResult> SelectMany<TResult>(Func<T, IMaybe<TResult>> operation);

        // M<T1> -> Func<T1, M<T2>> -> Func<T1, T2, TResult> -> M<TResult>
        IMaybe<TResult> SelectMany<T2, TResult>(
            Func<T, IMaybe<T2>> transform, Func<T, T2, TResult> composer);

        IMaybe<T> Where(Func<T, bool> predicate);
        void Do(Action<T> operation);

        TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none);
        void Match(Action<T> some, Action none);

        T OrElseDefault();
        T OrElse(T defaultValue);
        T OrElse(Func<T> valueFactory);
        T OrElseThrow(Func<Exception> exceptionFactory);
    }
}