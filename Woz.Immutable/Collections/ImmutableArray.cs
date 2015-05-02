#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Immutable.
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Woz.Monads.MaybeMonad;

namespace Woz.Immutable.Collections
{
    public static class ImmutableArray
    {
        public static ImmutableArray<T> Create<T>(IEnumerable<T> items)
        {
            return CreateBuilder(items).Build();
        }

        public static ImmutableArray<T>.Builder CreateBuilder<T>(IEnumerable<T> items)
        {
            return new ImmutableArray<T>.Builder(items);
        }
    }

    public sealed class ImmutableArray<T> : IImmutableArray<T>
    {
        #region Builder
        public sealed class Builder
        {
            private IMaybe<T[]> _buffer;
            private readonly T[] _source; 
            private bool _built;

            internal Builder(int length)
            {
                Debug.Assert(length >= 0);

                _buffer = (new T[length]).ToSome();
            }

            internal Builder(IEnumerable<T> data)
            {
                Debug.Assert(data != null);

                _buffer = data.ToArray().ToSome();
            }

            /// <summary>
            /// This entry is intended for updates on an existing immutable
            /// array buffer. It is a lazy operation that only constructs a 
            /// new buffer when actually updated. If no update is performed
            /// it will result in the original buffer being used
            /// </summary>
            /// <param name="data">The internal buffer of an immutable array
            /// </param>
            internal Builder(T[] data)
            {
                Debug.Assert(data != null);

                _buffer = Maybe<T[]>.None;
                _source = data;
            }

            public T this[int index]
            {
                get
                {
                    Debug.Assert(index >= 0);

                    return _buffer
                        .Select(buffer => buffer[index])
                        .OrElse(() => _source[index]);
                }
            }

            public int Count
            {
                get
                {
                    return _buffer
                        .Select(buffer => buffer.Length)
                        .OrElse(() => _source.Length);
                }
            }

            public Builder Set(int index, T value)
            {
                Debug.Assert(index >= 0);

                if (_built)
                {
                    throw new InvalidOperationException(
                        "ImmutableArray<T>.Builder already built");
                }

                if (!_buffer.HasValue)
                {
                    _buffer = _source.ToArray().ToMaybe();
                }

                _buffer.Value[index] = value;

                return this;
            }

            public ImmutableArray<T> Build()
            {
                if (_built)
                {
                    throw new InvalidOperationException(
                        "ImmutableArray<T>.Builder already built");
                }

                _built = true;

                return new ImmutableArray<T>(_buffer.OrElse(_source));
            }
        }
        #endregion

        private readonly T[] _storage;

        internal ImmutableArray(T[] items)
        {
            _storage = items;
        }

        public static ImmutableArray<T> Create(int length)
        {
            return CreateBuilder(length).Build();
        }

        public static Builder CreateBuilder(int length)
        {
            return new Builder(length);
        }

        public T this[int index]
        {
            get { return _storage[index]; }
        }

        public int Count
        {
            get { return _storage.Length; }
        }

        public IMaybe<int> IndexOf(Func<T, bool> predicate)
        {
            Debug.Assert(predicate != null);

            return Array
                .FindIndex(_storage, x => predicate(x))
                .ToSome()
                .Where(index => index != -1);
        }

        public ImmutableArray<T> Set(int index, T item)
        {
            return ToBuilder().Set(index, item).Build();
        }

        IImmutableArray<T> IImmutableArray<T>.Set(int index, T item)
        {
            return ToBuilder().Set(index, item).Build();
        }

        public Builder ToBuilder()
        {
            return new Builder(_storage);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _storage.Cast<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}