using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Lex.CodeAnalysis.Syntax
{
    public abstract class SeparatedSyntaxList
    {
        public abstract ImmutableArray<SyntaxNode> GetWithSeparators(); 
    }

    // print("hello")
    // add(1,2)
    public sealed class SeparatedSyntaxList<T> : SeparatedSyntaxList, IEnumerable<T>
        where T:SyntaxNode
    {
        private readonly ImmutableArray<SyntaxNode> _separatorAndNodes;

        public SeparatedSyntaxList(ImmutableArray<SyntaxNode> separatorAndNodes)
        {
            _separatorAndNodes = separatorAndNodes;
        }
        public int Count => _separatorAndNodes.Length + 1 /2;
        public T this[int index] => (T) _separatorAndNodes[index * 2];
        public SyntaxToken GetSeparator(int index) =>(SyntaxToken) _separatorAndNodes[index * 2 +1];
        public override ImmutableArray<SyntaxNode> GetWithSeparators() => _separatorAndNodes;
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0;i<Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}