// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit.Abstractions;

namespace Microsoft.DotNet.XUnitSrcGen.Generator;

public sealed class RoslynMethodInfo(Compilation comp, IMethodSymbol method) : IMethodInfo
{
    public IMethodSymbol MethodSymbol => method;

    public bool IsAbstract => throw new NotImplementedException();

    public bool IsGenericMethodDefinition => method.IsDefinition && method.IsGenericMethod;

    public bool IsPublic => throw new NotImplementedException();

    public bool IsStatic => throw new NotImplementedException();

    public string Name => method.Name;

    public ITypeInfo ReturnType => throw new NotImplementedException();

    public ITypeInfo Type => throw new NotImplementedException();

    public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
        => RoslynAttributeInfo.GetAttributeInfos(comp, method.GetAttributes(), assemblyQualifiedAttributeTypeName);

    public IEnumerable<ITypeInfo> GetGenericArguments() => throw new NotImplementedException();
    public IEnumerable<IParameterInfo> GetParameters()
        => method.Parameters.Select(p => new RoslynParameterInfo(comp, p));

    public IMethodInfo MakeGenericMethod(params ITypeInfo[] typeArguments) => throw new NotImplementedException();
}