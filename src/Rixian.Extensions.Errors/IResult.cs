// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    /// <summary>
    /// Represents a result that is either a success or an error.
    /// See: https://stackoverflow.com/a/4280626/6640574.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Gets a value indicating whether the value is an error or not.
        /// </summary>
        public bool IsFail { get; }

        /// <summary>
        /// Gets the error.
        /// </summary>
#pragma warning disable CA1716 // Identifiers should not match keywords
        public Error Error { get; }
#pragma warning restore CA1716 // Identifiers should not match keywords
    }
}
