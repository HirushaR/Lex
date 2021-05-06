using System.Collections.Immutable;
using Lex.CodeAnalysis.Syntax;

namespace Lex.CodeAnalysis.Symbols
{
    public sealed class FunctionSymbol : Symbol
    {
        public FunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameter,TypeSymbol type, FunctionDeclarationSyntax declaration = null)
            :base(name)
        {
           // Name = name;
            Parameter = parameter;
            Type = type;
            Declaration = declaration;
        }

        public override SymbolKind kind => SymbolKind.Function;

        // public new string Name { get; }
        public ImmutableArray<ParameterSymbol> Parameter { get; }
        public TypeSymbol Type { get; }
        public FunctionDeclarationSyntax Declaration { get; }
    }

}