using System.Collections.Generic;
using System.Collections.Immutable;


namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundScope
    {
        private Dictionary<string, VariableSymble> _variables = new Dictionary<string, VariableSymble>();

        public BoundScope Perant { get; }

        public BoundScope(BoundScope perant)
        {
            Perant = perant;
        }

        public bool TryDeclare(VariableSymble variable)
        {
            if(_variables.ContainsKey(variable.Name))
                return false;

            _variables.Add(variable.Name, variable);
            return true;
        }
        public bool TryLookUp(string name, out VariableSymble variable)
        {
            if(_variables.TryGetValue(name, out variable))
                return true;
            
            if(Perant == null)
                return false;
            return Perant.TryLookUp(name, out variable);
        }

        public ImmutableArray<VariableSymble> GetDeclardVariables()
        {
            return _variables.Values.ToImmutableArray();
        }
    }
}
