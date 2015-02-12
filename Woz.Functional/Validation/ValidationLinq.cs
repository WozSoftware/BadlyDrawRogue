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

namespace Woz.Functional.Validation
{
    public static class ValidationLinq
    {
        public static IValidation<TResult> Select<T, TResult>(
            this IValidation<T> trySuccess, Func<T, TResult> selector)
        {
            return trySuccess.IsValid
                ? selector(trySuccess.Value).ToValid()
                : trySuccess.ErrorMessage.ToInvalid<TResult>();
        }

        public static IValidation<TResult> TrySelect<T, TResult>(
            this IValidation<T> trySuccess, Func<T, TResult> selector)
        {
            try
            {
                return trySuccess.Select(selector);
            }
            catch (Exception ex)
            {
                return ex.Message.ToInvalid<TResult>();
            }
        }

        public static IValidation<TResult> SelectMany<T, TResult>(
            this IValidation<T> trySuccess, Func<T, IValidation<TResult>> selector)
        {
            return trySuccess.Bind(selector);
        }

        public static IValidation<TResult> TrySelectMany<T, TResult>(
            this IValidation<T> trySuccess, Func<T, IValidation<TResult>> selector)
        {
            return trySuccess.TryBind(selector);
        }

        public static IValidation<TResult> SelectMany<T1, T2, TResult>(
            this IValidation<T1> trySuccess,
            Func<T1, IValidation<T2>> transform, 
            Func<T1, T2, TResult> composer)
        {
            return trySuccess.Bind(x => 
                    transform(x).Bind(y => 
                        composer(x, y).ToValid()));
        }

        public static IValidation<TResult> TrySelectMany<T1, T2, TResult>(
            this IValidation<T1> trySuccess,
            Func<T1, IValidation<T2>> transform, 
            Func<T1, T2, TResult> composer)
        {
            try
            {
                return trySuccess.SelectMany(transform, composer);
            }
            catch (Exception ex)
            {
                return ex.Message.ToInvalid<TResult>();
            }
        }
    }
}