namespace Lex.CodeAnalysis.Text
{
    public sealed class TextLine
    {
        public TextLine(SourceText text,int start, int length, int lenghtIncludeLineBreak)
        {
            Text = text;
            Start = start;
            Length = length;
            LenghtIncludeLineBreak = lenghtIncludeLineBreak;
        }

        public SourceText Text { get; }
        public int Start { get; }
        public int Length { get; }
        public int end => Start + Length;
        public int LenghtIncludeLineBreak { get; }
        public TextSpan Span => new TextSpan(Start, Length);
        public TextSpan SpanIncludingLineBreak => new TextSpan(Start, LenghtIncludeLineBreak);
        
        public override string ToString() => Text.ToString(Span);

    }

}