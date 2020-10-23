using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Lex.CodeAnalysis.Symbols;
using Lex.CodeAnalysis.Syntax;


namespace Lex.CodeAnalysis.Binding
{

    internal sealed class Binder
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        private BoundScope _scope;

        public Binder(BoundScope parent)
        {
            _scope = new BoundScope(parent);
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnitSyntax syntax)
        {
            var parentScope = CreateParentScope(previous);
            var binder = new Binder(parentScope);
            var expression = binder.BindStatement(syntax.Statement);
            var variables = binder._scope.GetDeclaredVariables();
            var diagnostics = binder.Diagnostics.ToImmutableArray();

            if (previous != null)
                diagnostics = diagnostics.InsertRange(0, previous.Diagnostics);

            return new BoundGlobalScope(previous, diagnostics, variables, expression);
        }

        private static BoundScope CreateParentScope(BoundGlobalScope previous)
        {
            var stack = new Stack<BoundGlobalScope>();
            while (previous != null)
            {
                stack.Push(previous);
                previous = previous.Previous;
            }
            var parent = CreateRootScope();

            while (stack.Count > 0)
            {
                previous = stack.Pop();
                var scope = new BoundScope(parent);
                foreach (var v in previous.Variables)
                    scope.TryDeclareVariable(v);

                parent = scope;
            }

            return parent;
        }

