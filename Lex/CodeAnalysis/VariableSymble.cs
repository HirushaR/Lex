using System;

namespace Lex.CodeAnalysis
{
    public sealed class VariableSymble
    {
        public VariableSymble(string name,bool isreadOnly, Type type)
        {
            Name = name;
            isReadOnly = isreadOnly;
            Type = type;
        }

        public string Name { get; }
        public bool isReadOnly { get; }
        public Type Type { get; }
    }

}