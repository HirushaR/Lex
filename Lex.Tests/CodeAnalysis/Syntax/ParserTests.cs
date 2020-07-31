using Lex.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using Xunit;

namespace Lex.Tests.CodeAnalysis.Syntax
{


    public class ParserTests
    {
        [Theory]
        [MemberData(GetBinaryOperatorPairsData)]
        public void Parser_BinaryExpression_honorsPrecedences(SyntaxKind op1, SyntaxKind op2)
        {

        }

     
    }
}