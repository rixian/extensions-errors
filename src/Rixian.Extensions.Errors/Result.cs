// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    using System;

    /// <summary>
    /// Represents a result that is either a success or an error.
    /// See: https://stackoverflow.com/a/4280626/6640574.
    /// </summary>
    public struct Result : IEquatable<Result>
    {
        private readonly ErrorBase error;
        private readonly int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> struct.
        /// </summary>
        /// <param name="error">The error to store.</param>
        public Result(ErrorBase error)
            : this(1, value1: error)
        {
        }

        private Result(int index, ErrorBase value1 = default(ErrorBase))
        {
            this.index = index;
            this.error = value1;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public object Value
        {
            get
            {
                switch (this.index)
                {
                    case 0:
                        return null;
                    case 1:
                        return this.error;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the value is a result or not.
        /// </summary>
        public bool IsSuccess => this.index == 0;

        /// <summary>
        /// Gets a value indicating whether the value is an error or not.
        /// </summary>
        public bool IsError => this.index == 1;

        /// <summary>
        /// Gets the error.
        /// </summary>
        public ErrorBase Error
        {
            get
            {
                if (this.index != 1)
                {
                    throw new InvalidOperationException($"Cannot return as T1 as result is T{this.index}");
                }

                return this.error;
            }
        }

#pragma warning disable CA2225 // Operator overloads have named alternates
        /// <summary>
        /// Converts an ErrorBase into an ErrorResult.
        /// </summary>
        /// <param name="error">The ErrorBase.</param>
        public static implicit operator Result(ErrorBase error) => new Result(1, value1: error);
#pragma warning restore CA2225 // Operator overloads have named alternates

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
        public static Result<T> Create<T>(ErrorBase error)
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
        /// Creates a new result with default value for the type.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The results with a default value.</returns>
        public static Result<T> Default<T>()
        {
            return new Result<T>(default(T));
        }

        /// <summary>
        /// Executes one of the actions depending on the type of the stored value.
        /// </summary>
        /// <param name="onSuccess">The action to execute for a value.</param>
        /// <param name="onError">The action to execute for an error.</param>
        public void Switch(Action onSuccess, Action<ErrorBase> onError)
        {
            if (this.index == 0 && onSuccess != null)
            {
                onSuccess();
                return;
            }

            if (this.index == 1 && onError != null)
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
        public TResult Match<TResult>(Func<TResult> onSuccess, Func<ErrorBase, TResult> onError)
        {
            if (this.index == 0 && onSuccess != null)
            {
                return onSuccess();
            }

            if (this.index == 1 && onError != null)
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
            switch (this.index)
            {
                case 0: return string.Empty;
                case 1: return FormatValue(typeof(ErrorBase), this.error);
            }

            return null;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode;
                switch (this.index)
                {
                    case 0:
                        hashCode = 0;
                        break;
                    case 1:
                        hashCode = this.error?.GetHashCode() ?? 0;
                        break;
                    default:
                        hashCode = 0;
                        break;
                }

                return (hashCode * 397) ^ this.index;
            }
        }

        /// <inheritdoc/>
        public bool Equals(Result other)
        {
            if (this.index != other.index)
            {
                return false;
            }

            switch (this.index)
            {
                case 0: return true;
                case 1: return Equals(this.error, other.error);
                default: return false;
            }
        }

        private static string FormatValue<TValue>(Type type, TValue value) => $"{type.FullName}: {value?.ToString()}";
    }
}
