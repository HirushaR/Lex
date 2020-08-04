﻿using System;

namespace Lex.CodeAnalysis
{
    public struct TextSpan
    {
        public TextSpan(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;

        internal static TextSpan FromBounds(int start, int end)
        {
            var length = end - start;
            return new TextSpan(start,length);
        }
    }

}