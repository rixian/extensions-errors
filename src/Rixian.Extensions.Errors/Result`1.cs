// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    using System;

    /// <summary>
    /// Represents a result that is either a value or an error.
    /// </summary>
    public sealed record Result<T> : Result
    {
        private readonly T? value;
        private readonly Error? error;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="value">The value to store.</param>
        public Result(T? value)
            : base(isSuccess: true)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="error">The error to store.</param>
        public Result(Error error)
            : base(isSuccess: false)
        {
            this.error = error;
        }

        /// <summary>
        /// Gets the result value.
        /// </summary>
        public T? Value
        {
            get
            {
                if (this.IsFail)
                {
                    throw new InvalidOperationException("InvalidCastToValueErrorMessage");
                }

                return this.value;
            }
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Error Error
        {
            get
            {
                if (this.IsSuccess)
                {
                    throw new InvalidOperationException("InvalidCastToErrorErrorMessage");
                }

                if (this.error is null)
                {
                    throw new System.NotSupportedException("Result is in an impossible state.");
                }

                return this.error;
            }
        }

#pragma warning disable CA2225 // Operator overloads have named alternates
        /// <summary>
        /// Converts a value into a Result instance.
        /// </summary>
        /// <param name="t">The value.</param>
        public static implicit operator Result<T>(T t) => new Result<T>(t);

        /// <summary>
        /// Converts an Error into a Result instance.
        /// </summary>
        /// <param name="error">The error.</param>
        public static implicit operator Result<T>(Error error) => new Result<T>(error);

        /// <summary>
        /// Converts a Result into a value. Throws if not a success result.
        /// </summary>
        /// <param name="result">The result.</param>
        public static implicit operator T?(Result<T> result) => result.Value;

        /// <summary>
        /// Converts a Result into an Error instance. Throws if not a fail result.
        /// </summary>
        /// <param name="result">The Error instance.</param>
        public static implicit operator Error(Result<T> result) => result.Error;

        /// <summary>
        /// Converts a Result into a Fail instance. Throws if not a fail result.
        /// </summary>
        /// <param name="result">The Fail instance.</param>
        public static implicit operator Fail(Result<T> result) => new Fail(result.Error);

        /// <summary>
        /// Converts a Fail into a Result instance.
        /// </summary>
        /// <param name="fail">The Fail instance.</param>
        public static implicit operator Result<T>(Fail fail) => new Result<T>(fail.Error);

        /// <summary>
        /// Converts a Success into a Result.
        /// </summary>
        /// <param name="success">The Success instance.</param>
        public static implicit operator Result<T>(Success<T> success) => new Result<T>(success.Value);

        /// <summary>
        /// Converts a Result instance into a Success. Throws if not a success result.
        /// </summary>
        /// <param name="result">The Success instance.</param>
        public static implicit operator Success<T>(Result<T> result) => new Success<T>(result.Value);

        /// <summary>
        /// Converts a Result instance to a tuple.
        /// </summary>
        /// <param name="result">The Result instance.</param>
        /// <returns>The tuple containing the result values.</returns>
        public static implicit operator (T?, Error?)(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return (result.Value, default);
            }
            else if (result.IsFail)
            {
                return (default, result.Error);
            }
            else
            {
                throw new System.NotSupportedException("Result is in an impossible state.");
            }
        }

        /// <summary>
        /// Converts a tuple to a Result instance.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <returns>The Result containing the tuple values.</returns>
        public static implicit operator Result<T>((T? t, Error? error) tuple)
        {
            if (tuple.error is null)
            {
                return new Result<T>(tuple.t);
            }
            else
            {
                return new Result<T>(tuple.error);
            }
        }
#pragma warning restore CA2225 // Operator overloads have named alternates

        /// <summary>
        /// Executes one of the actions depending on the type of the stored value.
        /// </summary>
        /// <param name="onValue">The action to execute for a value.</param>
        /// <param name="onError">The action to execute for an error.</param>
        public void Switch(Action<T?> onValue, Action<Error> onError)
        {
            if (this.IsSuccess && onValue != null)
            {
                onValue(this.Value);
                return;
            }

            if (this.IsFail && onError != null)
            {
                onError(this.Error);
                return;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Performs a mapping depending on the type of the stored value.
        /// </summary>
        /// <typeparam name="TResult">The resultant type.</typeparam>
        /// <param name="onValue">The mapping for a value.</param>
        /// <param name="onError">The mapping for an error.</param>
        /// <returns>The mapped result.</returns>
        public TResult Match<TResult>(Func<T?, TResult> onValue, Func<Error, TResult> onError)
        {
            if (this.IsSuccess && onValue != null)
            {
                return onValue(this.Value);
            }

            if (this.IsFail && onError != null)
            {
                return onError(this.Error);
            }

            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.IsSuccess)
            {
                return FormatValue(typeof(T), this.value);
            }

            if (this.IsFail)
            {
                return FormatValue(typeof(Error), this.error);
            }

            return base.ToString();
        }

        private static string FormatValue<TValue>(Type type, TValue value) => $"{type.FullName}: {value?.ToString()}";
    }
}
