using System;
using System.Collections.Generic;

namespace Woz.Functional.Try
{
    internal struct TryFailed<T> : ITry<T>
    {
        private readonly string _errorMessage;

        internal TryFailed(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public bool IsValid
        {
            get { return false; }
        }

        public T Value
        {
            get
            {
                throw new InvalidOperationException(
                    string.Format("Try has no value, failed with: {0}", _errorMessage));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }


        public ITry<TResult> Bind<TResult>(Func<T, ITry<TResult>> operation)
        {
            return _errorMessage.ToFailed<TResult>();
        }

        public ITry<TResult> TryBind<TResult>(Func<T, ITry<TResult>> operation)
        {
            return _errorMessage.ToFailed<TResult>();
        }

        public ITry<T> ThrowOnError(Func<string, Exception> exceptionBuilder)
        {
            throw exceptionBuilder(_errorMessage);
        }

        public T ReturnOrThrow(Func<string, Exception> exceptionBuilder)
        {
            throw exceptionBuilder(_errorMessage);
        }

        public bool Equals(ITry<T> other)
        {
            return
                !other.IsValid &&
                EqualityComparer<string>.Default.Equals(_errorMessage, other.ErrorMessage);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is TryFailed<T> && Equals((TryFailed<T>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<string>.Default.GetHashCode(_errorMessage);
        }
    }
}