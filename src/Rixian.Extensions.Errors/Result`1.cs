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
    public struct Result<T> : IEquatable<Result<T>>
    {
        private readonly T value;
        private readonly ErrorBase error;
        private readonly int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> struct.
        /// </summary>
        /// <param name="value">The value to store.</param>
        public Result(T value)
            : this(0, value0: value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> struct.
        /// </summary>
        /// <param name="error">The error to store.</param>
        public Result(ErrorBase error)
            : this(1, value1: error)
        {
        }

        private Result(int index, T value0 = default(T), ErrorBase value1 = default(ErrorBase))
        {
            this.index = index;
            this.value = value0;
            this.error = value1;
        }

        /// <summary>
        /// Gets a value indicating whether the value is a result or not.
        /// </summary>
        public bool IsResult => this.index == 0;

        /// <summary>
        /// Gets the result value.
        /// </summary>
        public T Value
        {
            get
            {
                if (this.index != 0)
                {
                    throw new InvalidOperationException(Properties.Resources.InvalidCastToValueErrorMessage);
                }

                return this.value;
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
                    throw new InvalidOperationException(Properties.Resources.InvalidCastToErrorErrorMessage);
                }

                return this.error;
            }
        }

#pragma warning disable CA2225 // Operator overloads have named alternates
        /// <summary>
        /// Converts an ErrorBase into an ErrorResult.
        /// </summary>
        /// <param name="error">The ErrorBase.</param>
        public static implicit operator Result<T>(ErrorBase error) => new Result<T>(1, value1: error);

        /// <summary>
        /// Converts a value into an ErrorResult.
        /// </summary>
        /// <param name="value">The value.</param>
        public static implicit operator Result<T>(T value) => new Result<T>(0, value0: value);

        /// <summary>
        /// Converts a value into aa value.
        /// </summary>
        /// <param name="result">The result.</param>
        public static implicit operator T(Result<T> result) => result.Value;

        /// <summary>
        /// Converts a value into an ErrorResult.
        /// </summary>
        /// <param name="result">The result.</param>
        public static implicit operator ErrorBase(Result<T> result) => result.Error;
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
        public void Switch(Action<T> onValue, Action<ErrorBase> onError)
        {
            if (this.index == 0 && onValue != null)
            {
                onValue(this.value);
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
                return onValue(this.value);
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

            return obj is Result<T> && this.Equals((Result<T>)obj);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            switch (this.index)
            {
                case 0: return FormatValue(typeof(T), this.value);
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
                        hashCode = this.value?.GetHashCode() ?? 0;
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
        public bool Equals(Result<T> other)
        {
            if (this.index != other.index)
            {
                return false;
            }

            switch (this.index)
            {
                case 0: return Equals(this.value, other.value);
                case 1: return Equals(this.error, other.error);
                default: return false;
            }
        }

        private static string FormatValue<TValue>(Type type, TValue value) => $"{type.FullName}: {value?.ToString()}";
    }
}
