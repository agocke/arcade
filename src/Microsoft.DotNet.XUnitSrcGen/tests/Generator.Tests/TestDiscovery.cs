// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;
using Xunit;

namespace Microsoft.DotNet.XunitSrcGen.Generator.Tests;

public sealed class TestDiscovery
{
    private static ImmutableArray<MetadataReference> s_metadataReferences;

    internal static async ValueTask<CSharpCompilation> CreateCompilation(string src)
    {
        var globalUsings = """
global using Xunit;
""";

        if (s_metadataReferences.IsDefault)
        {
            var ns20 = await ReferenceAssemblies.NetStandard.NetStandard20.ResolveAsync(null, CancellationToken.None); ;
            ns20 = ns20.AddRange(new[] {
                MetadataReference.CreateFromFile(typeof(FactAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Xunit.Sdk.FactDiscoverer).Assembly.Location),
            });
            ImmutableInterlocked.InterlockedInitialize(ref s_metadataReferences, ns20);
        }

        var comp = CSharpCompilation.Create(
            Guid.NewGuid().ToString(),
            new[] { src, globalUsings }.Select(src => SyntaxFactory.ParseSyntaxTree(src)),
            s_metadataReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        return comp;
    }

    [Fact]
    public async Task FactAndTheory()
    {
        var comp = await CreateCompilation("""
public class UnitTest1
{
    [Fact]
    public void Test1()
    { }

    [Theory]
    [InlineData("Hello")]
    [InlineData("Goodbye")]
    public void Theory1(string str)
    { }
}

""");

        var cases = RoslynTestDiscoverer.RunDiscovery(comp, comp.Assembly).TestCases;

        Assert.Equal([
            "UnitTest1.Test1",
            "UnitTest1.Theory1(str: \"Hello\")",
            "UnitTest1.Theory1(str: \"Goodbye\")"
        ], cases.Select(c => c.DisplayName).ToList());
    }
}