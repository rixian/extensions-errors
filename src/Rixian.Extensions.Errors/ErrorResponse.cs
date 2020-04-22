// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    using Newtonsoft.Json;

    /// <summary>
    /// The top level error response object.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        public ErrorResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="error">The error to use.</param>
        public ErrorResponse(Error error)
        {
            this.Error = error;
        }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        [JsonRequired]
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}
