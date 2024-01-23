// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis;
using Xunit.Abstractions;

namespace Microsoft.DotNet.XUnitSrcGen.Generator;

public sealed class RoslynParameterInfo(Compilation comp, IParameterSymbol param) : IParameterInfo
{
    public string Name => param.Name;

    public ITypeInfo ParameterType => new RoslynTypeInfo(comp, param.Type);
}