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
        public readonly int X;
        public readonly int Y;

        private Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector Create(int x, int y)
        {
            return new Vector(x, y);
        }

        public Vector Add(Vector vector)
        {
            return new Vector(X + vector.X, Y + vector.Y);
        }

        public Vector ScaleBy(int scale)
        {
            return new Vector(X * scale, Y * scale);
        }

        public double DistanceFrom(Vector target)
        {
            var diffX = Math.Abs(X - target.X);
            var diffY = Math.Abs(Y - target.Y);

            return Math.Sqrt((diffX * diffX) + (diffY * diffY));
        }

        public override bool Equals(object obj)
        {
            return obj is Vector && Equals((Vector)obj);
        }

        public bool Equals(Vector other)
        {
            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(Vector vector1, Vector vector2)
        {
            return vector1.Equals(vector2);
        }

        public static bool operator !=(Vector vector1, Vector vector2)
        {
            return !vector1.Equals(vector2);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }
    }
}