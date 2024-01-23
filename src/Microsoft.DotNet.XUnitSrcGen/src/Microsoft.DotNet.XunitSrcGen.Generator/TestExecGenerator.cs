// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Microsoft.DotNet.XunitSrcGen.Generator.Tests;

[Generator]
public sealed class TestExecGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var (discoverer, cases) = RoslynTestDiscoverer.RunDiscovery(context.Compilation, context.Compilation.Assembly);
        var srcBuilder = new StringBuilder();
        srcBuilder.Append("""
namespace Microsoft.DotNet.XunitSrcGen;

internal static class TestCasesContainer
{
    public static readonly string[] AllCases = new[] {

""");

        foreach (var @case in cases)
        {
            var serialized = discoverer.Serialize(@case);
            srcBuilder.AppendLine($"@\"{serialized}\",");
        }
        srcBuilder.Append("""
    };
}
""");
        context.AddSource("TestCaseContainer", srcBuilder.ToString());
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }
}