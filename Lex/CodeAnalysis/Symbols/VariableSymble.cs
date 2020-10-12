using System;

namespace Lex.CodeAnalysis.Symbols
{
    public class VariableSymble: Symbol
    {
        public VariableSymble(string name,bool isreadOnly, TypeSymbol type)
        :base(name)
        {
   
            isReadOnly = isreadOnly;
            Type = type;
        }

        public override SymbolKind kind => SymbolKind.Variable;

        public bool isReadOnly { get; }
        public TypeSymbol Type { get; }

    }

}