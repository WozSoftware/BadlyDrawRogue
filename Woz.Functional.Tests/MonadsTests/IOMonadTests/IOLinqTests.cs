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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Functional.Monads.IOMonad;

namespace Woz.Functional.Tests.MonadsTests.IOMonadTests
{
    [TestClass]
    public class IOLinqTests
    {
        [TestMethod]
        public void Select()
        {
            IO<string> io = () => "hello";

            var boundIo = io.Select(value => value + "world");

            Assert.AreEqual("helloworld", boundIo());
        }

        [TestMethod]
        public void SelectMany()
        {
            IO<string> io1 = () => "hello";
            IO<string> io2 = () => "world";

            var operation =
                from val1 in io1
                from val2 in io2
                select val1 + val2;

            Assert.AreEqual("helloworld", operation());
        }
    }
}
