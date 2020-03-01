// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    using Newtonsoft.Json;

    /// <summary>
    /// The inner error object.
    /// </summary>
    public class InnerError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InnerError"/> class.
        /// </summary>
        /// <param name="code">The specific error code.</param>
        public InnerError(string code)
        {
            this.Code = code;
        }

        /// <summary>
        /// Gets or sets a more specific error code for the error.
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the inner error.
        /// </summary>
        [JsonProperty("innererror", NullValueHandling = NullValueHandling.Ignore)]
        public InnerError Error { get; set; }
    }
}
