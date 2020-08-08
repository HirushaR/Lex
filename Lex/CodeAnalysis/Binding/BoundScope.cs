using System.Collections.Generic;
using System.Collections.Immutable;


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

        public bool TryDeclare(VariableSymble variable)
        {
            if (_variables.ContainsKey(variable.Name))
                return false;

            _variables.Add(variable.Name, variable);
            return true;
        }

        public bool TryLookup(string name, out VariableSymble variable)
        {
            if (_variables.TryGetValue(name, out variable))
                return true;

            if (Parent == null)
                return false;
            
            return Parent.TryLookup(name, out variable);
        }

        public ImmutableArray<VariableSymble> GetDeclaredVariables()
        {
            return _variables.Values.ToImmutableArray();
        }
    }
}
