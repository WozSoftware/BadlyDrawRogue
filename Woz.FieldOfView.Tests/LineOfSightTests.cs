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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Geometry;
using Woz.Monads.MaybeMonad;

namespace Woz.FieldOfView.Tests
{
    using SlopeVectors = Tuple<Vector, IMaybe<Vector>>;

    [TestClass]
    public class LineOfSightTests
    {
        // NOTE: All map grids reversed as map treats 0, 0 as bottom
        // left but array construction is row 0 at the top

        private readonly string[] _map =
            {
                "    +      ",
                "    +      ",
                "           ",
                "   ++ +    ",
                "    +      ",
            };


        [TestMethod]
        public void LineOfSightModiferNoBlock()
        {
            var result = Vector
                .Create(0, 0)
                .LineOfSightModifer(Vector.Create(5, 3), vector => false);

            Assert.AreEqual(1d, result);
        }

        [TestMethod]
        public void LineOfSightBlockedStraightLine()
        {
            var result = Vector
                .Create(0, 0)
                .LineOfSightModifer(Vector.Create(6, 0),
                BlocksMovement);

            Assert.AreEqual(0d, result);
        }

        [TestMethod]
        public void LineOfSightBlockedCrossTwoBlocks()
        {
            var result = Vector
                .Create(0, 0)
                .LineOfSightModifer(Vector.Create(10, 1),
                BlocksMovement);

            Assert.AreEqual(0d, result);
        }

        [TestMethod]
        public void LineOfSightEndInWall()
        {
            var result = Vector
                .Create(0, 0)
                .LineOfSightModifer(Vector.Create(3, 0),
                BlocksMovement);

            Assert.AreEqual(1d, result);
        }

        [TestMethod]
        public void LineOfSightPartialShallow()
        {
            var result = Vector
                .Create(0, 1)
                .LineOfSightModifer(Vector.Create(10, 2),
                BlocksMovement);

            Assert.AreEqual(0.4d, result);
        }

        [TestMethod]
        public void LineOfSightPartialBlockedDiagonal()
        {
            var result = Vector
                .Create(3, 1)
                .LineOfSightModifer(Vector.Create(5, 3),
                BlocksMovement);

            Assert.AreEqual(0.5d, result);
        }

        [TestMethod]
        public void LineOfSightOctantMapped()
        {
            var result = Vector
                .Create(8, 4)
                .LineOfSightModifer(Vector.Create(6, 2),
                BlocksMovement);

            Assert.AreEqual(0.5d, result);
        }

        private bool BlocksMovement(Vector location)
        {
            return _map[location.Y].ToCharArray()[location.X] == '+';
        }

        [TestMethod]
        public void CalculateSlope()
        {
            Assert.AreEqual(
                1d / 3d,
                LineOfSight.CalculateSlope(
                    Vector.Create(0, 0), Vector.Create(3, 1)));

            Assert.AreEqual(
                0.25d,
                LineOfSight.CalculateSlope(
                    Vector.Create(0, 0), Vector.Create(4, 1)));

            Assert.AreEqual(
                1d,
                LineOfSight.CalculateSlope(
                    Vector.Create(0, 0), Vector.Create(2, 2)));

            Assert.AreEqual(
                0d,
                LineOfSight.CalculateSlope(
                    Vector.Create(0, 0), Vector.Create(6, 0)));
        }

        [TestMethod]
        public void BlockFactor()
        {
            Assert.AreEqual(0.76d, LineOfSight.BlockFactor(1.2423d));
            Assert.AreEqual(1d, LineOfSight.BlockFactor(1d));
        }

        [TestMethod]
        public void OffsetBlockFactor()
        {
            Assert.AreEqual(0.24d, LineOfSight.OffsetBlockFactor(1.2423d));
            Assert.AreEqual(0d, LineOfSight.OffsetBlockFactor(1d));
        }
    }
}
