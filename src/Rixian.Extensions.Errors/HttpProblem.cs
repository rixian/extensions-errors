﻿// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Errors
{
    /// <summary>
    /// Http problem type, specifically "application/problem+json".
    /// See: https://tools.ietf.org/html/rfc7807 for details.
    /// </summary>
    public class HttpProblem
    {
        /// <summary>
        /// Gets or sets the problem type.
        /// </summary>
        /// <remarks>
        /// A URI reference [RFC3986] that identifies the problem type.
        /// This specification encourages that, when dereferenced, it provide human-readable documentation for the problem type (e.g., using HTML [W3C.REC-html5-20141028]).
        /// When this member is not present, its value is assumed to be "about:blank".
        ///
        /// Consumers MUST use the "type" string as the primary identifier for the problem type.
        /// Consumers SHOULD NOT automatically dereference the type URI.
        ///
        /// Note that the "type" property accepts relative URIs; this means that they must be resolved relative to the document's base URI, as per[RFC3986], Section 5.
        /// </remarks>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the problem title.
        /// </summary>
        /// <remarks>
        /// A short, human-readable summary of the problem type.
        /// It SHOULD NOT change from occurrence to occurrence of the problem, except for purposes of localization (e.g., using proactive content negotiation; see[RFC7231], Section 3.4).
        ///
        /// The "title" string is advisory and included only for users who are not aware of the semantics of the URI and do not have the ability to discover them (e.g., offline log analysis).
        /// </remarks>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the problem status.
        /// </summary>
        /// <remarks>
        /// The HTTP status code([RFC7231], Section 6) generated by the origin server for this occurrence of the problem.
        ///
        /// The "status" member, if present, is only advisory; it conveys the HTTP status code used for the convenience of the consumer.
        /// Generators MUST use the same status code in the actual HTTP response, to assure that generic HTTP software that does not understand this format still behaves correctly.
        /// See Section 5 for further caveats regarding its use.
        ///
        /// Consumers can use the status member to determine what the original status code used by the generator was, in cases where it has been changed(e.g., by an intermediary or cache),
        /// and when message bodies persist without HTTP information.
        /// Generic HTTP software will still use the HTTP status code.
        /// </remarks>
        public int? Status { get; set; }

        /// <summary>
        /// Gets or sets the problem details.
        /// </summary>
        /// <remarks>
        /// A human-readable explanation specific to this occurrence of the problem.
        ///
        /// The "detail" member, if present, ought to focus on helping the client correct the problem, rather than giving debugging information.
        ///
        /// Consumers SHOULD NOT parse the "detail" member for information; extensions are more suitable and less error-prone ways to obtain such information.
        /// </remarks>
        public string Detail { get; set; }

        /// <summary>
        /// Gets or sets the problem instance.
        /// </summary>
        /// <remarks>
        /// A URI reference that identifies the specific occurrence of the problem.
        /// It may or may not yield further information if dereferenced.
        ///
        /// Note that the "instance" property accepts relative URIs; this means that they must be resolved relative to the document's base URI, as per[RFC3986], Section 5.
        /// </remarks>
        public string Instance { get; set; }
    }
}
