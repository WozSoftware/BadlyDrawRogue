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
using System.Linq;
using Woz.Core.Geometry;

namespace Woz.FieldOfView
{
    // This algorithm only processes within octant 1.
    // When working in other octants the coordinates are mapped
    // such that they overlap on octant 1. See Octants

    public static class LineOfSight
    {
        public const double Tolerance = 0.0001d;

        public struct StepCandidate
        {
            private readonly Vector _stepLocation;
            private readonly IEnumerable<TestCandidate> _testCandidates;

            private StepCandidate(
                Vector stepLocation, IEnumerable<TestCandidate> testCandidates)
            {
                _stepLocation = stepLocation;
                _testCandidates = testCandidates;
            }

            public static StepCandidate Create(
                Vector stepLocation, IEnumerable<TestCandidate> testCandidates)
            {
                return new StepCandidate(stepLocation, testCandidates);
            }

            public Vector StepLocation
            {
                get { return _stepLocation; }
            }

            public IEnumerable<TestCandidate> TestCandidates
            {
                get { return _testCandidates; }
            }
        }

        public struct TestCandidate
        {
            private readonly Vector _location;
            private readonly double _blockFactor;

            private TestCandidate(Vector location, double blockFactor)
            {
                _location = location;
                _blockFactor = blockFactor;
            }

            public static TestCandidate Create(
                Vector location, double blockFactor)
            {
                return new TestCandidate(location, blockFactor);
            }

            public Vector Location
            {
                get { return _location; }
            }

            public double Factor
            {
                get { return _blockFactor; }
            }
        }

        public static double LineOfSightModifer(
            this Vector currentLocation,
            Vector target,
            Func<Vector, bool> blocksLineOfSight)
        {
            var octant = Octants.DetermineOctant(currentLocation, target);

            var mapToOctant1 =
                Octants.CreateMapToOctant1(octant);

            var mapFromOctant1 =
                Octants.CreateMapFromOctant1(octant);

            Func<Vector, bool> mappedBlocksLineOfSight =
                vector => blocksLineOfSight(mapFromOctant1(vector));

            return CalculateModifier(
                mapToOctant1(currentLocation),
                mapToOctant1(target),
                mappedBlocksLineOfSight);
        }

        public static double CalculateModifier(
            Vector currentLocation,
            Vector target,
            Func<Vector, bool> blocksLineOfSight)
        {
            var factor = 1d;

            foreach (var stepCandidate in WalkSlope(currentLocation, target))
            {
                factor = Math.Min(
                    factor,
                    CalculateStepModifier(stepCandidate, blocksLineOfSight));

                if (stepCandidate.StepLocation == target)
                {
                    // If target is wall always visible to stop crazy
                    // LOS on walls
                    return blocksLineOfSight(target) ? 1d : factor;
                }

                if (Math.Abs(factor) < Tolerance)
                {
                    return factor;
                }
            }

            throw new InvalidOperationException(
                "Should be impossible to reach here");
        }

        public static IEnumerable<StepCandidate>
            WalkSlope(Vector currentLocation, Vector target)
        {
            var slope = CalculateSlope(currentLocation, target);
            var xLength = target.X - currentLocation.X;

            return Enumerable
                .Range(1, xLength)
                .Select(xOffset => CreateStepCandidate(
                    currentLocation, slope, xOffset));
        }

        public static StepCandidate CreateStepCandidate(
            Vector location, double slope, int xOffset)
        {
            var yOffsetRaw = xOffset * slope;
            var yOffset = (int)yOffsetRaw;

            var stepLocation = Vector
                .Create(location.X + xOffset, location.Y + yOffset);

            var candidates =
                new List<TestCandidate>
                {
                    TestCandidate.Create(
                        stepLocation,
                        BlockFactor(yOffsetRaw))
                };

            // If slope is 1d, using tolerace test
            if (Math.Abs(slope - 1d) < Tolerance)
            {
                candidates.Add(TestCandidate.Create(
                    stepLocation + Directions.South, 0.5d));

                candidates.Add(TestCandidate.Create(
                    stepLocation + Directions.West, 0.5d));
            }
            else if (Math.Abs(yOffsetRaw - yOffset) > Tolerance)
            {
                candidates.Add(TestCandidate.Create(
                    stepLocation + Directions.North,
                    OffsetBlockFactor(yOffsetRaw)));
            }

            return StepCandidate.Create(stepLocation, candidates);
        }

        public static double CalculateStepModifier(
            StepCandidate stepCandidate,
            Func<Vector, bool> blocksLineOfSight)
        {
            Func<double, TestCandidate, double> calculateFactor =
                (factor, candidate) => blocksLineOfSight(candidate.Location)
                    ? factor - candidate.Factor
                    : factor;

            return Math.Max(
                0d,
                stepCandidate
                    .TestCandidates
                    .Aggregate(1d, calculateFactor));
        }

        public static double CalculateSlope(Vector from, Vector to)
        {
            var difference = to - from;

            // Should always be Octant 1
            Debug.Assert(
                difference.X >= 0 &&
                difference.Y >= 0 &&
                difference.X >= difference.Y);

            return difference.Y / (double)difference.X;
        }

        public static double BlockFactor(double rawY)
        {
            return 1 - OffsetBlockFactor(rawY);
        }

        public static double OffsetBlockFactor(double rawY)
        {
            return Math.Round(rawY - (int)rawY, 2);
        }
    }
}
