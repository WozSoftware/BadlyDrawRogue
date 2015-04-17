#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Immutable.
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
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Immutable.Collections;

namespace Woz.Immutable.Tests.CollectionsTests
{
    [TestClass]
    public class ImmutableGridTests
    {
        [TestMethod]
        public void CreateValueType()
        {
            var grid = ImmutableGrid<int>.Create(10, 20);

            Assert.AreEqual(10, grid.Width);
            Assert.AreEqual(20, grid.Height);
            Assert.IsTrue(grid.All(x => x.Item2 == default(int)));
        }

        [TestMethod]
        public void CreateReferenceType()
        {
            var grid = ImmutableGrid<object>.Create(10, 20);

            Assert.AreEqual(10, grid.Width);
            Assert.AreEqual(20, grid.Height);
            Assert.IsTrue(grid.All(x => x.Item2 == default(object)));
        }

        [TestMethod]
        public void Indexer()
        {
            var grid = ImmutableGrid<int>
                .Create(2, 2)
                .Set(0, 0, 5)
                .Set(0, 1, 6);

            Assert.AreEqual(5, grid[0, 0]);
            Assert.AreEqual(6, grid[0, 1]);
        }

        [TestMethod]
        public void Set()
        {
            var source = ImmutableGrid<int>.Create(2, 2).Set(0, 0, 5);
            var grid = source.Set(0, 0, 6);

            Assert.AreEqual(6, grid[0, 0]);

            Assert.AreEqual(5, source[0, 0]);
        }

        [TestMethod]
        public void CreateBuilder()
        {
            var builder = ImmutableGrid<int>.CreateBuilder(2, 3);

            Assert.AreEqual(2, builder.Width);
            Assert.AreEqual(3, builder.Height);
        }

        [TestMethod]
        public void ToBuilder()
        {
            var source = ImmutableGrid<int>.Create(2, 2).Set(0, 0, 5);
            var grid = source.ToBuilder().Set(1, 1, 3).Build();

            Assert.AreEqual(5, grid[0, 0]);
            Assert.AreEqual(3, grid[1, 1]);

            Assert.AreEqual(5, source[0, 0]);
            Assert.AreEqual(default(int), source[1, 1]);
        }

#if PerformanceTest
        [TestMethod]
        public void WritePerformanceDirect()
        {
            const int width = 150;
            const int height = 150;

            var grid = new int[width, height];
            var immutableGrid = ImmutableGrid<int>.Create(width, height);

            var gridTime = BenchmarkHelper(
                width, height, (x, y) => grid[x, y] = x + y);

            var immutableGridTime = BenchmarkHelper(
                width, height, (x, y) => immutableGrid.Set(x, y, x + y));

            // Use the builder for many updates :)
            // This looks ugly but still 1/2 time of nested immutable
            // dictionary
            var factor = immutableGridTime / gridTime;
#if DEBUG
            Assert.IsTrue(factor < 800, "Factor was " + factor);
#else
            Assert.IsTrue(factor < 700, "Factor was " + factor);
#endif
        }

        [TestMethod]
        public void WritePerformanceBuilder()
        {
            const int width = 150;
            const int height = 150;

            var grid = new int[width, height];
            var immutableGrid = ImmutableGrid<int>.CreateBuilder(width, height);

            var gridTime = BenchmarkHelper(
                width, height, (x, y) => grid[x, y] = x + y);

            var immutableGridTime = BenchmarkHelper(
                width, height, (x, y) => immutableGrid.Set(x, y, x + y));

            var factor = immutableGridTime / gridTime;
#if DEBUG
            Assert.IsTrue(factor < 5, "Factor was " + factor);
#else
            Assert.IsTrue(factor < 4, "Factor was " + factor);
#endif
        }

        [TestMethod]
        public void ReadPerformance()
        {
            const int width = 150;
            const int height = 150;

            var grid = new int[width, height];
            var immutableGrid = ImmutableGrid<int>.Create(width, height);

            var gridTime = BenchmarkHelper(
                width, height, (x, y) => { var cell = grid[x, y]; });

            var immutableGridTime = BenchmarkHelper(
                width, height, (x, y) => { var cell = immutableGrid[x, y]; });

            var factor = immutableGridTime / gridTime;
#if DEBUG
            Assert.IsTrue(factor < 5, "Factor was " + factor);
#else
            Assert.IsTrue(factor < 2, "Factor was " + factor);
#endif
        }
#endif

        public double BenchmarkHelper(
            int width, int height, Action<int, int> indexOperation)
        {
            const int iterations = 2;
            double total = 0;

            for (var iteration = 0; iteration < iterations; iteration++)
            {
                GC.Collect();
                var stopwatch = Stopwatch.StartNew();

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        indexOperation(x, y);
                    }
                }

                stopwatch.Stop();
                total += stopwatch.Elapsed.TotalMilliseconds;
            }
            return total / iterations / width / height;
        }
    }
}