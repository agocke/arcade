// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SampleProject.Tests;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.DotNet.XunitSrcGen;

public sealed class FactTests
{
    [Fact]
    public void BasicFact()
    {
        var testCases = new[] {
            new GeneratedTestCase(
                new GeneratedTestMethod(
                    new GeneratedMethodInfo(typeof(SampleTestClass).GetMethod(nameof(SampleTestClass.Fact1))!),
                    new GeneratedTestClass(
                        new GeneratedTypeInfo(typeof(SampleTestClass)),
                        new GeneratedTestCollection(
                            new GeneratedTestAssembly()
                        )
                    )
                )
            )
        };
        SrcGenTestRunner.ExecuteTests(testCases);
    }
}

public sealed class SampleTestClass
{
    [Fact]
    public void Fact1()
    { }
}