using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Lex.CodeAnalysis.Lowering;
using Lex.CodeAnalysis.Symbols;
using Lex.CodeAnalysis.Syntax;
using Lex.CodeAnalysis.Text;



namespace Lex.CodeAnalysis.Binding
{

    internal sealed class Binder
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        private Stack<(BoundLabel BreakLabel, BoundLabel ContinueLabel)> _loopStack = new Stack<(BoundLabel BreakLabel, BoundLabel ContinueLabel)>();
        private int _labelCounter;
        private BoundScope _scope;
        private readonly FunctionSymbol _function;

        public Binder(BoundScope parent,FunctionSymbol function)
        {
            _scope = new BoundScope(parent);
             _function = function;

            if (function != null)
            {
                foreach (var p in function.Parameter)
                    _scope.TryDeclareVariable(p);
            }
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnitSyntax syntax)
        {
            var parentScope = CreateParentScope(previous);
            var binder = new Binder(parentScope, function: null);

            foreach (var function in syntax.Members.OfType<FunctionDeclarationSyntax>())
                binder.BindFunctionDeclaration(function);

             var statements = ImmutableArray.CreateBuilder<BoundStatement>();

            foreach (var globalStatement in syntax.Members.OfType<GlobalStatementSyntax>())
            {
                var statement = binder.BindStatement(globalStatement.Statement);
                statements.Add(statement);
            }

            var functions = binder._scope.GetDeclaredFunctions();
            var variables = binder._scope.GetDeclaredVariables();
            var diagnostics = binder.Diagnostics.ToImmutableArray();

            if (previous != null)
                diagnostics = diagnostics.InsertRange(0, previous.Diagnostics);

            return new BoundGlobalScope(previous, diagnostics, functions, variables, statements.ToImmutable());
        }
        public static BoundProgram BindProgram(BoundGlobalScope globalScope)
        {
            var parentScope = CreateParentScope(globalScope);

            var functionBodies = ImmutableDictionary.CreateBuilder<FunctionSymbol, BoundBlockStatemnet>();
            var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();

            var scope = globalScope;
            while (scope != null)
            {
                foreach (var function in scope.Functions)
                {
                    var binder = new Binder(parentScope, function);
                    var body = binder.BindStatement(function.Declaration.Body);
                    var loweredBody = Lowerer.Lower(body);
                    functionBodies.Add(function, loweredBody);

                    diagnostics.AddRange(binder.Diagnostics);
                }

                scope = scope.Previous;
            }

            var statement = Lowerer.Lower(new BoundBlockStatemnet(globalScope.Statements));

            return new BoundProgram(diagnostics.ToImmutable(), functionBodies.ToImmutable(), statement);
        }

