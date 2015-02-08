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
        private const EntityType Type = EntityType.Tile;
        private const string Name = "Name";
        private readonly IImmutableDictionary<EntityAttributes, int> _emptyAttributes =
            ImmutableDictionary<EntityAttributes, int>.Empty;
        private readonly IImmutableDictionary<EntityFlags, bool> _emptyFlags =
            ImmutableDictionary<EntityFlags, bool>.Empty;
        private readonly IImmutableList<IEntity> _emptyChildren =
            ImmutableList<IEntity>.Empty;

        private IEntity _entity;

        [TestInitialize]
        public void Setup()
        {
            _entity = Entity.Build(
                Id, Type, Name, _emptyAttributes, _emptyFlags, _emptyChildren);
        }
            
        [TestMethod]
        public void Build()
        {
            Assert.AreEqual(Id, _entity.Id);
            Assert.AreEqual(Type, _entity.EntityType);
            Assert.AreSame(Name, _entity.Name);
            Assert.AreSame(_emptyAttributes, _entity.Attributes);
            Assert.AreSame(_emptyFlags, _entity.Flags);
            Assert.AreSame(_emptyChildren, _entity.Children);
        }

        [TestMethod]
        public void SetWithAttributes()
        {
            var newAttributes = _entity.Attributes.SetItem(EntityAttributes.Luck, 1);

            var newEntity = _entity.Set(attributes: newAttributes);

            CollectionAssert.AreEquivalent(
                newAttributes.Values.ToArray(), 
                newEntity.Attributes.Values.ToArray());
        }

        [TestMethod]
        public void SetWithFlags()
        {
            var newFlags = _entity.Flags.SetItem(EntityFlags.BlocksLineOfSight, true);

            var newEntity = _entity.Set(flags: newFlags);

            CollectionAssert.AreEquivalent(
                newFlags.Values.ToArray(),
                newEntity.Flags.Values.ToArray());
        }

        [TestMethod]
        public void SetWithChildren()
        {
            var newChildren = _entity
                .Children
                .Add(Entity.Build(5, EntityType.Item, "A"));

            var newEntity = _entity.Set(children: newChildren);

            CollectionAssert.AreEquivalent(
                newChildren.ToArray(),
                newEntity.Children.ToArray());
        }
    }
}