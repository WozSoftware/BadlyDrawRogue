using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Woz.Lenses.Tests;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Tests.LevelsTests
{
    using ICombatStatistics = IImmutableDictionary<DamageTypes, int>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    [TestClass]
    public class ThingLensTests : BaseLensTests
    {
        [TestMethod]
        public void Id()
        {
            TestLensWithAreEqual(ThingTests.Thing, ThingLens.Id, 7L);
        }

        [TestMethod]
        public void ThingType()
        {
            TestLensWithAreEqual(
                ThingTests.Thing, ThingLens.ThingType, ThingTypes.Furniture);
        }

        [TestMethod]
        public void Name()
        {
            TestLensWithAreEqual(ThingTests.Thing, ThingLens.Name, "A");
        }

        [TestMethod]
        public void EquipableAs()
        {
            TestLensWithAreEqual(
                ThingTests.Thing, ThingLens.EquipableAs, EquipmentSlots.Belt);
        }

        [TestMethod]
        public void Equiped()
        {
            TestLensWithAreEqual(ThingTests.Thing, ThingLens.Equiped, false);
        }

        [TestMethod]
        public void AttackDetails()
        {
            TestLensWithAreEqual(
                ThingTests.Thing, ThingLens.AttackDetails, 
                new Mock<ICombatStatistics>().Object);
        }

        [TestMethod]
        public void DefenseDetails()
        {
            TestLensWithAreEqual(
                ThingTests.Thing, ThingLens.DefenseDetails,
                new Mock<ICombatStatistics>().Object);
        }

        [TestMethod]
        public void Contains()
        {
            TestLensWithAreEqual(
                ThingTests.Thing, ThingLens.Contains,
                new Mock<IThingStore>().Object);
        }
    }
}