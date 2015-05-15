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
using System.Collections.Generic;
using System.Linq;
using Woz.Core.Geometry;

namespace Woz.FieldOfView
{
    // This algorithm only processes within octant 1.
    // When working in other octants the coordinates are mapped
    // such that they overlap on octant 1. See Octants

    public static class LineOfSight
    {
        public static IEnumerable<Vector> CastRay(this Vector start, Vector end)
        {
            return CastRay(start, end, false, false);
        }

        public static IEnumerable<Vector> CastRay(
            this Vector start, Vector end, bool includeStart, bool includeEnd)
        {
            var delta = (start - end).Abs();

            var current = start;

            var xIncrement = (end.X > start.X) ? 1 : -1;
            var yIncrement = (end.Y > start.Y) ? 1 : -1;

            var error = delta.X - delta.Y;
            var errorCorrect = delta * 2;

            while (true)
            {
                if ((current == start && includeStart) ||
                    (current == end && includeEnd) ||
                    (current != start && current != end))
                {
                    yield return current;
                }

                if (current == end)
                {
                    yield break;
                }

                if (error > 0)
                {
                    current = Vector.Create(current.X + xIncrement, current.Y);
                    error -= errorCorrect.Y;
                }
                else if (error < 0)
                {
                    current = Vector.Create(current.X, current.Y + yIncrement);
                    error += errorCorrect.X;
                }
                else
                {
                    current = Vector.Create(
                        current.X + xIncrement,
                        current.Y + yIncrement);
                }
            }
        }

        public static bool CanSee(
            this Vector location, 
            Vector target, 
            Func<Vector, bool> blocksLineOfSight)
        {
            return CastRay(location, target).All(x => !blocksLineOfSight(x));
        }
    }
}
