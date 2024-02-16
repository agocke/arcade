// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace SampleTests;

public class SampleTestClass
{
    [Fact]
    public void Fact1()
    { }

    [Fact]
    public void FailingFact()
    {
        Assert.True(false);
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("Goodbye")]
    public void Theory1(string str)
    {
        Assert.NotNull(str);
    }
}