#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Monads.
//
// Woz.Linq is free software: you can redistribute it 
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
using Woz.Monads.StateMonad;

namespace Woz.Monads.Tests.StateMonadTests
{
    [TestClass]
    public class StateTests
    {
        [TestMethod]
        public void ToState()
        {
            var monad = 5.ToState<string, int>();
            var result = monad("A");

            Assert.AreEqual("A", result.State);
            Assert.AreEqual(5, result.Value);
        }

        [TestMethod]
        public void Get()
        {
            var monad = State.Get<string>();
            var result = monad("A");

            Assert.AreEqual("A", result.State);
            Assert.AreEqual("A", result.Value);
        }

        [TestMethod]
        public void Put()
        {
            var monad = State.Put("A");
            var result = monad("B");

            Assert.AreEqual("A", result.State);
            Assert.AreSame(Unit.Value, result.Value);
        }

        [TestMethod]
        public void Modify()
        {
            var monad = State.Modify<string>(s => "A");
            var result = monad("B");

            Assert.AreEqual("A", result.State);
            Assert.AreSame(Unit.Value, result.Value);
        }

        [TestMethod]
        public void Exec()
        {
            var monad = State.Put("A");
            var result = monad.Exec("B");

            Assert.AreEqual("A", result);
        }

        [TestMethod]
        public void Eval()
        {
            var monad = State.Put("A");
            var result = monad.Eval("B");

            Assert.AreSame(Unit.Value, result);
        }

        [TestMethod]
        public void Select()
        {
            var monad = 5.ToState<string, int>().Select(x => x + 1);
            var result = monad("A");

            Assert.AreEqual("A", result.State);
            Assert.AreEqual(6, result.Value);
        }

        [TestMethod]
        public void SelectMany()
        {
            var monad = 5
                .ToState<string, int>()
                .SelectMany(
                    x =>
                    {
                        return State
                            .Modify<string>(s => s + s)
                            .Select(_ => x + 1);
                    });
            var result = monad("A");

            Assert.AreEqual("AA", result.State);
            Assert.AreEqual(6, result.Value);
        }

        [TestMethod]
        public void Compose()
        {
            var operation =
                from first in FirstOperation()
                from second in SecondOperation(first)
                from third in ThirdOperation()
                select first + second + third;

            var result = operation(1);
            Assert.AreEqual(4, result.State);
            Assert.AreEqual("AAB55", result.Value);
        }

        private static State<int, string> FirstOperation()
        {
            return state => StateResult.Create(state, "A");
        }

        private static State<int, string> SecondOperation(string value)
        {
            return state => StateResult.Create(state + 1, value + "B");
        }

        private static State<int, int> ThirdOperation()
        {
            return state => StateResult.Create(state + 2, 55);
        }
    }
}