// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// The base error object.
    /// </summary>
#pragma warning disable CA1716 // Identifiers should not match keywords
    public class Error
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        /// <summary>
        /// Gets or sets the server-defined error codes.
        /// </summary>
        [JsonProperty("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the human-readable error message.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the error target.
        /// </summary>
        [JsonProperty("target", NullValueHandling = NullValueHandling.Ignore)]
        public string? Target { get; set; }

        /// <summary>
        /// Gets or sets the error details.
        /// </summary>
        [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Error>? Details { get; set; }

        /// <summary>
        /// Gets or sets the inner error with more detailed information.
        /// </summary>
        [JsonProperty("innererror", NullValueHandling = NullValueHandling.Ignore)]
        public Error? InnerError { get; set; }
    }
}
