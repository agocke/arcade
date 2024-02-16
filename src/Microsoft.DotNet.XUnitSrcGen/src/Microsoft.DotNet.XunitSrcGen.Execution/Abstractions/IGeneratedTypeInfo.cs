// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.DotNet.XunitSrcGen;

public interface IGeneratedTypeInfo : ITypeInfo
{
    object[] CreateTestClassConstructorArguments();
    ConstructorInfo SelectTestClassConstructor();
    List<Task> GetClassFixturesAsync();
}

public interface IGeneratedMethodInfo : IMethodInfo
{
}