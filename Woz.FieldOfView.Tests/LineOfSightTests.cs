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
using Woz.Immutable.Collections;
using Woz.Linq.Collections;
using Woz.Monads.MaybeMonad;

namespace Woz.FieldOfView.Tests
{
    using SlopeVectors = Tuple<Vector, IMaybe<Vector>>;

    [TestClass]
    public class LineOfSightTests
    {
        [TestMethod]
        public void CastRayIncludeStart()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(0, 6);

            var ray = start.CastRay(target, true, false);

            Assert.IsTrue(ray.Contains(start));
        }

        [TestMethod]
        public void CastRayIncludeEnd()
        {
            var start = Vector.Create(0, 0);
            var target = Vector.Create(0, 6);

            var ray = start.CastRay(target, false, true);

            Assert.IsTrue(ray.Contains(target));
        }

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

        [TestMethod]
        public void CalculateVisibleRegionNoWalls()
        {
            var expected =
                new[]
                {
                    "         ",
                    "  yyyyy  ",
                    " yyyyyyy ",
                    " yyyyyyy ",
                    " yyy@yyy ",
                    " yyyyyyy ",
                    " yyyyyyy ",
                    "  yyyyy  ",
                    "         "
                };

            TestCreateVisibleRegion(vector => false, expected);
        }

        [TestMethod]
        public void CalculateVisibleRegionWithWalls()
        {
            var map =
                new[]
                {
                    "         ",
                    "         ",
                    " ++ +  + ",
                    "    +    ",
                    "    @+   ",
                    "    + +  ",
                    "    + +  ",
                    "      +  ",
                    "         "
                };

            Func<Vector, bool> blocksLineOfSight =
                vector => map[vector.Y][vector.X] == '+';

            var expected =
                new[]
                {
                    "         ",
                    "         ",
                    " ++   y  ",
                    " yyy+y   ",
                    " yyy@+   ",
                    " yyy+y   ",
                    " yy   +  ",
                    "         ",
                    "         "
                };

            TestCreateVisibleRegion(blocksLineOfSight, expected);
        }

        public void TestCreateVisibleRegion(
            Func<Vector, bool> blocksLineOfSight, string[] expected)
        {

            var viewPort = Vector.Create(4, 4)
                .CalculateVisibleRegion(4, blocksLineOfSight);

            var toTest =
                from x in Enumerable.Range(0, 9)
                from y in Enumerable.Range(0, 9)
                select Vector.Create(x, y);

            toTest.ForEach(
                vector =>
                {
                    var expectedCanSee = expected[vector.Y][vector.X] != ' ';
                    Assert.AreEqual(
                        expectedCanSee, viewPort(vector),
                        string.Format("Vector({0},{1})", vector.X, vector.Y));
                });
        }

        [TestMethod]
        public void RayEndPoints()
        {
            var min = Vector.Create(5, 5);
            var max = Vector.Create(7, 7);

            Func<IEnumerable<Vector>, Vector[]> prepare =
                vectors => vectors
                    .OrderBy(v => v.X)
                    .ThenBy(v => v.Y)
                    .ToArray();

            var expected = prepare(
                new[]
                {
                    Vector.Create(5, 5),
                    Vector.Create(5, 6),
                    Vector.Create(5, 7),
                    Vector.Create(7, 5),
                    Vector.Create(7, 6),
                    Vector.Create(7, 7),
                    Vector.Create(6, 5),
                    Vector.Create(6, 7)
                });

            var result = prepare(LineOfSight.RayEndPoints(min, max));

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CreateIsVisibleWhenLessThanMin()
        {
            TestCreateIsVisible(Vector.Create(1, 1), false);
        }

        [TestMethod]
        public void CreateIsVisibleWhenGreaterThanMax()
        {
            TestCreateIsVisible(Vector.Create(20, 20), false);
        }

        [TestMethod]
        public void CreateIsVisibleWhenVisible()
        {
            TestCreateIsVisible(Vector.Create(10, 10), true);
        }

        [TestMethod]
        public void CreateIsVisibleWhenNotVisible()
        {
            TestCreateIsVisible(Vector.Create(11, 11), false);
        }

        public void TestCreateIsVisible(Vector location, bool expected)
        {
            var min = Vector.Create(5, 5);
            var max = Vector.Create(15, 15);

            Func<Vector, Vector> mapper = 
                vector => Vector.Create(vector.X - min.X, vector.Y - min.Y);

            var viewPort = ImmutableGrid<bool>
                .CreateBuilder(max.X - min.X + 1, max.Y - min.Y + 1)
                .Set(5, 5, true)
                .Build();

            var isVisible = LineOfSight
                .CreateIsVisible(min, max, mapper, viewPort)(location);

            Assert.AreEqual(expected, isVisible);
        }
    }
}
