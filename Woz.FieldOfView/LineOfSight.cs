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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Woz.Core.Geometry;
using Woz.Immutable.Collections;
using Woz.Linq.Collections;

namespace Woz.FieldOfView
{
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

        [SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
        public static IImmutableGrid<bool> VisibleRegion(
            this Vector location,
            int radius,
            Func<Vector, bool> blocksLineOfSight)
        {
            // Not as fast as a shadowcast because we visit tiles closer
            // to location multiple times when ray casting out from
            // location to each target. The cost is worth it as we are
            // sure what players/monsters can target matches what the 
            // player can see

            Func<Vector, Vector> gridMapper = 
                vector => Vector.Create(
                    vector.X - (location.X - radius), 
                    vector.Y - (location.Y - radius));

            var axisLength = radius * 2 + 1;

            var gridBuilder = ImmutableGrid<bool>
                .CreateBuilder(axisLength, axisLength);

            var xs = Enumerable.Range(location.X - radius, axisLength);
            var ys = Enumerable.Range(location.Y - radius, axisLength).ToArray();
            var coordinates = xs.SelectMany(x => ys, Vector.Create);

            coordinates
                .Where(target => location.DistanceFrom(target) < radius)
                .Where(target => location.CanSee(target, blocksLineOfSight))
                .ForEach(target => gridBuilder.Set(gridMapper(target), true));

            return gridBuilder.Build();
        }
    }
}
