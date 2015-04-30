#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Core.
//
// Woz.Core is free software: you can redistribute it 
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

namespace Woz.Core.Geometry
{
    public struct Size : IEquatable<Size>
    {
        private readonly int _width;
        private readonly int _height;

        private Size(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public static Size Create(int width, int height)
        {
            return new Size(width, height);
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public override bool Equals(object obj)
        {
            return obj is Size && Equals((Size)obj);
        }

        public bool Equals(Size other)
        {
            return _width == other._width && _height == other._height;
        }

        public static bool operator ==(Size size1, Size size2)
        {
            return size1.Equals(size2);
        }

        public static bool operator !=(Size size1, Size size2)
        {
            return !size1.Equals(size2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = _width.GetHashCode();
                hash = (hash * 397) ^ _height.GetHashCode();
                return hash;
            }
        }
    }
}