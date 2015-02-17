#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Functional.
//
// Woz.Functional is free software: you can redistribute it 
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

namespace Woz.Functional.Monads.ValidationMonad
{
    public static class Validator
    {
        public static IValidation<T> For<T>(T value)
        {
            return new Valid<T>(value);
        }

        public static IValidation<T> ToValid<T>(this T value)
        {
            return new Valid<T>(value);
        }

        public static IValidation<T> ToInvalid<T>(this string errorMessage)
        {
            return new Invalid<T>(errorMessage);
        }
    }
}