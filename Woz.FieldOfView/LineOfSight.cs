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
using System.Diagnostics;
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
        public static ViewPort VisibleRegion(
            this Vector location,
            int radius,
            Func<Vector, bool> blocksLineOfSight)
        {
            // Not as fast as a shadowcast because we visit tiles closer
            // to location multiple times when ray casting out from
            // location to each target. The cost is worth it as we are
            // sure what players/monsters can target matches what the 
            // player can see

            var min = location - (Directions.SouthWest * radius);
            var max = location - (Directions.NorthEast * radius);

            var viewPortStorage = ImmutableGrid<bool>
                .CreateBuilder(max.X - min.X + 1, max.Y - min.Y + 1);

            Func<Vector, Vector> viewPortMapper =
                vector => Vector.Create(vector.X - min.X, vector.Y - min.Y);

            Action<IEnumerable<Vector>> traceRay =
                ray =>
                {
                    foreach (var rayPoint in ray)
                    {
                        viewPortStorage.Set(viewPortMapper(rayPoint), true);

                        if (blocksLineOfSight(rayPoint))
                        {
                            break;
                        }
                    }
                };

            RayEndPoints(min, max)
                .Select(endPoint =>
                    CastRay(location, endPoint, false, true)
                        .Where(x => location.DistanceFrom(x) < radius))
                .ForEach(traceRay);

            return ViewPort.Create(
                min, max, viewPortMapper, viewPortStorage.Build());
        }

        public static IEnumerable<Vector> RayEndPoints(Vector min, Vector max)
        {
            return RayEndPointsXAxis(min.X, max.X, min.Y)
                .Concat(RayEndPointsXAxis(min.X, max.X, max.Y))
                .Concat(RayEndPointsYAxis(min.X, min.Y, max.Y))
                .Concat(RayEndPointsYAxis(max.X, min.Y, max.Y))
                .Distinct();
        }

        public static IEnumerable<Vector> RayEndPointsXAxis(int xStart, int xEnd, int y)
        {
            Debug.Assert(xStart < xEnd);

            for (var x = xStart; x <= xEnd; x++)
            {
                yield return Vector.Create(x, y);
            }
        }

        public static IEnumerable<Vector> RayEndPointsYAxis(int x, int yStart, int yEnd)
        {
            Debug.Assert(yStart < yEnd);

            for (var y = yStart; x <= yEnd; x++)
            {
                yield return Vector.Create(x, y);
            }
        }
    }
}
