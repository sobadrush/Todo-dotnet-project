using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace Test_EFCore_Project;

public class UnitTest1
{
    // JetBrains Rider will warn you about usages of Console.WriteLine inside Fact methods
    // and suggest a quick-fix that will convert these usages to instances of ITestOutputHelper.
    // ref. https://www.jetbrains.com/help/rider/Xunit.XunitTestWithConsoleOutput.html
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test1()
    {
        _testOutputHelper.WriteLine("========== 我是 Test1 ==========");
        var result = 111;
        var expected = 111;
        Assert.Equal(expected, result);
    }
}