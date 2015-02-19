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

namespace Woz.Functional.Monads.MaybeMonad
{
    internal class Some<T> : IMaybe<T>
    {
        private readonly T _value;

        public Some(T value)
        {
            _value = value;
        }

        public bool HasValue
        {
            get { return true; }
        }

        public T Value
        {
            get { return _value; }
        }

        // M<T> -> Func<T, TResult> -> M<TResult>
        public IMaybe<TResult> Select<TResult>(Func<T, TResult> operation)
        {
            return operation(_value).ToMaybe();
        }

        // M<T> -> Func<T, M<TResult>> -> M<TResult>
        public IMaybe<TResult> SelectMany<TResult>(Func<T, IMaybe<TResult>> operation)
        {
            return operation(_value);
        }

        // M<T1> -> Func<T1, M<T2>> -> Func<T1, T2, TResult> -> M<TResult>
        public IMaybe<TResult> SelectMany<T2, TResult>(
            Func<T, IMaybe<T2>> transform, Func<T, T2, TResult> composer)
        {
            var value1 = _value; // Capture for closure
            return transform(value1)
                .SelectMany(value2 => composer(value1, value2).ToMaybe());
        }

        public IMaybe<T> Where(Func<T, bool> predicate)
        {
            return predicate(_value) ? this : Maybe<T>.Nothing;
        }

        public T OrElseDefault()
        {
            return _value;
        }

        public T OrElse(T defaultValue)
        {
            return _value;
        }

        public T OrElse(Func<T> builder)
        {
            return _value;
        }

        public T OrElse(Func<Exception> builder)
        {
            return _value;
        }

        public bool Equals(IMaybe<T> other)
        {
            return 
                other != null && 
                other.HasValue && 
                _value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            var some = obj as Some<T>;
            return some != null && Equals(some);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}