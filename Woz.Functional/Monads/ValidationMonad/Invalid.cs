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
    internal struct Invalid<T> : IValidation<T>
    {
        private readonly string _errorMessage;

        internal Invalid(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public bool IsValid
        {
            get { return false; }
        }

        public T Value
        {
            get
            {
                throw new InvalidOperationException(
                    string.Format("Try has no value, failed with: {0}", _errorMessage));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }


        public IValidation<TResult> Map<TResult>(
            Func<T, TResult> operation)
        {
            return _errorMessage.ToInvalid<TResult>();
        }

        public IValidation<TResult> FlatMap<TResult>(
            Func<T, IValidation<TResult>> operation)
        {
            return _errorMessage.ToInvalid<TResult>();
        }

        public IValidation<T> ThrowOnError(
            Func<string, Exception> exceptionBuilder)
        {
            throw exceptionBuilder(_errorMessage);
        }

        public T OrElse(Func<string, Exception> exceptionBuilder)
        {
            throw exceptionBuilder(_errorMessage);
        }

        public bool Equals(IValidation<T> other)
        {
            return !other.IsValid && _errorMessage.Equals(other.ErrorMessage);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is Invalid<T> && Equals((Invalid<T>)obj);
        }

        public override int GetHashCode()
        {
            return _errorMessage.GetHashCode();
        }
    }
}