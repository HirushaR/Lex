using System.Collections.Generic;
using System.Collections.Immutable;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundScope
    {        
        private Dictionary<string, VariableSymble> _variables = new Dictionary<string, VariableSymble>();

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

        public ImmutableArray<VariableSymble> GetDeclaredVariables()
        {
            return _variables.Values.ToImmutableArray();
        }
    }
}
