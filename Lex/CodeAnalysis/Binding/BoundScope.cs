using System.Collections.Generic;
using System.Collections.Immutable;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundScope
    {        
        private Dictionary<string, VariableSymble> _variables;
        private Dictionary<string, FunctionSymbol> _functions;

        public BoundScope(BoundScope parent)
        {
            Parent = parent;
        }

        public BoundScope Parent { get; }

        public bool TryDeclareVariable(VariableSymble variable)
        {
            if(_variables == null)
                _variables = new Dictionary<string, VariableSymble>();
            if (_variables.ContainsKey(variable.Name))
                return false;

            _variables.Add(variable.Name, variable);
            return true;
        }

        public bool TryLookupVariable(string name, out VariableSymble variable)
        {
            variable = null;
            if (_variables != null && _variables.TryGetValue(name, out variable))
                return true;

            if (Parent == null)
                return false;
            
            return Parent.TryLookupVariable(name, out variable);
        }
         public bool TryDeclareFunction(FunctionSymbol function)
        {
            if(_functions == null)
                _functions = new Dictionary<string, FunctionSymbol>();
            if (_functions.ContainsKey(function.Name))
                return false;

            _functions.Add(function.Name, function);
            return true;
        }

        public bool TryLookupFunction(string name, out FunctionSymbol function)
        {
            function = null;
            if (_functions != null && _functions.TryGetValue(name, out function))
                return true;

            if (Parent == null)
                return false;
            
            return Parent.TryLookupFunction(name, out function);
        }

        public ImmutableArray<VariableSymble> GetDeclaredVariables()
        {
            if(_variables == null)
                return ImmutableArray<VariableSymble>.Empty;
            return _variables.Values.ToImmutableArray();
        }
        public ImmutableArray<FunctionSymbol> GetDeclaredFunction()
        {
             if(_functions == null)
                return ImmutableArray<FunctionSymbol>.Empty;
            return _functions.Values.ToImmutableArray();
        }
    }
}