        private static BoundScope CreateRootScope()
        {
            var result = new BoundScope(null);

            foreach(var f in BuiltinFunctions.GetAll())
                result.TryDeclareFunction(f);

            return result;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private BoundStatement BindStatement(StatementSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.BlockStatement:
                    return BindBlockStatement((BlockStatementSynatx)syntax);
                case SyntaxKind.VeriableDeclaration:
                    return BindVeriableDeclaration((VeriableDeclarationSyntax)syntax);
                case SyntaxKind.IfStatement:
                    return BindIfStatement((IfStatementSyntax)syntax);
                case SyntaxKind.WhileStatement:
                    return BindWhileStatement((WhileStatementSyntax)syntax);
                case SyntaxKind.ForStatement:
                    return BindForStatement((ForStatementSyntax)syntax); 
                case SyntaxKind.ExpressionStatemnet:
                    return BindExpressionStatement((ExpressionStatemnetSyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        
        private BoundStatement BindVeriableDeclaration(VeriableDeclarationSyntax syntax)
        {
 
            var initializer = BindExpression(syntax.Initializer);
            var variable = BindVariable(syntax.Identifier,false, initializer.Type);
            

            return new BoundVeriableDeclaration(variable,initializer);
            
        }

        private BoundStatement BindBlockStatement(BlockStatementSynatx syntax)
        {
            var statements = ImmutableArray.CreateBuilder<BoundStatement>();
            _scope = new BoundScope(_scope);

            foreach( var statementSyntax in syntax.Statements)
            {
                var statement = BindStatement(statementSyntax);
                statements.Add(statement);
            }
            _scope = _scope.Parent;

            return new BoundBlockStatemnet(statements.ToImmutable());
        }

         private BoundStatement BindIfStatement(IfStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, TypeSymbol.Bool);
            var thenStatement = BindStatement(syntax.ThenStatement);
            var elseStatement = syntax.ElseClouse == null ? null : BindStatement(syntax.ElseClouse.ElseStatement);
            return new BoundIfStatement(condition, thenStatement, elseStatement);
        }
        private BoundStatement BindForStatement(ForStatementSyntax syntax)
        {
            var lowerBound = BindExpression(syntax.LowerBound, TypeSymbol.Int);
            var upperBound = BindExpression(syntax.UpperBoud, TypeSymbol.Int);


            _scope = new BoundScope(_scope);

            SyntaxToken identifier = syntax.Identifier;
            var Ittetarot = syntax.Itterator == null ? null : BindExpression(syntax.Itterator, TypeSymbol.Int);
            VariableSymble variable = BindVariable(identifier,true,TypeSymbol.Int);

            var body = BindStatement(syntax.Body);

            _scope = _scope.Parent;

            return new BoundForStatement(variable, lowerBound, upperBound, Ittetarot, body);
        }

        private VariableSymble BindVariable(SyntaxToken identifier,bool isreadonly,TypeSymbol @int)
        {
            var name = identifier.Text ?? "?";
            var declare = !identifier.isMissing;
     
            var variable = new VariableSymble(name, isreadonly, @int);

            if (declare &&!_scope.TryDeclareVariable(variable))
                _diagnostics.ReportVariableAlreadyDecleard(identifier.Span, name);
            return variable;
        }

        private BoundStatement BindWhileStatement(WhileStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, TypeSymbol.Bool);
            var body = BindStatement(syntax.Body);
            return new BoundWhileStatement(condition, body);
        }

        private BoundStatement BindExpressionStatement(ExpressionStatemnetSyntax syntax)
        {
            var expression = BindExpression(syntax.Expression,canBeVoid: true);
            return new BoundExpressionStatemnet(expression);
        }
        private BoundExpression BindExpression(ExpressionSyntax syntax,TypeSymbol TargetType)
        {
            var result = BindExpression(syntax);
            if (result.Type != TypeSymbol.Error &&
                TargetType != TypeSymbol.Error &&
                 result.Type != TargetType)
                _diagnostics.ReportCannotConvert(syntax.Span, result.Type,TargetType);
            
            return result;
        }

         private BoundExpression BindExpression(ExpressionSyntax syntax,bool canBeVoid = false)
         {
             
             var result = BindExpressionInternal(syntax);
             if(!canBeVoid && result.Type == TypeSymbol.Void)
             {
                 _diagnostics.ReportExpressionMustHaveVale(syntax.Span);
                 return new BoundErrorExpression();
             }
             return result;
         }
        private BoundExpression BindExpressionInternal(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.ParenthesizedExpression:
                    return BindParenthesizedExpression((ParenthesizedExpressionSyntax)syntax);
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.NameExpression:
                    return BindNameExpression((NameExpressionSyntax)syntax);
                case SyntaxKind.AssigmentExpression:
                    return BindAssignmentExpression((AssigmentExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);
                case SyntaxKind.CallExpression:
                    return BindCallExpression((CallExpressionSyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundExpression BindCallExpression(CallExpressionSyntax syntax)
        {
         
            
            var boundArguments = ImmutableArray.CreateBuilder<BoundExpression>();

            foreach(var argument in syntax.Arguments)
            {
                var boundArgument = BindExpression(argument);
                boundArguments.Add(boundArgument);
            }
            if(!_scope.TryLookupFunction(syntax.Identifier.Text, out var function))
            {
                 _diagnostics.ReportUndefinedFunction(syntax.Identifier.Span,syntax.Identifier.Text);
                return new BoundErrorExpression();
            }
            
            if(syntax.Arguments.Count != function.Parameter.Length)
            {
                 _diagnostics.ReportWrongArgumentCount(syntax.Span,function.Name, function.Parameter.Length, syntax.Arguments.Count);
                return new BoundErrorExpression();
            }

            for (var i = 0; i<syntax.Arguments.Count;i++)
            {
               
                var Argument = boundArguments[i];
                var parameter = function.Parameter[i];

                if(Argument.Type != parameter.Type)
                {
                     _diagnostics.ReportWrongArgumentType(syntax.Span,function.Name, parameter.Name, parameter.Type,Argument.Type);
                    return new BoundErrorExpression();
                }
            }
            return new BoundCallExpression(function, boundArguments.ToImmutable());
           
        }

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            if(syntax.IdentifierToken.isMissing)
            {
                // this means the token was inserted by the parser. We already
                // reported the error we can just return an error expression
                return new BoundErrorExpression();
            }

            if (!_scope.TryLookupVariable(name, out var variable))
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new BoundErrorExpression();
            }

            return new BoundVariableExpression(variable);
        }

        private BoundExpression BindAssignmentExpression(AssigmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);

            if (!_scope.TryLookupVariable(name, out var variable))
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return boundExpression;
            }
            if(variable.isReadOnly)
                _diagnostics.ReportCannotAssign(syntax.EqualsToken.Span,name);

            if (boundExpression.Type != variable.Type)
            {
                _diagnostics.ReportCannotConvert(syntax.Expression.Span, boundExpression.Type, variable.Type);
                return boundExpression;
            }

            return new BoundAssignmentExpression(variable, boundExpression);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            if(boundOperand.Type == TypeSymbol.Error)
                return new BoundErrorExpression();
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperator == null)
            {
                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperand.Type);
                return new BoundErrorExpression();
            }

            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);

            if(boundLeft.Type == TypeSymbol.Error || boundRight.Type == TypeSymbol.Error)
                return new BoundErrorExpression();
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            
            if (boundOperator == null)
            {
                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundLeft.Type, boundRight.Type);
                return new BoundErrorExpression();
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }
    
        private TypeSymbol LookupType(string  name)  
        {
            switch (name)
            {
                case "bool" :
                     return TypeSymbol.Bool;
                case "int" :
                     return TypeSymbol.Int;
                case "string" :
                     return TypeSymbol.String;
                default:
                     return  null;
            }
        }
    
    }
}