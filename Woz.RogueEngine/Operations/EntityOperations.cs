//#region License
//// Copyright (C) Woz.Software 2015
//// [https://github.com/WozSoftware/BadlyDrawRogue]
////
//// This file is part of Woz.RoqueEngine.
////
//// Woz.RoqueEngine is free software: you can redistribute it 
//// and/or modify it under the terms of the GNU General Public 
//// License as published by the Free Software Foundation, either 
//// version 3 of the License, or (at your option) any later version.
////
//// This program is distributed in the hope that it will be useful,
//// but WITHOUT ANY WARRANTY; without even the implied warranty of
//// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//// GNU General Public License for more details.
////
//// You should have received a copy of the GNU General Public License
//// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//#endregion

//using System;
//using System.Collections.Immutable;
//using System.Diagnostics;
//using Woz.RogueEngine.Entities;

//namespace Woz.RogueEngine.Operations
//{
//    using IAttributeStore = IImmutableDictionary<EntityAttributes, int>;
//    using IFlagStore = IImmutableDictionary<EntityFlags, bool>;
//    using IChildStore = IImmutableDictionary<long, IEntity>;

//    public static class EntityOperations
//    {
//        public static IEntity EditAttributes(
//            this IEntity entity, 
//            Func<IAttributeStore, IAttributeStore> attributeEditor)
//        {
//            Debug.Assert(entity != null);
//            Debug.Assert(attributeEditor != null);

//            return entity.With(attributes: attributeEditor(entity.Attributes));
//        }

//        public static IEntity EditFlags(
//            this IEntity entity, 
//            Func<IFlagStore, IFlagStore> flagEditor)
//        {
//            Debug.Assert(entity != null);
//            Debug.Assert(flagEditor != null);

//            return entity.With(flags: flagEditor(entity.Flags));
//        }

//        public static IEntity AddChild(this IEntity entity, IEntity child)
//        {
//            Debug.Assert(entity != null);
//            Debug.Assert(child != null);

//            return entity.EditChild(x => x.Add(child.Id, child));
//        }

//        public static IEntity UpdateChild(this IEntity entity, IEntity child)
//        {
//            Debug.Assert(entity != null);
//            Debug.Assert(child != null);

//            return entity.EditChild(x => x.SetItem(child.Id, child));
//        }

//        public static IEntity RemoveChild(this IEntity entity, long childId)
//        {
//            Debug.Assert(entity != null);

//            return entity.EditChild(x => x.Remove(childId));
//        }

//        public static IEntity EditChild(
//            this IEntity entity, Func<IChildStore, IChildStore> childEditor)
//        {
//            Debug.Assert(entity != null);
//            Debug.Assert(childEditor != null);

//            return entity.With(children: childEditor(entity.Children));
//        }
//    }
//}