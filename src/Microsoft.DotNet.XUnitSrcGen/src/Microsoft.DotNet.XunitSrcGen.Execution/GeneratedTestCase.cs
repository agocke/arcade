// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.DotNet.XunitSrcGen;

public sealed class GeneratedTestCase(
    GeneratedTestMethod method) : ITestCase, IXunitTestCase
{
    public string DisplayName => throw new NotImplementedException();

    public string SkipReason => throw new NotImplementedException();

    public ISourceInformation SourceInformation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public ITestMethod TestMethod => method;

    public object[] TestMethodArguments => throw new NotImplementedException();

    public Dictionary<string, List<string>> Traits => throw new NotImplementedException();

    public string UniqueID => throw new NotImplementedException();

    public Exception InitializationException => throw new NotImplementedException();

    public IMethodInfo Method => throw new NotImplementedException();

    public int Timeout => throw new NotImplementedException();

    public void Deserialize(IXunitSerializationInfo info) => throw new NotImplementedException();
    public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) => throw new NotImplementedException();
    public void Serialize(IXunitSerializationInfo info) => throw new NotImplementedException();
}

public sealed class GeneratedTestMethod(
    GeneratedMethodInfo methodInfo,
    GeneratedTestClass testClass) : ITestMethod
{
    public IMethodInfo Method => methodInfo;

    public ITestClass TestClass => testClass;

    public void Deserialize(IXunitSerializationInfo info) => throw new NotImplementedException();
    public void Serialize(IXunitSerializationInfo info) => throw new NotImplementedException();
}

public sealed class GeneratedTestClass(
    GeneratedTypeInfo typeInfo,
    GeneratedTestCollection collection) : ITestClass
{
    public ITypeInfo Class => typeInfo;

    public ITestCollection TestCollection => collection;

    public void Deserialize(IXunitSerializationInfo info) => throw new NotImplementedException();
    public void Serialize(IXunitSerializationInfo info) => throw new NotImplementedException();
}

public sealed class GeneratedTestCollection(GeneratedTestAssembly testAssembly) : ITestCollection
{
    public ITypeInfo? CollectionDefinition => null;

    public string DisplayName => throw new NotImplementedException();

    public ITestAssembly TestAssembly => testAssembly;

    public Guid UniqueID { get; } = Guid.NewGuid();

    public void Deserialize(IXunitSerializationInfo info) => throw new NotImplementedException();
    public void Serialize(IXunitSerializationInfo info) => throw new NotImplementedException();
}

public sealed class GeneratedTestAssembly : ITestAssembly
{
    public IAssemblyInfo Assembly => throw new NotImplementedException();

    public string ConfigFileName => throw new NotImplementedException();

    public void Deserialize(IXunitSerializationInfo info) => throw new NotImplementedException();
    public void Serialize(IXunitSerializationInfo info) => throw new NotImplementedException();
}

public sealed class GeneratedMethodInfo(MethodInfo methodInfo) : IMethodInfo
{
    public bool IsAbstract => throw new NotImplementedException();

    public bool IsGenericMethodDefinition => throw new NotImplementedException();

    public bool IsPublic => throw new NotImplementedException();

    public bool IsStatic => throw new NotImplementedException();

    public string Name => methodInfo.Name;

    public ITypeInfo ReturnType => throw new NotImplementedException();

    public ITypeInfo Type => throw new NotImplementedException();

    public MethodInfo MethodInfo => methodInfo;

    public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName) => throw new NotImplementedException();
    public IEnumerable<ITypeInfo> GetGenericArguments() => throw new NotImplementedException();
    public IEnumerable<IParameterInfo> GetParameters() => throw new NotImplementedException();
    public IMethodInfo MakeGenericMethod(params ITypeInfo[] typeArguments) => throw new NotImplementedException();
}

public sealed class GeneratedTypeInfo(Type type) : ITypeInfo
{
    public IAssemblyInfo Assembly => throw new NotImplementedException();

    public ITypeInfo BaseType => throw new NotImplementedException();

    public IEnumerable<ITypeInfo> Interfaces => throw new NotImplementedException();

    public bool IsAbstract => throw new NotImplementedException();

    public bool IsGenericParameter => throw new NotImplementedException();

    public bool IsGenericType => throw new NotImplementedException();

    public bool IsSealed => throw new NotImplementedException();

    public bool IsValueType => throw new NotImplementedException();

    public string Name => type.Name;

    public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
        => type.GetCustomAttributesData()
            .Where(a => a?.AttributeType?.AssemblyQualifiedName == assemblyQualifiedAttributeTypeName)
            .Select(a => new GeneratedAttributeInfo(a));

    public IEnumerable<ITypeInfo> GetGenericArguments() => throw new NotImplementedException();
    public IMethodInfo GetMethod(string methodName, bool includePrivateMethod) => throw new NotImplementedException();
    public IEnumerable<IMethodInfo> GetMethods(bool includePrivateMethods) => throw new NotImplementedException();
}

public sealed class GeneratedAttributeInfo(CustomAttributeData attributeData) : IAttributeInfo
{
    public IEnumerable<object?> GetConstructorArguments() => attributeData.ConstructorArguments.Select(a => a.Value);
    public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName) => throw new NotImplementedException();
    public TValue GetNamedArgument<TValue>(string argumentName) => throw new NotImplementedException();
}