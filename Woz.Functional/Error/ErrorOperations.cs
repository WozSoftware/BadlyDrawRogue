namespace Woz.Functional.Error
{
    public static class ErrorOperations
    {
        public static Error<T> ToSuccees<T>(this T value)
        {
            return new Error<T>(value);
        }

        public static Error<T> ToError<T>(this string errorMessage)
        {
            return new Error<T>(errorMessage);
        }

        public static Error<T> Collapse<T>(this Error<Error<T>> error)
        {
            // using implicit cast
            return error;
        }
    }
}