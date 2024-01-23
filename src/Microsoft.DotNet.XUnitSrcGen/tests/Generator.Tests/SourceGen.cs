// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Microsoft.DotNet.XunitSrcGen.Generator.Tests;

public sealed class SourceGen
{
    [Fact]
    public async Task FactTest()
    {
        var comp = await TestDiscovery.CreateCompilation("""
public class UnitTest1
{
    [Fact]
    public void Test1()
    { }
}
""");
        var (discoverer, cases) = RoslynTestDiscoverer.RunDiscovery(comp, comp.Assembly);
        var serialized = cases.Select(c => discoverer.Serialize(c)).ToList();
        List<string> expected = [ "Xunit.Sdk.XunitTestCase, xunit.execution.{Platform}:VGVzdE1ldGhvZDpYdW5pdC5TZGsuVGVzdE1ldGhvZCwgeHVuaXQuZXhlY3V0aW9uLntQbGF0Zm9ybX06VFdWMGFHOWtUbUZ0WlRwVGVYTjBaVzB1VTNSeWFXNW5PbFpIVm5wa1JFVTlDbFJsYzNSRGJHRnpjenBZZFc1cGRDNVRaR3N1VkdWemRFTnNZWE56TENCNGRXNXBkQzVsZUdWamRYUnBiMjR1ZTFCc1lYUm1iM0p0ZlRwV1IxWjZaRVZPZG1KSGVHeFpNMUp3WWpJME5sZElWblZoV0ZGMVZUSlNja3hzVW14ak0xSkVZako0YzFwWFRqQmhWemwxVEVOQ05HUlhOWEJrUXpWc1pVZFdhbVJZVW5CaU1qUjFaVEZDYzFsWVVtMWlNMHAwWmxSd1UxSXllRFpaTUdRMFlVZFdWazVYYUdsV01WVXlWbFJPYzJWdFVraFdibEpOWWtVMGQxa3lNWE5rVm5BMlkwWmtVMDFXYnpKWGExWlBVVEpHZEZOWWJHeFRSVFZvVm1wQk1HUXhiRmRaZWxaclZsZDRTVll5TldGaGJVWldVMnhrV21KVVJucFVWVnBoVTBaYWRXTkhkRk5TVmxVMVVUSjRVMkpIVFhwVmEwcHFUVEExYzFsc1pFdGpNbFpWWTBac2ExWjZWbmRhUlUweFZrWndTR016VmxkU01WbzJXa1ZXUjJWdFRYbFdibEphWWxobk1WUkZUa05PUjFKWVRsaENhMUY2Vm5OYVZXUlhZVzFTV1ZWdVFtbE5hbEl4V2xSR1EyTXhiRmxWYlRGcFRUQndNRnBzVW5kVmJHUkdUbFJhV0dKSFRqUlpWbVJMVTFkS1IxSnNjRmhTYTNBeVZrUktORlF3TlZoVVdIQldZbGhvY0ZaWWNGZFZWbVJZVFZoa2FWSnRkRFpXVmxZd1ZXc3hjV0pFUmxoaVZFWjZXVzE0UzJSSFNrbFViVVpYVmtaYWRsZFhlR3RXYXpWelZsaHdhVk5JUW5KVmFrWmhUVVpTU0dONlZtaFdNRm93Vm0weGQyRXhSbGxSYkdoWVlrZG9URnBYTVVkWFJUbFpWbXhDYVZKVVVYaFhXSEJQVlRKS1NGTnNVazlXYkVweVZUQldkMlF4YkhSTlYzQlBZa1pLVjFaR1VrTmhNREZKWVVob1ZsWnRhRmhXUjNoSFZsVXhSV0V3ZEZkV2VsWjNXVEZvVjJKR1RsWlZWRnBXVFRKNE5scEZaRmRrUlhoelZHcENhbUpYZURGWGJuQjNWVVphUjJOSVRsVldiVkpNV1d0VmVHUkdVbGxWYlVaVFRVWndkbFpITlhKa01ERkhZa2hHVmxaRldrNVdhMlJxWlVaU2NsVnJOV3RTVkZaV1YyNXdUMWxXV1hkV2FrNVlZbFJHVEZsVlZYaGpWbVIwWXpCMFUxSXhXbkZaYTJSSFpWWnNXVlZ1UW1sTmFsWkRXWHBPVDJKSFNsaFRiazVzVmxSV2IxbHNaRlpPYkZWNllraHdhMUl4V2pCVVIzTTFZVmRHZEZadGNHdFJXRUpHVjJ4a1QyTXhiRmxUYldoclVqSjRNbGx0ZUZOT1YwNUlWbXM1V2xaNlJuTlVNbmhQVGxkTmVsVnRlR2xWZWxaUlYxY3hkMkpHYTNwVlZEQkxVVEo0YUdNelRrSmpNMDVzWWxkS2MyVlZOV2hpVjFVMlZUTnNlbVJIVm5STWJFNHdZMjFzZFZwNmNFOVNSVlkxVjJ4U2FrMUZNVFpSV0ZKT1lXMW9jMVJ0YTNkTlJuQlZVbTF3VFZaSGFISlVNR1JTWkVVMWRGZFlhRnBXUm5CeFYxUktUbVZWTUhsWFdHTkxVVEo0YUdNelRsVmxXRUpzVkcxR2RGcFVjRlJsV0U0d1dsY3dkVlV6VW5saFZ6VnVUMnhhV0U1WVFtdFNiRXB6V1hwT1VtVkJQVDA9ClRlc3RNZXRob2RBcmd1bWVudHM6U3lzdGVtLk9iamVjdApEZWZhdWx0TWV0aG9kRGlzcGxheTpTeXN0ZW0uU3RyaW5nOlEyeGhjM05CYm1STlpYUm9iMlE9CkRlZmF1bHRNZXRob2REaXNwbGF5T3B0aW9uczpTeXN0ZW0uU3RyaW5nOlRtOXVaUT09ClRpbWVvdXQ6U3lzdGVtLkludDMyOjA=" ];
        Assert.Equal(expected[0], serialized[0]);
    }

    [Fact]
    public async Task GeneratorRuns()
    {
        var comp = await TestDiscovery.CreateCompilation("""
public class UnitTest1
{
    [Fact]
    public void Test1()
    { }
}
""");
        var driver = CSharpGeneratorDriver.Create(new TestExecGenerator());
        driver.RunGeneratorsAndUpdateCompilation(comp, out var outputCompilation, out var diagnostics);
        Assert.Empty(diagnostics);
        var trees = outputCompilation.SyntaxTrees.ToList();
        Assert.Equal(3, trees.Count);
        Assert.Contains("TestCaseContainer", trees.Last().ToString());
    }
}