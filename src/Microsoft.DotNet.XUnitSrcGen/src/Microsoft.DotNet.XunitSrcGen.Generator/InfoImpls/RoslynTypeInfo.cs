// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit.Abstractions;

namespace Microsoft.DotNet.XUnitSrcGen.Generator;

public sealed class RoslynTypeInfo(Compilation comp, ITypeSymbol type) : ITypeInfo
{
    public IAssemblyInfo Assembly => new RoslynAssemblyInfo(comp, type.ContainingAssembly);

    public ITypeInfo BaseType => throw new NotImplementedException();

    public IEnumerable<ITypeInfo> Interfaces => throw new NotImplementedException();

    public bool IsAbstract => type.IsAbstract;

    public bool IsGenericParameter => throw new NotImplementedException();

    public bool IsGenericType => throw new NotImplementedException();

    public bool IsSealed => throw new NotImplementedException();

    public bool IsValueType => throw new NotImplementedException();

    public string Name => type.Name;

    public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
    {
        return RoslynAttributeInfo.GetAttributeInfos(comp, type.GetAttributes(), assemblyQualifiedAttributeTypeName);
    }

    public IEnumerable<ITypeInfo> GetGenericArguments() => throw new NotImplementedException();
    public IMethodInfo GetMethod(string methodName, bool includePrivateMethod) => throw new NotImplementedException();
    public IEnumerable<IMethodInfo> GetMethods(bool includePrivateMethods)
    {
        return type.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(m => m.MethodKind == MethodKind.Ordinary)
            .Select(m => new RoslynMethodInfo(comp, m));
    }
}