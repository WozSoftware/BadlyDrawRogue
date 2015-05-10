#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.PathFinding.
//
// Woz.RoqueEngine is free software: you can redistribute it 
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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Geometry;
using Woz.Monads.MaybeMonad;

namespace Woz.PathFinding.Tests
{
    [TestClass]
    public class RouteFinderTests
    {
        [TestMethod]
        public void FindRouteNoWalls()
        {
            var result = RouteFinder.FindRoute(
                Vector.Create(1, 1), Vector.Create(1, 3), vector => true);

            Assert.IsTrue(result.HasValue);

            AssertPath(
                new []{Vector.Create(1, 2), Vector.Create(1,3)},
                result);
        }

        [TestMethod]
        public void FindRouteWalkRoundBlock()
        {
            AssertPathFromMap(
                new[]
                {
                    "  ",
                    "+ ",
                    "  ",
                },
                Vector.Create(0, 0), Vector.Create(0, 2),
                new[]
                {
                    Vector.Create(1, 0), 
                    Vector.Create(1, 1), 
                    Vector.Create(1, 2),
                    Vector.Create(0, 2)
                });
        }

        [TestMethod]
        public void FindRouteHoleInWall()
        {
            AssertPathFromMap(
                new[]
                {
                    "     ",
                    "++ ++",
                    "     ",
                },
                Vector.Create(0, 0), Vector.Create(4, 2),
                new[]
                {
                    Vector.Create(1, 0), 
                    Vector.Create(2, 0), 
                    Vector.Create(2, 1),
                    Vector.Create(2, 2),
                    Vector.Create(3, 2),
                    Vector.Create(4, 2)
                });
        }

        private static bool IsValidMove(string[] map, Vector location)
        {
            if (location.X < 0 || location.X >= map[0].Length)
            {
                return false;
            }

            if (location.Y < 0 || location.Y >= map.Length)
            {
                return false;
            }

            return map[location.Y].ToCharArray()[location.X] != '+';
        }

        private static void AssertPathFromMap(
            string[] map, Vector from, Vector to, IEnumerable<Vector> expected)
        {
            var result = RouteFinder.FindRoute(
                from, to, vector => IsValidMove(map, vector));

            AssertPath(expected, result);
        }

        private static void AssertPath(
            IEnumerable<Vector> expected, IMaybe<Path> maybePath)
        {
            Assert.IsTrue(maybePath.HasValue);

            var path = maybePath.Value;

            foreach (var location in expected)
            {
                Assert.IsTrue(path.HasNextLocation);

                Assert.AreEqual(location, path.NextLocation);

                path = path.ConsumeNextLocation();
            }
        }
    }
}
