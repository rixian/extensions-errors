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
*     --- Fixed one of the types to ErrorBase.
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
    public struct ErrorResult<T> : IEquatable<ErrorResult<T>>
    {
        private readonly T result;
        private readonly ErrorBase error;
        private readonly int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResult{T}"/> struct.
        /// </summary>
        /// <param name="value">The value to store.</param>
        public ErrorResult(T value)
            : this(0, value0: value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResult{T}"/> struct.
        /// </summary>
        /// <param name="error">The error to store.</param>
        public ErrorResult(ErrorBase error)
            : this(1, value1: error)
        {
        }

        private ErrorResult(int index, T value0 = default(T), ErrorBase value1 = default(ErrorBase))
        {
            this.index = index;
            this.result = value0;
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
                        return this.result;
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
        public bool IsResult => this.index == 0;

        /// <summary>
        /// Gets the result.
        /// </summary>
        public T Result
        {
            get
            {
                if (this.index != 0)
                {
                    throw new InvalidOperationException($"Cannot return as T0 as result is T{this.index}");
                }

                return this.result;
            }
        }

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
        public static implicit operator ErrorResult<T>(ErrorBase error) => new ErrorResult<T>(1, value1: error);

        /// <summary>
        /// Converts a value into an ErrorResult.
        /// </summary>
        /// <param name="value">The value.</param>
        public static implicit operator ErrorResult<T>(T value) => new ErrorResult<T>(0, value0: value);
#pragma warning restore CA2225 // Operator overloads have named alternates

        /// <summary>
        /// Determines if two instance of ErrorResult are equal.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The equality result.</returns>
        public static bool operator ==(ErrorResult<T> left, ErrorResult<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines if two instance of ErrorResult are not equal.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The equality result.</returns>
        public static bool operator !=(ErrorResult<T> left, ErrorResult<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Executes one of the actions depending on the type of the stored value.
        /// </summary>
        /// <param name="onValue">The action to execute for a value.</param>
        /// <param name="onError">The action to execute for an error.</param>
        public void Switch(Action<T> onValue, Action<ErrorBase> onError)
        {
            if (this.index == 0 && onValue != null)
            {
                onValue(this.result);
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
        /// <param name="onValue">The mapping for a value.</param>
        /// <param name="onError">The mapping for an error.</param>
        /// <returns>The mapped result.</returns>
        public TResult Match<TResult>(Func<T, TResult> onValue, Func<ErrorBase, TResult> onError)
        {
            if (this.index == 0 && onValue != null)
            {
                return onValue(this.result);
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

            return obj is ErrorResult<T> && this.Equals((ErrorResult<T>)obj);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            switch (this.index)
            {
                case 0: return FormatValue(typeof(T), this.result);
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
                        hashCode = this.result?.GetHashCode() ?? 0;
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
        public bool Equals(ErrorResult<T> other)
        {
            if (this.index != other.index)
            {
                return false;
            }

            switch (this.index)
            {
                case 0: return Equals(this.result, other.result);
                case 1: return Equals(this.error, other.error);
                default: return false;
            }
        }

        private static string FormatValue<TValue>(Type type, TValue value) => $"{type.FullName}: {value?.ToString()}";
    }
}
