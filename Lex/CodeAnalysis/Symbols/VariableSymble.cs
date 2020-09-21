using System;

namespace Lex.CodeAnalysis.Symbols
{
    public sealed class VariableSymble: Symbol
    {
        public VariableSymble(string name,bool isreadOnly, Type type)
        :base(name)
        {
   
            isReadOnly = isreadOnly;
            Type = type;
        }

        public override SymbolKind kind => SymbolKind.Variable;

        public bool isReadOnly { get; }
        public Type Type { get; }

    }

}