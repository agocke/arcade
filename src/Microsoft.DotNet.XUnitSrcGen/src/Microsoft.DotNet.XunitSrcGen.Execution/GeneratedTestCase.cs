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

public sealed class GeneratedTypeInfo(Type type) : ITypeInfo, IGeneratedTypeInfo
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

    public object[] CreateTestClassConstructorArguments()
    {
        throw new InvalidOperationException();
        //var isStaticClass = Class.Type.GetTypeInfo().IsAbstract && Class.Type.GetTypeInfo().IsSealed;
        //if (!isStaticClass)
        //{
        //    var ctor = SelectTestClassConstructor();
        //    if (ctor != null)
        //    {
        //        var unusedArguments = new List<Tuple<int, ParameterInfo>>();
        //        var parameters = ctor.GetParameters();

        //        object[] constructorArguments = new object[parameters.Length];
        //        for (int idx = 0; idx < parameters.Length; ++idx)
        //        {
        //            var parameter = parameters[idx];
        //            object argumentValue;

        //            if (TryGetConstructorArgument(ctor, idx, parameter, out argumentValue))
        //                constructorArguments[idx] = argumentValue;
        //            else if (parameter.HasDefaultValue)
        //                constructorArguments[idx] = parameter.DefaultValue;
        //            else if (parameter.IsOptional)
        //                constructorArguments[idx] = parameter.ParameterType.GetTypeInfo().GetDefaultValue();
        //            else if (parameter.GetCustomAttribute<ParamArrayAttribute>() != null)
        //                constructorArguments[idx] = Array.CreateInstance(parameter.ParameterType, 0);
        //            else
        //                unusedArguments.Add(Tuple.Create(idx, parameter));
        //        }

        //        if (unusedArguments.Count > 0)
        //            Aggregator.Add(new TestClassException(FormatConstructorArgsMissingMessage(ctor, unusedArguments)));

        //        return constructorArguments;
        //    }
        //}

        //return new object[0];
    }

    public ConstructorInfo SelectTestClassConstructor()
    {
        throw new InvalidOperationException();
        //    var ctors = Class.Type.GetTypeInfo()
        //                          .DeclaredConstructors
        //                          .Where(ci => !ci.IsStatic && ci.IsPublic)
        //                          .ToList();

        //    if (ctors.Count == 1)
        //        return ctors[0];

        //    Aggregator.Add(new TestClassException("A test class may only define a single public constructor."));
        //    return null;
    }
}

public sealed class GeneratedAttributeInfo(CustomAttributeData attributeData) : IAttributeInfo
{
    public IEnumerable<object?> GetConstructorArguments() => attributeData.ConstructorArguments.Select(a => a.Value);
    public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName) => throw new NotImplementedException();
    public TValue GetNamedArgument<TValue>(string argumentName) => throw new NotImplementedException();
}