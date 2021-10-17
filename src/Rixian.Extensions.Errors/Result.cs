// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    using System;

    /// <summary>
    /// Represents a result that is either a success or an error.
    /// See: https://stackoverflow.com/a/428ResultType.Success626/664ResultType.Success574.
    /// </summary>
    public abstract record Result
    {
        private static readonly Type SuccessType = typeof(Success<>);

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        internal Result()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="isSuccess">The success indicator.</param>
        internal protected Result(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }

        /// <summary>
        /// Gets a value indicating whether the value is a result or not.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the value is an error or not.
        /// </summary>
        public bool IsFail => !this.IsSuccess;

        /// <summary>
        /// Gets a default success result.
        /// </summary>
        public static Result Default { get; } = Null<object>();

#pragma warning disable CA2225 // Operator overloads have named alternates
        /// <summary>
        /// Converts an Error instance to a Result.
        /// </summary>
        /// <param name="error">The error.</param>
        public static implicit operator Result(Error error) => new Fail(error);
#pragma warning restore CA2225 // Operator overloads have named alternates

        /// <summary>
        /// Casts the current Result instance to an instance of Success.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>The Success instance.</returns>
        public Success<T> AsSuccess<T>()
        {
            return (Success<T>)this;
        }

        /// <summary>
        /// Casts the current Result instance to an instance of Fail.
        /// </summary>
        /// <returns>The Fail instance.</returns>
        public Fail AsFail()
        {
            return (Fail)this;
        }

        /// <summary>
        /// Converts an object to a Result.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The Result instance containing the object.</returns>
        public static Result? AsResult(object obj) => Activator.CreateInstance(SuccessType.MakeGenericType(obj.GetType()), obj) as Result;

        /// <summary>
        /// Converts a typed value to a Result.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The Result instance containing the object.</returns>
        public static Success<T> AsResult<T>(T? value) => new Success<T>(value);

        /// <summary>
        /// Converts an Error to a Result.
        /// </summary>
        /// <param name="error">The Error.</param>
        /// <returns>The Result instance containing the Error.</returns>
        public static Fail AsResult(Error error) => new Fail(error);

        /// <summary>
        /// Converts a typed value to a Result.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The Result instance containing the object.</returns>
        public static Success<T> Success<T>(T? value) => new Success<T>(value);

        /// <summary>
        /// Converts an Error to a Result.
        /// </summary>
        /// <param name="error">The Error.</param>
        /// <returns>The Result instance containing the Error.</returns>
        public static Fail Fail(Error error) => new Fail(error);

        /// <summary>
        /// Converts a Result to a tuple.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="result">The Result.</param>
        /// <returns>The tuple containing the result values.</returns>
        public static (T? Value, Error? Err) AsTuple<T>(Result result)
        {
            if (result.IsSuccess && result is Success<T> success)
            {
                return (success.Value, default);
            }
            else if (result.IsFail && result is Fail fail)
            {
                return (default, fail.Error);
            }
            else
            {
                throw new System.NotSupportedException("Result is in an impossible state.");
            }
        }

        /// <summary>
        /// Converts a tuple to a Result.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="tuple">The tuple.</param>
        /// <returns>The Result containing the tuple values.</returns>
        public static Result FromTuple<T>((T? Value, Error? Err) tuple)
        {
            if (tuple.Err is null)
            {
                return new Success<T>(tuple.Value);
            }
            else
            {
                return new Fail(tuple.Err);
            }
        }

        /// <summary>
        /// Converts a Result to a typed Result.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="result">The Result.</param>
        /// <returns>The Result containing the values.</returns>
        public static Result<T> AsResult<T>(Result result)
        {
            if (result.IsSuccess && result is Success<T> success)
            {
                return new Result<T>(success.Value);
            }
            else if (result.IsFail && result is Fail fail)
            {
                return new Result<T>(fail.Error);
            }
            else
            {
                throw new System.NotSupportedException("Result is in an impossible state.");
            }
        }

        /// <summary>
        /// Creates a new Success with a value.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The Success.</returns>
        public static Success<T> New<T>(T value)
        {
            return new Success<T>(value);
        }

        /// <summary>
        /// Creates a new Fail with an error.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="error">The error.</param>
        /// <returns>The Fail.</returns>
        public static Fail New<T>(Error error)
        {
            return new Fail(error);
        }

        /// <summary>
        /// Creates a new result with a null value.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The results with an null value.</returns>
        public static Result<T> Null<T>()
            where T : class
        {
            return new Result<T>((T?)null);
        }

        /// <summary>
        /// Executes one of the actions depending on the type of the stored value.
        /// </summary>
        /// <param name="onSuccess">The action to execute for a value.</param>
        /// <param name="onError">The action to execute for an error.</param>
        public void Switch(Action onSuccess, Action<Error> onError)
        {
            if (this.IsSuccess && onSuccess != null)
            {
                onSuccess();
                return;
            }

            if (this.IsFail && onError != null)
            {
                onError(this.AsFail().Error);
                return;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Executes one of the actions depending on the type of the stored value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="onSuccess">The action to execute for a value.</param>
        /// <param name="onError">The action to execute for an error.</param>
        public void Switch<TValue>(Action<TValue?> onSuccess, Action<Error> onError)
        {
            if (this.IsSuccess && onSuccess != null)
            {
                onSuccess(this.AsSuccess<TValue>().Value);
                return;
            }

            if (this.IsFail && onError != null)
            {
                onError(this.AsFail().Error);
                return;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Performs a mapping depending on the type of the stored value.
        /// </summary>
        /// <typeparam name="TResult">The resultant type.</typeparam>
        /// <param name="onSuccess">The mapping for a value.</param>
        /// <param name="onError">The mapping for an error.</param>
        /// <returns>The mapped result.</returns>
        public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onError)
        {
            if (this.IsSuccess && onSuccess != null)
            {
                return onSuccess();
            }

            if (this.IsFail && onError != null)
            {
                return onError(this.AsFail().Error);
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Performs a mapping depending on the type of the stored value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TResult">The resultant type.</typeparam>
        /// <param name="onSuccess">The mapping for a value.</param>
        /// <param name="onError">The mapping for an error.</param>
        /// <returns>The mapped result.</returns>
        public TResult Match<TValue, TResult>(Func<TValue?, TResult> onSuccess, Func<Error, TResult> onError)
        {
            if (this.IsSuccess && onSuccess != null)
            {
                return onSuccess(this.AsSuccess<TValue>().Value);
            }

            if (this.IsFail && onError != null)
            {
                return onError(this.AsFail().Error);
            }

            throw new InvalidOperationException();
        }
    }
}
