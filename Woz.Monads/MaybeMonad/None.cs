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
using System.Diagnostics;

namespace Woz.Monads.MaybeMonad
{
    internal class None<T> : IMaybe<T>
    {
        public bool HasValue
        {
            get { return false; }
        }

        public T Value
        {
            get { throw new InvalidOperationException("Maybe has no value"); }
        }

        // M<T> -> Func<T, TResult> -> M<TResult>
        public IMaybe<TResult> Select<TResult>(Func<T, TResult> operation)
        {
            Debug.Assert(operation != null);

            return Maybe<TResult>.None;
        }

        // M<T> -> Func<T, M<TResult>> -> M<TResult>
        public IMaybe<TResult> SelectMany<TResult>(Func<T, IMaybe<TResult>> operation)
        {
            Debug.Assert(operation != null);

            return Maybe<TResult>.None;
        }

        // M<T1> -> Func<T1, M<T2>> -> Func<T1, T2, TResult> -> M<TResult>
        public IMaybe<TResult> SelectMany<T2, TResult>(
            Func<T, IMaybe<T2>> transform, Func<T, T2, TResult> composer)
        {
            Debug.Assert(transform != null);
            Debug.Assert(composer != null);

            return Maybe<TResult>.None;
        }

        public IMaybe<T> Where(Func<T, bool> predicate)
        {
            Debug.Assert(predicate != null);

            return this;
        }

        public void Do(Action<T> operation)
        {
            Debug.Assert(operation != null);

            // Do nothing
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            Debug.Assert(some != null);
            Debug.Assert(none != null);

            return none();
        }

        public void Match(Action<T> some, Action none)
        {
            Debug.Assert(some != null);
            Debug.Assert(none != null);

            none();
        }

        public T OrElseDefault()
        {
            return default(T);
        }

        public T OrElse(T defaultValue)
        {
            return defaultValue;
        }

        public T OrElse(Func<T> valueFactory)
        {
            Debug.Assert(valueFactory != null);

            return valueFactory();
        }

        public T OrElseThrow(Func<Exception> exceptionFactory)
        {
            Debug.Assert(exceptionFactory != null);

            throw exceptionFactory();
        }

        public bool Equals(IMaybe<T> other)
        {
            return other != null && !other.HasValue;
        }

        public override bool Equals(object obj)
        {
            var none = obj as None<T>;
            return none != null && Equals(none);
        }

        public override int GetHashCode()
        {
            return false.GetHashCode();
        }
    }
}