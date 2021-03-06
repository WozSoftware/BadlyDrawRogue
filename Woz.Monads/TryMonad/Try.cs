﻿#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Monads.
//
// Woz.Linq is free software: you can redistribute it 
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

using System;
using System.Diagnostics;

namespace Woz.Monads.TryMonad
{
    public static class Try
    {
        public static ITry<T> ToTry<T>(this T value)
        {
            return new Success<T>(value);
        }

        public static ITry<T> ToException<T>(this Exception exception)
        {
            return new Failed<T>(exception);
        }

        public static ITry<T> Catcher<T>(this Func<ITry<T>> operation)
        {
            Debug.Assert(operation != null);

            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                return ex.ToException<T>();
            }
        }
    }
}