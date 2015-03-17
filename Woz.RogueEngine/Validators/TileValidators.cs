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

//using Woz.Functional.Monads.ValidationMonad;
//using Woz.RogueEngine.Entities;
//using Woz.RogueEngine.Rules;

//namespace Woz.RogueEngine.Validators
//{
//    public static class TileValidators
//    {
//        public static IValidation<IEntity> CanOpenDoor(this IEntity entity)
//        {
//            return Validator
//                .For(entity)
//                .SelectMany(x => x.RuleIs(EntityType.Tile))
//                .SelectMany(x => x.RuleIsTileType(TileTypes.Door))
//                .SelectMany(x => x.RuleIsDoorClosed());
//        }

//        public static IValidation<IEntity> CanCloseDoor(this IEntity entity)
//        {
//            return Validator
//                .For(entity)
//                .SelectMany(x => x.RuleIs(EntityType.Tile))
//                .SelectMany(x => x.RuleIsTileType(TileTypes.Door))
//                .SelectMany(x => x.RuleIsDoorOpen());
//        }
//    }
//}