using System;
using System.Collections.Immutable;

namespace Lex.CodeAnalysis.Text
{
    public sealed class SourceText
    {
       
        private SourceText(string text)
        {
            Lines = ParseLines(this, text);
        }
         public ImmutableArray<TextLine> Lines { get; }

        private static ImmutableArray<TextLine> ParseLines(SourceText sourceText, string text)
        {
            var result = ImmutableArray.CreateBuilder<TextLine>();
            var position = 0;
            var lineStart = 0;

            while(position < text.Length)
            {

                var lineBreakWidth = GetLineBreakWidth(text,position);
                if(lineBreakWidth == 0 )
                {
                    position++;
                }
                else
                {
                    AddLine(sourceText, position, lineStart, lineBreakWidth);
                    position += lineBreakWidth;
                    lineStart = position;
                }
            }
            if(position > lineStart)
                AddLine(sourceText, position, lineStart, 0);

            return result.ToImmutable();
        }

        private static void AddLine(SourceText sourceText, int position, int lineStart, int lineBreakWidth)
        {
            var lineLength = position - lineStart;
            var lineLengthIncludeLineBreak = lineLength + lineBreakWidth;
            var line = new TextLine(sourceText, lineStart, lineLength, lineLengthIncludeLineBreak);
        }

        private static int GetLineBreakWidth(string text, int position)
        {
            var c = text[position];
            var l = position + 1 >= text.Length ? '\0': text[position+1];

            if(c == '\r' && l == '\n')
                return 2;

            if(c== '\r' || c == '\n')
                return 1;
            return 0;
        }

        public static SourceText From(string text)
        {
            return new SourceText(text);
        }
    }

}