using Xunit.Abstractions;

namespace Aethon.Glare.Parsing
{
    public abstract class ParsingUnitTest
    {
        protected ITestOutputHelper Out { get; }

        protected ParsingUnitTest(ITestOutputHelper output)
        {
            Out = output;
        }
    }
}