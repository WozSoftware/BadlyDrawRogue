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

using System;
using Woz.Functional.Try;

namespace Woz.Functional.IO
{
    public delegate T IO<out T>();

    public static class IO
    {
        public static IO<T> Build<T>(this Func<T> operation)
        {
            return new IO<T>(operation);
        }

        public static IO<TResult> Bind<T, TResult>(
            this IO<T> io, Func<T, IO<TResult>> operation)
        {
            return operation(io());
        }

        public static ITry<T> Run<T>(this IO<T> operation)
        {
            return operation.ToSuccess().Bind(x => x().ToSuccess());
        }
    }
}