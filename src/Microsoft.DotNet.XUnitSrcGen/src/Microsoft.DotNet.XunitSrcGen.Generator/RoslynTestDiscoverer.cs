// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.XUnitSrcGen.Generator;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.DotNet.XunitSrcGen.Generator;

public sealed class RoslynTestDiscoverer : TestFrameworkDiscoverer, ITestFrameworkDiscoverer
{
    public const string DisplayName = nameof(RoslynTestDiscoverer);

    public IXunitTestCollectionFactory TestCollectionFactory { get; private set; }

    RoslynTestDiscoverer(
        IAssemblyInfo assemblyInfo,
        ISourceInformationProvider sourceProvider,
        IMessageSink diagnosticMessageSink)
    : base(assemblyInfo, sourceProvider, diagnosticMessageSink)
    {
        IAttributeInfo? attributeInfo = assemblyInfo.GetCustomAttributes(typeof(CollectionBehaviorAttribute)).SingleOrDefault();
        bool flag = attributeInfo?.GetNamedArgument<bool>("DisableTestParallelization") ?? false;
        string? configFileName = null;
        TestAssembly testAssembly = new TestAssembly(assemblyInfo, configFileName);
        TestCollectionFactory = ExtensibilityPointFactory.GetXunitTestCollectionFactory(diagnosticMessageSink, attributeInfo, testAssembly);
        base.TestFrameworkDisplayName = string.Format(CultureInfo.InvariantCulture, "{0} [{1}, {2}]", new object[3]
        {
            DisplayName,
            TestCollectionFactory.DisplayName,
            flag ? "non-parallel" : "parallel"
        });
    }

    public static (ITestFrameworkDiscoverer Discoverer, List<ITestCase> TestCases) RunDiscovery(Compilation comp, IAssemblySymbol assembly)
    {
        var asmInfo = new RoslynAssemblyInfo(comp, assembly);
        using var sourceInfoProvider = new NullSourceInformationProvider();
        var consoleMessageSink = new ConsoleDiagnosticMessageSink();
        var discoverer = new RoslynTestDiscoverer(asmInfo, sourceInfoProvider, consoleMessageSink);
        var testDiscoverySink = new TestDiscoverySink();
        discoverer.Find(includeSourceInformation: false, testDiscoverySink, TestFrameworkOptions.ForDiscovery());
        testDiscoverySink.Finished.WaitOne();
        return (discoverer, testDiscoverySink.TestCases);
    }

    internal class ConsoleDiagnosticMessageSink : IMessageSink
    {
        public bool OnMessage(IMessageSinkMessage message)
        {
            if (message is IDiagnosticMessage diagnosticMessage)
            {
                return true;
            }
            return false;
        }
    }

    public new void Find(bool includeSourceInformation, IMessageSink discoveryMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions)
    {
        using IMessageBus messageBus = CreateMessageBus(discoveryMessageSink, discoveryOptions);
        foreach (ITypeInfo item in AssemblyInfo.GetTypes(includePrivateTypes: false).Where(IsValidTestClass))
        {
            ITestClass testClass = CreateTestClass(item);
            if (!FindTestsForType(testClass, includeSourceInformation, messageBus, discoveryOptions))
            {
                break;
            }
        }

        messageBus.QueueMessage(new DiscoveryCompleteMessage());
    }

    public new void Find(string typeName, bool includeSourceInformation, IMessageSink discoveryMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions)
    {
        using IMessageBus messageBus = CreateMessageBus(discoveryMessageSink, discoveryOptions);
        ITypeInfo type = AssemblyInfo.GetType(typeName);
        if (type != null && IsValidTestClass(type))
        {
            ITestClass testClass = CreateTestClass(type);
            FindTestsForType(testClass, includeSourceInformation, messageBus, discoveryOptions);
        }

        messageBus.QueueMessage(new DiscoveryCompleteMessage());
    }

    protected override ITestClass CreateTestClass(ITypeInfo @class)
    {
        return new TestClass(TestCollectionFactory.Get(@class), @class);
    }

    internal ITestClass CreateTestClass(ITypeInfo @class, Guid testCollectionUniqueId)
    {
        ITestCollection testCollection = TestCollectionFactory.Get(@class);
        return new TestClass(new TestCollection(testCollection.TestAssembly, testCollection.CollectionDefinition, testCollection.DisplayName, testCollectionUniqueId), @class);
    }

    internal bool FindTestsForMethod(ITestMethod testMethod, bool includeSourceInformation, IMessageBus messageBus, ITestFrameworkDiscoveryOptions discoveryOptions)
    {
        List<IAttributeInfo> list = testMethod.Method.GetCustomAttributes(typeof(FactAttribute)).ToList();
        if (list.Count > 1)
        {
            string errorMessage = string.Format(CultureInfo.InvariantCulture, "Test method '{0}.{1}' has multiple [Fact]-derived attributes", new object[2]
            {
                testMethod.TestClass.Class.Name,
                testMethod.Method.Name
            });
            ExecutionErrorTestCase testCase = new ExecutionErrorTestCase(base.DiagnosticMessageSink, Xunit.Sdk.TestMethodDisplay.ClassAndMethod, Xunit.Sdk.TestMethodDisplayOptions.None, testMethod, errorMessage);
            return ReportDiscoveredTestCase(testCase, includeSourceInformation, messageBus);
        }

        IAttributeInfo? attributeInfo = list.FirstOrDefault();
        if (attributeInfo == null)
        {
            return true;
        }

        Type? value = null;
        IAttributeInfo? attributeInfo2 = attributeInfo.GetCustomAttributes(typeof(XunitTestCaseDiscovererAttribute)).FirstOrDefault();
        if (attributeInfo2 != null)
        {
            List<string> list2 = attributeInfo2.GetConstructorArguments().Cast<string>().ToList();
            value = Xunit.Sdk.SerializationHelper.GetType(list2[1], list2[0]);
        }

        if (value is null)
        {
            return true;
        }

        IXunitTestCaseDiscoverer? discoverer = GetDiscoverer(value);
        if (discoverer == null)
        {
            return true;
        }

        foreach (IXunitTestCase item in discoverer.Discover(discoveryOptions, testMethod, attributeInfo))
        {
            if (!ReportDiscoveredTestCase(item, includeSourceInformation, messageBus))
            {
                return false;
            }
        }

        return true;
    }

    private IXunitTestCaseDiscoverer? GetDiscoverer(Type discovererType)
    {
        return ExtensibilityPointFactory.GetXunitTestCaseDiscoverer(base.DiagnosticMessageSink, discovererType);
    }

    protected override bool FindTestsForType(ITestClass testClass, bool includeSourceInformation, IMessageBus messageBus, ITestFrameworkDiscoveryOptions discoveryOptions)
    {
        foreach (IMethodInfo method in testClass.Class.GetMethods(includePrivateMethods: true))
        {
            TestMethod testMethod = new TestMethod(testClass, method);
            if (!FindTestsForMethod(testMethod, includeSourceInformation, messageBus, discoveryOptions))
            {
                return false;
            }
        }

        return true;
    }

    private static IMessageBus CreateMessageBus(IMessageSink messageSink, ITestFrameworkDiscoveryOptions options)
    {
        if (options.SynchronousMessageReportingOrDefault())
        {
            return new SynchronousMessageBus(messageSink);
        }

        return new MessageBus(messageSink);
    }

    private sealed class DiscoveryCompleteMessage : IDiscoveryCompleteMessage { }
}