#region License
// Copyright (C) Woz.Software 2015
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

using System.Drawing;
using Woz.Functional.Monads.ValidationMonad;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Rules
{
    public static class LevelRules
    {
        public static IValidation<ILevel> RuleIsValidCoordinate(
            this ILevel level, Point location)
        {
            return level.Tiles.Bounds().Contains(location)
                ? level.ToValid()
                : string
                    .Format(
                        "Location ({0},{1} is outside the level boundary of (1,1 -> {2},{3})",
                        location.X, location.Y, 
                        level.Tiles.Size.Width, level.Tiles.Size.Height)
                    .ToInvalid<ILevel>();
        }
    }
}