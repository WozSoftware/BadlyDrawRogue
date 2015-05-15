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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Geometry;
using Woz.Monads.MaybeMonad;

namespace Woz.FieldOfView.Tests
{
    using SlopeVectors = Tuple<Vector, IMaybe<Vector>>;

    [TestClass]
    public class LineOfSightTests
    {
        [TestMethod]
        public void CastRayYOnly()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(0, 6);

            var expectedVisits = Enumerable
                .Range(1, 5)
                .Select(y => Vector.Create(0, y));

            TestCastRay(start, target, expectedVisits);
        }

        [TestMethod]
        public void CastRayYOnlyReverse()
        {
            var start = Vector.Create(0, 6);
            var target = Vector.Create(0, 0);

            var expectedVisits = Enumerable
                .Range(1, 5)
                .Select(y => Vector.Create(0, y))
                .Reverse();

            TestCastRay(start, target, expectedVisits);
        }

        [TestMethod]
        public void CastRayXOnly()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(6, 0);

            var expectedVisits = Enumerable
                .Range(1, 5)
                .Select(x => Vector.Create(x, 0));

            TestCastRay(start, target, expectedVisits);
        }

        [TestMethod]
        public void CastRayXOnlyReverse()
        {
            var start = Vector.Create(6, 0);
            var target = Vector.Create(0, 0);

            var expectedVisits = Enumerable
                .Range(1, 5)
                .Select(x => Vector.Create(x, 0))
                .Reverse();

            TestCastRay(start, target, expectedVisits);
        }

        [TestMethod]
        public void CastRayXAndYEqual()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(6, 6);

            var expectedVisits = Enumerable
                .Range(1, 5)
                .Select(n => Vector.Create(n, n));

            TestCastRay(start, target, expectedVisits);
        }

        [TestMethod]
        public void CastRayXAndYEqualReverse()
        {
            var start = Vector.Create(6, 6);
            var target = Vector.Create(0, 0);

            var expectedVisits = Enumerable
                .Range(1, 5)
                .Select(n => Vector.Create(n, n))
                .Reverse();

            TestCastRay(start, target, expectedVisits);
        }

        [TestMethod]
        public void CastRayXBiggerThanYSteep()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(5, 4);

            var expectedVisits =
                new[]
                {
                    Vector.Create(1, 0),
                    Vector.Create(1, 1),
                    Vector.Create(2, 1),
                    Vector.Create(2, 2),
                    Vector.Create(3, 2),
                    Vector.Create(3, 3),
                    Vector.Create(4, 3),
                    Vector.Create(4, 4)
                };

            TestCastRay(start, target, expectedVisits);
        }

        [TestMethod]
        public void CastRayXBiggerThanYShallow()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(8, 1);

            var expectedVisits =
                new[]
                {
                    Vector.Create(1, 0),
                    Vector.Create(2, 0),
                    Vector.Create(3, 0),
                    Vector.Create(4, 0),
                    Vector.Create(4, 1),
                    Vector.Create(5, 1),
                    Vector.Create(6, 1),
                    Vector.Create(7, 1)
                };

            TestCastRay(start, target, expectedVisits);
        }

        [TestMethod]
        public void CastRayYBiggerThanXSteep()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(4, 5);

            var expectedVisits =
                new[]
                {
                    Vector.Create(0, 1),
                    Vector.Create(1, 1),
                    Vector.Create(1, 2),
                    Vector.Create(2, 2),
                    Vector.Create(2, 3),
                    Vector.Create(3, 3),
                    Vector.Create(3, 4),
                    Vector.Create(4, 4)
                };

            TestCastRay(start, target, expectedVisits);
        }

        [TestMethod]
        public void CastRayYBiggerThanXShallow()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(1, 8);

            var expectedVisits =
                new[]
                {
                    Vector.Create(0, 1),
                    Vector.Create(0, 2),
                    Vector.Create(0, 3),
                    Vector.Create(0, 4),
                    Vector.Create(1, 4),
                    Vector.Create(1, 5),
                    Vector.Create(1, 6),
                    Vector.Create(1, 7)
                };

            TestCastRay(start, target, expectedVisits);
        }

        private void TestCastRay(
            Vector start, Vector target, IEnumerable<Vector> expectedSquares)
        {
            var result = start.CastRay(target);

            CollectionAssert.AreEqual(
                expectedSquares.ToArray(), result.ToArray());
        }

        [TestMethod]
        public void CanSeeNotBlocked()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(0, 6);

            Assert.IsTrue(
                start.CanSee(target, vector => false));
        }

        [TestMethod]
        public void CanSeeBlocked()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(0, 6);

            Assert.IsFalse(
                start.CanSee(target, vector => vector == Vector.Create(0, 3)));
        }

        [TestMethod]
        public void CanSeeEndDoesNotBlock()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(0, 6);

            Assert.IsTrue(
                start.CanSee(target, vector => vector == target));
        }
    }
}
