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

namespace Woz.Functional.Monads.ValidationMonad
{
    internal struct Valid<T> : IValidation<T>
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

        public IValidation<TResult> Map<TResult>(
            Func<T, TResult> operation)
        {
            return operation(_value).ToValid();
        }

        public IValidation<TResult> FlatMap<TResult>(
            Func<T, IValidation<TResult>> operation)
        {
            return operation(_value);
        }

        public IValidation<T> ThrowOnError(
            Func<string, Exception> exceptionBuilder)
        {
            return this;
        }

        public T OrElse(Func<string, Exception> exceptionBuilder)
        {
            return _value;
        }

        public bool Equals(IValidation<T> other)
        {
            return other.IsValid && _value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is Valid<T> && Equals((Valid<T>)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}