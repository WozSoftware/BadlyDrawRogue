using System;
using System.Collections.Generic;

namespace Woz.Functional.Error
{
    public struct Error<T>
    {
        private readonly T _value;
        private readonly string _errorMessage;

        internal Error(T value)
        {
            _value = value;
            _errorMessage = null;
        }

        internal Error(string errorMessage)
        {
            _value = default(T);
            _errorMessage = errorMessage;
        }

        public T Value
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException(
                        string.Format("Error has occurred: {0}", _errorMessage));    
                }

                return _value;
            }
        }

        public bool IsValid
        {
            get { return _errorMessage == null; }    
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public Error<TResult> Bind<TResult>(Func<T, Error<TResult>> operation)
        {
            return IsValid
                ? operation(_value)
                : _errorMessage.ToError<TResult>();
        }

        public Error<T> ThrowOnError(Func<string, Exception> exceptionBuilder)
        {
            if (!IsValid)
            {
                throw exceptionBuilder(_errorMessage);
            }

            return this;
        }

        public T Return(Func<string, Exception> exceptionBuilder)
        {
            return ThrowOnError(exceptionBuilder).Value;
        }

        public bool Equals(Error<T> other)
        {
            return IsValid
                ? EqualityComparer<T>.Default.Equals(_value, other._value)
                : _errorMessage.Equals(other._errorMessage);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is Error<T> && Equals((Error<T>)obj);
        }

        public override int GetHashCode()
        {
            return IsValid
                ? EqualityComparer<T>.Default.GetHashCode(_value)
                : EqualityComparer<string>.Default.GetHashCode(_errorMessage);
        }

        public static implicit operator Error<T>(Error<Error<T>> doubleError)
        {
            return doubleError.IsValid 
                ? doubleError.Value 
                : doubleError.ErrorMessage.ToError<T>();
        }

        public static bool operator ==(Error<T> left, Error<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Error<T> left, Error<T> right)
        {
            return !left.Equals(right);
        }
    }
}