// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    using System;

    /// <summary>
    /// Represents a result that is a value.
    /// </summary>
    public sealed record Success<T> : Result
    {
        private readonly T? value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Success{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Success(T? value)
            : base(isSuccess: true)
        {
            this.value = value;
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
        /// Converts a value to a Success.
        /// </summary>
        /// <param name="t">The value.</param>
        public static implicit operator Success<T>(T t) => new Success<T>(t);

        /// <summary>
        /// Converts a Result to a tuple.
        /// </summary>
        /// <param name="success">The Success instance.</param>
        /// <returns>The tuple containing the result values.</returns>
        public static implicit operator (T?, Error?)(Success<T> success) => (success.Value, default);
    }
}
