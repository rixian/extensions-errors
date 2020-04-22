// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    using System;

    /// <summary>
    /// Represents a result that is either a success or an error.
    /// See: https://stackoverflow.com/a/428ResultType.Success626/664ResultType.Success574.
    /// </summary>
    public struct Result : IResult, IEquatable<Result>
    {
        /// <summary>
        /// Gets a result with no error.
        /// </summary>
        public static readonly Result Default = new Result(ResultType.Success);

        private readonly Error error;
        private readonly ResultType resultType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> struct.
        /// </summary>
        /// <param name="error">The error to store.</param>
        public Result(Error error)
            : this(ResultType.Fail, error: error)
        {
        }

        private Result(ResultType resultType, Error error = default(Error))
        {
            this.resultType = resultType;
            this.error = error;
        }

        private enum ResultType
        {
            Fail = 0,
            Success,
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public object Value
        {
            get
            {
                switch (this.resultType)
                {
                    case ResultType.Success:
                        return null;
                    case ResultType.Fail:
                        return this.error;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the value is a result or not.
        /// </summary>
        public bool IsSuccess => this.resultType == ResultType.Success;

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
                    throw new InvalidOperationException($"Cannot return as TResultType.Fail as result is T{this.resultType}");
                }

                return this.error;
            }
        }

        /// <summary>
        /// Determines if two instance of ErrorResult are equal.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The equality result.</returns>
        public static bool operator ==(Result left, Result right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines if two instance of ErrorResult are not equal.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The equality result.</returns>
        public static bool operator !=(Result left, Result right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Creates a new ErrorResponse with a value.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The ErrorResponse.</returns>
        public static Result<T> Create<T>(T value)
        {
            return new Result<T>(value);
        }

        /// <summary>
        /// Creates a new ErrorResponse with an error.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="error">The error.</param>
        /// <returns>The ErrorResponse.</returns>
        public static Result<T> Create<T>(Error error)
        {
            return new Result<T>(error);
        }

        /// <summary>
        /// Creates a new result with a null value.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The results with an null value.</returns>
        public static Result<T> Null<T>()
            where T : class
        {
            return new Result<T>((T)null);
        }

        /// <summary>
        /// Executes one of the actions depending on the type of the stored value.
        /// </summary>
        /// <param name="onSuccess">The action to execute for a value.</param>
        /// <param name="onError">The action to execute for an error.</param>
        public void Switch(Action onSuccess, Action<Error> onError)
        {
            if (this.resultType == ResultType.Success && onSuccess != null)
            {
                onSuccess();
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
        /// <param name="onSuccess">The mapping for a value.</param>
        /// <param name="onError">The mapping for an error.</param>
        /// <returns>The mapped result.</returns>
        public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onError)
        {
            if (this.resultType == ResultType.Success && onSuccess != null)
            {
                return onSuccess();
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

            return obj is Result && this.Equals((Result)obj);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            switch (this.resultType)
            {
                case ResultType.Success: return string.Empty;
                case ResultType.Fail: return FormatValue(typeof(Error), this.error);
                default: return string.Empty;
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
                        hashCode = 0;
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
        public bool Equals(Result other)
        {
            if (this.resultType != other.resultType)
            {
                return false;
            }

            switch (this.resultType)
            {
                case ResultType.Success: return true;
                case ResultType.Fail: return Equals(this.error, other.error);
                default: return false;
            }
        }

        private static string FormatValue<TValue>(Type type, TValue value) => $"{type.FullName}: {value?.ToString()}";
    }
}
