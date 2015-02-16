#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Functional.
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
using Woz.Functional.Monads.IOMonad;

namespace Woz.Functional.Tests.MonadsTests.IOMonadTests
{
    [TestClass]
    public class IOTests
    {
        [TestMethod]
        public void ToIO()
        {
            var io = IO.ToIO(() => "hello");

            Assert.AreEqual("hello", io());
        }

        [TestMethod]
        public void Map()
        {
            IO<string> io = () => "hello";

            var boundIo = io.Map(value => value + "world");

            Assert.AreEqual("helloworld", boundIo());
        }

        [TestMethod]
        public void FlatMap()
        {
            IO<string> io = () => "hello";

            var boundIo = io.FlatMap(value => IO.ToIO(() => value + "world"));

            Assert.AreEqual("helloworld", boundIo());
        }

        [TestMethod]
        public void RunWhenOk()
        {
            IO<string> io = () => "hello";

            var boundIo = io.Map(value => value + "world");

            var runResults = boundIo.Run();

            Assert.IsTrue(runResults.IsValid);
            Assert.AreEqual("helloworld", runResults.Value);
        }

        [TestMethod]
        public void RunWhenException()
        {

            IO<string> func1 =
                () =>
                {
                    throw new Exception("Bang");
                };

            var boundIo = func1.Map(value => value + "world");

            var runResults = boundIo.Run();

            Assert.IsFalse(runResults.IsValid);
            Assert.AreEqual("Bang", runResults.Error.Message);
        }
    }
}
