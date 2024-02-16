// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.DotNet.XunitSrcGen;

public sealed class FactTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void BasicFact()
    {
        var testCases = new[] {
            new GeneratedTestCase_Fact1()
        };
        var xunitTestFx = new SrcGenTestRunner(new ConsoleDiagnosticMessageSink(outputHelper));
        var testSink = xunitTestFx.TestsSink;
        testSink.Execution.TestMethodStartingEvent += (methodStarting) => {
            outputHelper.WriteLine($"Starting: {methodStarting.Message}");
        };
        testSink.Execution.TestMethodFinishedEvent += (methodFinished) => outputHelper.WriteLine($"Finished: {methodFinished.Message}");
        Assert.Equal(0, xunitTestFx.ExecuteTests(testCases));
    }

    private class ConsoleDiagnosticMessageSink(ITestOutputHelper outputHelper) : IMessageSink
    {
        public bool OnMessage(IMessageSinkMessage message)
        {
            if (message is IDiagnosticMessage diagnosticMessage)
            {
                outputHelper.WriteLine(diagnosticMessage.Message);
                return true;
            }
            return false;
        }
    }
}