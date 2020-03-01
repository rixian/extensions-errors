// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Http problem type as an error, specifically "application/problem+json".
    /// See: https://tools.ietf.org/html/rfc7807 for details.
    /// </summary>
    public class HttpProblemError : ErrorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpProblemError"/> class.
        /// </summary>
        /// <param name="problem">The HttpProblem.</param>
        public HttpProblemError(HttpProblem problem)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            this.Code = problem.Type;
            this.Message = string.Join(Environment.NewLine, problem.Title, problem.Detail);
            this.Status = problem.Status;
            this.Instance = problem.Instance;
        }

        /// <summary>
        /// Gets or sets the problem status.
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Gets or sets the problem instance.
        /// </summary>
        public string Instance { get; set; }
    }
}
