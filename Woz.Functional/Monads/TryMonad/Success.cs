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
using System.Diagnostics;

namespace Woz.Functional.Monads.TryMonad
{
    internal class Success<T> : ITry<T>
    {
        private readonly T _value;

        internal Success(T value)
        {
            _value = value;
        }

        public bool IsValid
        {
            get { return true; }
        }

        public T Value
        {
            get { return _value; }
        }

        public Exception Error
        {
            get
            {
                throw new InvalidOperationException(
                    "Try has not failed, no exception present");
            }
        }

        // M<T> -> Func<T, TResult> -> M<TResult>
        public ITry<TResult> Select<TResult>(Func<T, TResult> operation)
        {
            Debug.Assert(operation != null);

            var value = _value; // Capture for closure
            return Try.Catcher(() => operation(value).ToSuccess());
        }

        // M<T> -> Func<T, M<TResult>> -> M<TResult>
        public ITry<TResult> SelectMany<TResult>(
            Func<T, ITry<TResult>> operation)
        {
            Debug.Assert(operation != null);

            var value = _value; // Capture for closure
            return Try.Catcher(() => operation(value));
        }

        // M<T1> -> Func<T1, M<T2>> -> Func<T1, T2, TResult> -> M<TResult>
        public ITry<TResult> SelectMany<T2, TResult>(
            Func<T, ITry<T2>> transform, Func<T, T2, TResult> composer)
        {
            Debug.Assert(transform != null);
            Debug.Assert(composer != null);

            return SelectMany(x =>
                transform(x).SelectMany(y =>
                    composer(x, y).ToSuccess()));
        }

        public ITry<T> ThrowOnError(Func<Exception, Exception> exceptionBuilder)
        {
            Debug.Assert(exceptionBuilder != null);

            return this;
        }

        public ITry<T> ThrowOnError()
        {
            return this;
        }

        public T OrElse(Func<Exception, Exception> exceptionBuilder)
        {
            Debug.Assert(exceptionBuilder != null);

            return _value;
        }

        public T OrElseException()
        {
            return _value;
        }

        public bool Equals(ITry<T> other)
        {
            return 
                other != null && 
                other.IsValid && 
                _value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            var successs = obj as Success<T>;
            return successs != null && Equals(successs);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}