        private void BindFunctionDeclaration(FunctionDeclarationSyntax syntax)
        {
            var parameters = ImmutableArray.CreateBuilder<ParameterSymbol>();

            var seenParameterNames = new HashSet<string>();

            foreach (var parameterSyntax in syntax.Parameters)
            {
                var parameterName = parameterSyntax.Identifier.Text;
                var parameterType = BindTypeClause(parameterSyntax.Type);
                if (!seenParameterNames.Add(parameterName))
                {
                    _diagnostics.ReportParameterAlreadyDeclared(parameterSyntax.Span, parameterName);
                }
                else
                {
                    var parameter = new ParameterSymbol(parameterName, parameterType);
                    parameters.Add(parameter);
                }
            }

            var type = BindTypeClause(syntax.Type) ?? TypeSymbol.Void;

            // if (type != TypeSymbol.Void)
            //     _diagnostics.ReportInvalidReturn(syntax.Type.Span);

            var function = new FunctionSymbol(syntax.Identifier.Text, parameters.ToImmutable(), type, syntax);
            if (!_scope.TryDeclareFunction(function))
                _diagnostics.ReportSymbolAlreadyDeclared(syntax.Identifier.Span, function.Name);
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

                foreach (var f in previous.Functions)
                    scope.TryDeclareFunction(f);

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

        private BoundStatement BindErrorStatement()
        {
            return new BoundExpressionStatemnet(new BoundErrorExpression());
        }
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
                case SyntaxKind.BreakStatement:
                    return BindBreakStatement((BreakStatementSyntax)syntax);
                case SyntaxKind.ContinueStatement:
                    return BindContinueStatement((ContinueStatementSyntax)syntax);
                case SyntaxKind.ReturnStatement:
                    return BindReturnStatement((ReturnStatementSyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }
        private BoundStatement BindReturnStatement(ReturnStatementSyntax syntax)
        {
            var expression = syntax.Expression == null ? null : BindExpression(syntax.Expression);

            if (_function == null)
            {
                _diagnostics.ReportInvalidReturn(syntax.ReturnKeyword.Span);
            }
            else
            {
                if (_function.Type == TypeSymbol.Void)
                {
                    if (expression != null)
                        _diagnostics.ReportInvalidReturnExpression(syntax.Expression.Span, _function.Name);
                }
                else
                {
                    if (expression == null)
                        _diagnostics.ReportMissingReturnExpression(syntax.ReturnKeyword.Span, _function.Type);
                    else
                        expression = BindConversion(syntax.Expression.Span, expression, _function.Type);
                }
            }

            return new BoundReturnStatement(expression);
        }
        
        private BoundStatement BindVeriableDeclaration(VeriableDeclarationSyntax syntax)
        {
 
            var type = BindTypeClause(syntax.TypeClause);
            var initializer = BindExpression(syntax.Initializer);
            var variableType = type ?? initializer.Type;
            var variable = BindVariable(syntax.Identifier, false, variableType);
            var convertedInitializer = BindConversion(syntax.Initializer.Span, initializer, variableType);
            

            return new BoundVeriableDeclaration(variable, convertedInitializer);
            
        }

        private TypeSymbol BindTypeClause(TypeClauseSyntax syntax)
        {
            if (syntax == null)
                return null;

            var type = LookupType(syntax.Identifier.Text);
            if (type == null)
                _diagnostics.ReportUndefinedType(syntax.Identifier.Span, syntax.Identifier.Text);

            return type;
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

            var body = BindLoopBody(syntax.Body, out var bodyLabel, out var breakLabel, out var continueLabel);

            _scope = _scope.Parent;

            return new BoundForStatement(variable, lowerBound, upperBound, Ittetarot, body,bodyLabel, breakLabel, continueLabel);
        }

        private BoundStatement BindLoopBody(StatementSyntax body, out BoundLabel bodyLabel,out BoundLabel breakLabel, out BoundLabel continueLabel)
        {
            _labelCounter++;
            bodyLabel = new BoundLabel($"body{_labelCounter}");
            breakLabel = new BoundLabel($"break{_labelCounter}");
            continueLabel = new BoundLabel($"continue{_labelCounter}");

            _loopStack.Push((breakLabel, continueLabel));
            var boundBody = BindStatement(body);
            _loopStack.Pop();

            return boundBody;
        }

        private BoundStatement BindBreakStatement(BreakStatementSyntax syntax)
        {
            if (_loopStack.Count == 0)
            {
                _diagnostics.ReportInvalidBreakOrContinue(syntax.Keyword.Span, syntax.Keyword.Text);
                return BindErrorStatement();
            }

            var breakLabel = _loopStack.Peek().BreakLabel;
            return new BoundGotoStatment(breakLabel);
        }

        private BoundStatement BindContinueStatement(ContinueStatementSyntax syntax)
        {
            if (_loopStack.Count == 0)
            {
                _diagnostics.ReportInvalidBreakOrContinue(syntax.Keyword.Span, syntax.Keyword.Text);
                return BindErrorStatement();
            }

            var continueLabel = _loopStack.Peek().ContinueLabel;
            return new BoundGotoStatment(continueLabel);
        }


        private VariableSymble BindVariable(SyntaxToken identifier,bool isreadonly,TypeSymbol type)
        {
            var name = identifier.Text ?? "?";
            var declare = !identifier.isMissing;
     
           var variable = _function == null
                                ? (VariableSymble) new GlobalVariableSymbol(name, isreadonly, type)
                                : new LocalVariableSymbol(name, isreadonly, type);


            if (declare &&!_scope.TryDeclareVariable(variable))
                _diagnostics.ReportVariableAlreadyDecleard(identifier.Span, name);
            return variable;
        }

        private BoundStatement BindWhileStatement(WhileStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, TypeSymbol.Bool);
            var body = BindLoopBody(syntax.Body, out var bodyLabel, out var breakLabel, out var continueLabel);
            return new BoundWhileStatement(condition, body, bodyLabel, breakLabel, continueLabel);
        }

        private BoundStatement BindExpressionStatement(ExpressionStatemnetSyntax syntax)
        {
            var expression = BindExpression(syntax.Expression,canBeVoid: true);
            return new BoundExpressionStatemnet(expression);
        }
        private BoundExpression BindExpression(ExpressionSyntax syntax,TypeSymbol TargetType)
        {
            return BindConversion(syntax, TargetType);
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
            if(syntax.Arguments.Count == 1 && LookupType(syntax.Identifier.Text) is TypeSymbol type )
                return BindConversion(syntax.Arguments[0], type,  allowExplicit: true);
            
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
                     _diagnostics.ReportWrongArgumentType(syntax.Arguments[i].Span,function.Name, parameter.Name, parameter.Type,Argument.Type);
                    return new BoundErrorExpression();
                }
            }
            return new BoundCallExpression(function, boundArguments.ToImmutable());
           
        }

        private BoundExpression BindConversion(ExpressionSyntax syntax, TypeSymbol type, bool allowExplicit = false)
        {
            var expression = BindExpression(syntax);
            return BindConversion(syntax.Span, expression, type,allowExplicit);
        }

        private BoundExpression BindConversion(TextSpan diagnosticSpan, BoundExpression expression, TypeSymbol type, bool allowExplicit = false)
        {
            var conversion = Conversion.Classify(expression.Type, type);

            if (!conversion.Exist)
            {
                if (expression.Type != TypeSymbol.Error && type != TypeSymbol.Error)
                {
                    _diagnostics.ReportCannotConvert(diagnosticSpan, expression.Type, type);
                }

                return new BoundErrorExpression();
            }
            if (!allowExplicit && conversion.IsExplictic)
            {
                _diagnostics.ReportCannotConvertImplicitly(diagnosticSpan, expression.Type, type);
            }

            if (conversion.IsIdentity)
                return expression;

            return new BoundConversionExpression(type, expression);
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