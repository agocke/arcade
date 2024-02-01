// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using Xunit.Abstractions;

namespace Microsoft.DotNet.XunitSrcGen
{
    class TestDiscoveryVisitor : IMessageSink, IDisposable
    {
        public TestDiscoveryVisitor()
        {
            Finished = new ManualResetEvent(initialState: false);
            TestCases = new List<ITestCase>();
        }

        public ManualResetEvent Finished { get; }

        public List<ITestCase> TestCases { get; }

        /// <inheritdoc/>
        public void Dispose()
        {
            Finished.Dispose();
        }

        /// <inheritdoc/>
        public bool OnMessage(IMessageSinkMessage message)
        {
            var discoveryMessage = message as ITestCaseDiscoveryMessage;
            if (discoveryMessage != null)
                TestCases.Add(discoveryMessage.TestCase);

            if (message is IDiscoveryCompleteMessage)
                Finished.Set();

            return true;
        }
    }
}