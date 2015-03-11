using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Functional.Monads.StateMonad;

namespace Woz.Functional.Tests.MonadsTests.StateMonadTests
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