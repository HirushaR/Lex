using System;

namespace Lex.CodeAnalysis
{
    public sealed class VariableSymble
    {
        public VariableSymble(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public Type Type { get; }
    }

}