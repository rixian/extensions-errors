// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    /// <summary>
    /// Helper methods for the ErrorResult class.
    /// See: https://stackoverflow.com/a/4280626/6640574
    /// </summary>
    public static class ErrorResult
    {
        /// <summary>
        /// Creates a new ErrorResponse with a value.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The ErrorResponse.</returns>
        public static ErrorResult<T> Create<T>(T value)
        {
            return new ErrorResult<T>(value);
        }

        /// <summary>
        /// Creates a new ErrorResponse with an error.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="error">The error.</param>
        /// <returns>The ErrorResponse.</returns>
        public static ErrorResult<T> Create<T>(ErrorBase error)
        {
            return new ErrorResult<T>(error);
        }
    }
}
