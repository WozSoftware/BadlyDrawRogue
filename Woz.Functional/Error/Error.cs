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
using System.Collections.Generic;

namespace Woz.Functional.Error
{
    public struct Error<T>
    {
        private readonly T _value;
        private readonly string _errorMessage;

        internal Error(T value)
        {
            _value = value;
            _errorMessage = null;
        }

        internal Error(string errorMessage)
        {
            _value = default(T);
            _errorMessage = errorMessage;
        }

        public T Value
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException(
                        string.Format("Error has occurred: {0}", _errorMessage));    
                }

                return _value;
            }
        }

        public bool IsValid
        {
            get { return _errorMessage == null; }    
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public Error<TResult> Bind<TResult>(Func<T, Error<TResult>> operation)
        {
            return IsValid
                ? operation(_value)
                : _errorMessage.ToError<TResult>();
        }

        public Error<TResult> TryBind<TResult>(Func<T, Error<TResult>> operation)
        {
            try
            {
                return Bind(operation);
            }
            catch (Exception ex)
            {
                return ex.Message.ToError<TResult>();
            }
        }

        public Error<T> ThrowOnError(Func<string, Exception> exceptionBuilder)
        {
            if (!IsValid)
            {
                throw exceptionBuilder(_errorMessage);
            }

            return this;
        }

        public T Return(Func<string, Exception> exceptionBuilder)
        {
            return ThrowOnError(exceptionBuilder).Value;
        }

        public bool Equals(Error<T> other)
        {
            return IsValid
                ? EqualityComparer<T>.Default.Equals(_value, other._value)
                : _errorMessage.Equals(other._errorMessage);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is Error<T> && Equals((Error<T>)obj);
        }

        public override int GetHashCode()
        {
            return IsValid
                ? EqualityComparer<T>.Default.GetHashCode(_value)
                : EqualityComparer<string>.Default.GetHashCode(_errorMessage);
        }

        public static implicit operator Error<T>(Error<Error<T>> doubleError)
        {
            return doubleError.IsValid 
                ? doubleError.Value 
                : doubleError.ErrorMessage.ToError<T>();
        }

        public static bool operator ==(Error<T> left, Error<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Error<T> left, Error<T> right)
        {
            return !left.Equals(right);
        }
    }
}