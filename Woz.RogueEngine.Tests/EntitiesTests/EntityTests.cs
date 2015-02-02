using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Tests.EntitiesTests
{
    [TestClass]
    public class EntityTests
    {
        private const int Id = 1;
        private const string Name = "Name";

        [TestMethod]
        public void ConstructorBasics()
        {
            var entity = new Entity(Id, Name, null, null, null);

            Assert.AreEqual(Id, entity.Id);
            Assert.AreEqual(Name, entity.Name);
        }

        [TestMethod]
        public void ConstructorNullLists()
        {
            var entity = new Entity(Id, Name, null, null, null);

            Assert.AreSame(ImmutableDictionary<string, Field<int>>.Empty, entity.Attributes);
            Assert.AreSame(ImmutableDictionary<string, Field<bool>>.Empty, entity.Flags);
            Assert.AreSame(ImmutableDictionary<long, Entity>.Empty, entity.Children);
        }

        [TestMethod]
        public void ConstructorEmptyLists()
        {
            var entity = new Entity(
                Id, Name, new Field<int>[0], new Field<bool>[0], new Entity[0]);

            Assert.AreSame(ImmutableDictionary<string, Field<int>>.Empty, entity.Attributes);
            Assert.AreSame(ImmutableDictionary<string, Field<bool>>.Empty, entity.Flags);
            Assert.AreSame(ImmutableDictionary<long, Entity>.Empty, entity.Children);
        }

        [TestMethod]
        public void ConstructorAttributesSupplied()
        {
            var attributes =
                new[]
                {
                    new Field<int>("A", 1),
                    new Field<int>("B", 2)
                };

            var entity = new Entity(Id, Name, attributes, null, null);

            CollectionAssert
                .AreEquivalent(attributes, entity.Attributes.Values.ToArray());
        }

        [TestMethod]
        public void ConstructorFlagsSupplied()
        {
            var flags =
                new[]
                {
                    new Field<bool>("A", true),
                    new Field<bool>("B", false)
                };

            var entity = new Entity(Id, Name, null, flags, null);

            CollectionAssert
                .AreEquivalent(flags, entity.Flags.Values.ToArray());
        }

        [TestMethod]
        public void ConstructorChildrenSupplied()
        {
            var children =
                new[]
                {
                    new Entity(2, "A", null, null, null),
                    new Entity(3, "B", null, null, null)
                };

            var entity = new Entity(Id, Name, null, null, children);

            CollectionAssert
                .AreEquivalent(children, entity.Children.Values.ToArray());
        }

        [TestMethod]
        public void SetAttributes()
        {
            var entity = new Entity(Id, Name, null, null, null);

            var newAttributes = entity
                .Attributes
                .Add("A", new Field<int>("A", 1));

            var newEntity = entity.Set(attributes: newAttributes);

            CollectionAssert.AreEquivalent(
                newAttributes.Values.ToArray(), 
                newEntity.Attributes.Values.ToArray());
        }

        [TestMethod]
        public void SetFlags()
        {
            var entity = new Entity(Id, Name, null, null, null);

            var newFlags = entity
                .Flags
                .Add("A", new Field<bool>("A", true));

            var newEntity = entity.Set(flags: newFlags);

            CollectionAssert.AreEquivalent(
                newFlags.Values.ToArray(),
                newEntity.Flags.Values.ToArray());
        }

        [TestMethod]
        public void SetChildren()
        {
            var entity = new Entity(Id, Name, null, null, null);

            var newChildren = entity
                .Children
                .Add(5, new Entity(5, "A", null, null, null));

            var newEntity = entity.Set(children: newChildren);

            CollectionAssert.AreEquivalent(
                newChildren.Values.ToArray(),
                newEntity.Children.Values.ToArray());
        }
    }
}