using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Geometry;
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
            var actorState = ActorState.Create(1, new Vector(1, 1));

            Assert.IsTrue(actorState.IsWithinMoveRange(new Vector(1, 2)).IsValid);
            Assert.IsFalse(actorState.IsWithinMoveRange(new Vector(2, 2)).IsValid);
        }
        
    }
}