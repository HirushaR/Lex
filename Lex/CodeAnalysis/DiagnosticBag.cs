using System;
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

        public void ReportUndefinedName(TextSpan span, string name)
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

        internal void ReportCannotAssign(TextSpan span, string name)
        {
           var message =  $"Variable '{name}' is read-only and cannot be assigned to.";
            Report(span, message);
        }

       
    }




}