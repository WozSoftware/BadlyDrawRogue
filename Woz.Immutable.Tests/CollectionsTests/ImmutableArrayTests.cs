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
using Woz.Core.Collections;
using Woz.Immutable.Collections;

// ReSharper disable UnusedVariable

namespace Woz.Immutable.Tests.CollectionsTests
{
    [TestClass]
    public class ImmutableArrayTests
    {
        [TestMethod]
        public void CreateLengthValueType()
        {
            var array = ImmutableArray<int>.Create(10);

            Assert.AreEqual(10, array.Count);
            Assert.IsTrue(array.All(x => x == default(int)));
        }

        [TestMethod]
        public void CreateLengthReferenceType()
        {
            var array = ImmutableArray<object>.Create(10);

            Assert.AreEqual(10, array.Count);
            Assert.IsTrue(array.All(x => x == default(object)));
        }

        [TestMethod]
        public void CreateFromSource()
        {
            var source = new[] {1, 4, 5};

            var array = ImmutableArray.Create(source);

            Assert.IsTrue(source.SequenceEqual(array));
        }

        [TestMethod]
        public void Indexer()
        {
            var source = new[] { 1, 4, 5 };

            var array = ImmutableArray.Create(source);

            Enumerable
                .Range(0, array.Count)
                .ForEach(x => Assert.AreEqual(source[x], array[x]));
        }

        [TestMethod]
        public void IndexOfWhenFound()
        {
            var source = new[] { 1, 4, 5 };

            var array = ImmutableArray.Create(source);

            Assert.AreEqual(1, array.IndexOf(x => x == 4).Value);
        }

        [TestMethod]
        public void IndexOfWhenNotFound()
        {
            var source = new[] { 1, 4, 5 };

            var array = ImmutableArray.Create(source);

            Assert.IsFalse(array.IndexOf(x => x == 8).HasValue);
        }

        [TestMethod]
        public void Set()
        {
            var source = new[] { 1, 4, 5 };

            var array = ImmutableArray.Create(source).Set(1, 6);

            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(6, array[1]);
            Assert.AreEqual(5, array[2]);

            Assert.AreEqual(1, source[0]);
            Assert.AreEqual(4, source[1]);
            Assert.AreEqual(5, source[2]);
        }

        [TestMethod]
        public void CreateBuilderByLength()
        {
            var builder = ImmutableArray<int>.CreateBuilder(3);

            Assert.AreEqual(3, builder.Count);
            Enumerable
                .Range(0, builder.Count)
                .ForEach(x => Assert.AreEqual(default(int), builder[x]));
        }

        [TestMethod]
        public void BuilderBuiltArray()
        {
            var expected = new[] { 1, 4, 5 };

            var array = ImmutableArray<int>
                .CreateBuilder(3)
                .Set(0, 1)
                .Set(1, 4)
                .Set(2, 5)
                .Build();

            Assert.IsTrue(expected.SequenceEqual(array));
        }

        [TestMethod]
        public void ToBuilder()
        {
            var source = ImmutableArray.Create(new[] { 1, 4, 5 });
            var array = source.ToBuilder().Set(1, 6).Build();

            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(6, array[1]);
            Assert.AreEqual(5, array[2]);

            Assert.AreEqual(1, source[0]);
            Assert.AreEqual(4, source[1]);
            Assert.AreEqual(5, source[2]);
        }

        [TestMethod]
        public void WritePerformanceDirect()
        {
            const int length = 150;

            var array = new int[length];
            var immutableArray = ImmutableArray<int>.Create(length);

            var arrayTime = 
                BenchmarkHelper(length, i => array[i] = i);

            var immutableArrayTime = 
                BenchmarkHelper(length, i => immutableArray.Set(i, i));

            // Use the builder for many updates :)
            // This looks ugly but still 1/2 time of nested immutable
            // dictionary

            var factor = immutableArrayTime / arrayTime;
#if DEBUG
            Assert.IsTrue(factor < 50, "Factor was " + factor);
#else
            Assert.IsTrue(factor < 40, "Factor was " + factor);
#endif
        }

        [TestMethod]
        public void WritePerformanceBuilder()
        {
            const int length = 150;

            var array = new int[length];
            var builder = ImmutableArray<int>.CreateBuilder(length);

            var arrayTime =
                BenchmarkHelper(length, i => array[i] = i);

            var immutableArrayTime =
                BenchmarkHelper(length, i => builder.Set(i, i));


            var factor = immutableArrayTime / arrayTime;
#if DEBUG
            Assert.IsTrue(factor < 8, "Factor was " + factor);
#else
            Assert.IsTrue(factor < 7, "Factor was " + factor);
#endif
        }

        [TestMethod]
        public void ReadPerformance()
        {
            const int length = 150;

            var array = new int[length];
            var immutableArray = ImmutableArray<int>.Create(length);

            var arrayTime =
                BenchmarkHelper(length, i => { var a = array[i]; });

            var immutableArrayTime =
                BenchmarkHelper(length, i => { var a = immutableArray[i]; });

            var factor = immutableArrayTime / arrayTime;
#if DEBUG
            Assert.IsTrue(factor < 2.5, "Factor was " + factor);
#else
            Assert.IsTrue(factor < 2, "Factor was " + factor);
#endif
        }

        public double BenchmarkHelper(int length, Action<int> indexOperation)
        {
            const int iterations = 50;
            double total = 0;

            for (var iteration = 0; iteration < iterations; iteration++)
            {
                GC.Collect();
                var stopwatch = Stopwatch.StartNew();

                for (var index = 0; index < length; index++)
                {
                    indexOperation(index);
                }

                stopwatch.Stop();
                total += stopwatch.Elapsed.TotalMilliseconds;
            }
            return total / iterations / length;
        }
    }
}
