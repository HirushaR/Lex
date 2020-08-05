using Lex.CodeAnalysis.Text;
using Xunit;

namespace Lex.Tests.CodeAnalysis.Text
{
    public class SourceTextTests
    {
        [Theory]
        [InlineData(".",1)]
        [InlineData(".\r\n",2)]
        [InlineData(".\r\n\r\n",3)]
        public void SourceText_IncludeLastLine(string text, int expectedLineCount)
        {
            var sourceText = SourceText.From(text);
            Assert.Equal(expectedLineCount,sourceText.Lines.Length);
        }
    }
}