﻿using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using Lex.CodeAnalysis.Binding;
using Lex.CodeAnalysis.Syntax;

namespace Lex.CodeAnalysis
{
    public sealed class Compilation
    { 
        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }
        public SyntaxTree Syntax { get; }

        public EvaluationResult Evaluate(Dictionary<VariableSymble, object> variables)
        {
            var binder = new Binder(variables);
            var boundExpression = binder.BindExpression(Syntax.Root.Expression);

            var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToImmutableArray();
            if (diagnostics.Any())
            {
                return new EvaluationResult(diagnostics, null);
            }

            var evaluator = new Evaluator(boundExpression, variables);
            var value = evaluator.Evaluate();

            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty,value);
        }
        
    }


}