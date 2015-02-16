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

namespace Woz.Functional.Monads.TryMonad
{
    internal struct Success<T> : ITry<T>
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

        public ITry<TResult> Map<TResult>(Func<T, TResult> operation)
        {
            try
            {
                return operation(_value).ToSuccess();
            }
            catch (Exception ex)
            {
                return ex.ToException<TResult>();
            }
        }

        public ITry<TResult> FlatMap<TResult>(Func<T, ITry<TResult>> operation)
        {
            try
            {
                return operation(_value);
            }
            catch (Exception ex)
            {
                return ex.ToException<TResult>();
            }
        }

        public ITry<T> ThrowOnError(Func<Exception, Exception> exceptionBuilder)
        {
            return this;
        }

        public ITry<T> ThrowOnError()
        {
            return this;
        }

        public T OrElse(Func<Exception, Exception> exceptionBuilder)
        {
            return _value;
        }

        public T OrElseException()
        {
            return _value;
        }

        public bool Equals(ITry<T> other)
        {
            return other.IsValid && _value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is Success<T> && Equals((Success<T>)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}