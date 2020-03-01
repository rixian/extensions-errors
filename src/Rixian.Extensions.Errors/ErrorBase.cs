// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// The base error object.
    /// </summary>
    public class ErrorBase
    {
        /// <summary>
        /// Gets or sets the server-defined error codes.
        /// </summary>
        [JsonRequired]
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the human-readable error message.
        /// </summary>
        [JsonRequired]
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the error target.
        /// </summary>
        [JsonProperty("target", NullValueHandling = NullValueHandling.Ignore)]
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the error details.
        /// </summary>
        [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ErrorBase> Details { get; set; }

        /// <summary>
        /// Gets or sets the inner error with more detailed information.
        /// </summary>
        [JsonProperty("innererror", NullValueHandling = NullValueHandling.Ignore)]
        public InnerError InnerError { get; set; }

        /// <summary>
        /// Creates an <see cref="ErrorResponse"/> from this Error.
        /// </summary>
        /// <returns>The <see cref="ErrorResponse"/>.</returns>
        public ErrorResponse ToResponse()
        {
            return new ErrorResponse(this);
        }
    }
}
