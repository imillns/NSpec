using FluentAssertions;
using NSpec;

public class describe_before : nspec
{
    void they_run_before_each_example()
    {
        before = () => number = 1;
        it["number should be 2"] = () => (number = number + 1).Should().Be(2);
        //even though it was incremented in the previous example
        //the before runs again for each spec resetting it to 1
        it["number should be 1"] = () => number.Should().Be(1);
    }
    int number;
}

public static class describe_before_output
{
    public static string Output = @"
describe before
  they run before each example
    number should be 2 (__ms)
    number should be 1 (__ms)

2 Examples, 0 Failed, 0 Pending
";
    public static int ExitCode = 0;
}
