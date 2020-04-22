# Errors and Results

[![NuGet package](https://img.shields.io/nuget/v/Rixian.Extensions.Errors.svg)](https://nuget.org/packages/Rixian.Extensions.Errors)

## Overview

This library provides base types for working with errors deliberate way within your applications. The main types found in the library are:

- `Error`
- `Result`/`Result<T>`
- `HttpProblem`

## Features

- The `Error` object provides an implementation of the error response in the [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines/blob/vNext/Guidelines.md#errorresponse--object)
- The `Result` and `Result<T>` objects provide a mechanism for methods to return either a value or an error.
- The `HttpProblem` class represents an HttpResponse of type `application/problem+json`. See the [RFC](https://tools.ietf.org/html/rfc7807)
- A [`Prelude`](#prelude) class is provided to add additional helper methods to make working with Errors and Results easier.

## Usage

### Error

The `Error` class provides the basic properties required in the [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines/blob/vNext/Guidelines.md#errorresponse--object). This class explicitly allows for subclassing in order to create predefined errors or add extra properties beyond the basics. For example, we provide the `UnhandledError` class which looks like this (simplified):

```csharp
public class UnhandledError : Error
{
    public UnhandledError()
    {
        this.Code = "unhandled";
    }
}
```

You can also add other properties to expand on the error as needed:

```csharp
public class InvalidUserError : Error
{
    public InvalidUserError(string username)
    {
        this.Code = "invalid_user";
        this.UserName = username;
    }

    public string UserName { get; set; }
}
```

### Results

The `Result` and `Result<T>` classes are implementations of a discriminated union where it can either be an `Error` or a value. These classes exist to make error handling more explicit by having developers check if a `Result` contains an error before grabbing the value.

AN example usage may look like this:
```csharp
public class Calc
{
    public Result<double> Add(double x, double y)
    {
        return x + y; // Implicit conversion to Result<double>
    }

    public Result<double> Divide(double numerator, double denominator)
    {
        if (denominator == 0)
        {
            Error error = Error("divide_by_zero"); // Method provided by the Prelude class
            return error; // Implicit conversion to Result<double>.
        }

        return numerator / denominator;
    }
}
```

### Prelude

The `Prelude` class makes use of the C# [using static directive](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-static). In your class add the following usings:

```csharp
using Rixian.Extensions.Errors;
using static Rixian.Extensions.Errors.Prelude;
```

You now have access to the following:

- `Error()`
- `BadArgumentError()`
- `NullArgumentDisallowedError()`
- `NullValueDisallowedError()`
- `EmptyGuidDisallowedError()`
- `Result<T>()`
- `NullResult<T>()`
- `ErrorResult()`
- `ErrorResult<T>()`
- `RequireGuid()`
- `DefaultResult` (Readonly field)

### HttpProblem

The `HttpProblem` class and its sibling `HttpProblemError` are used for working with the [HTTP Problem RFC](https://tools.ietf.org/html/rfc7807). This class is intended to bridge the gap into the `Error` class to take advantage of those helpers.
