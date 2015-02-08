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
    public class EntityFactoryTests
    {
        [TestMethod]
        public void BuildFactory()
        {
            var tileEntity1 = Entity.Build(0, EntityType.Tile, "Tile1");
            var tileEntity2 = Entity.Build(0, EntityType.Tile, "Tile2");
            var itemEntity1 = Entity.Build(0, EntityType.Item, "Item1");

            var factory = EntityFactory.Build(new[] {tileEntity1, itemEntity1, tileEntity2});

            CollectionAssert.AreEquivalent(
                new[]{EntityType.Tile, EntityType.Item},
                factory.Templates.Keys.ToArray());

            CollectionAssert.AreEquivalent(
                new[]{tileEntity1, tileEntity2}, 
                factory.Templates[EntityType.Tile].ToArray());

            CollectionAssert.AreEquivalent(
                new[] { itemEntity1},
                factory.Templates[EntityType.Item].ToArray());
        }

        [TestMethod]
        public void BuildFromTemplate()
        {
            const string newName = "NewName";

            var child = Entity.Build(0, EntityType.Spell, "Spell");

            var template = Entity.Build(
                0,
                EntityType.Tile,
                "Tile1",
                ImmutableDictionary<EntityAttributes, int>.Empty.SetItem(EntityAttributes.Luck, 1),
                ImmutableDictionary<EntityFlags, bool>.Empty.SetItem(EntityFlags.IsFood, true),
                ImmutableList<IEntity>.Empty.Add(child));

            var factory = EntityFactory.Build(new IEntity[0]);

            var builtEntity1 = factory.Build(template, newName);

            Assert.AreNotEqual(0, builtEntity1.Id);
            Assert.AreEqual(newName, builtEntity1.Name);
            Assert.AreEqual(template.EntityType, builtEntity1.EntityType);
            Assert.AreSame(template.Attributes, builtEntity1.Attributes);
            Assert.AreSame(template.Flags, builtEntity1.Flags);
            Assert.AreSame(template.Children, builtEntity1.Children);

            var builtEntity2 = factory.Build(template, newName);

            Assert.AreNotEqual(builtEntity1.Id, builtEntity2.Id);
        }
    }
}