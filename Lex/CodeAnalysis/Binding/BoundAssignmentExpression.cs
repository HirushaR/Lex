﻿using System;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{

    internal sealed class BoundAssignmentExpression : BoundExpression
    {

       public BoundAssignmentExpression(VariableSymble variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public override TypeSymbol Type => Expression.Type;
        public VariableSymble Variable { get; }
        public BoundExpression Expression { get; }
        
    }
}
