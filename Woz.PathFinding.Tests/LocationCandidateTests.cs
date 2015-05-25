using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Geometry;
using Woz.Monads.MaybeMonad;

namespace Woz.PathFinding.Tests
{
    [TestClass]
    public class LocationCandidateTests 
    {
        [TestMethod]
        public void EstimateRemainingMoveCost()
        {
            var location1 = Vector.Create(10, 10);
            var location2 = Vector.Create(12, 15);
            
            Assert.AreEqual(
                7 * LocationCandiate.DistanceMultiplier,
                LocationCandiate.EstimateRemainingMoveCost(location1, location2));
        }

        [TestMethod]
        public void CalculateMoveCostNoParent()
        {
            var location = Vector.Create(19, 10);

            var moveCost = LocationCandiate
                .CalculateMoveCost(Maybe<LocationCandiate>.None, location);

            Assert.AreEqual(0, moveCost);
        }

        [TestMethod]
        public void CalculateMoveCostFromParent()
        {
            var target = Vector.Create(10, 10);
            var location = Vector.Create(19, 10);

            var parent = LocationCandiate.Create(
                target, Vector.Create(20, 10));

            var moveCost = LocationCandiate
                .CalculateMoveCost(parent.ToSome(), location);

            Assert.AreEqual(1 * LocationCandiate.DistanceMultiplier, moveCost);
        }

        [TestMethod]
        public void CreateNoParent()
        {
            var target = Vector.Create(10, 10);
            var location = Vector.Create(19, 10);

            var candidate = LocationCandiate.Create(target, location);

            Assert.AreEqual(location, candidate.Location);
            Assert.AreEqual(0, candidate.CurrentCost);
            
            Assert.AreEqual(
                9 * LocationCandiate.DistanceMultiplier, 
                candidate.RemainingEstimatedCost);

            Assert.AreEqual(
                candidate.CurrentCost + candidate.RemainingEstimatedCost,
                candidate.OverallCost);

            Assert.IsFalse(candidate.Parent.HasValue);
        }

        [TestMethod]
        public void CreateWithParent()
        {
            var target = Vector.Create(10, 10);
            var location = Vector.Create(19, 10);

            var parent = LocationCandiate.Create(
                target, Vector.Create(20, 10));

            var candidate = LocationCandiate
                .Create(target, location, parent.ToSome());

            Assert.AreEqual(location, candidate.Location);

            Assert.AreEqual(
                1 * LocationCandiate.DistanceMultiplier,
                candidate.CurrentCost);

            Assert.AreEqual(
                9 * LocationCandiate.DistanceMultiplier,
                candidate.RemainingEstimatedCost);

            Assert.AreEqual(
                candidate.CurrentCost + candidate.RemainingEstimatedCost,
                candidate.OverallCost);

            Assert.IsTrue(candidate.Parent.HasValue);
            Assert.AreSame(parent, candidate.Parent.Value);
        }
    }
}