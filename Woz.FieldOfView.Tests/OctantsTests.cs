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

namespace Woz.FieldOfView.Tests
{
    [TestClass]
    public class OctantsTests
    {
        [TestMethod]
        public void BuildCoordinateMapper()
        {
            // End is expected to map to 2, 1 in Octant 1

            var start = Vector.Create(0, 0);

            // Octant 1
            Assert.AreEqual(
                1, 
                Octants.DetermineOctant(start, Vector.Create(2, 1)));
            // Octant 2
            Assert.AreEqual(
                2, 
                Octants.DetermineOctant(start, Vector.Create(2, -1)));
            // Octant 3
            Assert.AreEqual(
                3, 
                Octants.DetermineOctant(start, Vector.Create(1, -2)));
            // Octant 4
            Assert.AreEqual(
                4, 
                Octants.DetermineOctant(start, Vector.Create(-1, -2)));
            // Octant 5
            Assert.AreEqual(
                5, 
                Octants.DetermineOctant(start, Vector.Create(-2, -1)));
            // Octant 6
            Assert.AreEqual(
                6, 
                Octants.DetermineOctant(start, Vector.Create(-2, 1)));
            // Octant 7
            Assert.AreEqual(
                7, 
                Octants.DetermineOctant(start, Vector.Create(-1, 2)));
            // Octant 8
            Assert.AreEqual(
                8, 
                Octants.DetermineOctant(start, Vector.Create(1, 2)));

            // Using offset coordinates
            Assert.AreEqual(
                8,
                Octants.DetermineOctant(
                    start + Directions.SouthWest, 
                    Vector.Create(1, 2) + Directions.SouthWest));
        }

        [TestMethod]
        public void CreateMapToOctant1()
        {
            Action<int, Vector> testMapping =
                (octant, toMap) =>
                {
                    var mapper = Octants.CreateMapToOctant1(octant);

                    Assert.AreEqual(Vector.Create(2, 1), mapper(toMap));
                };

            // End is expected to map to 2, 1 in Octant 1

            // Octant 1
            testMapping(1, Vector.Create(2, 1));
            // Octant 2
            testMapping(2, Vector.Create(2, -1));
            // Octant 3
            testMapping(3, Vector.Create(1, -2));
            // Octant 4
            testMapping(4, Vector.Create(-1, -2));
            // Octant 5
            testMapping(5, Vector.Create(-2, -1));
            // Octant 6
            testMapping(6, Vector.Create(-2, 1));
            // Octant 7
            testMapping(7, Vector.Create(-1, 2));
            // Octant 8
            testMapping(8, Vector.Create(1, 2));
        }

        [TestMethod]
        public void CreateMapFromOctant1()
        {
            Action<int, Vector> testMapping =
                (octant, expected) =>
                {
                    var mapper = Octants.CreateMapFromOctant1(octant);

                    Assert.AreEqual(expected, mapper(Vector.Create(2, 1)));
                };

            // End is expected to map to 2, 1 in Octant 1

            // Octant 1
            testMapping(1, Vector.Create(2, 1));
            // Octant 2
            testMapping(2, Vector.Create(2, -1));
            // Octant 3
            testMapping(3, Vector.Create(1, -2));
            // Octant 4
            testMapping(4, Vector.Create(-1, -2));
            // Octant 5
            testMapping(5, Vector.Create(-2, -1));
            // Octant 6
            testMapping(6, Vector.Create(-2, 1));
            // Octant 7
            testMapping(7, Vector.Create(-1, 2));
            // Octant 8
            testMapping(8, Vector.Create(1, 2));
        }

    }
}
