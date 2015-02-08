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

namespace Woz.Functional.Error
{
    public static class ErrorLinq
    {
        public static Error<TResult> Select<T, TResult>(
            this Error<T> error, Func<T, TResult> selector)
        {
            return error.IsValid
                ? selector(error.Value).ToSuccees()
                : error.ErrorMessage.ToError<TResult>();
        }

        public static Error<TResult> TrySelect<T, TResult>(
            this Error<T> error, Func<T, TResult> selector)
        {
            try
            {
                return error.Select(selector);
            }
            catch (Exception ex)
            {
                return ex.Message.ToError<TResult>();
            }
        }

        public static Error<TResult> SelectMany<T, TResult>(
            this Error<T> error, Func<T, Error<TResult>> selector)
        {
            return error.Bind(selector);
        }

        public static Error<TResult> TrySelectMany<T, TResult>(
            this Error<T> error, Func<T, Error<TResult>> selector)
        {
            return error.TryBind(selector);
        }

        public static Error<TResult> SelectMany<T1, T2, TResult>(
            this Error<T1> error, Func<T1, Error<T2>> transform, Func<T1, T2, TResult> composer)
        {
            return error.Bind(x => 
                    transform(x).Bind(y => 
                        composer(x, y).ToSuccees()));
        }

        public static Error<TResult> TrySelectMany<T1, T2, TResult>(
            this Error<T1> error, Func<T1, Error<T2>> transform, Func<T1, T2, TResult> composer)
        {
            try
            {
                return error.SelectMany(transform, composer);
            }
            catch (Exception ex)
            {
                return ex.Message.ToError<TResult>();
            }
        }
    }
}