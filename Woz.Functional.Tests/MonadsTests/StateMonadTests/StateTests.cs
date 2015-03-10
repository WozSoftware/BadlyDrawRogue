using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Functional.Monads.StateMonad;

namespace Woz.Functional.Tests.MonadsTests.StateMonadTests
{
    [TestClass]
    public class StateTests
    {
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
    }
}