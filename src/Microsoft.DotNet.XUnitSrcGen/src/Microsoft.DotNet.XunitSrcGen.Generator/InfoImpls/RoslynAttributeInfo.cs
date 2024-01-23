// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit.Abstractions;

namespace Microsoft.DotNet.XUnitSrcGen.Generator;

public sealed class RoslynAttributeInfo(Compilation comp, AttributeData attributeData) : IAttributeInfo
{
    public static IEnumerable<IAttributeInfo> GetAttributeInfos(
        Compilation comp,
        ImmutableArray<AttributeData> attributes,
        string assemblyQualifiedAttributeTypeName)
    {
        var (typeName, asmName) = ParseAqn(assemblyQualifiedAttributeTypeName);
        var matchingAsmSymbol = comp.References
            .Select(r => (IAssemblySymbol?)comp.GetAssemblyOrModuleSymbol(r))
            .Where(a => a?.Name == asmName)
            .SingleOrDefault();
        var targetType = matchingAsmSymbol?.GetTypeByMetadataName(typeName);
        return attributes
            .Where(a => comp.HasImplicitConversion(a.AttributeClass, targetType))
            .Select(a => new RoslynAttributeInfo(comp, a))
            .ToList();
    }

    private static (string TypeName, string AsmName) ParseAqn(string aqn)
    {
        var parts = aqn.Split(',');
        var typeName = parts[0].Trim();
        var asmName = parts[1].Trim();
        return (typeName, asmName);
    }

    public IEnumerable<object?> GetConstructorArguments()
    {
        foreach (var arg in attributeData.ConstructorArguments)
        {
            if (arg.Kind == TypedConstantKind.Array)
            {
                yield return (object?)arg.Values.Select(v => v.Value);
            }
            else
            {
                yield return arg.Value;
            }
        }
    }

    /// <summary>
    /// Returns the attributes on the attribute type.
    /// </summary>
    public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
    {
        if (attributeData.AttributeClass is null)
        {
            return Enumerable.Empty<IAttributeInfo>();
        }
        return GetAttributeInfos(comp, attributeData.AttributeClass.GetAttributes(), assemblyQualifiedAttributeTypeName);
    }

    public TValue? GetNamedArgument<TValue>(string argumentName)
    {
        var arg = attributeData.NamedArguments
            .Where(a => a.Key == argumentName)
            .Select(a => a.Value)
            .SingleOrDefault();
        if (arg.Kind == TypedConstantKind.Array)
        {
            throw new InvalidOperationException("Can't decode arrays");
        }
        else if (arg.IsNull)
        {
            return default;
        }
        else
        {
            return (TValue?)arg.Value;
        }
    }
}
