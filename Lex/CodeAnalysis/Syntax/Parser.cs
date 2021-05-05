using System.Collections.Generic;
using System.Collections.Immutable;
using Lex.CodeAnalysis.Text;

namespace Lex.CodeAnalysis.Syntax
{
    internal sealed class Parser
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly ImmutableArray<SyntaxToken> _tokens;
        HashSet<string> _variables = new HashSet<string>();
        private readonly SourceText _text;
        private int _position;

        public Parser(SourceText text)
        {
            var tokens = new List<SyntaxToken>();

            var lexer = new Lexer(text);
        
            SyntaxToken token;
            do
            {
                token = lexer.Lex();

                if (token.Kind != SyntaxKind.WhitespaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);   
                }
            } while (token.Kind != SyntaxKind.EndOfFileToken);

            _tokens = tokens.ToImmutableArray();
            _diagnostics.AddRange(lexer.Diagnostics);
            _text = text;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];

            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            _diagnostics.ReportUnexpectedToken(Current.Span,Current.Kind,kind);
            return new SyntaxToken(kind, Current.Position, null, null);
        }
        public CompilationUnitSyntax ParseCompilationUnit()
        {
            var statement = ParseStatemnet();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new CompilationUnitSyntax(statement, endOfFileToken);
        }

        private StatementSyntax ParseStatemnet()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OpenBraceToken:
                    return ParseBlockStatemnt();
                case SyntaxKind.IdentifierToken:
                    return ParseVeriableDeclearation();
                case SyntaxKind.IfKeyword:
                    return ParseIfStatement();
                case SyntaxKind.WhileKeyword:
                    return ParseWhileStatement();
                case SyntaxKind.ForKeyword:
                    return ParseForStatement();
                default:
                    return ParseExpressionStatement();
            }
        }

      

        private StatementSyntax ParseVeriableDeclearation()
        {
            if (!_variables.Contains(Current.Text) && Peek(1).Kind == SyntaxKind.EaqlesToken ||!_variables.Contains(Current.Text) && Peek(1).Kind == SyntaxKind.ColonToken)
            {
                _variables.Add(Current.Text);
                var identifier = MatchToken(SyntaxKind.IdentifierToken);
                var typeClause = ParseOptionalTypeClause();
                var equalToken = MatchToken(SyntaxKind.EaqlesToken);
                var initializer = ParseExpression();
               
                return new VeriableDeclarationSyntax(identifier,typeClause, equalToken, initializer);
            }
            return ParseExpressionStatement();
        }

        private TypeClauseSyntax ParseOptionalTypeClause()
        {
            if (Current.Kind != SyntaxKind.ColonToken)
                return null;

            return ParseTypeClause();
        }

        private TypeClauseSyntax ParseTypeClause()
        {
            var colonToken = MatchToken(SyntaxKind.ColonToken);
            var identifier = MatchToken(SyntaxKind.IdentifierToken);
            return new TypeClauseSyntax(colonToken, identifier);
        }
       
        private StatementSyntax ParseIfStatement()
        {
           var keyword = MatchToken(SyntaxKind.IfKeyword);
           var condition = ParseExpression();
           var statement = ParseStatemnet();
           var elseClouse = ParseElseCLouse();

           return new IfStatementSyntax(keyword,condition, statement,elseClouse);
        }

        private ElseClouseSyntax ParseElseCLouse()
        {
            if(Current.Kind != SyntaxKind.ElseKeyword)
                return null;
            var keyword = NextToken();
            var statement = ParseStatemnet();
            return new ElseClouseSyntax(keyword,statement);
        }
       
          private StatementSyntax ParseForStatement()
        {
            
            var keyword = MatchToken(SyntaxKind.ForKeyword);
            var identifier = MatchToken(SyntaxKind.IdentifierToken);
            var equalsToken = MatchToken(SyntaxKind.EaqlesToken);
            var lowerBound = ParseExpression();
            var toKeyword = MatchToken(SyntaxKind.ToKeyword);
            var upperBound = ParseExpression();
            
            if(Current.Kind != SyntaxKind.ByKeyWord)
            {
                 var body = ParseStatemnet();
                 return new ForStatementSyntax(keyword, identifier, equalsToken, lowerBound, toKeyword, upperBound, body);
            }
            else
            {
                
                var byKeyWord  = MatchToken(SyntaxKind.ByKeyWord);
                var itterator  = ParseExpression();
                var body = ParseStatemnet();
                return new ForStatementSyntax(keyword, identifier, equalsToken, lowerBound, toKeyword, upperBound,byKeyWord, itterator, body);
            }
            
           // var body = ParseStatemnet();


           
            
        }

        
        private StatementSyntax ParseWhileStatement()
        {
           var keyword = MatchToken(SyntaxKind.WhileKeyword);
           var condition = ParseExpression();
           var body = ParseStatemnet();

           return new WhileStatementSyntax(keyword,condition,body);

        }


        private BlockStatementSynatx ParseBlockStatemnt()
        {
            var statements = ImmutableArray.CreateBuilder<StatementSyntax>();

            var openBraceToken = MatchToken(SyntaxKind.OpenBraceToken);

          
            while(Current.Kind != SyntaxKind.EndOfFileToken &&
                  Current.Kind != SyntaxKind.CloseBraceToken)
                {
                    var startToken = Current;
                    
                    var statement = ParseStatemnet();
                    statements.Add(statement);

                    // If Parse Statement() did not consume any token
                    // lets skip the current token and contine.
                    // in order to avoid an infinite loop
                    // we do not need report an error, because we'll
                    //already tried tp parse an expression statement
                    // and reported one
                    if(Current == startToken)
                    {   
                        NextToken();
                    }

                    startToken = Current;
                }

            var closeBraceToken = MatchToken(SyntaxKind.CloseBraceToken);

            return new BlockStatementSynatx(openBraceToken,statements.ToImmutable(),closeBraceToken);

        }

        private ExpressionStatemnetSyntax ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatemnetSyntax(expression);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParsAssigmentExpression();
        }
        private ExpressionSyntax ParsAssigmentExpression()
        {


            if(Peek(0).Kind == SyntaxKind.IdentifierToken &&
                Peek(1).Kind == SyntaxKind.EaqlesToken)
            {
                var identifierToken = NextToken();
                var operatorToken = NextToken();
                var right = ParsAssigmentExpression();
                return new AssigmentExpressionSyntax(identifierToken, operatorToken, right);
            }


            return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseBinaryExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence)
                    break;

                var operatorToken = NextToken();
                var right = ParseBinaryExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OpenParenthesisToken:                    
                        return ParseParenthesizedExpression();
                    

                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                        return ParseBoolenLiteral();
                    
                case SyntaxKind.NumberToken:
                    return ParseNumberLiteral();
                
                 case SyntaxKind.StringToken:
                    return ParseStringLiteral();
                
                case SyntaxKind.IdentifierToken:  
                default:
                     return ParseNameOrCallExpression();
            }
            
            
        }

        private ExpressionSyntax ParseNameOrCallExpression()
        {
            if(Peek(0).Kind == SyntaxKind.IdentifierToken && Peek(1).Kind == SyntaxKind.OpenParenthesisToken)
                return ParseCallExpression();
            
            return ParseNameExpression();
        }

        private ExpressionSyntax ParseCallExpression()
        {
            var identifier = MatchToken(SyntaxKind.IdentifierToken);
            var OpenParenthesisToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var arguments = ParseArguments();
            var CloseParenthesisToken = MatchToken(SyntaxKind.CloseParenthesisToken);

            return new CallExpressionSyntax(identifier, OpenParenthesisToken,arguments,CloseParenthesisToken);
        }

        private SeparatedSyntaxList<ExpressionSyntax> ParseArguments()
        {
            var nodesAndSeparator = ImmutableArray.CreateBuilder<SyntaxNode>();

            while(Current.Kind  != SyntaxKind.CloseParenthesisToken &&
                  Current.Kind != SyntaxKind.EndOfFileToken)
                {
                    var expression = ParseExpression();
                    nodesAndSeparator.Add(expression);
                    if(Current.Kind != SyntaxKind.CloseParenthesisToken)
                    {
                        var comma = MatchToken(SyntaxKind.CommaToken);
                        nodesAndSeparator.Add(comma);
                    }
                }

            return new SeparatedSyntaxList<ExpressionSyntax>(nodesAndSeparator.ToImmutableArray());
        }

        private ExpressionSyntax ParseParenthesizedExpression()
        {
            var left = MatchToken(SyntaxKind.OpenParenthesisToken);
            var expression = ParseExpression();
            var right = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new ParenthesizedExpressionSyntax(left, expression, right);
        }

        private ExpressionSyntax ParseBoolenLiteral()
        {
            var isTrue =Current.Kind == SyntaxKind.TrueKeyword;
            var keywordToken = isTrue? MatchToken(SyntaxKind.TrueKeyword) : MatchToken(SyntaxKind.FalseKeyword);
            return new LiteralExpressionSyntax(keywordToken, isTrue);
        }

        private ExpressionSyntax ParseNameExpression()
        {
            var identifierToken = MatchToken(SyntaxKind.IdentifierToken);
            return new NameExpressionSyntax(identifierToken);
        }
        
        
        private ExpressionSyntax ParseNumberLiteral()
        {
            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }

        private ExpressionSyntax ParseStringLiteral()
        {
            var stringToken = MatchToken(SyntaxKind.StringToken);
            return new LiteralExpressionSyntax(stringToken);
        }
    }
}