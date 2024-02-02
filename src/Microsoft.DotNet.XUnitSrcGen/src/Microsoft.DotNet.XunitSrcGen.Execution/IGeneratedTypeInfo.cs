// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Xunit.Abstractions;

namespace Microsoft.DotNet.XunitSrcGen;

public interface IGeneratedTypeInfo : ITypeInfo
{
    object[] CreateTestClassConstructorArguments();
    ConstructorInfo SelectTestClassConstructor();
}