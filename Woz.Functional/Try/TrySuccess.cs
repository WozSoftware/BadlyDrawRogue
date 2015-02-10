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

namespace Woz.Functional.Try
{
    internal struct TrySuccess<T> : ITry<T>
    {
        private readonly T _value;

        internal TrySuccess(T value)
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
                    "Try has not failed, no error message");
            }
        }

        public ITry<TResult> Bind<TResult>(Func<T, ITry<TResult>> operation)
        {
            return operation(_value);
        }

        public ITry<TResult> TryBind<TResult>(Func<T, ITry<TResult>> operation)
        {
            try
            {
                return Bind(operation);
            }
            catch (Exception ex)
            {
                return ex.Message.ToFailed<TResult>();
            }
        }

        public ITry<T> ThrowOnError(Func<string, Exception> exceptionBuilder)
        {
            return this;
        }

        public T ReturnOrThrow(Func<string, Exception> exceptionBuilder)
        {
            return _value;
        }

        public bool Equals(ITry<T> other)
        {
            return 
                other.IsValid && 
                EqualityComparer<T>.Default.Equals(_value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is TrySuccess<T> && Equals((TrySuccess<T>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(_value);
        }
    }
}