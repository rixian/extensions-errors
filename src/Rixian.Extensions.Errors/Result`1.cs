// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

/***************************************************************************************
*    Derived from: OneOf<T0, T1>
*    Author: Harry McIntyre (mcintyre321)
*    Date downloaded: 2019-09-17
*    Commit: 1fb4ff16b87cc140d5b7c8d3fa73730749b590a0
*    Location: https://github.com/mcintyre321/OneOf/commit/1fb4ff16b87cc140d5b7c8d3fa73730749b590a0
*    License: MIT (https://github.com/mcintyre321/OneOf/blob/master/licence.md)
*    Changes:
*     --- Renamed class to ErrorResult.
*     --- Fixed one of the types to IInnerError.
*     --- Renamed properties and methods to be more specific.
*     --- Added XML comments
*     --- Added IEquatable<> interface
*     --- Added equality overloads
*     --- Removed extra methods.
*
***************************************************************************************/

namespace Rixian.Extensions.Errors
{
    using System;

    /// <summary>
    /// Represents a result that is either a value or an error.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public struct Result<T> : IResult, IEquatable<Result<T>>
    {
        private readonly T value;
        private readonly Error error;
        private readonly ResultType resultType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> struct.
        /// </summary>
        /// <param name="value">The value to store.</param>
        public Result(T value)
            : this(ResultType.Success, value: value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> struct.
        /// </summary>
        /// <param name="error">The error to store.</param>
        public Result(Error error)
            : this(ResultType.Fail, error: error)
        {
        }

        private Result(ResultType resultType, T value = default(T), Error error = default(Error))
        {
            this.resultType = resultType;
            this.value = value;
            this.error = error;
        }

        private enum ResultType
        {
            Fail = 0,
            Success,
        }

        /// <summary>
        /// Gets a value indicating whether the value is a result or not.
        /// </summary>
        public bool IsSuccess => this.resultType == ResultType.Success;

        /// <summary>
        /// Gets the result value.
        /// </summary>
        public T Value
        {
            get
            {
                if (this.resultType != ResultType.Success)
                {
                    throw new InvalidOperationException(Properties.Resources.InvalidCastToValueErrorMessage);
                }

                return this.value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the value is an error or not.
        /// </summary>
        public bool IsFail => this.resultType == ResultType.Fail;

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Error Error
        {
            get
            {
                if (this.resultType != ResultType.Fail)
                {
                    throw new InvalidOperationException(Properties.Resources.InvalidCastToErrorErrorMessage);
                }

                return this.error;
            }
        }

        /// <inheritdoc/>
        object IResult.Value => this.Value;

#pragma warning disable CA2225 // Operator overloads have named alternates
        /// <summary>
        /// Converts a value into an ErrorResult.
        /// </summary>
        /// <param name="value">The value.</param>
        public static implicit operator Result<T>(T value) => new Result<T>(ResultType.Success, value: value, error: default);

        /// <summary>
        /// Converts a value into an ErrorResult.
        /// </summary>
        /// <param name="error">The error.</param>
        public static implicit operator Result<T>(Error error) => new Result<T>(ResultType.Fail, value: default, error: error);

        /// <summary>
        /// Converts a value into aa value.
        /// </summary>
        /// <param name="result">The result.</param>
        public static implicit operator T(Result<T> result) => result.Value;
#pragma warning restore CA2225 // Operator overloads have named alternates

        /// <summary>
        /// Determines if two instance of ErrorResult are equal.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The equality result.</returns>
        public static bool operator ==(Result<T> left, Result<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines if two instance of ErrorResult are not equal.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The equality result.</returns>
        public static bool operator !=(Result<T> left, Result<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Executes one of the actions depending on the type of the stored value.
        /// </summary>
        /// <param name="onValue">The action to execute for a value.</param>
        /// <param name="onError">The action to execute for an error.</param>
        public void Switch(Action<T> onValue, Action<Error> onError)
        {
            if (this.resultType == ResultType.Success && onValue != null)
            {
                onValue(this.value);
                return;
            }

            if (this.resultType == ResultType.Fail && onError != null)
            {
                onError(this.error);
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
        public TResult Match<TResult>(Func<T, TResult> onValue, Func<Error, TResult> onError)
        {
            if (this.resultType == ResultType.Success && onValue != null)
            {
                return onValue(this.value);
            }

            if (this.resultType == ResultType.Fail && onError != null)
            {
                return onError(this.error);
            }

            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Result<T> && this.Equals((Result<T>)obj);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            switch (this.resultType)
            {
                case ResultType.Success: return FormatValue(typeof(T), this.value);
                case ResultType.Fail: return FormatValue(typeof(Error), this.error);
                default: return null;
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode;
                switch (this.resultType)
                {
                    case ResultType.Success:
                        hashCode = this.value?.GetHashCode() ?? 0;
                        break;
                    case ResultType.Fail:
                        hashCode = this.error?.GetHashCode() ?? 0;
                        break;
                    default:
                        hashCode = 0;
                        break;
                }

                return (hashCode * 397) ^ this.resultType.GetHashCode();
            }
        }

        /// <inheritdoc/>
        public bool Equals(Result<T> other)
        {
            if (this.resultType != other.resultType)
            {
                return false;
            }

            switch (this.resultType)
            {
                case ResultType.Success: return Equals(this.value, other.value);
                case ResultType.Fail: return Equals(this.error, other.error);
                default: return false;
            }
        }

        private static string FormatValue<TValue>(Type type, TValue value) => $"{type.FullName}: {value?.ToString()}";
    }
}
