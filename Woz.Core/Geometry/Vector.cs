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
    public struct Vector : IEquatable<Vector>
    {
        private readonly int _x;
        private readonly int _y;

        private Vector(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public static Vector Create(int x, int y)
        {
            return new Vector(x, y);
        }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public Vector Abs()
        {
            return new Vector(Math.Abs(_x), Math.Abs(_y));
        }

        public double DistanceFrom(Vector target)
        {
            var diffX = Math.Abs(_x - target._x);
            var diffY = Math.Abs(_y - target._y);

            return Math.Sqrt((diffX * diffX) + (diffY * diffY));
        }

        public override bool Equals(object obj)
        {
            return obj is Vector && Equals((Vector)obj);
        }

        public bool Equals(Vector other)
        {
            return _x == other._x && _y == other._y;
        }

        public static bool operator ==(Vector vector1, Vector vector2)
        {
            return vector1.Equals(vector2);
        }

        public static bool operator !=(Vector vector1, Vector vector2)
        {
            return !vector1.Equals(vector2);
        }

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1._x + vector2._x, vector1._y + vector2._y);
        }

        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector(vector1._x - vector2._x, vector1._y - vector2._y);
        }

        public static Vector operator *(Vector vector, int scale)
        {
            return new Vector(vector._x * scale, vector._y * scale);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = _x.GetHashCode();
                hash = (hash * 397) ^ _y.GetHashCode();
                return hash;
            }
        }
    }
}