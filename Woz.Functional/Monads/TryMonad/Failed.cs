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
    internal struct Failed<T> : ITry<T>
    {
        private readonly Exception _error;

        internal Failed(Exception error)
        {
            _error = error;
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
                    string.Format(
                        "Try has no value, failed with: {0}", 
                        _error.Message));
            }
        }

        public Exception Error
        {
            get { return _error; }
        }


        public ITry<TResult> Map<TResult>(Func<T, TResult> operation)
        {
            return _error.ToException<TResult>();
        }

        public ITry<TResult> FlatMap<TResult>(Func<T, ITry<TResult>> operation)
        {
            return _error.ToException<TResult>();
        }

        public ITry<T> ThrowOnError(Func<Exception, Exception> exceptionBuilder)
        {
            throw exceptionBuilder(_error);
        }

        public ITry<T> ThrowOnError()
        {
            throw _error;
        }

        public T OrElse(Func<Exception, Exception> exceptionBuilder)
        {
            throw exceptionBuilder(_error);
        }

        public T OrElseException()
        {
            throw _error;
        }

        public bool Equals(ITry<T> other)
        {
            return !other.IsValid && _error.Equals(other.Error);
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
            return _error.GetHashCode();
        }
    }
}