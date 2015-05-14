#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.FieldOfView.
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
using Woz.Core.Geometry;

namespace Woz.FieldOfView
{
    public static class Octants
    {
        // The octants are divided as follows
        //
        // \77|88/
        // 6\7|8/1
        // 66\|/11
        // ---+---    
        // 55/|\22
        // 5/4|3\2
        // /44|33\

        public static int
            DetermineOctant(Vector start, Vector target)
        {
            var distance = target - start;
            var absDistance = distance.Abs();

            if (absDistance.X >= absDistance.Y)
            {
                if (distance.X >= 0)
                {
                    if (distance.Y >= 0)
                    {
                        // Octant 1
                        return 1;
                    }

                    // Octant 2
                    return 2;
                }

                if (distance.Y >= 0)
                {
                    // Octant 6
                    return 6;
                }

                // Octant 5
                return 5;
            }

            if (distance.Y >= 0)
            {
                if (distance.X >= 0)
                {
                    // Octant 8
                    return 8;
                }

                // Octant 7;
                return 7;
            }

            if (distance.X >= 0)
            {
                // Octant 3
                return 3;
            }

            // Octant 4;
            return 4;
        }

        public static Func<Vector, Vector> CreateMapToOctant1(int octant)
        {
            switch (octant)
            {
                case 1:
                    return vector => vector;
                case 2:
                    return vector => Vector.Create(vector.X, -vector.Y);
                case 3:
                    return vector => Vector.Create(-vector.Y, vector.X);
                case 4:
                    return vector => Vector.Create(-vector.Y, -vector.X);
                case 5:
                    return vector => Vector.Create(-vector.X, -vector.Y);
                case 6:
                    return vector => Vector.Create(-vector.X, vector.Y);
                case 7:
                    return vector => Vector.Create(vector.Y, -vector.X);
                case 8:
                    return vector => Vector.Create(vector.Y, vector.X);
            }

            throw new ArgumentOutOfRangeException(
                string.Format("Unknown octant {0}", octant));
        }

        public static Func<Vector, Vector> CreateMapFromOctant1(int octant)
        {
            switch (octant)
            {
                case 1:
                    return vector => vector;
                case 2:
                    return vector => Vector.Create(vector.X, -vector.Y);
                case 3:
                    return vector => Vector.Create(vector.Y, -vector.X);
                case 4:
                    return vector => Vector.Create(-vector.Y, -vector.X);
                case 5:
                    return vector => Vector.Create(-vector.X, -vector.Y);
                case 6:
                    return vector => Vector.Create(-vector.X, vector.Y);
                case 7:
                    return vector => Vector.Create(-vector.Y, vector.X);
                case 8:
                    return vector => Vector.Create(vector.Y, vector.X);
            }

            throw new ArgumentOutOfRangeException(
                string.Format("Unknown octant {0}", octant));
        }
    }
}