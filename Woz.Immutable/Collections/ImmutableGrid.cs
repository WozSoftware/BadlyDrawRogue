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
using System.Drawing;
using System.Linq;
using Woz.Monads.MaybeMonad;

namespace Woz.Immutable.Collections
{
    public sealed class ImmutableGrid<T> : IImmutableGrid<T>
    {
        #region Builder
        public sealed class Builder
        {
            private readonly Size _size;
            private T[][] _buffer;
            private readonly T[][] _source; 
            private bool _built;

            internal Builder(Size size)
            {
                Debug.Assert(size.Width >= 0 && size.Height >= 0);

                _size = size;

                _buffer = new T[_size.Width][];
                for (var x = 0; x < size.Width; x++)
                {
                    _buffer[x] = new T[size.Height];
                }
            }

            /// <summary>
            /// This entry is intended for updates on an existing immutable
            /// array buffer. It is a lazy operation that only constructs a 
            /// new buffer when actually updated. If no update is performed
            /// it will result in the original buffer being used
            /// </summary>
            /// <param name="data">The internal buffer of an immutable array
            /// </param>
            internal Builder(T[][] data)
            {
                var width = data.Length;
                var height = width > 0 ? data[0].Length : 0;

                _size = new Size(width, height);
                _source = data;
            }

            public T this[int x, int y]
            {
                get
                {
                    Debug.Assert(IsValidLocation(x, y));

                    return _buffer.ToMaybe()
                        .SelectMany(buffer => buffer[x].ToMaybe())
                        .Select(column => column[y])
                        .OrElse(() => _source[x][y]);
                }
            }

            public T this[Point location]
            {
                get { return this[location.X, location.Y]; }
            }

            public int Width
            {
                get { return _size.Width; }
            }

            public int Height
            {
                get { return _size.Height; }
            }

            public Size Size
            {
                get { return _size; }
            }

            public bool IsValidLocation(int x, int y)
            {
                return x >= 0 && x < _size.Width && y >= 0 && y < _size.Height;
            }

            public Builder Set(int x, int y, T item)
            {
                Debug.Assert(IsValidLocation(x, y));

                if (_built)
                {
                    throw new InvalidOperationException(
                        "ImmutableGrid<T>.Builder already built");
                }

                if (_buffer == null)
                {
                    _buffer = new T[_size.Width][];
                }

                if (_buffer[x] == null)
                {
                    _buffer[x] = (T[])_source[x].Clone();
                }

                _buffer[x][y] = item;

                return this;
            }

            public Builder Set(Point location, T item)
            {
                return Set(location.X, location.Y, item);
            }

            public ImmutableGrid<T> Build()
            {
                if (_built)
                {
                    throw new InvalidOperationException(
                        "ImmutableGrid<T>.Builder already built");
                }

                _built = true;

                if (_buffer == null)
                {
                    _buffer = _source;
                }
                else
                {
                    for (var x = 0; x < _size.Width; x++)
                    {
                        if (_buffer[x] == null)
                        {
                            _buffer[x] = _source[x];
                        }
                    }
                }

                return new ImmutableGrid<T>(_size, _buffer);
            }
        }
        #endregion

        private readonly Size _size;
        private readonly T[][] _storage;
 
        private ImmutableGrid(Size size, T[][] items)
        {
            _size = size;
            _storage = items;
        }

        public static ImmutableGrid<T> Create(int width, int height)
        {
            return CreateBuilder(new Size(width, height)).Build();
        }

        public static ImmutableGrid<T> Create(Size size)
        {
            return CreateBuilder(size).Build();
        }

        public static Builder CreateBuilder(int width, int height)
        {
            return new Builder(new Size(width, height));
        }

        public static Builder CreateBuilder(Size size)
        {
            return new Builder(size);
        }

        public T this[int x, int y]
        {
            get
            {
                Debug.Assert(IsValidLocation(x, y));

                return _storage[x][y];
            }
        }

        public T this[Point location]
        {
            get { return _storage[location.X][location.Y]; }
        }

        public int Width
        {
            get { return _size.Width; }
        }

        public int Height
        {
            get { return _size.Height; }
        }

        public Size Size
        {
            get { return _size; }
        }

        public bool IsValidLocation(int x, int y)
        {
            return x >= 0 && x < _size.Width && y >= 0 && y < _size.Height;
        }

        public bool IsValidLocation(Point location)
        {
            return IsValidLocation(location.X, location.Y);
        }

        public ImmutableGrid<T> Set(int x, int y, T item)
        {
            return ToBuilder().Set(x, y, item).Build();
        }

        IImmutableGrid<T> IImmutableGrid<T>.Set(int x, int y, T item)
        {
            return ToBuilder().Set(x, y, item).Build();
        }

        public ImmutableGrid<T> Set(Point location, T item)
        {
            return ToBuilder().Set(location.X, location.Y, item).Build();
        }

        IImmutableGrid<T> IImmutableGrid<T>.Set(Point location, T item)
        {
            return ToBuilder().Set(location.X, location.Y, item).Build();
        }

        public Builder ToBuilder()
        {
            return new Builder(_storage);
        }

        public IEnumerator<Tuple<Point, T>> GetEnumerator()
        {
            var query = 
                from x in Enumerable.Range(0, Width)
                from y in Enumerable.Range(0, Height)
                select Tuple.Create(new Point(x, y), _storage[x][y]);

            return query.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}