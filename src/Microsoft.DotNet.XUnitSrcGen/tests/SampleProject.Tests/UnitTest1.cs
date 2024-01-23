// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace SampleProject.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {

    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("Goodbye")]
    public void Theory1(string str)
    {
        Assert.NotNull(str);
    }
}