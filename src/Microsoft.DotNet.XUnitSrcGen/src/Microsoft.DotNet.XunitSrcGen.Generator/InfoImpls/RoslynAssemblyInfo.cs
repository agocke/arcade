// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit.Abstractions;

namespace Microsoft.DotNet.XUnitSrcGen.Generator;

public sealed class RoslynAssemblyInfo(Compilation comp, IAssemblySymbol assembly) : IAssemblyInfo
{
    public string? AssemblyPath => null;

    public string Name => assembly.Name;

    public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
    {
        return RoslynAttributeInfo.GetAttributeInfos(comp, assembly.GetAttributes(), assemblyQualifiedAttributeTypeName);
    }

    public ITypeInfo GetType(string typeName) => throw new NotImplementedException();
    public IEnumerable<ITypeInfo> GetTypes(bool includePrivateTypes)
    {
        var types = GetTypes(assembly.GlobalNamespace).ToList();
        return types;

        IEnumerable<ITypeInfo> GetTypes(INamespaceOrTypeSymbol ns)
        {
            var typeMembers = ns.GetTypeMembers()
                .Select(t => new RoslynTypeInfo(comp, t))
                .Concat(ns
                    .GetMembers()
                    .OfType<INamespaceOrTypeSymbol>()
                    .SelectMany(m => GetTypes(m)));
            return typeMembers;
        }
    }
}