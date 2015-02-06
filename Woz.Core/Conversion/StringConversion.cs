using System;
using Functional.Maybe;

namespace Woz.Core.Conversion
{
    public static class StringConversion
    {
        public static T ParseAs<T>(this string value)
            where T : struct
        {
            return value
                .ParseAsMaybe<T>()
                .OrElse(
                    () => new ArgumentException(
                        string.Format(
                            "{0} does not convert to {1}]", 
                            value ?? "null", 
                            typeof (T).Name)));
        }

        public static T ParseAs<T>(this string value, T defaultValue)
            where T : struct
        {
            return value.ParseAsMaybe<T>().OrElse(defaultValue);
        }

        public static Maybe<T> ParseAsMaybe<T>(this string value)
            where T : struct
        {
            T result;
            return TryParseHelper<T>.TryParse(value, out result)
                ? result.ToMaybe()
                : Maybe<T>.Nothing;
        }
    
        private static class TryParseHelper<T>
            where T : struct
        {
            public delegate bool TryParseFunc(string str, out T result);

            public static readonly TryParseFunc TryParse = CreateTryParse();

            private static TryParseFunc CreateTryParse()
            {
                return (TryParseFunc)Delegate.CreateDelegate(
                    typeof(TryParseFunc), typeof(T), "TryParse");
            }
        }
    }
}