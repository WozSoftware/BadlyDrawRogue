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
        public static readonly Vector North = new Vector(0, 1);
        public static readonly Vector NorthEast = new Vector(1, 1);
        public static readonly Vector East = new Vector(1, 0);
        public static readonly Vector SouthEast = new Vector(1, -1);
        public static readonly Vector South = new Vector(0, -1);
        public static readonly Vector SouthWest = new Vector(-1, -1);
        public static readonly Vector West = new Vector(-1, 0);
        public static readonly Vector NorthWest = new Vector(-1, 1);

        public readonly int DeltaX;
        public readonly int DeltaY;

        public Vector(int deltaX, int deltaY)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector && Equals((Vector)obj);
        }

        public bool Equals(Vector other)
        {
            return DeltaX == other.DeltaX && DeltaY == other.DeltaY;
        }

        public static bool operator
            ==(Vector vector1, Vector vector2)
        {
            return vector1.Equals(vector2);
        }

        public static bool operator
            !=(Vector vector1, Vector vector2)
        {
            return !vector1.Equals(vector2);
        }

        public override int GetHashCode()
        {
            return DeltaX ^ DeltaY;
        }
    }
}