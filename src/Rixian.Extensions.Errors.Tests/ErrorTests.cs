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

    [Fact]
    public void ErrorResult_Converter_Implicit_Int_Success()
    {
        ErrorResult<int> result = 123;

        result.Result.Should().Be(123);
    }

    [Fact]
    public void ErrorResult_Converter_Implicit_String_Success()
    {
        var value = "abc";
        ErrorResult<string> result = value;

        result.Result.Should().Be(value);
    }

    [Fact]
    public void ErrorResult_Helper_Interface_Success()
    {
        IFoo value = (IFoo)null;
        ErrorResult<IFoo> result = ErrorResult.Create(value);

        result.Result.Should().Be(value);
    }

    [Fact]
    public void ErrorResult_Converter_Implicit_Class_Success()
    {
        var value = (Bar)null;
        ErrorResult<Bar> result = value;

        result.Result.Should().Be(value);
    }
}

interface IFoo
{
}

class Bar
{
}
