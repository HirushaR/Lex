using System;
using System.Collections.Generic;

namespace Lex.CodeAnalysis.Syntax
{
    public static class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 6;

                default:
                    return 0;
            }
        }
        
         
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.StarToken:
                case SyntaxKind.StarStarToken:
                case SyntaxKind.SlashToken:
              
                    return 5;


                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                
                    return 4;

                case SyntaxKind.EaqulesEaqlesToken:
                case SyntaxKind.BangEaqlesToken:
                case SyntaxKind.LessOrEqualToken:
                case SyntaxKind.GreaterOrEqualToken:
                case SyntaxKind.GreaterThanToken:
                case SyntaxKind.LessThanToken:
                case SyntaxKind.RemainderToken:
                    return 3;

                case SyntaxKind.AmpersandAmpersandToken:
                    return 2;

                case SyntaxKind.PipePieToken:
                    return 1;

                default:
                    return 0;
            }
        }

        public static IEnumerable<SyntaxKind> GetUnaryOperatorsKinds()
        {
            
            var kinds =(SyntaxKind[]) Enum.GetValues(typeof(SyntaxKind));
            foreach(var kind in kinds)
            {
                if(GetUnaryOperatorPrecedence(kind) > 0)
                    yield return kind;
            }
        }

        public static IEnumerable<SyntaxKind> GetBinaryOperatorsKinds()
        {
            
            var kinds =(SyntaxKind[]) Enum.GetValues(typeof(SyntaxKind));
            foreach(var kind in kinds)
            {
                if(GetBinaryOperatorPrecedence(kind) > 0)
                    yield return kind;
            }
        }


        public static SyntaxKind GetKeywordKind(string text)
        {
            switch(text)
            {
                case "else":
                    return SyntaxKind.ElseKeyword;
                case "false":
                    return SyntaxKind.FalseKeyword; 
                 case "for":
                    return SyntaxKind.ForKeyword;
                case "true":
                    return SyntaxKind.TrueKeyword;          
                 case "if":
                    return SyntaxKind.IfKeyword;
                case "let":
                    return SyntaxKind.LetKeyword;
                           
                case "var":
                    return SyntaxKind.VarKeyword;
                 case "while":
                    return SyntaxKind.WhileKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }
         public static string GetText(SyntaxKind kind)
         {
            switch(kind)
            {
                    case SyntaxKind.PlusToken: 
                        return "+";
                    case SyntaxKind.MinusToken: 
                        return "-";
                    case SyntaxKind.StarToken: 
                        return "*";
                    case SyntaxKind.StarStarToken: 
                        return "**";
                    case SyntaxKind.SlashToken: 
                        return "/";
                    case SyntaxKind.RemainderToken: 
                        return "%";
                    case SyntaxKind.GreaterThanToken: 
                        return ">";
                    case SyntaxKind.LessThanToken: 
                        return "<";
                    case SyntaxKind.LessOrEqualToken: 
                        return "<=";
                    case SyntaxKind.GreaterOrEqualToken: 
                        return ">=";
                    case SyntaxKind.EaqlesToken: 
                        return "=";
                    case SyntaxKind.BangToken: 
                        return "!";
                    case SyntaxKind.EaqulesEaqlesToken: 
                        return "==";
                    case SyntaxKind.BangEaqlesToken: 
                        return "!=";
                    case SyntaxKind.AmpersandAmpersandToken: 
                        return "&&";
                    case SyntaxKind.PipePieToken: 
                        return "||";
                    case SyntaxKind.OpenParenthesisToken: 
                        return "(";
                    case SyntaxKind.CloseParenthesisToken: 
                        return ")";
                     case SyntaxKind.OpenBraceToken: 
                        return "{";
                    case SyntaxKind.CloseBraceToken: 
                        return "}";
                    case SyntaxKind.ElseKeyword: 
                        return "else";
                    case SyntaxKind.FalseKeyword : 
                        return "false";
                    case SyntaxKind.ForKeyword : 
                        return "for";
                    case SyntaxKind.TrueKeyword: 
                        return "true";
                    case SyntaxKind.IfKeyword: 
                        return "if";
                    case SyntaxKind.LetKeyword: 
                        return "let";                 
                    case SyntaxKind.VarKeyword : 
                        return "var";
                    case SyntaxKind.WhileKeyword : 
                        return "while";
                    
                    default:
                        return null;
            }
                
         }

    }
}