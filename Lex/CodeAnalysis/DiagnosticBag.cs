﻿using System;
using System.Collections;
using System.Collections.Generic;
using Lex.CodeAnalysis.Symbols;
using Lex.CodeAnalysis.Syntax;
using Lex.CodeAnalysis.Text;

namespace Lex.CodeAnalysis
{
    internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        public readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        private void Report(TextSpan span, string message)
        {
            var diagnostics = new Diagnostic(span, message);
            _diagnostics.Add(diagnostics);
        }

        public void ReportInvalidNumber(TextSpan textSpan, string text, TypeSymbol type)
        {
            var message = $"The number {text} isn't valid {type}.";
            Report(textSpan, message);
        }

        public void AddRange(DiagnosticBag diagnostics)
        {
            _diagnostics.AddRange(diagnostics._diagnostics);
        }

        public void ReportBadCharactor(int position, char character)
        {
            var span = new TextSpan(position, 1);
            var message = $"ERROR: bad character input: '{character}'.";
            Report(span, message);
        }

        public void ReportUnterminatedString(TextSpan span)
        {
             var message = $"Unterminated string Literal.";
            Report(span, message);
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxKind actualKind, SyntaxKind expectKind)
        {
            var message = $"ERROR: Unexpected token <{actualKind}>, expected <{expectKind}>.";
            Report(span, message);
        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, TypeSymbol OperandType)
        {
            var message = $"Unary operator '{operatorText}' is not defined for type '{OperandType}'.";
            Report(span, message);
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string Operatortext, TypeSymbol leftType, TypeSymbol rightType)
        {

            var message = $"Binary operator '{Operatortext}' is not defined for types '{leftType}' and '{rightType}'.";
            Report(span, message);
        }

        public void ReportUndefinedVariable(TextSpan span, string name)
        {
            var message = $"Variable '{name}' doesn't exist.";
            Report(span, message);
        }

        internal void ReportVariableAlreadyDecleard(TextSpan span, string name)
        {
            var message = $"Variable '{name}' is already declared.";
            Report(span, message);
        }
        public void ReportCannotConvert(TextSpan span, TypeSymbol fromType, TypeSymbol toType)
        {
            var message = $"Cannot convert type '{fromType}' to '{toType}'.";
            Report(span, message);
        }
        public void ReportNotAVariable(TextSpan span, string name)
        {
            var message = $"'{name}' is not a variable.";
            Report(span, message);
        }
         public void ReportNotAFunction(TextSpan span, string name)
        {
            var message = $"'{name}' is not a function.";
            Report(span, message);
        }

        public void ReportCannotAssign(TextSpan span, string name)
        {
           var message =  $"Variable '{name}' is read-only and cannot be assigned to.";
            Report(span, message);
        }

        public void ReportWrongArgumentCount(TextSpan span, string name, int exprectedCount, int actualCount)
        {
           var message =  $"Function '{name}'  requires '{exprectedCount}' arguments but was given '{actualCount}'.";
            Report(span, message);
        }

        public void ReportUndefinedFunction(TextSpan span, string name)
        {
           var message =  $"Function '{name}'  doesn't exist.";
            Report(span, message);
        }

        public void ReportWrongArgumentType(TextSpan span, string name, string pname, TypeSymbol ptype, TypeSymbol actualType)
        {
            var message =  $"Function '{name}', parameter '{pname}' requires a value of type '{ptype}' arguments but was given a value of type  '{actualType}'.";
            Report(span, message);
        }
    
        public void ReportExpressionMustHaveVale(TextSpan span)
        {
            var message =  "Expression must have a Value.";
            Report(span, message);
        }

        internal void ReportSymbolAlreadyDeclared(TextSpan span, string name)
        {
            var message = $"'{name}' is already declared.";
            Report(span, message);
        }

        public void ReportUndefinedType(TextSpan span, string name)
        {
            var message = $"Type '{name}' doesn't exist.";
            Report(span, message);
        }
        public void ReportCannotConvertImplicitly(TextSpan span, TypeSymbol fromType, TypeSymbol toType)
        {
            var message = $"Cannot convert type '{fromType}' to '{toType}'. An explicit conversion exists (are you missing a cast?)";
            Report(span, message);
        }

        public void ReportParameterAlreadyDeclared(TextSpan span, string parameterName)
        {
            var message = $"A parameter with the name '{parameterName}' already exists.";
            Report(span, message);
        }
        public void ReportInvalidReturn(TextSpan span)
        {
            var message = "The 'return' keyword can only be used inside of functions.";
            Report(span, message);
        }

        public void ReportInvalidBreakOrContinue(TextSpan span, string text)
        {
            var message = $"The keyword '{text}' can only be used inside of loops.";
            Report(span, message);
        }

        public void ReportInvalidReturnExpression(TextSpan span, string functionName)
        {
            var message = $"Since the function '{functionName}' does not return a value the 'return' keyword cannot be followed by an expression.";
            Report(span, message);
        }

        public void ReportMissingReturnExpression(TextSpan span, TypeSymbol returnType)
        {
            var message = $"An expression of type '{returnType}' expected.";
            Report(span, message);
        }

        public void ReportAllPathsMustReturn(TextSpan span)
        {
            var message = "Not all code paths return a value.";
            Report(span, message);
        }

        
    }




}