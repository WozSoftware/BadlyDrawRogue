#region License
// Copyright � Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.RoqueEngine.
//
// Woz.RoqueEngine is free software: you can redistribute it 
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
        private readonly IImmutableDictionary<long, IEntity> _emptyChildren =
            ImmutableDictionary<long, IEntity>.Empty;

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
            var child = Entity.Build(5, EntityType.Item, "A");
 
            var newChildren = _entity.Children.SetItem(child.Id, child);

            var newEntity = _entity.Set(children: newChildren);

            CollectionAssert.AreEquivalent(
                newChildren.ToArray(),
                newEntity.Children.ToArray());
        }
    }
}