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
    internal class Failed<T> : ITry<T>
    {
        private readonly Exception _error;

        internal Failed(Exception error)
        {
            Debug.Assert(error != null);

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

        // M<T> -> Func<T, TResult> -> M<TResult>
        public ITry<TResult> Select<TResult>(Func<T, TResult> operation)
        {
            Debug.Assert(operation != null);

            return _error.ToException<TResult>();
        }

        // M<T> -> Func<T, M<TResult>> -> M<TResult>
        public ITry<TResult> SelectMany<TResult>(
            Func<T, ITry<TResult>> operation)
        {
            Debug.Assert(operation != null);

            return _error.ToException<TResult>();
        }

        // M<T1> -> Func<T1, M<T2>> -> Func<T1, T2, TResult> -> M<TResult>
        public ITry<TResult> SelectMany<T2, TResult>(
            Func<T, ITry<T2>> transform, Func<T, T2, TResult> composer)
        {
            Debug.Assert(transform != null);
            Debug.Assert(composer != null);

            return _error.ToException<TResult>();
        }

        public ITry<T> ThrowOnError(Func<Exception, Exception> exceptionBuilder)
        {
            Debug.Assert(exceptionBuilder != null);

            throw exceptionBuilder(_error);
        }

        public ITry<T> ThrowOnError()
        {
            throw _error;
        }

        public T OrElse(Func<Exception, Exception> exceptionBuilder)
        {
            Debug.Assert(exceptionBuilder != null);

            throw exceptionBuilder(_error);
        }

        public T OrElseException()
        {
            throw _error;
        }

        public bool Equals(ITry<T> other)
        {
            return 
                other != null && 
                !other.IsValid && 
                _error.Equals(other.Error);
        }

        public override bool Equals(object obj)
        {
            var failed = obj as Failed<T>;
            return failed != null && Equals(failed);
        }

        public override int GetHashCode()
        {
            return _error.GetHashCode();
        }
    }
}