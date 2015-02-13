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

namespace Woz.Functional.Maybe
{
    public struct Nothing<T> : IMaybe<T>
    {
        public bool HasValue
        {
            get { return false; }
        }

        public T Value
        {
            get { throw new InvalidOperationException("Maybe has no value"); }
        }

        public IMaybe<TResult> Bind<TResult>(Func<T, IMaybe<TResult>> operation)
        {
            return new Nothing<TResult>();
        }

        public T OrElseDefault()
        {
            return default(T);
        }

        public T OrElse(T defaultValue)
        {
            return defaultValue;
        }

        public T OrElse(Func<T> builder)
        {
            return builder();
        }

        public T OrElse(Func<Exception> builder)
        {
            throw builder();
        }

        public bool Equals(IMaybe<T> other)
        {
            return !other.HasValue;
        }

        public override bool Equals(object obj)
        {
            return obj is Nothing<T>;
        }

        public override int GetHashCode()
        {
            return false.GetHashCode();
        }
    }
}