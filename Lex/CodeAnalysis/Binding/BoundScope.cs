using System.Collections.Generic;
using System.Collections.Immutable;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundScope
    {        
        private Dictionary<string, VariableSymble> _variables = new Dictionary<string, VariableSymble>();
        private Dictionary<string, FunctionSymbol> _functions = new Dictionary<string, FunctionSymbol>();

        public BoundScope(BoundScope parent)
        {
            Parent = parent;
        }

        public BoundScope Parent { get; }

        public bool TryDeclareVariable(VariableSymble variable)
        {
            if (_variables.ContainsKey(variable.Name))
                return false;

            _variables.Add(variable.Name, variable);
            return true;
        }

        public bool TryLookupVariable(string name, out VariableSymble variable)
        {
            if (_variables.TryGetValue(name, out variable))
                return true;

            if (Parent == null)
                return false;
            
            return Parent.TryLookupVariable(name, out variable);
        }
         public bool TryDeclareFunction(FunctionSymbol function)
        {
            if (_functions.ContainsKey(function.Name))
                return false;

            _functions.Add(function.Name, function);
            return true;
        }

        public bool TryLookupFunction(string name, out FunctionSymbol function)
        {
            if (_functions.TryGetValue(name, out function))
                return true;

            if (Parent == null)
                return false;
            
            return Parent.TryLookupFunction(name, out function);
        }

        public ImmutableArray<VariableSymble> GetDeclaredVariables()
        {
            return _variables.Values.ToImmutableArray();
        }
        public ImmutableArray<FunctionSymbol> GetDeclaredFunction()
        {
            return _functions.Values.ToImmutableArray();
        }
    }
}
