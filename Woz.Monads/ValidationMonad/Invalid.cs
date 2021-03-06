#region License
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
using Woz.Monads.TryMonad;

namespace Woz.Monads.ValidationMonad
{
    internal class Invalid<T> : IValidation<T>
    {
        private readonly string _errorMessage;

        internal Invalid(string errorMessage)
        {
            Debug.Assert(!string.IsNullOrEmpty(errorMessage));

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


        public IValidation<TResult> Select<TResult>(
            Func<T, TResult> operation)
        {
            Debug.Assert(operation != null);

            return _errorMessage.ToInvalid<TResult>();
        }

        public IValidation<TResult> SelectMany<TResult>(
            Func<T, IValidation<TResult>> operation)
        {
            Debug.Assert(operation != null);

            return _errorMessage.ToInvalid<TResult>();
        }

        // M<T1> -> Func<T1, M<T2>> -> Func<T1, T2, TResult> -> M<TResult>
        public IValidation<TResult> SelectMany<T2, TResult>(
            Func<T, IValidation<T2>> transform, Func<T, T2, TResult> composer)
        {
            Debug.Assert(transform != null);
            Debug.Assert(composer != null);

            return _errorMessage.ToInvalid<TResult>();
        }

        public IValidation<T> WithErrorFrom<T2>(IValidation<T2> other)
        {
            return this;
        }

        public T OrElse(Func<string, Exception> exceptionFactory)
        {
            Debug.Assert(exceptionFactory != null);

            throw exceptionFactory(_errorMessage);
        }

        public ITry<T> ToTry(Func<string, Exception> exceptionFactory)
        {
            Debug.Assert(exceptionFactory != null);

            return exceptionFactory(_errorMessage).ToException<T>();
        }

        public bool Equals(IValidation<T> other)
        {
            return
                other != null &&
                !other.IsValid && 
                _errorMessage.Equals(other.ErrorMessage);
        }

        public override bool Equals(object obj)
        {
            var invalid = obj as Invalid<T>;
            return invalid != null && Equals(invalid);
        }

        public override int GetHashCode()
        {
            return _errorMessage.GetHashCode();
        }
    }
}