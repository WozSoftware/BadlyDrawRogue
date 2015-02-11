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
    internal struct Failed<T> : ITry<T>
    {
        private readonly string _errorMessage;

        internal Failed(string errorMessage)
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


        public ITry<TResult> Bind<TResult>(Func<T, ITry<TResult>> operation)
        {
            return _errorMessage.ToFailed<TResult>();
        }

        public ITry<TResult> TryBind<TResult>(Func<T, ITry<TResult>> operation)
        {
            return _errorMessage.ToFailed<TResult>();
        }

        public ITry<T> ThrowOnError(Func<string, Exception> exceptionBuilder)
        {
            throw exceptionBuilder(_errorMessage);
        }

        public T ReturnOrThrow(Func<string, Exception> exceptionBuilder)
        {
            throw exceptionBuilder(_errorMessage);
        }

        public bool Equals(ITry<T> other)
        {
            return
                !other.IsValid &&
                EqualityComparer<string>.Default.Equals(_errorMessage, other.ErrorMessage);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is Failed<T> && Equals((Failed<T>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<string>.Default.GetHashCode(_errorMessage);
        }
    }
}