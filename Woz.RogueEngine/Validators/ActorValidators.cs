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

using Woz.Core.Coordinates;
using Woz.Monads.ValidationMonad;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Validators
{
    public static class ActorValidators
    {
        public static IValidation<bool> IsWithinMoveRange(
            this ActorState actorState, Coordinate targetLocation)
        {
            return actorState.Location.DistanceFrom(targetLocation) > 1
                ? "Too far to move".ToInvalid<bool>()
                : true.ToValid();
        }
    }
}