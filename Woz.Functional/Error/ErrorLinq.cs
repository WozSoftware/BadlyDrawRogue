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

        public static Error<TResult> SelectMany<T, TResult>(
            this Error<T> error, Func<T, Error<TResult>> selector)
        {
            return error.Bind(selector);
        }

        public static Error<TResult> SelectMany<T1, T2, TResult>(
            this Error<T1> error, Func<T1, Error<T2>> transform, Func<T1, T2, TResult> composer)
        {
            return error.Bind(x => 
                    transform(x).Bind(y => 
                        composer(x, y).ToSuccees()));
        }
    }
}