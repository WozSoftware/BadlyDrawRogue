using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.RogueEngine.Levels;
using Woz.RogueEngine.Validators;

namespace Woz.RogueEngine.Tests.ValidatorsTests
{
    [TestClass]
    public class ActorValidatorsTests
    {
        [TestMethod]
        public void IsWithinMoveRange()
        {
            var actorState = ActorState.Create(1, new Point(1, 1));

            Assert.IsTrue(actorState.IsWithinMoveRange(new Point(1, 2)).IsValid);
            Assert.IsFalse(actorState.IsWithinMoveRange(new Point(2, 2)).IsValid);
        }
        
    }
}