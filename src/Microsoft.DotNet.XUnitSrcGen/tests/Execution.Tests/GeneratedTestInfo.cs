using SampleTests;

namespace Microsoft.DotNet.XunitSrcGen;

internal sealed class GeneratedTestMethod_Fact1 : GeneratedTestMethod
{
    public GeneratedTestMethod_Fact1()
    : base(new GeneratedTestClass_SampleTestClass(), nameof(SampleTestClass.Fact1), isStatic: false)
    { }

    public override bool IsAsync => false;

    public override bool IsAsyncVoid => false;

    public override object Invoke(object receiver, params object?[]? parameters)
    {
        ((SampleTestClass)receiver).Fact1();
        return null!;
    }
}

internal sealed class GeneratedTestClass_SampleTestClass : GeneratedTestClass
{
    public GeneratedTestClass_SampleTestClass() : base("Execution.Tests", nameof(SampleTestClass))
    {
    }

    public override object Create(object[] args) => new SampleTestClass();
}


internal sealed class GeneratedTestCase_Fact1() : GeneratedTestCase(Fact1TestMethod)
{
    public static readonly GeneratedTestMethod Fact1TestMethod = new GeneratedTestMethod_Fact1();
    public override string DisplayName => "Fact1";
    public override string? SkipReason => null;
    public override int Timeout => 0;
}