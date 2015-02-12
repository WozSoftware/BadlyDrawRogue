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
    internal struct Some<T> : IMaybe<T>
    {
        private readonly T _value;

        public Some(T value)
        {
            _value = value;
        }

        public bool HasValue
        {
            get { return true; }
        }

        public T Value
        {
            get { return _value; }
        }

        public IMaybe<TResult> Bind<TResult>(Func<T, IMaybe<TResult>> operation)
        {
            return operation(_value);
        }

        public T OrElseDefault()
        {
            return _value;
        }

        public T OrElse(T defaultValue)
        {
            return _value;
        }

        public T OrElse(Func<Exception> builder)
        {
            return _value;
        }

        public bool Equals(IMaybe<T> other)
        {
            return other.HasValue && _value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is Some<T> && Equals((Some<T>)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}