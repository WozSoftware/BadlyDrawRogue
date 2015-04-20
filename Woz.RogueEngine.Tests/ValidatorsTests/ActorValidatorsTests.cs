using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Coordinates;
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
            var actorState = ActorState.Create(1, new Coordinate(1, 1));

            Assert.IsTrue(actorState.IsWithinMoveRange(new Coordinate(1, 2)).IsValid);
            Assert.IsFalse(actorState.IsWithinMoveRange(new Coordinate(2, 2)).IsValid);
        }
        
    }
}