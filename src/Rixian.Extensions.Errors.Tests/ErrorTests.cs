// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Rixian.Extensions.Errors;
using Xunit;
using Xunit.Abstractions;

public class ErrorTests
{
    private readonly ITestOutputHelper logger;

    public ErrorTests(ITestOutputHelper logger)
    {
        this.logger = logger;
    }

    [Fact]
    public void AddOrSubtract()
    {
        ErrorBase error = new ErrorBase
        {
            Code = "Test",
            Message = "Test",
            Details = null,
        };

        ErrorResponse response = error.ToResponse();
        response.Should().NotBeNull();
    }
}
