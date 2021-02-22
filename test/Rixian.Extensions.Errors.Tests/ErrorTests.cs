// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Rixian.Extensions.Errors;
using Xunit;
using Xunit.Abstractions;
using static Rixian.Extensions.Errors.Prelude;

#nullable disable

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
        Error error = new Error
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
        Result(123);
        Error("Failed");

        Result<int> result = 123;

        result.Value.Should().Be(123);
    }

    [Fact]
    public void ErrorResult_Converter_Implicit_String_Success()
    {
        var value = "abc";
        Result<string> result = value;

        result.Value.Should().Be(value);
    }

    [Fact]
    public void ErrorResult_Helper_Interface_Success()
    {
        IFoo value = (IFoo)null;
        Result<IFoo> result = Result(value);

        result.Value.Should().Be(value);
    }

    [Fact]
    public void ErrorResult_Converter_Implicit_Class_Success()
    {
        var value = (Bar)null;
        Result<Bar> result = value;

        result.Value.Should().Be(value);
    }

    [Fact]
    public void ErrorResult_Converter_Implicit_TargetClass_Success()
    {
        var value = (Bar)null;
        Result<Bar> result = value;
        Bar finalValue = result;

        finalValue.Should().Be(value);
    }
}

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1201 // Elements should appear in the correct order
internal interface IFoo
{
}

#pragma warning disable CA1812
#pragma warning disable SA1600
internal class Bar
{
}
#pragma warning restore SA1600
#pragma warning restore CA1812
#pragma warning restore SA1201 // Elements should appear in the correct order
#pragma warning restore SA1402 // File may only contain a single type
