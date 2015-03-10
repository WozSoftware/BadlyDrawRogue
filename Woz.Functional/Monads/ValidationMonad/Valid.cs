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

namespace Woz.Functional.Monads.ValidationMonad
{
    internal class Valid<T> : IValidation<T>
    {
        private readonly T _value;

        internal Valid(T value)
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

        public string ErrorMessage
        {
            get
            {
                throw new InvalidOperationException(
                    "Validation has not failed, no error message");
            }
        }

        // M<T> -> Func<T, TResult> -> M<TResult>
        public IValidation<TResult> Select<TResult>(
            Func<T, TResult> operation)
        {
            Debug.Assert(operation != null);

            return operation(_value).ToValid();
        }

        // M<T> -> Func<T, M<TResult>> -> M<TResult>
        public IValidation<TResult> SelectMany<TResult>(
            Func<T, IValidation<TResult>> operation)
        {
            Debug.Assert(operation != null);

            return operation(_value);
        }

        // M<T1> -> Func<T1, M<T2>> -> Func<T1, T2, TResult> -> M<TResult>
        public IValidation<TResult> SelectMany<T2, TResult>(
            Func<T, IValidation<T2>> transform, Func<T, T2, TResult> composer)
        {
            Debug.Assert(transform != null);
            Debug.Assert(composer != null);

            var value1 = _value; // Capture for closure
            return transform(value1)
                .SelectMany(value2 => composer(value1, value2).ToValid());
        }

        public IValidation<T> ThrowOnError(
            Func<string, Exception> exceptionFactory)
        {
            return this;
        }

        public T OrElse(Func<string, Exception> exceptionFactory)
        {
            Debug.Assert(exceptionFactory != null);

            return _value;
        }

        public bool Equals(IValidation<T> other)
        {
            return 
                other != null && 
                other.IsValid && 
                _value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            var valid = obj as Valid<T>;
            return valid != null && Equals(valid);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}