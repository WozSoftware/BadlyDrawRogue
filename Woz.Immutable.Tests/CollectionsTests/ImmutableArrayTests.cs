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
using Woz.Linq;

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

            var array = ImmutableArray<int>.Create(source);

            Assert.IsTrue(source.SequenceEqual(array));
        }

        [TestMethod]
        public void Indexer()
        {
            var source = new[] { 1, 4, 5 };

            var array = ImmutableArray<int>.Create(source);

            Enumerable
                .Range(0, array.Count)
                .ForEach(x => Assert.AreEqual(source[x], array[x]));
        }

        [TestMethod]
        public void IndexOfWhenFound()
        {
            var source = new[] { 1, 4, 5 };

            var array = ImmutableArray<int>.Create(source);

            Assert.AreEqual(1, array.IndexOf(x => x == 4).Value);
        }

        [TestMethod]
        public void IndexOfWhenNotFound()
        {
            var source = new[] { 1, 4, 5 };

            var array = ImmutableArray<int>.Create(source);

            Assert.IsFalse(array.IndexOf(x => x == 8).HasValue);
        }

        [TestMethod]
        public void Set()
        {
            var source = new[] { 1, 4, 5 };

            var array = ImmutableArray<int>.Create(source).Set(1, 6);

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
            var source = ImmutableArray<int>.Create(new[] { 1, 4, 5 });
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

            Assert.IsTrue(immutableArrayTime < arrayTime * 110);
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

            Assert.IsTrue(immutableArrayTime < arrayTime * 10);
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

            Assert.IsTrue(immutableArrayTime < arrayTime * 2.5);
        }

        public double BenchmarkHelper(int length, Action<int> indexOperation)
        {
            const int iterations = 50;

            GC.Collect();

            var stopwatch = Stopwatch.StartNew();
            for (var iteration = 0; iteration < iterations; iteration++)
            {
                for (var index = 0; index < length; index++)
                {
                    indexOperation(index);
                }

            }
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds / iterations;
        }
    }
}
